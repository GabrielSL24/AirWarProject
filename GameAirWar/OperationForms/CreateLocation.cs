using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAirWar.OperationForms
{
    
    internal class CreateLocation
    {
        private Map _map;
        private List<(int,int)> _locationsInLand;

        public CreateLocation(Map map)
        {
            _map = map;
            _locationsInLand = new List<(int, int)>();
        }

        internal List<List<Point>> GenerarPosicion()
        {
            var LocationsWorld = _map.GenerarPosicionAleatoria();
            Console.WriteLine($"Cantidad de elementos en la variable: {LocationsWorld.Count}");

            return LocationsWorld;
        }
    }
}
