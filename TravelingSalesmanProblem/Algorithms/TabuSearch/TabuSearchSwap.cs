using System.Collections.Generic;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms.TabuSearch;

public class TabuSearchSwap : TabuSearch
{
  public TabuSearchSwap(Graph graph, double time) : base(graph, time)
  { }

  protected override void GetNeighbour(IList<int> input, int i, int j)
    => input.Swap(i, j);
}