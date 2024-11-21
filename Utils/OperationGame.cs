using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public enum LocationType
    {
        Ocean,
        Land,
        Error
    }

    public enum PriceWeight
    {
        Ocean = 100,
        Land = 75,
        Portaaviones = 50,
        Airport = 25
    }
}
