

using System;
using System.Collections.Generic;
using System.Linq;
using TravelingSalesManProblem.Model;

namespace TravelingSalesManProblem.Helper
{
    public class TreeFactory
    {
        public static Graph GetMinimumSpanningTree(Graph graph)
        {
            try
            {
                Graph tree = GraphFactory.CreateGraph(graph.Nodes.FirstOrDefault());
                do
                {
                    Tuple<Edge, Edge> minEdgePair = GetMinimumEdgePair(tree, graph);
                    tree.AddEdge(minEdgePair.Item1);
                    tree.AddEdge(minEdgePair.Item2);
                } while (graph.Nodes.Count != tree.Nodes.Count);
                return tree;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to create a minimum spanning tree.", e);
                return null;
            }
        }

        private static Tuple<Edge, Edge> GetMinimumEdgePair(Graph tree, Graph graph)
        {
            try
            {
                List<Edge> availableEdges = graph.Edges.Where(e => tree.Contains(e.Origin) == true && tree.Contains(e.Destination) == false && tree.Contains(e) == false).ToList();
                Tuple<Edge, Edge> minEdgePair = null;
                foreach (Edge edge in availableEdges)
                {
                    Edge oppositeDirectingEdge = graph.Edges.FirstOrDefault(e => e.Origin.Name.Equals(edge.Destination.Name) && e.Destination.Name.Equals(edge.Origin.Name));
                    if (oppositeDirectingEdge == null) continue;
                    if (minEdgePair == null || minEdgePair.Item1.Value + minEdgePair.Item2.Value > edge.Value + oppositeDirectingEdge.Value) minEdgePair = new Tuple<Edge, Edge>(edge, oppositeDirectingEdge);
                }
                return minEdgePair;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to get minimum edge pair.", e);
                return null;
            }
        }
    }
}
