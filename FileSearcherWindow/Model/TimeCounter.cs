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
    //public class TimeCounter : ObservableObject
    //{
    //    DispatcherTimer Timer;
    //    private string _timeExecute;
    //    private int _totalSeconds;
    //    public string TimeExecute 
    //            { 
    //        get { return _timeExecute; } 
    //        set { 
    //              _timeExecute = value; 
    //              OnPropertyChanged(); } 
    //            }
    //    public int TotalSeconds { get { return _totalSeconds; } set { _totalSeconds = value; OnPropertyChanged(); } }

    //    public TimeCounter()
    //    {
    //        TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(0).Duration());
    //        TotalSeconds = 0;
    //        Timer = new DispatcherTimer(DispatcherPriority.Render);
    //        Timer.Interval = TimeSpan.FromSeconds(1);
    //        Timer.Tick += Ticker;
    //        Timer.IsEnabled = true;
    //        //Timer = new Timer(1000);
    //        //Timer.Elapsed += Ticker;
    //        //Timer.AutoReset = true;
    //        //Timer.Enabled = true;
    //        Timer.Start();
    //    }

    //    void Ticker(object sender, EventArgs e)
    //    {
    //        TotalSeconds += 1;
    //        TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(TotalSeconds).Duration());
    //        Console.WriteLine($"Time is {TimeExecute}");
    //    }

    //    public void Start()
    //    {
    //        Console.WriteLine("Time is started");
    //        Timer.Start();

    //    }
    //    public void Stop()
    //    {
    //        Timer.Stop();
    //        Console.WriteLine($"Time executing {TimeExecute}");
    //    }
    //}

    //public class TimeCounter : ObservableObject
    //{

    //    System.Timers.Timer Timer;
    //    private string _timeExecute;
    //    private int _totalSeconds;
    //    public string TimeExecute { get { return _timeExecute; } set { _timeExecute = value; OnPropertyChanged(); } }
    //    public int TotalSeconds { get { return _totalSeconds; } set { _totalSeconds = value; OnPropertyChanged(); } }

    //    public TimeCounter()
    //    {
    //        TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(0).Duration());
    //        TotalSeconds = 0;
    //        Timer = new System.Timers.Timer(1000);

    //        Timer.Elapsed += Ticker;
    //        Timer.AutoReset = true;
    //        Timer.Enabled = true;
    //        Timer.Start();
    //    }

    //    void Ticker(object sender, EventArgs e)
    //    {
    //        TotalSeconds += 1;
    //        TimeExecute = string.Format("{0:hh\\:mm\\:ss}", TimeSpan.FromSeconds(TotalSeconds).Duration());
    //        //Console.WriteLine($"Time is {TimeExecute}");
    //    }

    //    public void Stop()
    //    {
    //        Timer.Stop();
    //        Console.WriteLine($"Time executing {TimeExecute}");
    //    }

    //    public void Start()
    //    {
    //        Timer.Start();
    //    }

    //}

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
            //CommandManager.InvalidateRequerySuggested();
        }
    }
}
