using DataStructures;
using Microsoft.VisualBasic.FileIO;
using TravelingSalesmanProblem.Algorithms;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem;

public class ConsoleMenu
{
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
    var files = FileSystem.GetFiles("graphs");

    Console.WriteLine("Wybierz plik:");
    var i = 1;
    foreach (var file in files)
    {
      Console.WriteLine($"{i++}. {file}");
    }

    var success = int.TryParse(Console.ReadLine(), out var chosenFile); 
    
    if (success && files.IsInRange(chosenFile - 1))
    {
      var graph = new Graph(files[chosenFile - 1]);
      graph.Print();
      
      var algo = new BruteForce();
      
      var (xd, xdd) = algo.Solve(graph, 0);

      Console.WriteLine($"\nNajkrotsza droga: {xd}\n");
      Console.WriteLine($"Droga: {xdd.CombineToString()}");
      
      
      var results = Benchmark.Measure(algo, graph, 10);

      PrintAndWait($"Sredni czas: {results.Average()}ms.");
    }
    else
    {
      Console.WriteLine("Podany plik nie istnieje");
      Console.ReadLine();
    }
  }
  
  private void GenerateRandom()
  {
    Console.WriteLine("Podaj rozmiar grafu:");

    var success = int.TryParse(Console.ReadLine(), out var size);
    
    if (success && size is > 0 and < 21)
    {
      var graph = GraphFactory.Generate(size, size);
      graph.Print();

      var results = Benchmark.Measure(new BruteForce(), graph, 10);

      PrintAndWait($"\nSredni czas: {results.Average()}ms.");
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
    Console.ReadLine();
  }
}

