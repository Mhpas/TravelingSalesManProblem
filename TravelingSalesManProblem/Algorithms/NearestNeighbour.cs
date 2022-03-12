
using System.Collections.Generic;
using System.Linq;
using TravelingSalesManProblem.Model;

namespace TravelingSalesManProblem.Algorithms
{
    public class NearestNeighbour : IAlgorithm
    {

        public Graph Run(Graph input, Node startPoint)
        {
            Graph route = new Graph();
            route = Solve(route, input, startPoint);

            //Return to Origin
            Edge finishingEdge = input.Edges.Find(x => x.Origin.Name.Equals(route.Nodes.Last().Name) && x.Destination.Name.Equals(startPoint.Name));
            route.AddEdge(finishingEdge);

            return route;
        }

        private Graph Solve(Graph route, Graph inputGraph, Node Node)
        {
            Edge shortestEdge = new Edge { Value = int.MaxValue };
            List<Edge> checkableEdges = inputGraph.Edges.Where(x => x.Origin.Name.Equals(Node.Name) && !route.Contains(x.Destination)).ToList();

            if(checkableEdges.Count < 1)
            {
                //All Edges in Route no more Edges to add
                return route;
            }

            foreach(Edge edge in checkableEdges)
            {
                if(edge.Value < shortestEdge.Value)
                {
                    shortestEdge = edge;
                }
            }

            route.AddEdge(shortestEdge);
            return Solve(route, inputGraph, shortestEdge.Destination);
        }

    }
}
