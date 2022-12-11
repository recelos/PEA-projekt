﻿using System.Collections.Generic;
using TravelingSalesmanProblem.DataStructures;

namespace TravelingSalesmanProblem.Algorithms.TabuSearch;

public class TabuSearchInsert : TabuSearch
{
  public TabuSearchInsert(Graph graph, double time, bool diversification) : base(graph, time, diversification) { }
  protected override void GetNeighbour(IList<int> input, int i, int j)
  {
    var temp = input[i];
    input.RemoveAt(i);
    input.Insert(j, temp);
  }
}