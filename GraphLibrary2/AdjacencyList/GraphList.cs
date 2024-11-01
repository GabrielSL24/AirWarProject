using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.AdjacencyList
{
    //Clase para crear los Grafos
    internal class GraphList : Graph
    {
        private Dictionary<string, GraphNode> nodes;
        private Random rand = new Random();

        //Constructor que incializa el grafo con un conjunto de nodos 
        internal GraphList(HashSet<string> nodes)
        {
            //Inicializa el diccionario con la capacidad del número de nodos
            this.nodes = new Dictionary<string, GraphNode>(nodes.Count);
            foreach (string node in nodes)
            {
                //Agrega un nuevo GraphNode al diccionario con su ID como clave
                this.nodes.Add(node, new GraphNode(node));
            }
        }

        //Método para agregar una arista entre dos nodos del grafo
        public void AddEdge(string sourceId, string targetId, int weight)
        {
            //Intenta obtener el nodo fuente (sorceNode) del diccionario usando su ID
            if (nodes.TryGetValue(sourceId, out GraphNode sourceNode))
            {
                sourceNode.edges.Add(new Edge(sourceId, targetId, weight));
            }
            else
            {
                throw new KeyNotFoundException($"No se encontró el nodo con ID: {sourceId}");
            }
        }

        //Función para obtener un nodo aleatoriamente
        public KeyValuePair<string, object>? GetRandomNode()
        {
            if (nodes.Count == 0)
                return null;
            
            int randomindex = rand.Next(nodes.Count);
            var randomNode = nodes.ElementAt(randomindex);
            return new KeyValuePair<string, object>(randomNode.Key, (object)randomNode.Value);
        }
    }
}
