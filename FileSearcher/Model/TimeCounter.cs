using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;

namespace FileSearcher.Model
{
    class TimeCounter
    {
        
        System.Timers.Timer Timer;
        string TimeExecute { get; set; }
        public int TotalSeconds { get; set; }

        public TimeCounter()
        {
            TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(0).Duration());
            TotalSeconds = 0;
            Timer = new System.Timers.Timer(1000);

            Timer.Elapsed += Ticker;
            Timer.AutoReset = true;
            Timer.Enabled = true;
            Timer.Start();
        }

        void Ticker(object sender, EventArgs e)
        {
            TotalSeconds += 1;
            TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(TotalSeconds).Duration());
            //Console.WriteLine($"Time is {TimeExecute}");
        }

        public void Stop()
        {
            Timer.Stop();
            Console.WriteLine($"Time executing {TimeExecute}");
        }

        public void Start()
        {
            Timer.Start();
        }

    }
}
