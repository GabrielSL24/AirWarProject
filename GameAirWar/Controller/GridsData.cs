using GraphLibrary.AdjacencyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAirWar.Controller
{
    internal class GridsData
    {
        // Método para borrar el avión del Grid
        internal void RemoveAvionFromDataGrid(Avion avion, DataGridView aviones)
        {
            foreach (DataGridViewRow row in aviones.Rows)
            {
                // Verificamos si los valores en la fila coinciden con los del avión
                if (row.Cells["Id"].Value.ToString() == avion.Id)
                {
                    aviones.Rows.Remove(row);
                    break;
                }
            }
        }

        // Método para borrar el Hangar del Grid
        internal void RemoveHangarFromDataGrid(GraphNode hangar, DataGridView hangares)
        {
            foreach (DataGridViewRow row in hangares.Rows)
            {
                // Verificamos si los valores en la fila coinciden con los del avión
                if (row.Cells["IdH"].Value.ToString() == hangar.Id)
                {
                    hangares.Rows.Remove(row);
                    break;
                }
            }
        }
    }
}
