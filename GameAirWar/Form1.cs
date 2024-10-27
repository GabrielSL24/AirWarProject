using GameAirWar.OperationForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameAirWar
{
    public partial class Form1 : Form
    {
        private Bitmap oceanShip;
        private Bitmap landAiroport;
        private Map map;
        private CreateLocation location;
        public Form1()
        {
            InitializeComponent();
            map = new Map();
            location = new CreateLocation(map);
            MapPictureBox.Image = map.GetMapBit();

            oceanShip = new Bitmap("Images/Portaaviones.png");
            landAiroport = new Bitmap("Images/Aeropuerto.png");

            location.GenerarPosicion();
            MapPictureBox.Paint += MapPictureBox_Paint;  
            MapPictureBox.Invalidate();
        }

        //Función para pintar las localizaciones
        private void MapPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (Point p in map.PointsInOcean)
            {
                g.DrawImage(oceanShip, p.X - oceanShip.Width / 2, p.Y - oceanShip.Height / 2);
            }

            foreach (Point p in map.PointsInLand)
            {
                g.DrawImage(landAiroport, p.X - landAiroport.Width / 2, p.Y - landAiroport.Height / 2);
            }

        }
    }
}
