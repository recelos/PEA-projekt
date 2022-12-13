using System.Collections.Generic;
using TravelingSalesmanProblem.DataStructures;
using TravelingSalesmanProblem.Extensions;

namespace TravelingSalesmanProblem.Algorithms.TabuSearch;

public class TabuSearchInvert : TabuSearch
{
  public TabuSearchInvert(Graph graph, double time) : base(graph, time) { }
  protected override void GetNeighbour(IList<int> input, int i, int j)
    => input.ReverseSubList(i, j);
  
}