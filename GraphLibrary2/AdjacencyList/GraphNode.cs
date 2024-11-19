using System.Collections.Generic;

namespace GraphLibrary.AdjacencyList
{
    // Clase para la estructura de un nodo
    public class GraphNode
    {
        internal string Id {  get; set; }

        internal List<Edge> edges { get; } = new List<Edge>();
        public List<IAvion> IdAviones { get; set; } = new List<IAvion>();
        public int cantAviones { get; set; } = 10;
        public int cantGasolina { get; set; } = 50;

        internal GraphNode(string id)
        {
            Id = id;
        }
    }
}
