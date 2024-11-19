using GameAirWar.OperationForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
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
        private List<Avion> aviones = new List<Avion>(); //Lista de aviones que se pondrán a volar
        private CreateLocation location;
        private Timer routeTimer; // Agregar un Timer para calcular rutas periódicamente
        private Timer avionTimer; // Agregar un Timer para actualizar el movimiento de los aviones
        private int processedNodesCount = 0; // Conteo de nodos procesados
        private const int maxNodesPerBatch = 5; // Máximo de nodos a procesar por iteración

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

            // Configuración del Timer para calcular las ruts automaticamente
            routeTimer = new Timer();
            routeTimer.Interval = 30000;
            routeTimer.Tick += (sender, e) => CalculateAutomaticRoutes();
            routeTimer.Start();

            // Configuración del Timer de aviones
            avionTimer = new Timer();
            avionTimer.Interval = 6000;
            avionTimer.Tick += AvionTimer_Tick;
            avionTimer.Start();

            // Calcular rutas automáticamente:
            CalculateAutomaticRoutes(); // Asegúrate de que esto se llame aquí

            MapPictureBox.Paint += MapPictureBox_Paint;  
            MapPictureBox.Invalidate();
            this.ClientSize = new System.Drawing.Size(855, 732); // Tamaño deseado
        }
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        

        // Función para dibujar las localizaciones
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

        // Se utiliza para calcular las rutas hacia los nodos desde el origen, se aplica dijsktra
        private void CalculateAutomaticRoutes()
        {
            ClearRoutes();  // Se llama para limpiar las rutas visulamente
            int currentCount = 0; // Contador de nodos procesados

            var graph = map.GetGraph() as GraphList;
            if (graph == null)
            {
                Console.WriteLine("El grafo no es del tipo esperado (GraphList).");
                return;
            }

            foreach (var origen in map.nodesInWorld)
            {
                if (processedNodesCount > 0) // Omite nodos ya procesados
                {
                    processedNodesCount--;
                    continue;
                }

                string destino = GetRandomNodeExcept(origen);

                if (destino != null)
                {
                    var result = graph.Dijkstra(origen, destino);

                    // Validar que se haya encontrado una ruta
                    if (result.path.Count > 0)
                    {
                        Console.WriteLine($"Ruta calculada: {string.Join(" -> ", result.path)}");
                        // Configuración de los aviones
                        var originNode = graph.GetNode(origen);
                        var destinyNode = graph.GetNode(destino);
                        bool createAvion = random.Next(0, 2) == 0; // True or False

                        if (originNode.IdAviones.Count != 0 && createAvion) // Verificar si el Hangar tiene ya un avión y otorgarle la ruta
                        {
                            Avion avion = (Avion)originNode.IdAviones[0];
                            avion.Origen = origen;
                            avion.Destino = destino;
                            avion.Ruta = result.path;
                            avion.Distancia = result.distance;
                            aviones.Add(avion);
                        }
                        else if (originNode.cantAviones > originNode.IdAviones.Count) // Verificar si elHangar no está lleno para crear el avión con ruta
                        {
                            Avion avion = new Avion(origen, destino, result.path, result.distance);
                            aviones.Add(avion);
                            originNode.IdAviones.Add(avion);
                        }
                        else { continue; }
                    }
                    else
                    {
                        Console.WriteLine($"No se encontró ruta entre {origen} y {destino}");
                    }
                }
                currentCount++;
                if (currentCount >= maxNodesPerBatch) // Detiene el procesamiento al alcanzar el máximo
                {   
                    processedNodesCount += currentCount;
                    break;
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

        // Método para limpiar las rutas actuales
        private void ClearRoutes()
        {
            aviones.Clear();
            MapPictureBox.Invalidate();
        }

        // Evento del Timer para mover los aviones
        private async void AvionTimer_Tick(object sender, EventArgs e)
        {
            avionTimer.Stop(); //Detiene el Timer mientra se procesa

            Bitmap mapBitmap = (Bitmap)MapPictureBox.Image.Clone();
            using (Graphics g = Graphics.FromImage(mapBitmap))
            {
                var graph = map.GetGraph() as GraphList;
                foreach (var avion in aviones)
                {
                    if ( avion.Ruta.Count > 1 ) // Si el avión tiene más nodos en su ruta
                    {
                        avion.Gasolina -= 4; //Reduce la gasolina del avión
                        // Actualiza las posiciones de los nodos
                        string stringCurrentNode = avion.Ruta[0];
                        string stringNextNode = avion.Ruta[1];
                        var currentNode = graph.GetNode(stringCurrentNode);
                        var nextNode = graph.GetNode(stringNextNode);

                        if (nextNode.cantAviones > nextNode.IdAviones.Count)
                        {
                            nextNode.IdAviones.Add(avion);
                            currentNode.IdAviones.Remove(avion);
                        } else { break; }

                        avion.RutaRecorrida.Add(stringCurrentNode); // Mueve el avión al siguiente nodo
                        avion.Ruta.RemoveAt(0);

                        // Verifica el combustible
                        if (avion.Gasolina <= 0) 
                        {
                            aviones.Remove(avion);
                            nextNode.IdAviones.Remove(avion);
                        }
                        else if (nextNode.cantGasolina > 15)
                        {
                            int tempCombustible = random.Next(1, 5);
                            nextNode.cantGasolina -= tempCombustible;
                            avion.Gasolina += tempCombustible;
                        }
                        else
                        {
                            int tempCombustible = random.Next(1, 3);
                            nextNode.cantGasolina -= tempCombustible;
                            avion.Gasolina += tempCombustible;
                        }
                    }
                }
            }
            MapPictureBox.Invalidate();

            //Espera aleatoria entre 1 y 3 segundos antes de reiniciar el temporizador
            int randomDelay = random.Next(2000, 5000);
            await Task.Delay(randomDelay);

            //Reiniciamos el temporizador
            avionTimer.Start();
        }
    }
}
