namespace DataStructures;

public class Graph
{
  public int[][] AdjacencyMatrix { get; }

  public int Size => AdjacencyMatrix.Length;
  
  public Graph(int[][] adjMatrix)
  {
    AdjacencyMatrix = adjMatrix;
  }

  public Graph(string fileName)
  {
    var fileContent = File.ReadAllLines(fileName);

    // wyczytanie rozmiaru grafu z pierwszej linii
    var size = int.Parse(fileContent[0]);

    AdjacencyMatrix = new int[size][];
    
    for (var i = 1; i <= size; i++)
    {
      AdjacencyMatrix[i - 1] = new int[size];

      var line = fileContent[i]
        .Trim(null)
        .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
      
      for (var j = 0; j < size; j++)
      {
        AdjacencyMatrix[i - 1][j] = int.Parse(line[j]);
      }
    }
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