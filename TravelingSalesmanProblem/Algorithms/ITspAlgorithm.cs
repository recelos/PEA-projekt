using System.Collections.Generic;

namespace TravelingSalesmanProblem.Algorithms;

public interface ITspAlgorithm
{
    (int, List<int>) Solve(int start);
}