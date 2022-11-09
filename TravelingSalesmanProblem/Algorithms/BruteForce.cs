using DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms;

public class BruteForce : ITspAlgorithm
{
  public (int, List<int>) Solve(Graph graph, int start)
  {
    var outputWeight = int.MaxValue;
    var outputPath = new List<int>();
    var permutation = new List<int>();
    
    // dodaj wszystkie krawedzie oprocz krawedzi startowej
    for(var i = 0; i < graph.Size; i++)
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
        currentWeight += graph.AdjacencyMatrix[currentVertex][vertex];
        currentVertex = vertex;
      }
      // dodanie wagi krawedzi z ostatniego wierzcholka ostatniego do startu sciezki
      currentWeight += graph.AdjacencyMatrix[currentVertex][start];

      // ustawienie wagi obecnie przeliczonej drogi jesli jest mniejsza od poprzedniej
      if(outputWeight > currentWeight)
      {
        outputWeight = currentWeight;
        outputPath = new List<int>(permutation);
        outputPath.Insert(0, start);
        outputPath.Add(start);
      }

      // ustawienie nowej permutacji (jesli istnieje)
      hasNextPermutation = FindNextPermutation(permutation);
    }
    return (outputWeight, outputPath);
  }
  
  // generowanie następnej permutacji poprzez porzadek leksykograficzny
  private static bool FindNextPermutation(IList<int> input)
  {
    if(input.Count <= 1) return false;
    var i = input.Count - 2;

    while (i >= 0 && input[i] >= input[i + 1])
    {
      i--;
    }

    if (i < 0)
      return false;

    var j = input.Count - 1;
    while (input[j] <= input[i]) j--;

    input.Swap(i, j);
    input.ReverseSubList(i + 1, input.Count - 1);
    return true;
  }
}