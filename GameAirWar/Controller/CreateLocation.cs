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
        private Map _map; //Referencia a la clase Map, que contiene la lógica para cargar el mapa, generando posiciones aleatorias en tierra y oceano.
        private List<(int,int)> _locationsInLand; //Lista para almacenar las ubicaciones en tierra.

        public CreateLocation(Map map)
        {
            _map = map;
            _locationsInLand = new List<(int, int)>(); //Se inicializa la lista.
        }

        internal List<List<Point>> GenerarPosicion() //Encargado de generar posiciones aleatorias.
        {
            var LocationsWorld = _map.GenerarPosicionAleatoria();//almacena la lista de posiciones generadas,
                                                                 //la cual se imprime en la consola para indicar
                                                                 //la cantidad de elementos generado
            Console.WriteLine($"Cantidad de elementos en la variable: {LocationsWorld.Count}");

            return LocationsWorld;
        }
    }
}
