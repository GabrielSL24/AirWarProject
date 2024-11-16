using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.AdjacencyList
{
    //Clase para crear los Grafos
    public class GraphList : Graph
    {
        private Dictionary<string, GraphNode> nodes;
        private Random rand = new Random();

        //Constructor que incializa el grafo con un conjunto de nodos 
        public GraphList(HashSet<string> nodes)
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

        // Método para calcular la ruta más corta usando Dijkstra
        public (int distance, List<string> path) Dijkstra(string startId, string targetId)
        {
            if (!nodes.ContainsKey(startId) || !nodes.ContainsKey(targetId))
            {
                throw new ArgumentException("Nodos no válidos.");
            }

            var distances = nodes.ToDictionary(n => n.Key, n => int.MaxValue);
            var previous = new Dictionary<string, string>();
            var priorityQueue = new SortedSet<(int distance, string node)>();

            distances[startId] = 0;
            priorityQueue.Add((0, startId));

            while (priorityQueue.Any())
            {
                var current = priorityQueue.First();
                priorityQueue.Remove(current);

                string currentNodeId = current.node;

                if (currentNodeId == targetId)
                {
                    break;
                }

                foreach (var edge in nodes[currentNodeId].edges)
                {
                    int newDist = distances[currentNodeId] + edge.Weight;
                    if (newDist < distances[edge.TargetId])
                    {
                        priorityQueue.Remove((distances[edge.TargetId], edge.TargetId));
                        distances[edge.TargetId] = newDist;
                        previous[edge.TargetId] = currentNodeId;
                        priorityQueue.Add((newDist, edge.TargetId));
                    }
                }
            }

            // Reconstruir la ruta
            var path = new List<string>();
            string currentNode = targetId;
            while (previous.ContainsKey(currentNode))
            {
                path.Insert(0, currentNode);
                currentNode = previous[currentNode];
            }

            if (path.Any())
            {
                path.Insert(0, startId);
            }

            return (distances[targetId], path);
        }

        /* ----------- verificcaciones adicionales para mejorar el cálculo de las rutas ---------------*/
        public bool HasEdge(string sourceId, string targetId)
        {
            if (nodes.TryGetValue(sourceId, out GraphNode sourceNode))
            {
                // Verificar si el nodo fuente tiene una arista hacia el nodo destino
                return sourceNode.edges.Exists(edge => edge.TargetId == targetId);
            }
            return false;
        }

        public List<string> GetNeighbors(string nodeId)
        {
            if (nodes.TryGetValue(nodeId, out GraphNode node))
            {
                // Retornar los IDs de los nodos vecinos
                return node.edges.ConvertAll(edge => edge.TargetId);
            }
            return new List<string>();
        }

    }
}
