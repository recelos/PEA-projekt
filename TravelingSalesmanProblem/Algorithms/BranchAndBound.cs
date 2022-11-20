using System.Collections;
using System.Collections.Specialized;
using System.Net.Sockets;
using DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms;

public class BranchAndBound
{
  private readonly Graph _graph;

  public BranchAndBound(Graph graph)
  {
    _graph = graph;
  }

  public (int, List<int>) Solve(int start)
  {
    var matrix = _graph.AdjacencyMatrix.DeepCopy();
    
    var reductionCost = ReduceMatrix(matrix);

    var availableVertices = new HashSet<int>();

    for (var i = 0; i < _graph.Size; i++)
    {
      if (i != 0)
      {
        availableVertices.Add(i);
      }
    }

    var pathsWithWeights = new List<PathHolder>();
    
    for (var i = 0; i < _graph.Size; i++)
    {
      if (!availableVertices.Contains(i)) continue;
      
      var tempMatrix = matrix.DeepCopy(); 
      MarkAsInfinity(tempMatrix, 0, i);

      var reduceCurrentMatrix = ReduceMatrix(tempMatrix);
      var matrixPath = new List<int> { i };
      var costToNextVertice = reduceCurrentMatrix + reductionCost + matrix[0][i];

      pathsWithWeights.Add(new PathHolder(matrixPath , costToNextVertice, tempMatrix));
    }


    while (pathsWithWeights.MinBy(x => x.Weight)!.Path.Count < _graph.Size - 1)
    {
      var shortestPath = pathsWithWeights.MinBy(x => x.Weight);

      for (var i = 1; i < _graph.Size; i++)
      {
        availableVertices.Add(i);
      }
      
      foreach (var i in shortestPath.Path)
      {
        availableVertices.Remove(i);
      }

      for (var i = 0; i < _graph.Size; i++)
      {
        if (!availableVertices.Contains(i)) continue;
        var lastVertice = shortestPath.Path.LastOrDefault();
        
        var tempMatrix = shortestPath.Matrix.DeepCopy();

        var cost = tempMatrix[lastVertice][i];
        
        MarkAsInfinity(tempMatrix, lastVertice, i);
        
        var reduceCurrentMatrix = ReduceMatrix(tempMatrix);
        
        var costToNextVertice = cost + reduceCurrentMatrix + shortestPath.Weight;

        var newList = new List<int>(shortestPath.Path) { i };

        pathsWithWeights.Add(new PathHolder(newList, costToNextVertice, tempMatrix));
      }
      
      pathsWithWeights.Remove(shortestPath);
    }

    var shortestWay = pathsWithWeights.MinBy(x => x.Weight);

    var weight = shortestWay.Weight;

    var path = shortestWay.Path;
    
    path.Insert(0, 0);
    path.Add(0);
    
    return (weight, path);
  }
  
  
  private static int ReduceMatrix(int[][] matrix)
  {
    var minimumRows = new int[matrix.Length];
    var minimumCols = new int[matrix.Length];

    for (int i = 0; i < matrix.Length; i++)
    {
      var temp = int.MaxValue;
      for (var j = 0; j < matrix.Length; j++)
      {
        if (matrix[i][j] < temp && i != j && matrix[i][j] >= 0)
        {
          temp = matrix[i][j];
        }
      }
      minimumRows[i] = temp == int.MaxValue ? 0 : temp;
    }

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
    
    for (var i = 0; i < matrix.Length; i++)
    {
      var temp = int.MaxValue;
      for (var j = 0; j < matrix.Length; j++)
      {
        if (matrix[j][i] < temp && i != j  && matrix[j][i] >= 0)
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

    return minimumCols.Sum() + minimumRows.Sum();
  }

  private static void MarkAsInfinity(int[][] input, int from, int to)
  {
    for (var i = 0; i < input.Length; i++)
    {
      input[from][i] = -1;
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