namespace DataStructures;

public static class GraphFactory
{
  public static Graph Generate(
    int range,
    int size)
  {
    var adjMatrix = GetRandomMatrix(range, size);
    return new Graph(adjMatrix);
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

    // wypelnianie losowymi wartosciami z podanego zakresu
    for (var i = 1; i < size; i++)
    {
      for (var j = 0; j < i; j++)
      {
        var value = random.Next(1, range);
        adjMatrix[i][j] = value;
        adjMatrix[j][i] = value;
      }
    }

    return adjMatrix;
  }
}