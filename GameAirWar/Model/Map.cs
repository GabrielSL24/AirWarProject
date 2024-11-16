using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphLibrary; // Espacio de nombres para manejar grafos
using GameAirWar.OperationsMap; // Para VerifyLocation
using GameAirWar.Controller;    // Para CreateRutas
using GraphLibrary.AdjacencyList;
using System.Linq;



namespace GameAirWar
{
    //Clase para crear y cargar el mapa
    public class Map
    {
        private Bitmap bitmap;
        private Random rand = new Random();
        private VerifyLocation Location;
        internal List<Point> PointsInLand = new List<Point>(); //Lista para las coordenadas en Tierra
        internal List<Point> PointsInOcean = new List<Point>(); //Lista para las coordenadas en Océano
        private List<List<Point>> PointsWorld = new List<List<Point>>(); //Lista para el total de coordenadas de Tierra y Océano (Aún no están haciendo nada pero sirva para un futuro o sino se elimina)
        internal HashSet<string> nodesInOcean = new HashSet<string>(); //HashSet para Portaaviones
        internal HashSet<string> nodesInLand = new HashSet<string>(); //HashSet para Aeropuertos
        internal HashSet<string> nodesInWorld = new HashSet<string>(); //HashSet para nodos de Aeropuertos y Portaaviones
        internal List<KeyValuePair<string, object>> ListRutas = new List<KeyValuePair<string, object>>();
        internal CreateRutas rutas;
        private Graph graph; // Nuevo campo para almacenar el grafo

        //Constructor de la clase
        internal Map()
        {
            CargarMap();
            Location = new VerifyLocation(bitmap);
        }

        //Función para cargar el mapa
        private void CargarMap()
        {
            string ruta = "View/Images/GameMap.png";  //Ruta estática para el mapa del mundo
            if (System.IO.File.Exists(ruta))
            {
                bitmap = new Bitmap(ruta); // Se crea el mapa de bits llamando al método GetMapBit
            }
            else
            {
                MessageBox.Show("La imagen no se encontró en la ruta especificada: " + System.IO.Path.GetFullPath(ruta));
            }
        }

        public Bitmap GetMapBit()//Se encarga de devolver un bitmap con la imagen que se le cargue.
        {
            return bitmap;
        }

        //Función para Generar las posiciones de aeropuertos y portaaviones
        public List<List<Point>> GenerarPosicionAleatoria()
        {
            int x, y; //Coordenadas

            //For para crear en Océano
            for (int i = 0; i < 8; i++)
            {
                do
                {
                    x = rand.Next(bitmap.Width);
                    y = rand.Next(bitmap.Height);
                } while (!Location.InOcean(x, y) || nodesInWorld.Contains((x, y).ToString()));

                PointsInOcean.Add(new Point(x, y));  //Coordenadas para mostrar en el mapa los Portaaviones
                nodesInOcean.Add((x, y).ToString());
                nodesInWorld.Add((x, y).ToString()); //Agrega las coordenadas (x, y) en el HashSet de los nodos de Portaaviones
            }

            //For para crear en Tierra
            for (int i = 0; i < 10; i++)
            {
                do
                {
                    x = rand.Next(bitmap.Width);
                    y = rand.Next(bitmap.Height);
                } while (!Location.InLand(x, y) || nodesInWorld.Contains((x, y).ToString()));

                PointsInLand.Add(new Point(x, y));  //Coordenadas para mostrar en el mapa los Aeropuertos
                nodesInLand.Add((x, y).ToString());
                nodesInWorld.Add((x, y).ToString());  //Agrega las coordenadas (x, y) en el HashSet de los nodos de Aeropuertos
            }

            graph = GraphFactory.CreateGraph(nodesInWorld);     //Crea de los HashSet los nodos de los grafos
            rutas = new CreateRutas(nodesInOcean, nodesInLand);
            rutas.GetMapRutas(graph, ListRutas); //Crear las rutas aleatorias de los nodos

            ValidarConexiones(graph); // Validar que los nodos tengan conexiones


                /*  Esto aún no hace nada pero sirva para un futuro o sino se elimina
                    Cantidad y cuenta de los puntos totales             **/
            PointsWorld.Add(PointsInOcean);
            PointsWorld.Add(PointsInLand);

            return PointsWorld;
        }


        public Graph GetGraph()
        {
            return graph;
        }

        public void ValidarConexiones(Graph graph)
        {
            var graphList = graph as GraphList;
            if (graphList == null)
            {
                Console.WriteLine("El grafo no es del tipo GraphList.");
                return;
            }

            foreach (var node in nodesInWorld)
            {
                if (!graphList.GetNeighbors(node).Any())
                {
                    Console.WriteLine($"Nodo aislado detectado: {node}");
                }
            }
        }

    }
}
