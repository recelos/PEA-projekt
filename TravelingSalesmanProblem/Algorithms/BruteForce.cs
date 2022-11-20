using System.Collections.Generic;
using DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms;

public class BruteForce : ITspAlgorithm
{
  private readonly Graph _graph;
    
  public BruteForce(Graph graph)
  {
    _graph = graph;
  }
  public (int, List<int>) Solve(int start)
  {
    var outputWeight = int.MaxValue;
    List<int>? outputPath = null;
    var permutation = new List<int>();
      
    // dodaj wszystkie krawedzie oprocz krawedzi startowej
    for(var i = 0; i < _graph.Size; i++)
    {
      if (i != start)
      {
        permutation.Add(i);
      }
    }
  
    var hasNextPermutation = true;
    while(hasNextPermutation)
    {
      var currentVertex = start;
        
      var currentWeight = 0;
      // zsumowanie wag krawedzi
      foreach (var vertex in permutation)
      {
        currentWeight += _graph.AdjacencyMatrix[currentVertex][vertex];
        currentVertex = vertex;
      }
      // dodanie wagi krawedzi z ostatniego wierzcholka ostatniego do startu sciezki
      currentWeight += _graph.AdjacencyMatrix[currentVertex][start];
  
      // ustawienie wagi obecnie przeliczonej drogi jesli jest mniejsza od poprzedniej
      if(outputWeight > currentWeight)
      {
        outputWeight = currentWeight;
        outputPath = new List<int>(permutation) { start };
      }
  
      // ustawienie nowej permutacji (jesli istnieje)
      hasNextPermutation = FindNextPermutation(permutation);
    }
    return (outputWeight, outputPath ?? new List<int>());
  }
    
  private static bool FindNextPermutation(IList<int> input)
  {
    if(input.Count <= 1) return false;
    var k = input.Count - 2;
  
    while (k >= 0 && input[k] >= input[k + 1])
    {
      k--;
    }
  
    if (k < 0)
    {
      return false;
    }
  
    var l = input.Count - 1;
      
    while (input[l] <= input[k])
    {
      l--;
    }
  
    input.Swap(k, l);
    input.ReverseSubList(k + 1, input.Count - 1);
    return true;
  }
}