using System.Collections.Generic;
using System.Linq;
using DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms;

public class BranchAndBound : ITspAlgorithm
{
  private readonly Graph _graph;

  public BranchAndBound(Graph graph)
  {
    _graph = graph;
  }

  public (int, List<int>) Solve(int start)
  {
    var matrix = _graph.AdjacencyMatrix.DeepCopy();

    // wstepna redukcja macierzy 
    var initialReductionCost = ReduceMatrix(matrix);

    var availableVertices = new HashSet<int>();
    for (var i = 0; i < _graph.Size; i++)
    {
      if (i != start)
      {
        availableVertices.Add(i);
      }
    }

    // kolekcja przechowujaca stany z waga, sciezka oraz zredukowana macierza
    var nodes = new List<PathHolder>();
    
    for (var i = 0; i < _graph.Size; i++)
    {
      if (!availableVertices.Contains(i)) continue;

      var tempMatrix = matrix.DeepCopy();
      MarkAsInfinity(tempMatrix, start, i);

      var reduceCurrentMatrix = ReduceMatrix(tempMatrix);
      var tempPath = new List<int> { i };
      var tempWeight = reduceCurrentMatrix + initialReductionCost + matrix[start][i];

      nodes.Add(new PathHolder(tempPath, tempWeight, tempMatrix));
    }

    var currentBound = ShortestPath(nodes);
    
    // powtarzaj petle tak dlugo dopoki sciezka z najmniejsza waga nie jest rowna dlugosci koncowej sciezki
    while (currentBound.Path.Count < _graph.Size - 1)
    {

      // dobranie mozliwych kolejnych sciezek
      for (var i = 1; i < _graph.Size; i++)
      {
        availableVertices.Add(i);
      }
      foreach (var i in currentBound.Path)
      {
        availableVertices.Remove(i);
      }

      var lastVertex = currentBound.Path.LastOrDefault();
      for (var i = 0; i < _graph.Size; i++)
      {
        //przeszukuj tylko dostepne wierzcholmi
        if (!availableVertices.Contains(i)) continue;

        var tempMatrix = currentBound.Matrix.DeepCopy();
        var travelCost = tempMatrix[lastVertex][i] > 0 ? tempMatrix[lastVertex][i] : 0 ;

        //oznacz kolumne, wiersz oraz krawedz trasy jako -1
        MarkAsInfinity(tempMatrix, lastVertex, i);

        // zredukuj macierz
        var reduceCurrentMatrix = ReduceMatrix(tempMatrix);
        var newWeight = travelCost + reduceCurrentMatrix + currentBound.Weight;
        var newPath = new List<int>(currentBound.Path) { i };

        nodes.Add(new PathHolder(newPath, newWeight, tempMatrix));
      }
      // usuwanie poprzedniej sciezki zeby uniknac zapetlania
      nodes.Remove(currentBound);
      currentBound = ShortestPath(nodes);
    }

    var weight = currentBound.Weight;
    var path = currentBound.Path;

    path.Insert(0, start);
    path.Add(start);

    return (weight, path);
  }

  // Zwraca najkrotsza zapisana sciezke
  private static PathHolder ShortestPath(List<PathHolder> input)
    => input.OrderBy(x => x.Weight).FirstOrDefault();
  
  private static int ReduceMatrix(int[][] matrix)
  {
    var minimumRows = new int[matrix.Length];
    var minimumCols = new int[matrix.Length];

    for (var i = 0; i < matrix.Length; i++)
    {
      var temp = int.MaxValue;
      //znajdz najmniejsza wartosc w wierszu
      for (var j = 0; j < matrix.Length; j++)
      {
        if (matrix[i][j] < temp && i != j && matrix[i][j] >= 0)
        {
          temp = matrix[i][j];
        }
      }
      minimumRows[i] = temp == int.MaxValue ? 0 : temp;
    }
    
    // odejmij ja od wszystkich wartosci w wierszu
    for (var i = 0; i < matrix.Length; i++)
    {
      for (var j = 0; j < matrix.Length; j++)
      {
        if (i != j && minimumRows[i] > 0)
        {
          matrix[i][j] -= minimumRows[i];
        }
      }
    }

    // to samo dla kolumn
    for (var i = 0; i < matrix.Length; i++)
    {
      var temp = int.MaxValue;
      for (var j = 0; j < matrix.Length; j++)
      {
        if (matrix[j][i] < temp && i != j && matrix[j][i] >= 0)
        {
          temp = matrix[j][i];
        }
      }

      minimumCols[i] = temp == int.MaxValue ? 0 : temp;
    }

    for (var i = 0; i < matrix.Length; i++)
    {
      for (var j = 0; j < matrix.Length; j++)
      {
        if (i != j && minimumCols[i] > 0)
        {
          matrix[j][i] -= minimumCols[i];
        }
      }
    }

    // suma jest kosztem redukcji
    return minimumCols.Sum() + minimumRows.Sum();
  }

  private static void MarkAsInfinity(int[][] input, int from, int to)
  {
    for (var i = 0; i < input.Length; i++)
    {
      // oznaczenie wiersza z ktorego wychodzimy
      input[from][i] = -1;
      // oznaczenie kolumny do ktorej wchodzimy
      input[i][to] = -1;
    }
    input[to][from] = -1;
  }
  
  private struct PathHolder
  {
    public PathHolder(List<int> path, int weight, int[][] matrix)
    {
      Path = path;
      Weight = weight;
      Matrix = matrix;
    }

    public List<int> Path { get; }
    public int Weight { get; }
    public int[][] Matrix { get; }
  }
}