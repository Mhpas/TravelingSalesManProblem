using System;
using System.Diagnostics;
using System.Threading;
using TravelingSalesManProblem.Algorithms;
using TravelingSalesManProblem.Helper;
using TravelingSalesManProblem.Model;

namespace TravelingSalesManProblem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Graph graph = GraphFactory.CreateGraph();
            //Initalizing Algorithms
            IAlgorithm nearestNeighbour = new NearestNeighbour();
            IAlgorithm christofides = new Christofides();

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Running NearestNeighbour...");
            stopwatch.Start();
            Graph route = nearestNeighbour.Run(graph, graph.Nodes[0]);
            stopwatch.Stop();
            Console.WriteLine("----------------Nearest Neighbour----------------");
            Console.WriteLine("Dauer: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
            Console.WriteLine("Route: " + route.ToString());
            Console.WriteLine("Routen Länge: ca. " + CalculateRouteLength(route) + " km");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");
            stopwatch.Reset();

            Console.WriteLine("Running Christofides...");
            stopwatch.Start();
            route = christofides.Run(graph, graph.Nodes[0]);
            stopwatch.Stop();
            Console.WriteLine("-------------------Christofides------------------");
            Console.WriteLine("Dauer: " + stopwatch.Elapsed.TotalMilliseconds + " ms");
            Console.WriteLine("Route: " + route.ToString());
            Console.WriteLine("Routen Länge: ca. " + CalculateRouteLength(route) + " km");
            Console.WriteLine("-------------------------------------------------");
        }

        private static int CalculateRouteLength(Graph route)
        {
            int sum = 0;
            foreach(Edge edge in route.Edges)
            {
                sum += edge.Value;
            }
            return sum;
        }
    }
}
