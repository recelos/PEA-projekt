using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelingSalesmanProblem.Algorithms;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.UI;

public class ConsoleMenu1
{
  private const string GraphDirectory = "graphs";
  
  public void Show()
  {
    Console.WriteLine("Wybierz rodzaj operacji:");
    Console.WriteLine("1. Wczytaj graf z pliku");
    Console.WriteLine("2. Wygeneruj losowy graf");
    Console.WriteLine("3. Zakoncz program");

    var x = int.Parse(Console.ReadLine() ?? string.Empty);
    
    switch (x)
    {
      case 1:
        ReadFromFile();
        break;
      case 2:
        GenerateRandom();
        break;
      case 3:
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
      Console.WriteLine("Wybierz algorytm: ");
      Console.WriteLine("1. Brute force ");
      Console.WriteLine("2. Branch and bound ");

      var success2 = int.TryParse(Console.ReadLine(), out var algo);
      if (!success2) return;

      var graph = GraphFactory.ReadFromFile(files[chosenFile - 1]);
      graph.Print();

      (int, List<int>) results;
      
      switch (algo)
      {
        case 1:
          results = new BruteForce(graph).Solve(0);
          break; 
        case 2:
          results = new BranchAndBound(graph).Solve(0);
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
  
  private void GenerateRandom()
  {
    Console.WriteLine("Podaj rozmiar grafu:");

    var success = int.TryParse(Console.ReadLine(), out var size);
    
    if (success && size is > 0 and < 100)
    {
      Console.WriteLine("Wybierz algorytm: ");
      Console.WriteLine("1. Brute force ");
      Console.WriteLine("2. Branch and bound ");

      var success2 = int.TryParse(Console.ReadLine(), out var algo);
      if (!success2) return;
      
      List<long> results;

      switch (algo)
      {
        case 1:
          results = new Benchmark().Measure(1, 100, size).ToList();
          break;
        case 2:
          results = new Benchmark().Measure(2, 100, size).ToList();
          break;
        default:
          return;
      }
      PrintAndWait($"Sredni czas obliczen: {results.Average()}ms.");
    }
    else
    {
      PrintAndWait("Nie prawidlowy rozmiar grafu (musi byc mniejszy lub rowny 20 oraz wiekszy od 0)");
    }
  }

  private static void PrintAndWait(string text)
  {
    Console.WriteLine(text);
    Console.WriteLine("Wcisnij dowolny klawisz by kontynuowac");
    Console.ReadKey();
  }
}

