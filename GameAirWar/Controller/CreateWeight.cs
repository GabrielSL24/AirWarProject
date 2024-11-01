using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace GameAirWar.Controller
{
    //Clase que crea los Pesos de los nodos
    internal class CreateWeight
    {
        internal int weight;
        private int tempWeight;
        //Convertir los tipos de precio de las opciones a enteros
        private int oceanPrice = (int)PriceWeight.Ocean;
        private int landPrice = (int)PriceWeight.Land;
        private int airportPrice = (int)PriceWeight.Airport;
        private int portaavionesPrice = (int)PriceWeight.Portaaviones;
        private LocationType TypeNode1;
        private LocationType TypeNode2;

        //Constructor de la clase
        public CreateWeight((LocationType type1, LocationType type2) Types, int Weight)
        {
            TypeNode1 = Types.type1;
            TypeNode2 = Types.type2;
            this.tempWeight = Weight;
            IdentifyWeight(TypeNode1, TypeNode2, tempWeight);
        }

        //Función que identifica el peso basado en los tipos de ubicación de ambos nodos
        private int IdentifyWeight(LocationType typeNode1, LocationType typeNode2, int weight2)
        {
            bool nodeInOcean = typeNode1.Equals(LocationType.Ocean);
            bool nodeInOcean2 = typeNode2.Equals(LocationType.Ocean);
            bool nodeInLand = typeNode1.Equals (LocationType.Land);
            bool nodeInLand2 = typeNode2.Equals (LocationType.Land);
            weight = 0;

            if (nodeInOcean && nodeInOcean2)
            {
                weight = portaavionesPrice + oceanPrice + weight2;
                return weight;
            }
            else if (nodeInLand && nodeInLand2)
            {
                weight = airportPrice + landPrice + weight2;
                return weight;
            }
            else if (nodeInOcean && nodeInLand2)
            {
                weight = oceanPrice + airportPrice + weight2;
                return weight;
            }
            else if (nodeInLand && nodeInOcean2)
            {
                weight = oceanPrice + portaavionesPrice + weight2;
                return weight;
            }
            return 0;
        }
    }
}
