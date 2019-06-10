using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGame
{
   
    public partial class Form3 : Form
    {
        List<int> result = new List<int>();

        public Form3()
         {
            InitializeComponent();
            setSizeGrid(trackBar2);
            setSizeGrid(trackBar3);
            label4.ClientSize = new Size(300, 200);
            label4.AutoSize = false;
            checkDifficulty(trackBar2, trackBar3,label4);
         }
 
        public static void setSizeGrid(TrackBar bar)
        {
            bar.Minimum = 8;
            bar.Maximum = 30;
            bar.TickFrequency = 10;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            textBox2.Text = "" + trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            textBox3.Text = "" + trackBar3.Value;
            checkDifficulty(trackBar2, trackBar3, label4);
        }
        //Rect color depends on difficulty
        private static void checkDifficulty(TrackBar bar, TrackBar bar1,Label label)
        {
            if( bar.Value * bar1.Value <= 100)
            {
                label.BackColor = Color.Green;
                label.Text = "Easy";
                Settings.Level = "easy";
            }
            else if(bar.Value * bar1.Value <= 225 &&  bar1.Value * bar.Value > 100)
            {
                label.BackColor = Color.Orange;
                label.Text = "Medium";
                Settings.Level = "medium";
            }
            else 
            {
                label.BackColor = Color.Red;
                label.Text = "Hard";
                Settings.Level = "hard";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Settings.Width = trackBar2.Value;
            Settings.Height = trackBar3.Value;
            this.Hide();
            Form Form2 = new Menu();
            Form2.Show();
        }
    }
}
