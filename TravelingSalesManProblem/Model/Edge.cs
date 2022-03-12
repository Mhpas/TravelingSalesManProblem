namespace TravelingSalesManProblem.Model
{
    public class Edge
    {
        public Node Origin { get; set; }
        public Node Destination { get; set; }
        public int Value { get; set; }

        public bool Equals(Edge edge)
        {
            if (Origin.Name.Equals(edge.Origin.Name) && Destination.Name.Equals(edge.Destination.Name) && Value == edge.Value) return true;
            return false;
        }
    }
}
