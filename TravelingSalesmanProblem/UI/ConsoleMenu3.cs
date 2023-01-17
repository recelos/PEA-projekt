using System;
using System.Collections.Generic;
using System.IO;
using TravelingSalesmanProblem.Algorithms;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.UI;

public class ConsoleMenu3
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

    // Console.Write("Podaj czas dzialania algorytmu: ");
    //
    // var time = double.Parse(Console.ReadLine());
    //
    // Console.Write("Podaj wspolczynnik krosowania: ");
    //
    // var crossRate = double.Parse(Console.ReadLine());
    //
    // Console.Write("Podaj wspolczynnik mutacji: ");
    //
    // var mutationRate = double.Parse(Console.ReadLine());
    
    
    if (success && files.IsInRange(chosenFile - 1))
    {
      var graph = GraphFactory.ReadAtspFile(files[chosenFile - 1]);

      if (graph.Size < 50) graph.Print();

      (int, List<int>) results;


      results = new Genetic(graph, 30, 1, 1).Solve(0);
      
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