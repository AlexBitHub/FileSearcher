using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using FileSearcherWindow.Core;
using FileSearcherWindow.Model.Pause;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace FileSearcherWindow.Model
{
    public class Searcher : ObservableObject
    {
        #region private fields
        private string _startedPath;
        private int _amountSeenFiles;
        private int _amountMatchFiles;
        private string _stringRegex;
        private int _amountFilesInDir;
        private double _searchingProgress;
        private TimeCounter _timing;
        #endregion

        #region public filds of MVVM
        public bool IsWaiting { get; set; } = false;
        public string StartedPath
        {
            get { return _startedPath; }
            set
            {
                _startedPath = value;
                OnPropertyChanged();
            }
        }
        public int AmountSeenFiles
        {
            get { return _amountSeenFiles; }
            set
            {
                _amountSeenFiles = value;
                OnPropertyChanged();
            }
        }
        public int AmountMatchFiles
        {
            get { return _amountMatchFiles; }
            set
            {
                _amountMatchFiles = value;
                OnPropertyChanged();
            }
        }
        public int AmountFilesInDir
        {
            get { return _amountFilesInDir; }
            set
            {
                _amountFilesInDir = value;
                OnPropertyChanged();
            }
        }
        public string StringRegex
        {
            get { return _stringRegex; }
            set
            {
                _stringRegex = value;
                OnPropertyChanged();
            }
        }
        public double SearchingProgress
        {
            get { return _searchingProgress; }
            set
            {
                _searchingProgress = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<FileItem> ObservableFiles { get; set; }
        //public string TimeExecute { get { return _timeExecute; } set { _timeExecute = value; OnPropertyChanged(); } }
        public TimeCounter Timing { get { return _timing; } set { _timing = value; OnPropertyChanged(); } }
        public Regex RegularExprsn { get; set; }
        public CancellationTokenSource cancelToken;
        public PauseTokenSource pauseToken;
        Dispatcher _dispatcher;
        
        #endregion

        public Searcher()
        {
            ObservableFiles = new ObservableCollection<FileItem>();
            _dispatcher = Dispatcher.CurrentDispatcher;
            pauseToken = new PauseTokenSource();
        }

        public void SelectDirectory()
        {
            FolderBrowserDialog diagWindow = new FolderBrowserDialog { Description = "Стартовая директория поиска файла" };
            if (diagWindow.ShowDialog() == DialogResult.OK)
            {
                StartedPath = diagWindow.SelectedPath;
                try
                {
                    AmountFilesInDir = Directory.GetFiles(StartedPath, "*", SearchOption.AllDirectories).Length;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
                    
            }
        }

        private async Task Awaiting(CancellationToken CancelToken)
        {
            while (IsWaiting)
            {
                try
                {
                    await Task.Delay(10, CancelToken);
                }
                catch (TaskCanceledException)
                {
                    IsWaiting = false;
                }
            }
        }
        //async Task SearchingFile(string path, FileItem Node)
        async void SearchingFile(string path, FileItem ObservableFolder)
        {
            string[] NameFiles = Directory.GetFiles(path);
            foreach (var nameFile in NameFiles)
            {

                await Awaiting(cancelToken.Token);
                if (cancelToken.IsCancellationRequested)
                    return;
                //await pauseToken.WaitWhilePausedAsync();
                Thread.Sleep(260);
                AmountSeenFiles += 1;
                string ClearNameFile = Path.GetFileNameWithoutExtension(nameFile);
                if (RegularExprsn.IsMatch(ClearNameFile))
                {
                    AmountMatchFiles += 1;
                    var newNode = new FileItem(ClearNameFile);
                    _dispatcher.Invoke(() => ObservableFolder.Items.Add(newNode));
                    //dispatcher.BeginInvoke(new Action(() => ObservableFolder.Items.Add(new FileItem(ClearNameFile))));
                }
            }

            string[] NameDirs = Directory.GetDirectories(path);
            foreach (var nameDir in NameDirs)
            {
                await Awaiting(cancelToken.Token);
                if (cancelToken.IsCancellationRequested)
                    return;
                //await pauseToken.WaitWhilePausedAsync();
                FileItem Node = new FileItem(new FileInfo(nameDir).Name);
                _dispatcher.Invoke(() => ObservableFolder.Items.Add(Node));
                if (!new FileInfo(nameDir).Attributes.HasFlag(FileAttributes.System))
                    SearchingFile(nameDir, Node);
            }
            if (AmountSeenFiles == AmountFilesInDir)
            {
                Console.WriteLine("THE END OPERATION");
                Timing.timer.Stop();
            }
        }

        //public async Task StartSearch(string startedPath)
        public void StartSearch(string startedPath)
        {
            _dispatcher.Invoke(() => ObservableFiles.Clear());
            AmountMatchFiles = 0; AmountSeenFiles = 0;
            RegularExprsn = new Regex($@"{StringRegex}");
            
            FileItem StartedFolder = new FileItem(Path.GetFileNameWithoutExtension(startedPath));
            _dispatcher.Invoke(() => ObservableFiles.Add(StartedFolder));
            //await Task.Run(() => SearchingFile(startedPath, StartedFolder));
            //await SearchingFile(startedPath, StartedFolder);
            SearchingFile(startedPath, StartedFolder);
            Console.WriteLine("Выход из функции в синхронном методе");
            
        }

        public async Task StartSearchAsync(string startedPath)
        {
            cancelToken = new CancellationTokenSource();
            cancelToken.Token.Register(() => Console.WriteLine("Operation is canceled"));
            Timing = new TimeCounter();
            pauseToken.stopingProcess += Timing.timer.Stop;
            pauseToken.continueProcess += Timing.timer.Start;
            Timing.timer.Start();

            await Task.Run(() => StartSearch(startedPath));

            Console.WriteLine("Выход из функции в Асинхронном методе");
            Timing.timer.Stop();
        }
    }
}
