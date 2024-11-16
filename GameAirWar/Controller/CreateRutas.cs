using GraphLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace GameAirWar.Controller
{
    // Clase para crear las rutas
    internal class CreateRutas
    {
        private Graph graph; // Campo para almacenar el grafo
        private Random rand = new Random();
        internal HashSet<string> Ocean;
        internal HashSet<string> Land;
        private CreateWeight createWeight;

        // Constructor de la clase
        internal CreateRutas(HashSet<string> ocean, HashSet<string> land)
        {
            this.Land = land;
            this.Ocean = ocean;

            // Inicializamos el grafo vacío
            this.graph = new GraphList(ocean.Union(land).ToHashSet());
        }

        // Función que genera rutas aleatorias entre nodos en el grafo y las agrega con un peso
        internal void GetMapRutas(Graph graph, List<KeyValuePair<string, object>> ListRutas)
        {
            var graphList = graph as GraphList;
            if (graphList == null)
            {
                Console.WriteLine("El grafo no es del tipo GraphList.");
                return;
            }

            for (int i = 0; i < 25; i++)
            {
                //Obtiene dos nodos aleatorios del grafo
                var randomNode = graphList.GetRandomNode();
                var randomNode2 = graphList.GetRandomNode();

                //Si ambos nodos son iguales termina la iteración
                if (randomNode == null || randomNode2 == null || randomNode.Equals(randomNode2))
                {
                    Console.WriteLine("Nodos inválidos o repetidos, saltando...");
                    continue;
                }

                //Obtiene las claves y valores de los nodos aleatorios
                string randomKey = randomNode.Value.Key;
                string randomKey2 = randomNode2.Value.Key;

                if (graphList.HasEdge(randomKey, randomKey2))
                {
                    Console.WriteLine($"Ya existe una ruta entre {randomKey} y {randomKey2}, saltando...");
                    continue;
                }

                //Crea el peso basándose en donde se ubican los nodos
                createWeight = new CreateWeight((LocationType.Land, LocationType.Land));
                //Agrega una Arista al grafo entre los nodos con el peso calculado
                graphList.AddEdge(randomKey, randomKey2, createWeight.weight);

                Console.WriteLine($"Ruta creada: {randomKey} -> {randomKey2} (Peso: {createWeight.weight})");
            }
        }

        //Función que verifica los tipos de ubicación de dos nodos de las listas
        private (LocationType, LocationType) Verify(HashSet<string> listOcean, HashSet<string> listLand, string node, string node2)
        {
            //Verifica si están en el océano o en tierra
            bool nodeInOcean = listOcean.Contains(node);
            bool nodeInOcean2 = listOcean.Contains(node2);
            bool nodeInLand = listLand.Contains(node);
            bool nodeInLand2 = listLand.Contains(node2);

            if (nodeInOcean && nodeInOcean2)
            {
                return (LocationType.Ocean, LocationType.Ocean); //"Both in Ocean"
            }
            else if (nodeInLand && nodeInLand2)
            {
                return (LocationType.Land, LocationType.Land);  //"Both in Land"
            }
            else if (nodeInLand && nodeInOcean2)
            {
                return (LocationType.Land, LocationType.Ocean); //"One in Land, One in Ocean"
            }
            else if (nodeInOcean && nodeInLand2)
            {
                return (LocationType.Ocean, LocationType.Land); //"One in Ocean, One in Land"
            }
            return (LocationType.Error, LocationType.Error);
        }

        // Método para devolver el grafo
        public Graph GetGraph()
        {
            return graph;
        }
    }
}
