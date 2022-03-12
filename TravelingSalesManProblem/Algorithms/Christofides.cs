
using System;
using System.Collections.Generic;
using System.Linq;
using TravelingSalesManProblem.Helper;
using TravelingSalesManProblem.Model;

namespace TravelingSalesManProblem.Algorithms
{
    public class Christofides : IAlgorithm
    {
        public Graph Run(Graph input, Node StartPoint)
        {
            Console.WriteLine("");
            Console.WriteLine("Minimal Spannender Baum wird erzeugt");
            //Step 1: Create a Minimum Spanning tree
            Graph minimumSpanningTree = TreeFactory.GetMinimumSpanningTree(input);
            Console.WriteLine("");
            Console.WriteLine("Minimal Spannender Baum - Fertig");
            //Step 1.5: Get Nodes with an odd number of edges connecting to it
            Graph oddDegreeNodes = GetOddDegreeVertices(minimumSpanningTree);
            //Step 2: Get Perfect Matching and add to Minimum Spanning Tree
            Graph perfectMatching = GetPerfectMinimumMatching(oddDegreeNodes, input);
            Console.WriteLine("Matching - Fertig");
            //Combine Minimum Spanning Tree and Perfect Matching
            Graph combinedGraph = Combine(minimumSpanningTree, perfectMatching); //The graph is ensured to be euclidean now with every node having a even degree so we can construct a tour
            Console.WriteLine("Kombinieren von Baum und Matching - Fertig");
            //Step 4: Construct Euler Tour
            Graph eulerTour = GetEulerTour(combinedGraph, StartPoint);
            Console.WriteLine("Euler Tour - Fertig");
            //Step 5: Shorten Route using Hammilton Circuit (Cut off double Nodes)
            Graph Route = ShortenRoute(eulerTour, input, StartPoint);
            Console.WriteLine("Abkürzungen - Fertig");
            Console.WriteLine("");
            return Route;
        }

        private Graph ShortenRoute(Graph eulerTour, Graph input, Node startPoint)
        {
            Graph HamiltonCircuit = new Graph();
            Node currentPoint = startPoint;
            while(eulerTour.Nodes.Count > 1)
            {
                Edge currentEdge = eulerTour.Edges[0];
                if (HamiltonCircuit.Contains(currentEdge.Destination))
                {
                    Edge nextEdge = eulerTour.Edges[1];
                    int value = input.Edges.Find(x => x.Origin.Equals(currentPoint) && x.Destination.Equals(nextEdge.Destination)).Value;
                    Edge directConnection = new Edge { Origin = currentPoint, Destination = nextEdge.Destination, Value = value };
                    HamiltonCircuit.AddEdge(directConnection);
                    eulerTour.Edges.Remove(currentEdge);
                    eulerTour.Edges.Remove(nextEdge);
                    currentPoint = nextEdge.Destination;
                    eulerTour.Nodes.Remove(currentPoint);
                }
                else
                {
                    HamiltonCircuit.AddEdge(currentEdge);
                    eulerTour.Edges.Remove(currentEdge);
                    currentPoint = currentEdge.Destination;
                    eulerTour.Nodes.Remove(currentPoint);
                }
            }

            //Return to StartPoint
            HamiltonCircuit.AddEdge(eulerTour.Edges[0]);
            return HamiltonCircuit;
        }

        /// <summary>
        /// Creates an Euler Tour using the Hierholzer Algorithm
        /// </summary>
        /// <param name="combinedGraph"></param>
        /// <param name="StartPoint"></param>
        /// <returns></returns>
        private Graph GetEulerTour(Graph combinedGraph, Node StartPoint)
        {
            Graph Tour = new Graph();
            Graph SubTour = new Graph();
            Node currentPoint = StartPoint;
            Node tourStart = StartPoint;
            while (combinedGraph.Edges.Count > 0)
            {
                List<Edge> possibleEdges = combinedGraph.Edges.Where(x => x.Origin.Equals(currentPoint) && !SubTour.Contains(x)).ToList();
                Edge tourEdge = possibleEdges.OrderBy(x => x.Value).First();

                SubTour.AddEdge(tourEdge);
                currentPoint = tourEdge.Destination;

                //Delete Edge from possibilities
                Edge opposite = combinedGraph.Edges.Where(x => x.Destination.Equals(tourEdge.Origin) && x.Origin.Equals(tourEdge.Destination)).FirstOrDefault();
                if (opposite is not null) combinedGraph.Edges.Remove(opposite);
                combinedGraph.Edges.Remove(tourEdge);

                //Sub Tour completed? 
                if (tourEdge.Destination.Equals(tourStart))
                {
                    Integrate(Tour, SubTour);
                    SubTour = new Graph();
                    //If there is still a point with available edges set it as new start point
                    if (combinedGraph.Edges.Count > 0)
                    {
                        tourStart = combinedGraph.Edges[0].Origin;
                        currentPoint = combinedGraph.Edges[0].Origin;
                    }
                }
            }

            return Tour;
        }

        private void Integrate(Graph Tour, Graph SubTour)
        {
            int index = 0;
            if (Tour.Edges.Count > 0)
            {
                Edge tourEnd = Tour.Edges.Find(x => x.Destination.Equals(SubTour.Nodes[0]));
                index = Tour.Edges.IndexOf(tourEnd)+1;
            }

            foreach(Edge edge in SubTour.Edges)
            {
                Tour.Edges.Insert(index,edge);
                Tour.AddNode(edge.Destination);
                Tour.AddNode(edge.Origin);
                index++;
            }
        }
        private Graph GetOddDegreeVertices(Graph input)
        {
            Graph oddNodes = new Graph();
            foreach(Node node in input.Nodes)
            {
                //Degree is the number of edges that connect to the node
                int degree = input.Edges.Where(e => e.Origin.Name.Equals(node.Name)).Count();
                if (degree % 2 == 1) oddNodes.Nodes.Add(node);
            }
            return oddNodes;
        }

        private Graph Combine(Graph a, Graph b)
        {
            Graph output = new Graph();
            foreach(Edge edge in a.Edges)
            {
                output.AddEdge(edge);
            }

            foreach (Edge edge in b.Edges)
            {
                output.AddEdge(edge);
            }

            return output;
        }

        private Graph GetPerfectMinimumMatching(Graph oddNodes, Graph input)
        {
            Graph perfectMatching = new Graph();
            while(oddNodes.Nodes.Count > 0)
            {
                Edge nearestMatchEdge = GetNearestNeighbour(oddNodes, input, oddNodes.Nodes[0]);
                //Get Opposite Edge
                Edge oppositeEgde = input.Edges.Find(e => e.Destination.Equals(nearestMatchEdge.Origin) && e.Origin.Equals(nearestMatchEdge.Destination));
                perfectMatching.AddEdge(nearestMatchEdge);
                perfectMatching.AddEdge(oppositeEgde);

                //Remove Matched Nodes from List
                oddNodes.Nodes.Remove(nearestMatchEdge.Origin);
                oddNodes.Nodes.Remove(nearestMatchEdge.Destination);
            }
            return perfectMatching;
        }

        private Edge GetNearestNeighbour(Graph Nodes, Graph input, Node node)
        {
            Edge nearestConnectingEdge = new Edge { Value = int.MaxValue };
            List<Edge> checkableEdges = input.Edges.Where(x => x.Origin.Name.Equals(node.Name) && Nodes.Contains(x.Destination)).ToList();
            foreach(Edge edge in checkableEdges)
            {
                if (edge.Value < nearestConnectingEdge.Value) nearestConnectingEdge = edge;
            }

            return nearestConnectingEdge;
        }
    }
}
