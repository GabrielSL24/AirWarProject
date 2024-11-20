using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAirWar.Model
{
    internal class Bullet
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Speed { get; set; } = 10;

        public Bullet (int x, int y, int speed)
        {
            X = x;
            Y = y;
            Speed = speed;
        }  
    }
}
