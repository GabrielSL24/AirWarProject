namespace GraphLibrary
{
    public interface Graph
    {
        /// <summary>
        /// Adds an edge to the graph
        /// </summary>
        /// <param name="sourceNodeId">Starting node of the edge</param>
        /// <param name="targetNodeId">Ending node of the edge</param>
        /// <param name="weight">Weight of the edge</param>
        void AddEdge(string sourceNodeId, string targetNodeId, int weight);
    }
}
