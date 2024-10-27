namespace GraphLibrary.AdjacencyList
{
    //Clase de estructura que debe de tener cuando se realiza una conexión entre dos nodos, osea una arista
    internal class Edge
    {
        public int Weight { get; set; } = int.MaxValue;
        public string SourceId { get; set; } = null;
        public string TargetId { get; set; } = null;

        public Edge(string sourceId, string targetId, int weight)
        {
            SourceId = sourceId;
            TargetId = targetId;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"{TargetId}({Weight})";
        }
    }
}
