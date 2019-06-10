using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public enum MoveType { Up, Down, Left, Right, End }

    public class Player 
    {
        public Coord position;
        public List<int> Keys;
        public List<int> Doors;
        public int BackPack = 0;
        public int openedDoors = 0;
        public int Coins;
        public static List<string> Status;
        public int PlayerScore = 0;
        public int PlayerHealth = 0;
        public int lives = 3;

        public Player() { }

        public Player(Coord pos)
        {
            position = pos;
        }

        public void ChangePosition(Coord c)
        {
            position = c;
        }

        public Coord Move(MoveType m)
        {
            Coord newCoord = new Coord { X = 0, Y = 0 };
            switch (m)
            {
                case MoveType.Up:
                    newCoord = position.Up();
                    break;
                case MoveType.Down:
                    newCoord = position.Down();
                    break;
                case MoveType.Left:
                    newCoord = position.Left();
                    break;
                case MoveType.Right:
                    newCoord = position.Right();
                    break;
                default:
                    break;
            }
            return newCoord;
        }

        public void printLives(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.White), 0, Settings.Height * Settings.Size + 25, 200, 80);
            for (int i = 0; i < lives; i++)
            {
                g.DrawImage(Resource1.Live, i * 60, Settings.Height * Settings.Size + 30);
            }
        }

        public void printKeys(Graphics g, Dictionary<int, Coord> keys)
        {
            if (Keys != null)
            {
                g.FillRectangle(new SolidBrush(Color.White), 225, Settings.Height * Settings.Size + 25, 200, 80);
                for (int i = 0; i < BackPack; i++)
                {
                    g.DrawImage(Resource1.Key_C, 225 + i * 60, Settings.Height * Settings.Size + 30);
                }
            }
        }

        public string printScore(Dictionary<int, Coord> doors)
        {
            return "You score is :" + "\n" + Coins + "coins" +"\n" + openedDoors +"opened doors" +"\n" + lives + "saved lives" + "\n" + Game.timer.sec + "seconds";
        }
    }
}
