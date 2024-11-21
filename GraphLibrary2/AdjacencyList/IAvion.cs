﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLibrary.AdjacencyList
{
    // Interface para los Aviones
    public interface IAvion
    {
        string Origen {  get; }
        string Destino { get; }
        List<string> Ruta { get; }
        int Distancia {  get; }
        List<string> RutaRecorrida { get; }
        int Gasolina { get; }
        PointF PosicionActual { get; set; }
        float Progreso { get; set; }
        Color RutaColor { get; set; }
    }
}
