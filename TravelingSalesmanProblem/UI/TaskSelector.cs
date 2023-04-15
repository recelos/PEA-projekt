using System;
using System.Linq;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.UI;

public class TaskSelector
{
  public void Run()
  {
    Console.WriteLine("1. Algorytmy dokladne"); 
    Console.WriteLine("2. Algorytmy z ograniczeniamy");
    Console.WriteLine("3. Algorytm genetyczy");
    Console.WriteLine("Wybierz zadanie: ");

    var success = int.TryParse(Console.ReadLine(), out var input);

    if (!success && !Enumerable.Range(1,3).ToList().IsInRange(input))
    {
      Console.WriteLine("Zly wybor");
      return;
    }
    
    Console.Clear();
    
    switch (input)
    {
      case 1:
        while (true)
        {
          new Task1Cli().Show();
          Console.Clear();
        }
      case 2:
        while (true)
        {
          new Task2Cli().Show();
          Console.Clear();
        }
      case 3:
        while (true)
        {
          new Task3Cli().Show();
          Console.Clear();
        }
      default:
        Console.WriteLine("Zly wybor");
        break;
    }
  }
}