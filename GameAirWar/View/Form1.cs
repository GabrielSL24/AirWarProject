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
using GameAirWar.Model;
using GameAirWar.Controller;



namespace GameAirWar
{
    public partial class Form1 : Form
    {
        private Bitmap oceanShip;
        private Bitmap landAiroport;
        private Bitmap bitmapAvion;
        private Bitmap bitmapGun;
        private Random random = new Random(); // Generador de números aleatorios
        private Map map;
        private List<Avion> aviones = new List<Avion>(); //Lista de aviones que se pondrán a volar
        private List<Gun> maps = new List<Gun>();
        private CreateLocation location;
        private Timer routeTimer; // Agregar un Timer para calcular rutas periódicamente
        private Timer avionTimer; // Agregar un Timer para actualizar el movimiento de rutas de los aviones
        private Timer visualAvionTimer; // Agregar un Timer para actualizar el movimiento continuo de los aviones
        private Timer gunTimer; // Agregar un Timer para el movimiento del cañon
        private Timer shootTimer; // Agregar un Timer para el disparo continuo
        private Gun gun = new Gun();
        private GridsData data = new GridsData(); // Instancia para la clase de los Grids
        private int processedNodesCount = 0; // Conteo de nodos procesados
        private const int maxNodesPerBatch = 5; // Máximo de nodos a procesar por iteración
        private bool isMouseDown = false;

        public Form1()
        {
            AllocConsole(); // Habilita la consola para poder ver el debug de os comentarios
            InitializeComponent(); //Se inicializa el componente gráfico

            map = new Map(); //Se crea un objeto del tipo Map
            location = new CreateLocation(map);
            MapPictureBox.Image = map.GetMapBit();

            oceanShip = new Bitmap("View/Images/PortaAviones2.jpeg");
            landAiroport = new Bitmap("View/Images/Aeropuerto.png");
            bitmapAvion = new Bitmap("View/Images/Avion2.jpg");
            bitmapGun = new Bitmap("View/Images/Gun.jpg");

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

            // Configuración del Timer para la visualización de los aviones
            visualAvionTimer = new Timer();
            visualAvionTimer.Interval = 50;
            visualAvionTimer.Tick += VisualTimer_Tick;
            visualAvionTimer.Start();

            // Configuración del Timer para la bala
            shootTimer = new Timer();
            shootTimer.Interval = 300;
            shootTimer.Tick += (s, e) =>
            {
                if (isMouseDown)
                {
                    gun.BulletSpeed += 1;
                }
            };
            shootTimer.Start();

            // Configuración del Timer para el movimiento de la pistola
            gunTimer = new Timer();
            gunTimer.Interval = 5;
            gunTimer.Tick += GunTimer_Tick;
            gunTimer.Start();

            // Configuración de la pistola
            gun.Y = MapPictureBox.Height - 25;

            // Calcular rutas automáticamente:
            CalculateAutomaticRoutes(); // Asegúrate de que esto se llame aquí

            MapPictureBox.Paint += MapPictureBox_Paint;  
            MapPictureBox.Invalidate();
            this.ClientSize = new System.Drawing.Size(1348, 750); // Tamaño deseado
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
                g.DrawImage(oceanShip, p.X - oceanShip.Width / 3, p.Y - oceanShip.Height / 3);
            }

            foreach (Point p in map.PointsInLand)
            {
                g.DrawImage(landAiroport, p.X - landAiroport.Width / 3, p.Y - landAiroport.Height / 3);
            }

            foreach (var avion in aviones)
            {
                if (avion.Ruta.Count > 1)
                {
                    var currentNode = ParsePoint(avion.Ruta[0]);
                    var nextNode = ParsePoint(avion.Ruta[1]);

                    // Calcular posición interpolada del avion
                    float progress = avion.Progreso;
                    int interpolatedX = (int)(currentNode.X + progress * (nextNode.X - currentNode.X));
                    int interpolatedY = (int)(currentNode.Y + progress * (nextNode.Y - currentNode.Y));
                    // Va dibujando el avión poco a poco
                    g.DrawImage(bitmapAvion, interpolatedX - 5, interpolatedY - 5);
                }

                // Generar un color aleatorio para la ruta
                Pen routePen = new Pen(avion.RutaColor);

                for (int i = 0; i < avion.Ruta.Count - 1; i++)
                {
                    var start = ParsePoint(avion.Ruta[i]);
                    var end = ParsePoint(avion.Ruta[i + 1]);
                    g.DrawLine(routePen, start, end);
                }
            }

            // Dibuja la linea para el cañon al igual que agrega la imagen y balas
            g.DrawLine(Pens.Black, 0, MapPictureBox.Height - 25, MapPictureBox.Width, MapPictureBox.Height - 25);
            g.DrawImage(bitmapGun, gun.X - 5, gun.Y - 5);
            foreach (var bullet in gun.GetBullets())
            {
                g.FillEllipse(Brushes.Gold, bullet.X, bullet.Y, 10, 10);
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
            
            // Elimina del Grid los Hangares que hay para actualizarlos
            foreach (string stringNode in map.nodesInOcean)
            {
                var node = graph.GetNode(stringNode);
                data.RemoveHangarFromDataGrid(node, HangarData);
            }
            foreach (string stringNode in map.nodesInLand)
            {
                var node = graph.GetNode(stringNode);
                data.RemoveHangarFromDataGrid(node, HangarData);
            }
            // Lógica principal que tiene el CalculateAutomaticRoutes
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

                        if (originNode.IdAviones.Count != 0) // Verificar si el Hangar tiene ya un avión y otorgarle la ruta
                        {
                            Avion randAvion = (Avion)originNode.IdAviones[random.Next(0, originNode.IdAviones.Count)];
                            data.RemoveAvionFromDataGrid(randAvion, AvionesData); // Primeramente lo borra para actualizar el Grid
                            Avion avion = randAvion;
                            avion.Origen = origen;
                            avion.Destino = destino;
                            avion.Ruta = result.path;
                            avion.Distancia = result.distance;
                            avion.RutaColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                            aviones.Add(avion);
                            AvionesData.Rows.Add(avion.Id, avion.Origen, avion.Destino, avion.Gasolina); // Se agrega al Grid de los aviones en caos de que ya exista el avión
                        }
                        else if (originNode.cantAviones > originNode.IdAviones.Count) // Verificar si elHangar no está lleno para crear el avión con ruta
                        {
                            Avion avion = new Avion(origen, destino, result.path, result.distance);
                            avion.RutaColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                            aviones.Add(avion);
                            originNode.IdAviones.Add(avion);
                            AvionesData.Rows.Add(avion.Id, avion.Origen, avion.Destino, avion.Gasolina); // Se agrega al Grid de los aviones
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

            List<Avion> avionesParaEliminar = new List<Avion>();
            var graph = map.GetGraph() as GraphList;
            foreach (var avion in aviones)
            {
                // Elimina del Grid los Hangares que hay para actualizarlos
                foreach (string stringNode in map.nodesInOcean)
                {
                    var node = graph.GetNode(stringNode);
                    data.RemoveHangarFromDataGrid(node, HangarData);
                }
                foreach (string stringNode in map.nodesInLand)
                {
                    var node = graph.GetNode(stringNode);
                    data.RemoveHangarFromDataGrid(node, HangarData);
                }

                // Eliminar los registros de los aviones en el Grid
                data.RemoveAvionFromDataGrid(avion, AvionesData);
                if ( avion.Ruta.Count > 1 && avion.Progreso >= 1.0f) // Si el avión tiene más nodos en su ruta
                {
                    avion.Progreso = 0.0f;

                    // Actualiza las posiciones de los nodos
                    string stringCurrentNode = avion.Ruta[0];
                    string stringNextNode = avion.Ruta[1];
                    var currentNode = graph.GetNode(stringCurrentNode);
                    var nextNode = graph.GetNode(stringNextNode);
    
                    if (nextNode.cantAviones > nextNode.IdAviones.Count)
                    {
                        //gun.RemoveAvionFromDataGrid(avion, AvionesData);
                        avion.Gasolina -= 5; //Reduce la gasolina del avión
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
                        data.RemoveAvionFromDataGrid(avion, AvionesData);
                    }
                    else if (nextNode.cantGasolina > 15)
                    {
                        int tempCombustible = random.Next(1, 3);
                        nextNode.cantGasolina -= tempCombustible;
                        avion.Gasolina += tempCombustible;
                    }
                    else
                    {
                        int tempCombustible = random.Next(1, 2);
                        nextNode.cantGasolina -= tempCombustible;
                        avion.Gasolina += tempCombustible;
                    }
                }
                AvionesData.Rows.Add(avion.Id, avion.Origen, avion.Destino, avion.Gasolina); // Se agrega al Grid de los aviones
            }

            // Agregar los nodos de los Hangares al Grid
            foreach (string stringNode in map.nodesInOcean)
            {
                List<string> edges = new List<string>();
                var node = graph.GetNode(stringNode);
                var ListEdges = graph.GetNeighbors(stringNode) ;
                foreach (var edge in ListEdges)
                {
                    edges.Add(edge.ToString());
                }
                // Se agrega al Grid
                HangarData.Rows.Add(node.Id, string.Join(", ", edges), node.IdAviones.Count, node.cantAviones, node.cantGasolina);
            }
            foreach (string stringNode in map.nodesInLand)
            {
                List<string> edges = new List<string>();
                var node = graph.GetNode(stringNode);
                var ListEdges = graph.GetNeighbors(stringNode);
                foreach (var edge in ListEdges)
                {
                    edges.Add(edge.ToString());
                }
                // Se agrega al Grid
                HangarData.Rows.Add(node.Id, string.Join(", ", edges), node.IdAviones.Count, node.cantAviones, node.cantGasolina);
            }


            //Espera aleatoria entre 1 y 3 segundos antes de reiniciar el temporizador
            int randomDelay = random.Next(2000, 5000);
            await Task.Delay(randomDelay);

            //Reiniciamos el temporizador
            avionTimer.Start();
        }

        // Evento para el movimiento del avión
        private void VisualTimer_Tick(object sender, EventArgs e)
        {
            foreach (var avion in aviones)
            {
                if (avion.Ruta.Count > 1)
                {
                    avion.Progreso += 0.01f;

                    if (avion.Progreso >= 1.0f)
                    {
                        avion.Progreso = 1.0f;
                    }
                }
            }
            MapPictureBox.Invalidate();
        }

        // Evento del Timer de la pistola para el movimiento
        private void GunTimer_Tick( object sender, EventArgs e )
        {
            var graph = map.GetGraph() as GraphList;
            // Reiniciar al borde izquierdo si llega al borde derecho
            if (gun.X >= MapPictureBox.Width)
            {
                gun.Speed = -Math.Abs(gun.Speed);
            }
            else if (gun.X <= 0)
            {
                gun.Speed = Math.Abs(gun.Speed);
            }
            gun.X += gun.Speed;

            // Mover las balas
            gun.MoveBullets();
            // Comprobar si alguna bala colisiona con algúna avión
            gun.CheckBulletCollision(aviones, graph);
            // Forzar redibujado
            MapPictureBox.Invalidate();
        }

        // Función cuando suelta el botón
        private void MapPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                gun.Shoot(AvionesData, gun.BulletSpeed);
                isMouseDown = false;
                gun.BulletSpeed = 10;
            }
        }

        // Función para cuando tiene el botón apretado
        private void MapPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }
    }
}
