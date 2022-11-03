using DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms;

public class BruteForce : ITspAlgorithm
{
  public (int, List<int>) Solve(Graph graph, int start)
  {
    var outputWeight = int.MaxValue;
    var outputPath = new List<int>();
    var vertices = new List<int>();
    
    // dodaj wszystkie krawedzie oprocz krawedzi startowej
    for (var i = 0; i < graph.Size; i++)
    {
      if (i != start)
      {
        vertices.Add(i);
      }
    }

    var hasNextPermutation = true;
    while (hasNextPermutation)
    {
      var currentVertex = start;
      
      // zsumowanie wag obecnej permutacji
      var currentWeight = graph.AdjacencyMatrix[currentVertex][start];
      foreach (var vertex in vertices)
      {
        currentWeight += graph.AdjacencyMatrix[currentVertex][vertex];
        currentVertex = vertex;
      }
      currentWeight += graph.AdjacencyMatrix[currentVertex][start];
      
      // ustawienie wagi obecnie przeliczonej drogi jesli jest mniejsza od poprzedniej
      if(outputWeight > currentWeight)
      {
        outputWeight = currentWeight;
        outputPath = new List<int>(vertices);
        outputPath.Insert(0, start);
        outputPath.Add(start);
      }

      // ustawienie nowej permutacji (jesli istnieje)
      hasNextPermutation = FindNextPermutation(vertices);
    }
    return (outputWeight, outputPath);
  }
  
  private static bool FindNextPermutation(IList<int> input)
  {
    if(input.Count <= 1) return false;
    var i = input.Count - 2;
    
    while(i >= 0 && input[i] >= input[i + 1]) i--;
    
    if (i < 0)
      return false;
    
    var j = input.Count - 1;              
    while(input[j] <= input[i]) j--;      
    
    input.Swap(i, j);                     
    input.ReverseSubList(i + 1, input.Count - 1);
    return true;
  }
}