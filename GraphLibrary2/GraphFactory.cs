using GraphLibrary.AdjacencyList;
using System.Collections.Generic;

namespace GraphLibrary
{
    public class GraphFactory
    {
        /// <summary>
        /// Creates a graph of the requested type
        /// </summary>
        /// <param name="nodesId">Set with all the known nodes Ids</param>
        /// <returns>Instance of a Graph</returns>
  
        
        public static Graph CreateGraph(HashSet<string> nodesId)
        {
            return new GraphList(nodesId);
        }
    }
}
