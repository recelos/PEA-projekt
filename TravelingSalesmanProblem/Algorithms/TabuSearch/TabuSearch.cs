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
      var previousWeight = currentWeight;

      var bestCurrentPath = currentPath;
      
      for (var i = 1; i < _graph.Size - 1; i++)
      {
        for (var j = i + 1; j < _graph.Size - 1; j++)
        {
          var neighbourPath = new List<int>(currentPath);
          GetNeighbour(neighbourPath, i, j);

          var neighbourWeight = GetCurrentCost(neighbourPath, start);

          if (neighbourWeight < outputWeight || (neighbourWeight < currentWeight && tabuMatrix[i, j] == 0))
          {
            currentWeight = neighbourWeight;
            tabuMatrix[i, j] = _graph.Size;
            bestCurrentPath = currentPath;
          }
        }
      }

      currentPath = bestCurrentPath;
      
      for (var i = 0; i < _graph.Size; i++)
      {
        for (var j = i + 1; j < _graph.Size; j++)
        {
          if (tabuMatrix[i, j] > 0)
            tabuMatrix[i, j] -= 1;
        }
      }

      if (currentWeight < outputWeight)
      {
        outputPath = new List<int>(currentPath);
        outputWeight = currentWeight;
      }
      // jezeli znaleziono minimum lokalne, nalezy zaczac szukac gdzie indziej
      else if(previousWeight == currentWeight && _diversification)
      {
        diversificationCounter++;
        if (diversificationCounter > 50)
        {
          diversificationCounter = 0;
          currentPath = currentPath.Shuffle();
          tabuMatrix = new int[_graph.Size,_graph.Size];
        }
      }
    }

    timer.Stop();

    outputPath.Insert(0, start);
    outputPath.Add(0);
    return (outputWeight, outputPath);
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

  private List<int> GreedyFirstPath(List<int> input, int start)
  {
    var output = new List<int>();
    var current = start;

    while (input.Any())
    {
      var shortest = int.MaxValue;
      var nextVertex = 0;
      foreach (var vertex in input)
      {
        if (_graph.AdjacencyMatrix[current][vertex] < shortest)
        {
          shortest = _graph.AdjacencyMatrix[current][vertex];
          nextVertex = vertex;
        }
      }

      input.Remove(nextVertex);
      output.Add(nextVertex);
      current = nextVertex;
    }

    return output;
  }
}