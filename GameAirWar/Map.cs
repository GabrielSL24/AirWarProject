using GameAirWar.OperationsMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAirWar
{
    //Clase para crear y cargar el mapa
    public class Map
    {
        private Bitmap bitmap;
        private VerifyLocation Location;
        internal List<Point> PointsInLand = new List<Point>();      //Lista para las coordenadas en Tierra
        internal List<Point> PointsInOcean = new List<Point>();     //Lista para las coordenadas en Océano
        private List<List<Point>> PointsWorld = new List<List<Point>>();

        //Constructor de la clase
        internal Map()
        {
            CargarMap();
            Location = new VerifyLocation(bitmap);
        }

        //Función para cargar el mapa
        private void CargarMap()
        {
            string ruta = "Images/GameMap.png";
            if (System.IO.File.Exists(ruta))
            {
                bitmap = new Bitmap(ruta);
            }
            else
            {
                MessageBox.Show("La imagen no se encontró en la ruta especificada: " + System.IO.Path.GetFullPath(ruta));
            }
        }

        public Bitmap GetMapBit()
        {
            return bitmap;
        }

        //Función para Generar las posiciones de aeropuertos y portaaviones
        public List<List<Point>> GenerarPosicionAleatoria()
        {
            Random rand = new Random();
            int x, y;

            //For para crear en Océano
            for (int i = 0; i < 8; i++)
            {
                do
                {
                    x = rand.Next(bitmap.Width);
                    y = rand.Next(bitmap.Height);
                }
                while (!Location.InOcean(x, y));
                PointsInOcean.Add(new Point(x, y));
            }

            //For para crear en Tierra
            for (int i = 0; i < 10; i++)
            {
                do
                {
                    x = rand.Next(bitmap.Width);
                    y = rand.Next(bitmap.Height);
                }
                while (!Location.InLand(x, y));
                PointsInLand.Add(new Point(x, y));
            }
            PointsWorld.Add(PointsInOcean);
            PointsWorld.Add(PointsInLand);
            return PointsWorld;
        }
    }
}
