using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGame
{
    public partial class Game: Form

    {
        enum Position { Left, Right, Up, Down}

        private Graphics g;
        private Player player;
        Map maze;
        Label timeLabel = new Label();
        Button endButton = new Button();
        Label scoreLabel = new Label();
        public static Timer timer = new Timer();

        public Game()
        {
            StartPosition = FormStartPosition.CenterScreen;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();
            this.KeyPreview = true;
            maze = new Map();
            Settings.Set(maze);
            player = new Player(maze.Enter);
            Settings.Size = Settings.setSizeElement(maze);
            this.Size = new Size(maze.W * Settings.Size + 100, maze.H * Settings.Size + 200);
            initTimer();
            initScorePanel();
        }
        //Drawing
        private void OnPaint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            maze.DrawMaze(maze, player, g, Settings.Size);
            player.printLives(g);
            player.printKeys(g,maze.Keys);
        }
        //Game process
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Coord nextPos = null;
            bool flag, ok;

            if (e.KeyCode == Keys.Left)
            {
                nextPos = player.Move(MoveType.Left);
            }
            else if (e.KeyCode == Keys.Right)
            {
                nextPos = player.Move(MoveType.Right);
            }
            else if (e.KeyCode == Keys.Down)
            {
                nextPos = player.Move(MoveType.Down);
            }
            else if (e.KeyCode == Keys.Up)
            {
                nextPos = player.Move(MoveType.Up);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Environment.Exit(0);
            }
            ok = maze.AllowedMove(player, nextPos);
            flag = maze.UpdateStatus(player, ok, nextPos, timer1 );
            if(flag == false)
            {
                stopGame();
            }
            Refresh();
        }
        //Timer 
        public void initTimer()
        {
            timeLabel.Location = new Point(0, Settings.Height * Settings.Size + 100);
            timeLabel.Font = new Font(timeLabel.Font.FontFamily, 24);
            timeLabel.Size = new Size(100, 50);
            timeLabel.BackColor = Color.Transparent;
            this.Controls.Add(timeLabel);
            timer1.Enabled = true;
        }
        //Score
        public void initScorePanel()
        {
            scoreLabel.Location = new Point(maze.W * Settings.Size - 100, maze.H * Settings.Size + 15);
            scoreLabel.Font = new Font(timeLabel.Font.FontFamily, 24);
            scoreLabel.BackColor = Color.Transparent;
            scoreLabel.Size = new Size(100, 70);
            this.Controls.Add(scoreLabel);
        }

        private void endButton_Click(object sender, EventArgs e)
        {
            stopGame();
        }
        //Minus life if no time / time limit depends on difficulty
        private void timer1_Tick(object sender, EventArgs e)
        {
            checkLives();
            timer.tick();
            if (timer.sec < 10)
            {
                timeLabel.Text = timer.min + ":0" + timer.sec;
            } 
            else
            {
                timeLabel.Text = timer.min + ":" + timer.sec;
            }
            switch (Settings.Level)
            {
                case "easy":
                    if(timer.sec % 10 == 0)
                    {
                        MinusLife();
                    }
                    break;
                case "medium":
                    if (timer.sec % 30 == 0)
                    {
                        MinusLife();
                    }
                    break;
                case "hard":
                    if (timer.sec % 45 == 0)
                    {
                        MinusLife();
                    }
                    break;
            }
        }

        public void stopGame()
        {
            this.Close();
            Form menu = new Menu();
            menu.Show();
        }

        public void checkLives()
        {
            if(player.lives == 0)
            {
                timer1.Enabled = false;
                MessageBox.Show("You are loser" + "\n" + "You time " + timer.sec + " seconds");
                stopGame();
            }
        }

        public void MinusLife()
        {
            player.lives -= 1;
            player.BackPack = 0;
            player.Coins = 0;
            player.openedDoors = 0;
            maze.Field = Settings.Reset(player, maze);
            Refresh();
        }
    }
}
