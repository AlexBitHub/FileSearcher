using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;
using FileSearcher.Pause;


namespace FileSearcher.Model
{
    public class Searcher
    {
        public string StartedPath { get; set; }
        public int AmountSeenFiles { get; set; }
        public int AmountMatchFiles { get; set; }
        public string StringRegex { get; set; }
        public ObservableCollection<FileItem> ObservableListFiles { get; set; }
        public Regex RegexExpression { get; set; }
        public CancellationToken cancelToken;
        public CancellationTokenSource pauseCancelToken;
        public PauseTokenSource pauseToken;
        public bool IsWaiting { get; set; } = false;
        public Searcher()
        {

        }

        public async Task Awaiting(CancellationToken CancelToken)
        {
            while (IsWaiting)
            {
                try
                {
                    await Task.Delay(10, CancelToken);
                    //pauseCancelToken = new CancellationTokenSource();
                    //pauseCancelToken.Token.Register(() => Console.WriteLine("Operation is paused"));
                }
                catch (TaskCanceledException)
                {
                    IsWaiting = false;
                }
            }
        }

        async Task SearchingFile(string path, FileItem Node)
        {
            string NameObject = new FileInfo(path).Name;
            FileItem ObservableFolder = new FileItem(NameObject);
            string[] NameFiles = Directory.GetFiles(path);
            foreach (var nameFile in NameFiles)
            {
                await Awaiting(cancelToken);
                if (cancelToken.IsCancellationRequested)
                    return;
                //await pauseToken.WaitWhilePausedAsync();
                
                Thread.Sleep(800);
                AmountSeenFiles += 1;
                Console.WriteLine($"Файлов посмотрено: {AmountSeenFiles}");
                string ClearNameFile = Path.GetFileNameWithoutExtension(nameFile);
                if (RegexExpression.IsMatch(ClearNameFile))
                {
                    Console.WriteLine($"{AmountMatchFiles} files found");
                    AmountMatchFiles += 1;
                    ObservableFolder.AddItem(new FileItem(ClearNameFile));
                }
            }

            Node.AddItem(ObservableFolder);

            string[] NameDirs = Directory.GetDirectories(path);
            foreach (var nameDir in NameDirs)
            {
                await Awaiting(cancelToken);
                if (cancelToken.IsCancellationRequested)
                    return;
                //await pauseToken.WaitWhilePausedAsync();

                if (!new FileInfo(nameDir).Attributes.HasFlag(FileAttributes.System))
                    await SearchingFile(nameDir, ObservableFolder);
            }
        }

        public async Task StartSearchAsync(string startedPath)
        {
            TimeCounter Timing = new TimeCounter();
            //pauseToken.stopingProcess += Timing.Stop;
            //pauseToken.continueProcess += Timing.Start;
            AmountMatchFiles = 0; AmountSeenFiles = 0;
            RegexExpression = new Regex($@"{StringRegex}");
            FileItem StartedFolder = new FileItem(Path.GetFileNameWithoutExtension(startedPath));
            ObservableListFiles = StartedFolder.Items;
            //await Task.Run(() => SearchingFile(startedPath, StartedFolder));
            await SearchingFile(startedPath, StartedFolder);
            Timing.Stop();
        }

        public async Task StartSearch(string startedPath)
        {
            //CancellationTokenSource cts = new CancellationTokenSource();
            //cts.Token.Register(() => Console.WriteLine("Operation is canceled"));

            //PauseTokenSource pts = new PauseTokenSource();

            await StartSearchAsync(startedPath);
        }

        async void  SearchinFile(string path, FileItem ObservableFolder)
        {
            string[] NameFiles = Directory.GetFiles(path);
            foreach (var nameFile in NameFiles)
            {
                await Awaiting(cancelToken.Token);
                if (cancelToken.IsCancellationRequested)
                    return;
                Thread.Sleep(260);
                AmountSeenFiles += 1;
                string ClearNameFile = Path.GetFileNameWithoutExtension(nameFile);
                if (RegularExprsn.IsMatch(ClearNameFile))
                {
                    AmountMatchFiles += 1;
                    var newNode = new FileItem(ClearNameFile);
                    _dispatcher.Invoke(() => ObservableFolder.Items.Add(newNode));
                }
            }

        }
    }
}
