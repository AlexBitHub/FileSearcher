using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using FileSearcherWindow.Core;

namespace FileSearcherWindow.Model
{

    public class TimeCounter : ObservableObject
    {
        public DispatcherTimer timer;
        //private TimeSpan ts = new TimeSpan(0, 0, 1);
        private string _timeExecute;
        private int _totalSeconds;
        public string TimeExecute
        {
            get => _timeExecute;
            set
            {
                if (value == _timeExecute) return;
                _timeExecute = value;
                OnPropertyChanged();
            }
        }
        public TimeCounter()
        {
            TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(0).Duration());
            _totalSeconds = 0;
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Ticker;
            //timer.Start();
        }

        void Ticker(object sender, EventArgs e)
        {
            _totalSeconds += 1;
            TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(_totalSeconds).Duration());
        }
    }
}
