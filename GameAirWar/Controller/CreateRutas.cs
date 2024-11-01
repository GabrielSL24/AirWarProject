using GraphLibrary;
using System;
using System.Collections.Generic;
using Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace GameAirWar.Controller
{
    //Clase para crear las rutas
    internal class CreateRutas
    {
        Random rand = new Random();
        internal HashSet<string> Ocean;
        internal HashSet<string> Land;
        private CreateWeight createWeight;
        private int weight;

        //Constructor de la clase
        internal CreateRutas(HashSet<string> ocean, HashSet<string> land)
        {
            this.Land = land;
            this.Ocean = ocean;
        }

        //Función que genera rutas aleatorias entre nodos en el grafo y las agrega con un peso
        internal void GetMapRutas(Graph graph, List<KeyValuePair<string, object>> ListRutas)
        {
            for (int i = 0; i < 25; i++)
            {
                //Obtiene dos nodos aleatorios del grafo
                var randomNode = graph.GetRandomNode();
                var randomNode2 = graph.GetRandomNode();
                //Si ambos nodos son iguales termina la iteración
                if (randomNode.Equals(randomNode2))
                    return;

                if (randomNode != null && randomNode2 != null)
                {
                    //Obtiene las claves y valores de los nodos aleatorios
                    string randomKey = randomNode.Value.Key;
                    string randomKey2 = randomNode2.Value.Key;
                    var randomValue = randomNode.Value.Value;
                    var randomValue2 = randomNode2.Value.Value;

                    //Verifica los tipos de ubicación de los nodos para calcular el peso adecuado
                    (LocationType, LocationType) location = Verify(Ocean, Land, randomKey, randomKey2);

                    //Crea el peso basandose en donde se ubican los nodos
                    createWeight = new CreateWeight(location);

                    //Agrega una rista al grafo entre los nodos con el peso calculado
                    graph.AddEdge(randomKey, randomKey2, createWeight.weight);
                }
                else
                {
                    Console.WriteLine("EL diccionario de nodos está vacío");
                }
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
                return (LocationType.Ocean, LocationType.Ocean);    //"Both in Ocean"
            }
            else if (nodeInLand && nodeInLand2)
            {
                return (LocationType.Land, LocationType.Land);    //"Both in Land"
            }
            else if (nodeInLand && nodeInOcean2)
            {
                return (LocationType.Land, LocationType.Ocean);    //"One in Land, One in Ocean"
            }
            else if (nodeInOcean && nodeInLand2)
            {
                return (LocationType.Ocean, LocationType.Land);     //"One in Ocean, One in Land"
            }
            return (LocationType.Error, LocationType.Error);
        }
    }
}
