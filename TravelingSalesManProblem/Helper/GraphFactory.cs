

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TravelingSalesManProblem.Model;

namespace TravelingSalesManProblem.Helper
{
    public class GraphFactory
    {
        private const string FILEPATH = "GraphData.json";
        /// <summary>
        /// Creates Graph from Text file
        /// </summary>
        /// <returns></returns>
        public static Graph CreateGraph()
        {
            Graph graph = new Graph();
            List<Edge> edges = JsonConvert.DeserializeObject<List<Edge>>(File.ReadAllText(FILEPATH));
            foreach(Edge edge in edges)
            {
                graph.AddEdge(edge);
            }
            return graph;
        }

        public static Graph CreateGraph(Node node)
        {
            Graph graph = new Graph();
            graph.Nodes.Add(node);
            return graph;
        }

        public static Graph CreateRandomGraph(int size)
        {
            Graph graph = new Graph();
            Random random = new Random();
            for(int i = 0; i < size; i++)
            {
                Node node = new Node { Name = i.ToString() };
                graph.Nodes.Add(node);
            }

            foreach(Node node in graph.Nodes)
            {
                foreach (Node destination in graph.Nodes)
                {
                    if (node == destination) continue;
                    //Check if opposite Edge is already present
                    Edge edge = graph.Edges.Find(e => e.Destination.Equals(node) && e.Origin.Equals(destination));
                    if (edge is null) edge = new Edge { Origin = node, Destination = destination, Value = random.Next(1, 11) };
                    else edge = new Edge { Origin = node, Destination = destination, Value = edge.Value };

                    graph.AddEdge(edge);
                }
            }

            return graph;
        }

    }
}
