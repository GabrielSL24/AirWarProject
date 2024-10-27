using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAirWar.OperationsMap
{
    //Clase para Verificar los puntos en el mapa
    internal class VerifyLocation
    {
        private Bitmap _bitmap;

        //Constructor de la clase
        public VerifyLocation (Bitmap bit)
        {
            _bitmap = bit;
        }

        //Función para verificar si se creo en el Océano
        internal bool InOcean(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _bitmap.Width || y >= _bitmap.Height)
                return false;

            Color color = _bitmap.GetPixel(x, y);

            return color.GetBrightness() > 0.9; ;
        }

        //Función para verificar si se creo en el Océano
        internal bool InLand(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _bitmap.Width || y >= _bitmap.Height)
                return false;

            Color color = _bitmap.GetPixel(x, y);

            return color.GetBrightness() < 0.1;
        }
    }
}
