using DataStructures;

namespace TravelingSalesmanProblem.Algorithms;

public interface ITspAlgorithm
{
    (int, List<int>) Solve(Graph graph, int start);
}