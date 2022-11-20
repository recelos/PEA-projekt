using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DataStructures;
using TravelingSalesmanProblem.Algorithms;

namespace TravelingSalesmanProblem;

public static class Benchmark
{
  public static IEnumerable<long> Measure(
    ITspAlgorithm algorithm, 
    Graph graph,
    int iterations)
  {
    var output = new List<long>();
        
    for (var i = 0; i < iterations; i++)
    {
      CalculateTime(algorithm, graph, output);
      Console.WriteLine($"Iteracja {i+1} wykonana. Czas wykonania: {output.Last()}ms.");
    }
    return output;
  }

  private static void CalculateTime(ITspAlgorithm algorithm, Graph graph, ICollection<long> output)
  {
    var stopwatch = new Stopwatch();
    stopwatch.Start();
    algorithm.Solve(0);
    stopwatch.Stop();
    output.Add(stopwatch.ElapsedMilliseconds);
  }
}