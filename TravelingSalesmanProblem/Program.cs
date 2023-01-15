using System;
using TravelingSalesmanProblem.UI;

namespace TravelingSalesmanProblem;

public static class Program
{
  public static void Main()
  {
    while (true)
    {
      new ConsoleMenu3().Show();
      Console.Clear();
    }
  }
}