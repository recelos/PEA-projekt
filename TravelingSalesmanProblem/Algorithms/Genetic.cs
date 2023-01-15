using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms;

public class Genetic : ITspAlgorithm
{
  private readonly Random _rand;
  private readonly Graph _graph;

  private const int PopulationSize = 10;
  private const double CrossRate = 0.8;
  private const double MutationRate = 0.01;
  private int _bestSolution = int.MaxValue;
  private List<int> _bestPath;
  private readonly long _time;
  
  public Genetic(Graph graph, long time)
  {
    _rand = new Random();
    _graph = graph;
    _time = time * 1000;
  }

  public (int, List<int>) Solve(int start)
  {
    var tournamentSize = 5;
    int p1, p2, p3;

    var population = CreateFilledDoubleTab(PopulationSize, _graph.Size - 1);
    var nextPopulation = CreateFilledDoubleTab(PopulationSize, _graph.Size - 1);
    var permutation = CreateFilledTab(_graph.Size - 1);

    for (var i = 0; i < PopulationSize; i++)
    {
      population[i] = GenerateRandomPath();
    }

    var ratedPopulation = CreateFilledTab(PopulationSize);
    for (var i = 0; i < population.Length; i++)
    {
      ratedPopulation[i] = CalculatePathCost(population[i]);
    }

    if (ratedPopulation[FindIndexOfMinElementFromTab(ratedPopulation)] < _bestSolution)
    {
      SwapTheBest(ratedPopulation, population[FindIndexOfMinElementFromTab(ratedPopulation)]);
    }


    var timer = new Stopwatch();
    
    timer.Start();
    
    while (true)
    {
      //zegar
      ratedPopulation = CreateFilledTab(PopulationSize);


      
      // wpisanie do tablicy i ocena ścieżek
      for (int i = 0; i < population.Length; i++)
      {
        ratedPopulation[i] = CalculatePathCost(population[i]);
      }

      // Tworzenie nowej populacji na drodze selekcji
      for (var j = 0; j < PopulationSize; j++)
      {
        var result = int.MaxValue;

        // Organizacja turnieju
        for (var k = 0; k < tournamentSize; k++)
        {
          var index = _rand.Next(PopulationSize);
          if (ratedPopulation[index] < result)
          {
            result = ratedPopulation[index];
            permutation = (int[])population[index].Clone();
          }
        }

        nextPopulation[SelectLastUnfilled(nextPopulation)] = (int[]) permutation.Clone();
      }

      // Podmiana pokoleń
      population = nextPopulation;
      nextPopulation = CreateFilledDoubleTab(PopulationSize, _graph.Size - 1);


      var rotate = PopulationSize - (int)(CrossRate * (float)PopulationSize);
      rotate = _rand.Next(rotate);

      // Rozpatrywanie krzyżowania
      for (var j = rotate; j < (int)(CrossRate * (float)PopulationSize) + rotate; j += 2)
      {
        population[j] = MyOrderCrossover(population[j], population[j + 1]);
        population[j + 1] = MyOrderCrossover(population[j + 1], population[j]);
      }

      //Rozpatrywanie mutacji
      for (var j = 0; j < (int)(MutationRate * (float)PopulationSize) + 1; j++)
      {
        do
        {
          p1 = _rand.Next(_graph.Size - 1);
          p2 = _rand.Next(_graph.Size - 1);
          p3 = _rand.Next(PopulationSize);
        } while (p1 == p2);

        Swap(p1, p2, population[p3]);
      }

      for (var i = 0; i < population.Length; i++)
      {
        //liczenie na nowo wartości
        ratedPopulation[i] = CalculatePathCost(population[i]);
      }

      if (ratedPopulation[FindIndexOfMinElementFromTab(ratedPopulation)] < _bestSolution)
      {
        SwapTheBest(ratedPopulation, population[FindIndexOfMinElementFromTab(ratedPopulation)]);
      }


      if (timer.ElapsedMilliseconds > _time)
      {
        return (_bestSolution, _bestPath);
      }
    }
  }

  private void SwapTheBest(int[] ratedPopulation, int[] ints)
  {
    _bestSolution = ratedPopulation[FindIndexOfMinElementFromTab(ratedPopulation)];
    _bestPath = new List<int>(ints);
  }


  private int CalculatePathCost(int[] path)
  {
    var cost = 0;
    for (var i = 0; i < path.Length - 1; i++)
    {
      cost += _graph.AdjacencyMatrix[path[i]][path[i + 1]];
    }

    cost += _graph.AdjacencyMatrix[0][path[0]];
    cost += _graph.AdjacencyMatrix[path[path.Length - 1]][0];
    return cost;
  }

  private int[] GenerateRandomPath()
  {
    //generowanie losowej ścieżki
    int[] randomPath = new int[_graph.AdjacencyMatrix.Length - 1];
    for (int i = 0; i < _graph.AdjacencyMatrix.Length - 1; i++)
    {
      randomPath[i] = i + 1;
    }

    for (int i = 0; i < randomPath.Length; i++)
    {
      //funkcja losująca kolejność
      var randomIndexToSwap = _rand.Next(1, randomPath.Length); //wszystkie oprocz 0
      (randomPath[randomIndexToSwap], randomPath[i]) = (randomPath[i], randomPath[randomIndexToSwap]);
    }

    return randomPath;
  }

  private static int SelectLastUnfilled(int[][] tab)
  {
    for (var i = 0; i < tab.Length; i++)
    {
      if (tab[i][1] == -1)
      {
        return i;
      }
    }

    return -2;
  }

  private int[] CreateFilledTab(int tabSize)
  {
    var tab = new int[tabSize];
    tab.Fill(-1);
    return tab;
  }

  private int[][] CreateFilledDoubleTab(int populationSize, int graphSize)
  {
    var tab = new int[populationSize][];
    for (var i = 0; i < populationSize; i++)
    {
      tab[i] = CreateFilledTab(graphSize);
    }

    return tab;
  }

  private void Swap(int i, int j, int[] path)
  {
    (path[i], path[j]) = (path[j], path[i]);
  }


  private int FindIndexOfMinElementFromTab(int[] tab)
  {
    var smallest = int.MaxValue;
    var index = -1;
    for (int i = 0; i < tab.Length; i++)
    {
      if (tab[i] < smallest)
      {
        smallest = tab[i];
        index = i;
      }
    }

    return index;
  }

  private int[] MyOrderCrossover(int[] tab1, int[] tab2)
  {
    var startIndex = _rand.Next(_graph.Size - 2);
    var endIndex = _rand.Next(_graph.Size - 1);
    int actualChildIndex;
    if (startIndex > endIndex)
    {
      (endIndex, startIndex) = (startIndex, endIndex);
    }

    var child = new int[_graph.Size - 1];
    child.Fill(-1);

    for (var i = startIndex; i < endIndex + 1; i++)
    {
      child[i] = tab1[i];
    }

    actualChildIndex = endIndex + 1;
    if (actualChildIndex > tab1.Length - 1)
    {
      actualChildIndex = 0;
    }

    for (var i = endIndex + 1; i < tab2.Length; i++)
    {
      if (!IsElementInTab(child, tab2[i]))
      {
        child[actualChildIndex] = tab2[i];
        actualChildIndex++;
      }

      if (actualChildIndex > tab1.Length - 1)
      {
        actualChildIndex = 0;
      }
    }

    for (var i = 0; IsElementInTab(child, -1); i++)
    {
      if (!IsElementInTab(child, tab2[i]))
      {
        child[actualChildIndex] = tab2[i];
        actualChildIndex++;
        if (actualChildIndex > tab1.Length - 1)
        {
          actualChildIndex = 0;
        }
      }
    }
    return child;
  }

  private static bool IsElementInTab(int[] tab, int element)
  {
    return tab.Any(i => i == element);
  }
}