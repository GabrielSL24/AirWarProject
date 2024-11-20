using GameAirWar.Controller;
using GraphLibrary;
using GraphLibrary.AdjacencyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAirWar.Model
{
    internal class Gun
    {
        public int X { get; set; } = 0;
        public int Y { get; set; }
        public int Speed { get; set; } = 5;
        public int BulletSpeed { get; set; } = 10;

        private List<Bullet> bullets = new List<Bullet>();
        private List<Avion> avionesEliminar = new List<Avion>();
        private DataGridView avionesData = new DataGridView();
        private GridsData data = new GridsData();

        // Método para disparar una bala
        public void Shoot(DataGridView avion, int bulletSpeed)
        {
            bullets.Add(new Bullet(X + 20, Y, -bulletSpeed));
            avionesData = avion;
        }

        // Método para mover las balas
        public void MoveBullets()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Y += bullets[i].Speed;
                
                // Eliminar balas fuera del mapa
                if (bullets[i].Y < 0)
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        public IEnumerable<Bullet> GetBullets()
        {
            return bullets;
        }

        // Método para verificar las coordenadas de cada uno y asi ver si colisiono
        public void CheckBulletCollision(List<Avion> aviones, GraphList graph)
        {

            foreach (var avion in aviones)
            {
                // Recorremos las balas disparadas
                foreach (var bullet in GetBullets())
                {
                    if (avion.Ruta.Count > 1)
                    {
                        string stringNextNode = avion.Ruta[1];
                        var nextNode = graph.GetNode(stringNextNode);

                        float distance = (float)Math.Sqrt(Math.Pow(bullet.X - avion.PosicionActual.X, 2) + Math.Pow(bullet.Y - avion.PosicionActual.Y, 2));

                        if (distance < 200)
                        {
                            
                            avionesEliminar.Add(avion);
                            nextNode.IdAviones.Remove(avion);
                            RemoveBullet(bullet);
                            data.RemoveAvionFromDataGrid(avion, avionesData);
                            Console.WriteLine($"¡Avión destruido en las coordenadas ({avion.PosicionActual.X}, {avion.PosicionActual.Y})!");
                            break;
                        }
                    } 
                }
            }
            foreach (var avion in avionesEliminar)
            {
                aviones.Remove(avion);
            }
            avionesEliminar.Clear();
        }

        private void RemoveBullet(Bullet bullet)
        {
            bullets.Remove(bullet);
        }
    }
}
