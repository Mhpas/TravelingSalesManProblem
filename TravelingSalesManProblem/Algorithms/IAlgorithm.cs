
using TravelingSalesManProblem.Model;

namespace TravelingSalesManProblem
{
    public interface IAlgorithm
    {
        Graph Run(Graph input, Node StartPoint);
    }
}
