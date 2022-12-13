using System;
using System.Collections.Generic;
using System.IO;
using TravelingSalesmanProblem.Algorithms.TabuSearch;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.UI;

public class ConsoleMenu2
{
  private const string GraphDirectory = "tsgraphs";
  
  public void Show()
  {
    Console.WriteLine("Wybierz rodzaj operacji:");
    Console.WriteLine("1. Wczytaj graf z pliku");
    Console.WriteLine("2. Zakoncz program");

    var x = int.Parse(Console.ReadLine() ?? string.Empty);
    
    switch (x)
    {
      case 1:
        ReadFromFile();
        break;
      case 2:
        Environment.Exit(0);
        break;
      default:
        return;
    }
  }
  
  private void ReadFromFile()
  {
    var files = Directory.GetFiles(GraphDirectory);

    Console.WriteLine("Wybierz plik:");
    var i = 1;
    foreach (var file in files)
    {
      Console.WriteLine($"{i++}. {file}");
    }

    var success = int.TryParse(Console.ReadLine(), out var chosenFile); 
    
    if (success && files.IsInRange(chosenFile - 1))
    {
      Console.WriteLine("Wybierz definicje sasiedztwa: ");
      Console.WriteLine("1. Swap");
      Console.WriteLine("2. Insert");
      Console.WriteLine("3. Inverse");

      
      var success2 = int.TryParse(Console.ReadLine(), out var algorithm);
      if (!success2) return;

      Console.Write("Podaj czas dzialania algorytmu: ");

      var success3 = double.TryParse(Console.ReadLine(), out var time);
      if (!success3) return;
      
      var graph = GraphFactory.ReadAtspFile(files[chosenFile - 1]);

      if (graph.Size < 50) graph.Print();

      (int, List<int>) results;
      
      switch (algorithm)
      {
        case 1:
          results = new TabuSearchSwap(graph, time).Solve(0);
          break; 
        case 2:
          results = new TabuSearchInsert(graph, time).Solve(0);
          break; 
        case 3:
          results = new TabuSearchInvert(graph, time).Solve(0);
          break;
        default: 
          return;
      }

      Console.WriteLine($"\nNajkrotsza droga: {results.Item1}\n");
      Console.WriteLine($"Droga: {results.Item2.CombineToString()}");
      Console.WriteLine("\nWcisnij dowolny klawisz by kontynuowac...");
      Console.ReadKey();
    }
    else
    {
      Console.WriteLine("Podany plik nie istnieje");
      Console.ReadKey();
    }
  }
}