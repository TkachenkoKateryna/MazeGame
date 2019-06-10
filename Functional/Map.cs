using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;

namespace MazeGame
{
    public class Map
    {
        public int W;
        public int H;
        public Coord Exit;
        public Coord Enter;
        public int Tag ;
        public Dictionary<int, Coord> Keys;
        public Dictionary<int, Coord> Doors;
        public List<Coord> Coins;
        public MapCell[,] Field;

        public Map() {
            Tag = 0;
        }

        public Map(int w, int h, Coord exit, Coord enter)
        {
            W = w;
            H = h;
            Exit = exit;
            Enter = enter;
        }
        //Function generates random number
        static Random rand = new Random();
        static int getRandomInRange(int x1, int x2)
        {
            return rand.Next(x1 + 1, x2 + 1);
        }
        //Function generates the field frame 
        //Returns walls, keys, doors
        public List<Coord> Generation(int N, int M, Coord exit, Coord entrance)
        {
            List<Coord> walls = new List<Coord> { };
            List<Coord> passages = new List<Coord> { };
            List<Chamber> chambers = new List<Chamber> { };
            Coord c;
            Coord c1 = new Coord(0, 0);
            Coord c2 = new Coord(N, M);

            passages.Add(exit);
            passages.Add(entrance);
            //Build field's grid
            for (int x = 0; x < c2.X - c1.X; x++)
            {
                for (int y = 0; y < c2.Y - c1.Y; y++)
                {
                    c = new Coord(x, y);
                    if (c.CheckCoord(exit.X, exit.Y) || (c.CheckCoord(entrance.X, entrance.Y))) { continue; }
                    if ((x == 0) || x == c2.X - c1.X - 1) { walls.Add(c); }
                    if ((y == 0) || y == c2.Y - c1.Y - 1) { walls.Add(c); }
                }
            }
            //Create first chamber
            chambers.Add(new Chamber(new Coord(1, 1), new Coord(N - 2, M - 2), entrance));
            //Loop where division happens
            while (!isEnd(chambers))
            {
                List<Chamber> newDivisions = new List<Chamber>();
                for (int i = 0; i < chambers.Count; i++)
                {
                    List<Chamber> parts = Divide(chambers[i], walls, passages);
                    newDivisions.AddRange(parts);
                }
                chambers = newDivisions;
            }
            return walls;
        }
        //Function where all devision logic happens
        public List<Chamber> Divide(Chamber mainChamber, List<Coord> allWalls, List<Coord> allPassages)
        {
            List<Chamber> res = new List<Chamber>();
            List<Coord> walls = new List<Coord>();
            Coord pass = new Coord();
            Chamber chamber1;
            Chamber chamber2;

            if (!mainChamber.canBeDivided())
            {
                res.Add(mainChamber);
                return res;
            }
            //Check how the wall should be build: horizontally or vertically
            bool xBase = mainChamber.RightBottom.X - mainChamber.TopLeft.X > mainChamber.RightBottom.Y - mainChamber.TopLeft.Y;
            int p, hole;
            do
            {
                if (xBase)
                {
                    p = getRandomInRange(mainChamber.TopLeft.X + 1, mainChamber.RightBottom.X - 1);
                }
                else
                {
                    p = getRandomInRange(mainChamber.TopLeft.Y + 1, mainChamber.RightBottom.Y - 1);
                }
            } while (passIsNotOk(p, mainChamber, allPassages, xBase));

            if (xBase)
            {
                hole = getRandomInRange(mainChamber.TopLeft.Y, mainChamber.RightBottom.Y);
            }
            else
            {
                hole = getRandomInRange(mainChamber.TopLeft.X, mainChamber.RightBottom.X);
            }

            if (xBase)
            {
                for (int y = mainChamber.TopLeft.Y; y <= mainChamber.RightBottom.Y; y++)
                {//Build the wall and  passage
                    if (y == hole || allWalls.Exists(element => element.X == p && element.Y == y)) continue;
                    walls.Add(new Coord(p, y));
                }
                pass = new Coord(p, hole);
                //Result of devision - 2 new chambers
                chamber1 = new Chamber(mainChamber.TopLeft, new Coord(p - 1, mainChamber.RightBottom.Y));
                chamber2 = new Chamber(new Coord(p + 1, mainChamber.TopLeft.Y), mainChamber.RightBottom);
            }
            else
            {
                for (int x = mainChamber.TopLeft.X; x <= mainChamber.RightBottom.X; x++)
                {
                    if (x == hole || allWalls.Exists(element => element.X == x && element.Y == p)) continue;
                    walls.Add(new Coord(x, p));
                }
                pass = new Coord(hole, p);

                chamber1 = new Chamber(mainChamber.TopLeft, new Coord(mainChamber.RightBottom.X, p - 1));
                chamber2 = new Chamber(new Coord(mainChamber.TopLeft.X, p + 1), mainChamber.RightBottom);
            }
            //Add chambers to the result
            res.Add(chamber1);
            res.Add(chamber2);
            //Set the enters of the chambers
            setEnters(mainChamber, chamber1, chamber2, pass);

            if (Doors.Keys.Count < 4)
            {
                //Generate keys,doors,coins
                AddKeysDoors(mainChamber, chamber1, chamber2, pass);
            }

            allPassages.Add(pass);
            allWalls.AddRange(walls);
            return res;
        }

        public void AddKeysDoors(Chamber ch, Chamber ch1, Chamber ch2, Coord pass)
        {
            var corners = new List<Coord>();
            if (IsInChamber(ch, Exit))
            {
                if (!(IsInChamber(ch1, Exit) && IsInChamber(ch1, ch.Enter)) || !(IsInChamber(ch2, Exit) && IsInChamber(ch2, ch.Enter)))
                {
                    Doors.Add(Tag, pass);
                    if (ch1.Enter == ch.Enter)
                    {
                        corners.Add(new Coord(ch1.TopLeft.X, ch1.TopLeft.Y));
                        corners.Add(new Coord(ch1.RightBottom.X, ch1.TopLeft.Y));
                        corners.Add(new Coord(ch1.TopLeft.X, ch1.RightBottom.Y));
                        corners.Add(new Coord(ch1.RightBottom.X, ch1.RightBottom.Y));
                    }
                    else
                    {
                        corners.Add(new Coord(ch2.TopLeft.X, ch2.TopLeft.Y));
                        corners.Add(new Coord(ch2.RightBottom.X, ch2.TopLeft.Y));
                        corners.Add(new Coord(ch2.TopLeft.X, ch2.RightBottom.Y));
                        corners.Add(new Coord(ch2.RightBottom.X, ch2.RightBottom.Y));
                    }
                    int index = getRandomInRange(0, 3);
                    Keys.Add(Tag, corners[index]);
                    corners.RemoveAt(index);
                    Coins.AddRange(corners);
                    Tag++;
                }
            }
        }

        public void setEnters(Chamber ch, Chamber ch1, Chamber ch2, Coord pass)
        {
            if (IsInChamber(ch1, ch.Enter))
            {
                ch1.Enter = ch.Enter;
                ch2.Enter = pass;
            }
            else
            {
                ch2.Enter = ch.Enter;
                ch1.Enter = pass;
            }
        }
        //Check if a chamber can be devided
        public bool isEnd(List<Chamber> chambers)
        {
            return !chambers.Exists(element => element.canBeDivided());
        }
        //Check whether we can build a wall at a random position 
        public bool passIsNotOk(int p, Chamber ch, List<Coord> pass, bool direction)
        {
            if (direction)
            {
                return pass.Exists(element => element.CheckCoord(p, ch.TopLeft.Y - 1)) || pass.Exists(element => element.CheckCoord(p, ch.RightBottom.Y + 1));
            }
            else
            {
                return pass.Exists(element => element.CheckCoord(ch.RightBottom.X + 1, p)) || pass.Exists(element => element.CheckCoord(ch.TopLeft.X - 1, p));
            }
        }
        //Check if hole is in chamber
        public bool IsInChamber(Chamber ch, Coord hole)
        {
            return ((ch.TopLeft.X <= hole.X && hole.X <= ch.RightBottom.X) && (hole.Y == ch.TopLeft.Y - 1 || hole.Y == ch.RightBottom.Y + 1))
                   || ((ch.TopLeft.Y <= hole.Y && hole.Y <= ch.RightBottom.Y) && (hole.X == ch.TopLeft.X - 1 || hole.X == ch.RightBottom.X + 1));
        }

        public int FindTag(Dictionary<int, Coord> dic, Coord c)
        {
            foreach (int k in dic.Keys)
            {
                if (dic[k].CheckCoord(c.X, c.Y))
                {
                    return k;
                }
            }
            return -1;

        }

        public bool InDic(Dictionary<int, Coord> dic, int x, int y)
        {
            return dic.FirstOrDefault(z => z.Value.CheckCoord(x, y)).Value is Coord;
        }

        public MapCell[,] MakeMap(List<Coord> walls)
        {
            for (var y = 0; y < H; y++)
            {
                for (var x = 0; x < W; x++)
                {
                    if (walls.Exists(element => element.X == x && element.Y == y))
                    {
                        Field[x, y] = new Wall();
                    }
                    else if (InDic(Doors, x, y))
                    {
                        var door = new Coord(x, y);
                        Field[x, y] = new Door(FindTag(Doors, door));
                    }
                    else if (InDic(Keys, x, y))
                    {
                        var key = new Coord(x, y);
                        Field[x, y] = new Key(FindTag(Keys, key));
                    }
                    else if (Coins.Exists(element => element.X == x && element.Y == y))
                    {
                        Field[x, y] = new Coin();
                    }
                    else
                    {
                        Field[x, y] = new Pass();
                    }
                }
            }
            return Field;
        }
        //Check if hole is in chamber
        public bool In(int x, int y)
        {
            return (x >= 0 && x < W) && (y < H && y >= 0);
        }
        //Check if you can make a move
        public bool AllowedMove(Player player, Coord nextPos)
        {
            return In(nextPos.X, nextPos.Y) && Field[nextPos.X, nextPos.Y].CanPass(); ;
        }
        //Update field's and player's state
        public bool UpdateStatus(Player player, bool ok, Coord nextPos, System.Windows.Forms.Timer timer)
        {
            if (ok)
            {
                player.ChangePosition(nextPos);
                MapCell currentCell = Field[player.position.X, player.position.Y];
                if (player.position.CheckCoord(Exit.X, Exit.Y))
                {
                    timer.Enabled = false;
                    DialogResult result = MessageBox.Show("You have won!");
                    DialogResult result1 = MessageBox.Show(player.printScore(Doors));
                    return false;
                }
                else if (currentCell is Key)
                {
                    player.Keys = new List<int>();
                    Key key = currentCell as Key;
                    player.Keys.Add(key.Tag);
                    if(key.Picked == false)
                    {
                        player.BackPack += 1;
                    }
                    key.Picked = true;
                }
                else if (currentCell is Coin)
                {
                    Coin coin = currentCell as Coin;
                    player.Coins++;
                    coin.Picked = true;
                }
                return true;
            }
            else
            {
                if (!In(nextPos.X, nextPos.Y) || Field[nextPos.X, nextPos.Y] is Wall)
                {
                    Console.Beep();
                }
                else
                {
                    Door door = Field[nextPos.X, nextPos.Y] as Door;
                    if (player.Keys == null)
                    {
                        Console.Beep();
                    }
                    else if (player.Keys.Contains(door.Tag))
                    {
                        player.Doors = new List<int>();
                        door.Opened = true;
                        player.Keys.Remove(door.Tag);
                        player.Doors.Add(door.Tag);
                        player.openedDoors += 1;
                        player.ChangePosition(nextPos);
                    }
                }
                return true;
            }
        }
        //Draw maze on a form
        public void DrawMaze(Map maze, Player player, Graphics g, int size)
        {
            for (var y = 0; y < maze.H; y++)
            {
                for (var x = 0; x < maze.W; x++)
                {
                    if (player.position.CheckCoord(x, y))
                    {
                        g.DrawImage(Resource1.photo_2019_05_19_16_34_47, new Rectangle(x * size, y * size, size, size));
                    }
                    else if (maze.Exit.CheckCoord(x, y))
                    {
                        g.DrawImage(Resource1.photo_2019_05_19_16_28_36, new Rectangle(x * size, y * size, size, size));
                    }
                    else
                    {
                        g.DrawImage(Field[x, y].Print(g), new Rectangle(x * size, y * size, size, size));
                    }
                }
            }
        }
    }
}


