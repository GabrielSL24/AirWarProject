using System;
using System.Collections.Generic; // Necesario para usar List<>
using GraphLibrary.AdjacencyList;

namespace GameAirWar
{
    //Clase dedicada a los Aviones del Juego, se les da distintas propiedades y atributos.
    public class Avion : IAvion
    {
        public string Id { get; private set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public List<string> Ruta { get; set; }
        public int Distancia { get; set; }
        public List<string> RutaRecorrida { get; set; }
        public int Gasolina { get; set; }

        public Avion(string origen, string destino, List<string> ruta, int distancia)
        {
            Id = Guid.NewGuid().ToString();
            Origen = origen;
            Destino = destino;
            Ruta = ruta;
            Distancia = distancia;
            RutaRecorrida = new List<string>();
            Gasolina = 20;
        }

        public override string ToString()
        {
            return $"Avión {Id}: Origen={Origen}, Destino={Destino}, Distancia={Distancia}, Ruta=[{string.Join(" -> ", Ruta)}]";
        }
    }
}
