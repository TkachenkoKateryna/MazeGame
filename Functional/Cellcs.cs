using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace MazeGame
{

    interface IPrintable
    {
       Bitmap Print(Graphics g);
    }

    public abstract class MapCell : IPrintable
    {
        public abstract bool CanPass();
        public abstract void Reset();
        public abstract Bitmap Print(Graphics g);
    }

    class Wall : MapCell
    {
        public Wall() { }

        public override Bitmap Print(Graphics g)
        {
            return Resource1.brick_wall;
        }

        public override bool CanPass()
        {
            return false;
        }
        public override void Reset()
        {
            return;
        }
    }

    class Pass : MapCell
    {
        public Pass() { }

        public override Bitmap Print(Graphics g)
        {
            return Resource1.sugar;
        }

        public override bool CanPass()
        {
            return true;
        }

        public override void Reset()
        {
            return;
        }
    }

    class Door : MapCell
    {
        public int Tag;

        public bool Opened = false;

        public Door(int t)
        {
            Tag = t;
        }

        public override Bitmap Print(Graphics g)
        {
            if (!Opened)
            {
                switch (Tag)
                {
                    case 0:
                        return Resource1.Door_A;
                    case 1:
                        return Resource1.Door_B;
                    case 2:
                        return Resource1.Door_C;
                    case 3:
                        return Resource1.Door_D;
                    default:
                        return null;
                }
            }
            else
            {
                return Resource1.sugar;
            }
        }

        public override void Reset()
        {
            if (Opened)
            {
                Opened = false;
            }
        }

        public override bool CanPass()
        {
            return Opened;
        }
    }

    class Key : MapCell
    {
        public int Tag;
        public bool Picked = false;

        public Key(int t)
        {
            Tag = t;
        }

        public override Bitmap Print(Graphics g)
        {
            if (!Picked)
            {
                switch (Tag)
                {
                    case 0:
                        return Resource1.Key_A;
                    case 1:
                        return Resource1.Key_B;
                    case 2:
                        return Resource1.Key_C;
                    case 3:
                        return Resource1.Key_D;
                    default:
                        return null;
                }
            }
            else
            {
                return Resource1.sugar;
            }
        }

        public override bool CanPass()
        {
            return true;
        }

        public override void Reset()
        {
            if (Picked)
            {
                Picked = false;
            }
        }
    }

    class Coin : MapCell
    {
        public Coin() { }

        public bool Picked = false;

        public override Bitmap Print(Graphics g)
        {
            if (!Picked)
            {
                return Resource1.photo_2019_05_19_16_34_42;
            }
            else
            {
                return Resource1.sugar;
            }
        }

        public override bool CanPass()
        {
            return true;
        }

        public override void Reset()
        {
            if (Picked)
            {
                Picked = false;
            }
        }
    }
}
