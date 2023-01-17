﻿using System;
using System.Collections.Generic;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms.Genetic;

public class GeneticSwapMutation : Genetic
{
  public GeneticSwapMutation(Graph graph, long time, double crossRate, double mutationRate, int populationSize) 
    : base(graph, time, crossRate, mutationRate, populationSize)
  {
  }

  protected override void Mutate(List<int> path, Random rand)
  {
    var lowerIndex = rand.Next(path.Count);
    var higherIndex = rand.Next(lowerIndex, path.Count);
    
    path.Swap(lowerIndex, higherIndex);
  }
}