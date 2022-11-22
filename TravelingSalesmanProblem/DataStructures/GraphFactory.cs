using System;
using System.IO;

namespace DataStructures;

public static class GraphFactory
{
  public static Graph GenerateRandom(
    int range,
    int size)
  {
    var adjMatrix = GetRandomMatrix(range, size);
    return new Graph(adjMatrix);
  }
  
  public static Graph ReadFromFile(string filename)
  {
    var fileContent = File.ReadAllLines(filename);
    
    // wyczytanie rozmiaru grafu z pierwszej linii
    var size = int.Parse(fileContent[0]);

    var matrix = new int[size][];
    
    for (var i = 1; i <= size; i++)
    {
      matrix[i - 1] = new int[size];

      var line = fileContent[i]
        .Trim(null)
        .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
      
      for (var j = 0; j < size; j++)
      {
        matrix[i - 1][j] = int.Parse(line[j]);
      }
    }

    return new Graph(matrix);
  }
  
  
  private static int[][] GetRandomMatrix(int range, int size)
  {
    var random = new Random();

    //inicjalizacja macierzy
    var adjMatrix = new int[size][];

    for (var i = 0; i < size; i++)
    {
      adjMatrix[i] = new int[size];
    }

    // wypelnij przekatne wartosciami -1
    for (var i = 0; i < size; i++)
    {
      adjMatrix[i][i] = -1;
    }
    
    for (var i = 0; i < size; i++)
    {
      for (var j = 0; j < size; j++)
      {
        if (i != j)
        {
          var value = random.Next(1, range);
          adjMatrix[i][j] = value;
        }
      }
    }
    
    return adjMatrix;
  }
}