using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Chamber
    {
        public Coord TopLeft;
        public Coord RightBottom;
        public Coord Enter;

        public Chamber(Coord c1, Coord c2)
        {
            this.TopLeft = c1;
            this.RightBottom = c2;
        }

        public Chamber(Coord c1, Coord c2, Coord enter)
        {
            this.TopLeft = c1;
            this.RightBottom = c2;
            Enter = enter;
        }

        public bool canBeDivided()
        {
            return RightBottom.X - TopLeft.X > 3 && RightBottom.Y - TopLeft.Y > 1 || RightBottom.Y - TopLeft.Y > 3 && RightBottom.X - TopLeft.X > 1;
        }
    }
}
