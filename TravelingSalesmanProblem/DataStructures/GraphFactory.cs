using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TravelingSalesmanProblem.DataStructures;

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

  public static Graph ReadAtspFile(string filename)
  {
    var fileContent = File.ReadAllLines(filename).ToList();
    var size = int.Parse(fileContent[0]);
    var matrix = new int[size][];

    for (var i = 0; i < size; i++)
    {
      matrix[i] = new int[size];
    }

    fileContent.RemoveAt(0);
    
    var line = string
      .Join("", fileContent).Trim(null)
      .Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
    
    var counter = 0;
    var lineCounter = 0;
    foreach (var text in line)
    {
      if (counter == size)
      {
        counter = 0;
        lineCounter++;
      }
      if (!int.TryParse(text, out var value))
      {
        break;
      }
      matrix[counter][lineCounter] = counter != lineCounter ? value : -1;

      counter++;
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