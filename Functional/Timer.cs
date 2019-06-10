using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public class Timer : System.ComponentModel.Component
    {
        public int sec;
        public int min;

        public Timer()
        {
            sec = 0;
            min = 0;
        }

        public void tick()
        {
            sec++;
            if (sec > 59)
            {
                min++;
                sec = 0;
            }
        }
    }
}
