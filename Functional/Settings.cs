using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace MazeGame
{
    public static class Settings
    {
        //Check size of a screen
        static Rectangle screen = Screen.PrimaryScreen.WorkingArea;

        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Size { get; set; }
        public static string Level { get; set; }
        //Set size of one square
        public static int setSizeElement(Map maze)
        {
            return Math.Min((screen.Width - 300) / maze.W, (screen.Height - 200) / maze.H);
        }
        //Set the maze
        public static void Set(Map maze)
        {
            maze.H = Height;
            maze.W = Width;
            maze.Keys = new Dictionary<int, Coord> { };
            maze.Doors = new Dictionary<int, Coord> { };
            maze.Coins = new List<Coord> { };
            maze.Field = new MapCell[maze.W, maze.H];
            maze.Enter = new Coord(1, 0);
            maze.Exit = new Coord(maze.W - 1, maze.H - 2);
            List<Coord> walls = maze.Generation(maze.W, maze.H, maze.Exit, maze.Enter);
            maze.Field = maze.MakeMap(walls);
        }
        //Reset the map in case of change
        public static MapCell[,] Reset(Player player, Map maze)
        {
            for (var y = 0; y < maze.H; y++)
            {
                for (var x = 0; x < maze.W; x++)
                {
                    if (player.position.CheckCoord(x, y))
                    {
                        player.position = maze.Enter;
                    }
                    else
                    {
                        maze.Field[x, y].Reset();
                    }
                }
            }
           
            if (player.Doors != null)
            {
                player.Doors.Clear();
            }
            if (player.Keys != null)
            {
                player.Keys.Clear();
            }

            return maze.Field;
        }
    }
}
