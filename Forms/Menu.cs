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
    public partial class Menu : Form
    {
        private Map maze = new Map();

        public Menu()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            label1.BackColor = Color.Transparent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form form;
            if (Settings.Width == 0 || Settings.Height == 0) 
            {
                form = new Form3();
            }
            else
            {
                form = new Game();
            }
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form Form3 = new Form3();
            Form3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form Rules = new Rules();
            Rules.Show();
        }
    }
}
