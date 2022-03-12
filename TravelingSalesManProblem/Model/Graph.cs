using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingSalesManProblem.Model
{
    public class Graph
    {
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }

        public Graph()
        {
            Nodes = new List<Node>();
            Edges = new List<Edge>();
        }

        /// <summary>
        /// Adds a new edge to the Graph.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="value"></param>
        public void AddEdge(string origin, string destination, int value)
        {
            try
            {
                Node originNode = FindOrCreateNode(origin);
                Node destinationNode = FindOrCreateNode(destination);
                Edges.Add(new Edge() { Origin = originNode, Destination = destinationNode, Value = value });
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add edge.", e);
            }

        }

        public void AddNode(Node node)
        {
            FindOrCreateNode(node);
        }

        /// <summary>
        /// Adds a new edge to the graph.
        /// </summary>
        /// <param name="edge"></param>
        public void AddEdge(Edge edge)
        {
            try
            {
                Node originNode = FindOrCreateNode(edge.Origin);
                Node destinationNode = FindOrCreateNode(edge.Destination);
                edge.Origin = originNode;
                edge.Destination = destinationNode;
                Edges.Add(edge);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to add an edge via edge input.", e);
            }
        }

        /// <summary>
        /// Searches for a simular node in the nodes list. If it doesn't exist yet, it creates a new one
        /// </summary>
        /// <param name="inputNode"></param>
        /// <returns></returns>
        private Node FindOrCreateNode(Node inputNode)
        {
            try
            {
                Node node = Nodes.FirstOrDefault(n => n.Name.Equals(inputNode.Name));
                if (node == null)
                {
                    Nodes.Add(inputNode);
                    return inputNode;
                }
                return node;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to find/create node via node input.", e);
                return null;
            }
        }

        /// <summary>
        /// Searches for a node with the given name in the nodes list. If it doesn't exist yet, it creates a new one
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Node FindOrCreateNode(string name)
        {
            try
            {
                Node node = Nodes.FirstOrDefault(n => n.Name.Equals(name));
                if (node == null)
                {
                    node = new Node() { Name = name };
                    Nodes.Add(node);
                }
                return node;
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to find/create node.", e);
                return null;
            }
        }

        /// <summary>
        /// Checks if a node with the same values is part of the graph.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Contains(Node node)
        {
            if (Nodes.Where(n => n.Name.Equals(node.Name)).ToList().Count > 0) return true;
            return false;
        }

        /// <summary>
        /// Checks if an edge with the same values is part of the graph.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public bool Contains(Edge edge)
        {
            if (Edges.Find(e => e.Equals(edge)) != null) return true;
            return false;
        }

        public override string ToString()
        {
            if (Edges.Count < 1) return "";
            string str = Edges[0].Origin.Name;
            foreach (Edge edge in Edges)
            {
                str +=  " -> " + edge.Destination.Name;
            }
            return str;
        }
    }
}
