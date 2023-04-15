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
  
  protected Genetic(
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
      //ocena jakosci rozwiazan
      foreach (var path in population)
      {
        path.Weight = GetWeight(path.Path, start);
      }
      
      //selekcja
      for (var i = 0; i < _populationSize; i++)
      {
        var first = population[_rand.Next(_populationSize)];
        var second = population[_rand.Next(_populationSize)];

        var toAdd = first.Weight <= second.Weight 
          ? new PathHolder(first.Weight, first.Path) 
          : new PathHolder(second.Weight, second.Path);
        
        nextPopulation.Add(toAdd);
      }

      population = nextPopulation;
      nextPopulation = new List<PathHolder>();

      // krzyzowanie
      for (var i = 0; i < (int)(_crossRate * _populationSize); i++)
      {
        population[i].Path = OrderCrossover(population[i].Path, population[i+1].Path);
      }

      //mutacja
      for (var i = 0; i < (int)(_mutationRate * _populationSize) + 1; i++)
      {
        var pathToMutateIndex = _rand.Next(_populationSize);
        Mutate(population[pathToMutateIndex].Path, _rand);
      }

      //sprawdzamy, czy znalezione zostalo nowe najlepsze rozwiazanie
      foreach (var chromosome in population)
      {
        chromosome.Weight = GetWeight(chromosome.Path, start);
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
  private List<int> OrderCrossover(
    IReadOnlyList<int> parent1, 
    IReadOnlyList<int> parent2)
  {
    var startIndex = _rand.Next(_graph.Size - 2);
    var endIndex = _rand.Next(startIndex, _graph.Size - 1);
    
    var output = new int[_graph.Size - 1];
    output.Fill(-1);
    
    for (var i = startIndex; i < endIndex + 1; i++)
    {
      output[i] = parent1[i];
    }
    
    var currentIndex = endIndex + 1;
    if (currentIndex > parent1.Count - 1)
    {
      currentIndex = 0;
    }
    
    for (var i = endIndex + 1; i < parent2.Count; i++)
    {
      if (output.All(x => x != parent2[i]))
      {
        output[currentIndex] = parent2[i];
        currentIndex++;
      }
    
      if (currentIndex > parent1.Count - 1)
      {
        currentIndex = 0;
      }
    }
    
    for (var i = 0; output.Any(element => element == -1); i++)
    {
      if (output.All(x => x != parent2[i]))
      {
        output[currentIndex] = parent2[i];
        currentIndex++;
        if (currentIndex > parent1.Count - 1)
        {
          currentIndex = 0;
        }
      }
    }
    return output.ToList();
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
    public int Weight { get; set; }
    public List<int> Path { get; set; }
    public PathHolder(int weight, List<int> path)
    {
      Weight = weight;
      Path = path;
    }
  }
}