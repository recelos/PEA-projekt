using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms.Genetic;

public abstract class Genetic : ITspAlgorithm
{
  private readonly Random _rand;
  private readonly Graph _graph;

  private readonly int _populationSize;
  private readonly double _crossRate;
  private readonly double _mutationRate;
  private readonly long _time;
  private const int TournamentSize = 4;
  
  
  public Genetic(
    Graph graph,
    long time,
    double crossRate,
    double mutationRate, 
    int populationSize)
  {
    _rand = new Random();
    _graph = graph;
    _populationSize = populationSize;
    _crossRate = crossRate;
    _mutationRate = mutationRate;
    _time = time * 1000;
  }

  public (int, List<int>) Solve(int start)
  {
    var population = new List<PathHolder>();
    var nextPopulation = new List<PathHolder>();
    
    for (var i = 0; i < _populationSize; i++)
    {
      var path = GenerateRandomPath(start);
      var weight = GetWeight(path, start);
      population.Add(new PathHolder(weight, path));
    }

    var bestWeight = ShortestPath(population).Weight;
    var bestPath = new List<int>(ShortestPath(population).Path);
    
    var timer = new Stopwatch();
    timer.Start();
    
    while (true)
    {
      foreach (var path in population)
      {
        path.Weight = GetWeight(path.Path, start);
      }
      for (var j = 0; j < _populationSize; j++)
      {
        var localResult = int.MaxValue;
        var toAdd = new PathHolder();
        
        for (var k = 0; k < TournamentSize; k++)
        {
          var index = _rand.Next(_populationSize);
          if (population[index].Weight < localResult)
          {
            localResult = population[index].Weight;
            toAdd =  new PathHolder(population[index].Weight, population[index].Path);
          }
        }
        nextPopulation.Add(toAdd);
      }

      population = nextPopulation;
      nextPopulation = new List<PathHolder>();

      for (var j = 0; j < (int)(_crossRate * (float)_populationSize); j += 2)
      {
        population[j].Path = OrderCrossover(population[j].Path, population[j+1].Path);
        population[j + 1].Path = OrderCrossover(population[j + 1].Path, population[j].Path);
      }

      for (var j = 0; j < (int)(_mutationRate * (float)_populationSize) + 1; j++)
      {
        var pathToMutateIndex = _rand.Next(_populationSize);
        Mutate(population[pathToMutateIndex].Path, _rand);
      }

      foreach (var element in population)
      {
        element.Weight = GetWeight(element.Path, start);
      }
      
      if (ShortestPath(population).Weight < bestWeight)
      {
        bestWeight = ShortestPath(population).Weight;
        bestPath = new List<int>(ShortestPath(population).Path);
      }
      
      if (timer.ElapsedMilliseconds > _time)
      {
        return (bestWeight, bestPath);
      }
    }
  }
  private List<int> OrderCrossover(IReadOnlyList<int> tab1, IReadOnlyList<int> tab2)
  {
    var startIndex = _rand.Next(_graph.Size - 2);
    var endIndex = _rand.Next(startIndex, _graph.Size - 1);
    
    var child = new int[_graph.Size - 1];
    child.Fill(-1);
    
    for (var i = startIndex; i < endIndex + 1; i++)
    {
      child[i] = tab1[i];
    }
    
    var actualChildIndex = endIndex + 1;
    if (actualChildIndex > tab1.Count - 1)
    {
      actualChildIndex = 0;
    }
    
    for (var i = endIndex + 1; i < tab2.Count; i++)
    {
      if (child.All(element => element != tab2[i]))
      {
        child[actualChildIndex] = tab2[i];
        actualChildIndex++;
      }
    
      if (actualChildIndex > tab1.Count - 1)
      {
        actualChildIndex = 0;
      }
    }
    
    for (var i = 0; child.Any(element => element == -1); i++)
    {
      if (child.All(element => element != tab2[i]))
      {
        child[actualChildIndex] = tab2[i];
        actualChildIndex++;
        if (actualChildIndex > tab1.Count - 1)
        {
          actualChildIndex = 0;
        }
      }
    }
    return child.ToList();
  }

  protected abstract void Mutate(List<int> path, Random rand);
  
  private List<int> GenerateRandomPath(int start)
  {
    var path = Enumerable.Range(0, _graph.Size)
      .Except(new[] {start})
      .ToList();
    path.Shuffle(_rand);
    return path;
  }
  
  private int GetWeight(IReadOnlyList<int> path, int start)
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

  private static PathHolder ShortestPath(IReadOnlyList<PathHolder> input)
  {
    var list = new List<PathHolder>(input);
    var output = list.OrderBy(x => x.Weight).First();
    return new PathHolder(output.Weight, new List<int>(output.Path));
  }
  private class PathHolder
  {
    public PathHolder() { }
    public int Weight { get; set; }
    public List<int> Path { get; set; }
    public PathHolder(int weight, List<int> path)
    {
      Weight = weight;
      Path = path;
    }
  }
}