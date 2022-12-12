using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms.TabuSearch;

public abstract class TabuSearch : ITspAlgorithm
{
  private readonly Graph _graph;

  private readonly double _maxTime;

  private readonly bool _diversification;
  
  protected TabuSearch(Graph graph, double time, bool diversification)
  {
    _graph = graph;
    _maxTime = time * 1000; // zamiana [s] na [ms]
    _diversification = diversification;
  }

  public (int, List<int>) Solve(int start)
  {
    // inicjalizacja tablicy tabu
    var tabuMatrix = new int[_graph.Size, _graph.Size];

    var currentPath = Enumerable.Range(0, _graph.Size)
      .Except(new[] {start})
      .ToList()
      .Shuffle();
    
    var currentWeight = GetCurrentCost(currentPath, start);
    var outputWeight = currentWeight;
    var outputPath = new List<int>();
    var diversificationCounter = 0;

    var timer = new Stopwatch();
    timer.Start();
    while (timer.ElapsedMilliseconds <= _maxTime)
    {
      int bestI = 0, bestJ = 0;
      
      var previousWeight = currentWeight;

      currentWeight = FindBestNeighbour(start, currentPath, outputWeight, currentWeight, tabuMatrix, ref bestI, ref bestJ); 
      
      GetNeighbour(currentPath, bestI, bestJ);
      tabuMatrix[bestI, bestJ] = _graph.Size;
      
      for (var i = 0; i < _graph.Size; i++)
        for (var j = i + 1; j < _graph.Size; j++)
          if (tabuMatrix[i, j] > 0) tabuMatrix[i, j] -= 1;
      
      if (currentWeight < outputWeight)
      {
        outputPath = new List<int>(currentPath);
        outputWeight = currentWeight;
        Console.WriteLine(outputWeight);
      }
      // jezeli znaleziono minimum lokalne, nalezy zaczac szukac gdzie indziej
      else if (previousWeight <= currentWeight)
      {
        diversificationCounter++;
        if (diversificationCounter > _graph.Size * 100)
        {
          diversificationCounter = 0;
          currentPath = currentPath.Shuffle();
          Console.WriteLine("SHUFFLED");
          Console.WriteLine(currentPath.CombineToString());
          tabuMatrix = new int[_graph.Size,_graph.Size];
        }
      }
    }

    timer.Stop();

    outputPath.Insert(0, start);
    outputPath.Add(0);
    return (outputWeight, outputPath);
  }

  private int FindBestNeighbour(int start, List<int> currentPath, int outputWeight, int currentWeight, int[,] tabuMatrix,
    ref int bestI, ref int bestJ)
  {
    for (var i = 0; i < _graph.Size - 1; i++)
    {
      for (var j = i + 1; j < _graph.Size - 1; j++)
      {
        var neighbourPath = new List<int>(currentPath);
        GetNeighbour(neighbourPath, i, j);

        var neighbourWeight = GetCurrentCost(neighbourPath, start);

        if (neighbourWeight < outputWeight || (neighbourWeight < currentWeight && tabuMatrix[i, j] == 0))
        {
          currentWeight = neighbourWeight;
          bestI = i;
          bestJ = j;
        }
      }
    }

    return currentWeight;
  }

  protected abstract void GetNeighbour(IList<int> input, int i, int j);

  private int GetCurrentCost(List<int> path, int start)
  {
    var weight = 0;
    var currentVertex = start;

    foreach (var vertex in path)
    {
      weight += _graph.AdjacencyMatrix[currentVertex][vertex];
      currentVertex = vertex;
    }

    weight += _graph.AdjacencyMatrix[currentVertex][start];
    return weight;
  }
}
