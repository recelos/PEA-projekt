using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataStructures;
using TravelingSalesmanProblem.Algorithms;

namespace TravelingSalesmanProblem;

public class Benchmark
{
  public IEnumerable<long> Measure(int input, int iterations, int size)
  {
    
    Warmup(size, input);
    
    
    var output = new List<long>();

    for (var i = 0; i < iterations; i++)
    {
      var graph = GraphFactory.GenerateRandom(50, size);
      ITspAlgorithm algorithm;
      switch (input)
      {
        case 1:
          algorithm = new BruteForce(graph);
          break;
        case 2:
          algorithm = new BranchAndBound(graph);
          break;
          default:
            return new List<long>();
      }
      
      CalculateTime(algorithm, output);
      Console.WriteLine($"Iteracja {i+1} wykonana. Czas wykonania: {output.Last()}ms.");
    }
    return output;
  }

  private static void CalculateTime(ITspAlgorithm algorithm, ICollection<long> output)
  {
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    algorithm.Solve(0);
    stopwatch.Stop();
    output.Add(stopwatch.ElapsedMilliseconds);
  }

  private static void Warmup(int size, int input)
  {
    for (var i = 0; i < 50; i++)
    {
      var graph = GraphFactory.GenerateRandom(50, size);
      ITspAlgorithm algorithm;
      switch (input)
      {
        case 1:
          algorithm = new BruteForce(graph);
          break;
        case 2:
          algorithm = new BranchAndBound(graph);
          break;
        default:
          return;
      }

      algorithm.Solve(0);
      Console.WriteLine($"Iteracja rozgrzewkowa nr {i+1}");
    }
  }
  
}