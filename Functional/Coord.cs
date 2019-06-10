using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Coord
    {
        public int X;
        public int Y;

        public Coord() { }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coord Up()
        {
            return new Coord(X, Y - 1);
        }
        public Coord Down()
        {
            return new Coord(X, Y + 1);
        }
        public Coord Left()
        {
            return new Coord(X - 1, Y);
        }
        public Coord Right()
        {
            return new Coord(X + 1, Y);
        }
        public bool CheckCoord(int x, int y)
        {
            return X == x && Y == y;
        }
    }
}
