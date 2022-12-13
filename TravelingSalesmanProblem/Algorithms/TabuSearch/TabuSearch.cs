using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms.TabuSearch;

public abstract class TabuSearch : ITspAlgorithm
{
  private readonly Graph _graph;
  private readonly double _maxTime;

  protected TabuSearch(Graph graph, double time)
  {
    _graph = graph;
    _maxTime = time * 1000; // zamiana [s] na [ms]
  }

  public (int, List<int>) Solve(int start)
  {
    var currentPath = Enumerable.Range(0, _graph.Size)
      .Except(new[] {start})
      .ToList();
    currentPath.Shuffle();
    
    var currentWeight = GetCurrentWeight(currentPath, start);
    var outputWeight = currentWeight;
    var outputPath = new List<int>();
    var diversificationCounter = 0;
    var tabuList = new Queue<(int, int)>();
    
    var timer = new Stopwatch();
    timer.Start();
    while (timer.ElapsedMilliseconds <= _maxTime)
    {
      var move = (0, 0);
      
      var previousWeight = currentWeight;
      currentWeight = FindBestMove(start, currentPath, currentWeight, tabuList, ref move); 
      
      GetNeighbour(currentPath, move.Item1, move.Item2);

      tabuList.Enqueue(move);

      if (tabuList.Count > _graph.Size)
      {
        tabuList.Dequeue();
      }
      
      // jesli znaleziono najlepsze dotychczasowe rozwiazanie, zapisz je
      if (currentWeight < outputWeight)
      {
        outputPath = new List<int>(currentPath);
        outputWeight = currentWeight;
      }
      // jezeli znaleziono minimum lokalne, nalezy zaczac szukac gdzie indziej
      else if (previousWeight <= currentWeight)
      {
        diversificationCounter++;
        if (diversificationCounter > _graph.Size * 10)
        {
          currentWeight = int.MaxValue;
          diversificationCounter = 0;
          currentPath.Shuffle();
          tabuList.Clear();
        }
      }
    }
    timer.Stop();

    outputPath.Insert(0, start);
    outputPath.Add(0);
    return (outputWeight, outputPath);
  }

  private int FindBestMove(int start, List<int> currentPath, 
    int currentWeight, Queue<(int, int)> tabuList, ref (int, int) move)
  {
    for (var i = 0; i < _graph.Size - 1; i++)
    {
      for (var j = i + 1; j < _graph.Size - 1; j++)
      {
        var neighbourPath = new List<int>(currentPath);
        GetNeighbour(neighbourPath, i, j);
        var neighbourWeight = GetCurrentWeight(neighbourPath, start);

        if (neighbourWeight < currentWeight && !tabuList.Contains((i, j)))
        {
          currentWeight = neighbourWeight;
          move = (i, j);
        }
      }
    }
    return currentWeight;
  }

  protected abstract void GetNeighbour(IList<int> input, int i, int j);

  private int GetCurrentWeight(List<int> path, int start)
  {
    var weight = 0;
    var currentVertex = start;

    for (var i = 0; i < path.Count; i++)
    {
      var vertex = path[i];
      weight += _graph.AdjacencyMatrix[currentVertex][vertex];
      currentVertex = vertex;
    }

    weight += _graph.AdjacencyMatrix[currentVertex][start];
    return weight;
  }
}
