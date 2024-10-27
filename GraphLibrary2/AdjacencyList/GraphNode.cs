using System.Collections.Generic;

namespace GraphLibrary.AdjacencyList
{
    //Clase para la estructura de un nodo
    internal class GraphNode
    {
        internal string Id {  get; set; }

        internal List<Edge> edges { get; } = new List<Edge>();

        internal GraphNode(string id)
        {
            Id = id;
        }
    }
}
