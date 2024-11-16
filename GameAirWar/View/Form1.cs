using GameAirWar.OperationForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using GameAirWar;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphLibrary.AdjacencyList;
using System.Runtime.InteropServices;



namespace GameAirWar
{
    public partial class Form1 : Form
    {
        private Bitmap oceanShip;
        private Bitmap landAiroport;
        private Random random = new Random(); // Generador de números aleatorios
        private Map map;
       
        private CreateLocation location;
        public Form1()
        {
            AllocConsole(); // Habilita la consola para poder ver el debug de os comentarios
            InitializeComponent(); //Se inicializa el componente gráfico

            map = new Map(); //Se crea un objeto del tipo Map
            location = new CreateLocation(map);
            MapPictureBox.Image = map.GetMapBit();

            oceanShip = new Bitmap("View/Images/Portaaviones.png");
            landAiroport = new Bitmap("View/Images/Aeropuerto.png");

            location.GenerarPosicion();

            //Calcular rutas automáticamente:
            CalculateAutomaticRoutes(); // Asegúrate de que esto se llame aquí

            MapPictureBox.Paint += MapPictureBox_Paint;  
            MapPictureBox.Invalidate();
            this.ClientSize = new System.Drawing.Size(855, 732); // Tamaño deseado
        }
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        private List<Avion> aviones = new List<Avion>(); //Lista de aviones que se pondrán a volar

        //Función para dibujar las localizaciones
        private void MapPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Dibujar los nodos (aeropuertos y portaaviones)
            foreach (Point p in map.PointsInOcean)
            {
                g.DrawImage(oceanShip, p.X - oceanShip.Width / 2, p.Y - oceanShip.Height / 2);
            }

            foreach (Point p in map.PointsInLand)
            {
                g.DrawImage(landAiroport, p.X - landAiroport.Width / 2, p.Y - landAiroport.Height / 2);
            }

            // Dibujar las rutas con colores únicos
            foreach (var avion in aviones)
            {
                // Generar un color aleatorio para la ruta
                Pen routePen = new Pen(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)), 2);

                for (int i = 0; i < avion.Ruta.Count - 1; i++)
                {
                    var start = ParsePoint(avion.Ruta[i]);
                    var end = ParsePoint(avion.Ruta[i + 1]);
                    g.DrawLine(routePen, start, end);
                }
            }
        }

        // Método auxiliar para convertir nodos en puntos
        private Point ParsePoint(string node)
        {
            var coords = node.Trim('(', ')').Split(',');
            return new Point(int.Parse(coords[0]), int.Parse(coords[1]));
        }

        //Se utiliza para calcular las rutas hacia los nodos desde el origen, se aplica dijsktra
        private void CalculateAutomaticRoutes()
        {
            var graph = map.GetGraph() as GraphList;

            if (graph == null)
            {
                Console.WriteLine("El grafo no es del tipo esperado (GraphList).");
                return;
            }

            foreach (var origen in map.nodesInWorld)
            {
                string destino = GetRandomNodeExcept(origen);

                if (destino != null)
                {
                    var result = graph.Dijkstra(origen, destino);

                    // Validar que se haya encontrado una ruta
                    if (result.path.Count > 0)
                    {
                        Console.WriteLine($"Ruta calculada: {string.Join(" -> ", result.path)}");

                        Avion avion = new Avion(origen, destino, result.path, result.distance);
                        aviones.Add(avion);
                    }
                    else
                    {
                        Console.WriteLine($"No se encontró ruta entre {origen} y {destino}");
                    }
                }
            }

            MapPictureBox.Invalidate(); //Se forza el redibujado de las rutas tras cada llamado.
        }


        // Método auxiliar para obtener un nodo aleatorio que no sea el origen
        private string GetRandomNodeExcept(string excludeNode)
        {
            var nodes = map.nodesInWorld.Where(n => n != excludeNode).ToList();
            if (nodes.Count == 0) return null;

            Random rand = new Random();
            return nodes[rand.Next(nodes.Count)];
        }
    }
}
