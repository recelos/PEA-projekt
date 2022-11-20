namespace DataStructures;

public class Graph
{
  public int[][] AdjacencyMatrix { get; }

  public int Size => AdjacencyMatrix.Length;
  
  public Graph(int[][] adjMatrix)
  {
    AdjacencyMatrix = adjMatrix;
  }
  
  public void Print()
  {
    foreach (var line in AdjacencyMatrix)
    {
      foreach (var value in line)
      {
        Console.Write($"{value, -5}");
      }
      Console.WriteLine();
    }
  }
}