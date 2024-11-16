using System;
using System.Collections.Generic; // Necesario para usar List<>

namespace GameAirWar
{
    //Clase dedicada a los Aviones del Juego, se les da distintas propiedades y atributos.
    public class Avion
    {
        public string Id { get; private set; }
        public string Origen { get; private set; }
        public string Destino { get; private set; }
        public List<string> Ruta { get; private set; }
        public int Distancia { get; private set; }

        public Avion(string origen, string destino, List<string> ruta, int distancia)
        {
            Id = Guid.NewGuid().ToString();
            Origen = origen;
            Destino = destino;
            Ruta = ruta;
            Distancia = distancia;
        }

        public override string ToString()
        {
            return $"Avión {Id}: Origen={Origen}, Destino={Destino}, Distancia={Distancia}, Ruta=[{string.Join(" -> ", Ruta)}]";
        }
    }
}
