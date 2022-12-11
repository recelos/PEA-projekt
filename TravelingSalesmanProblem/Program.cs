using System;
using TravelingSalesmanProblem.UI;

namespace TravelingSalesmanProblem;

public static class Program
{
  public static void Main()
  {
    while (true)
    {
      new ConsoleMenu2().Show();
      Console.Clear();
    }
  }
}