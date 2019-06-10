using System;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeGame
{
    public partial class Rules : Form
    {
        public Rules()
        {
            InitializeComponent();
            label2.Text = "Rules you need to follow: \n 1 - Find your way to Exit \n 2 - Pick key to open a door \n 3 - choose the difficulty of your game \n  Easy: you need to get to exit in 10 s \n Medium : you need to get to exit in 30 s \n Hard: you need to get to exit in 45 s \n If you failed , -1 life \n You have only 3 lives ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Menu Menu = new Menu();
            Menu.Show();
        }
    }
}
