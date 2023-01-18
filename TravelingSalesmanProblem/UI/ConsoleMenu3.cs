using System;
using System.Collections.Generic;
using System.IO;
using TravelingSalesmanProblem.Algorithms;
using TravelingSalesmanProblem.Algorithms.Genetic;
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

    
    Console.WriteLine("Wpisz rodzaj mutacji: ");
    Console.WriteLine("1. Transpozycja");
    Console.WriteLine("2. Odwracanie");
    var algo = int.Parse(Console.ReadLine());
    
    Console.Write("Podaj czas dzialania algorytmu: ");
    
    var time = int.Parse(Console.ReadLine());

    Console.Write("Podaj rozmiar populacji: ");

    var pop = int.Parse(Console.ReadLine());

    Console.Write("Podaj wspolczynnik mutacji: ");

    var mutRate = double.Parse(Console.ReadLine());

    Console.Write("Podaj wspolczynnik krzyzowania: ");

    var crossRate = double.Parse(Console.ReadLine());
    
    if (success && files.IsInRange(chosenFile - 1))
    {
      var graph = GraphFactory.ReadAtspFile(files[chosenFile - 1]);

      var results = algo switch
      {
        1 => new GeneticTranspositionMutation(graph, time, crossRate, mutRate, pop).Solve(0),
        2 => new GeneticInverseMutation(graph, time, crossRate, mutRate, pop).Solve(0),
        _ => new GeneticTranspositionMutation(graph, time, crossRate, mutRate, pop).Solve(0)
      };

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