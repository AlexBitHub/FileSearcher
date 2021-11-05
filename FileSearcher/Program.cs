using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSearcher.Model;
using FileSearcher.Pause;
using System.Threading;

namespace FileSearcher
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationTokenSource ctsAsPause = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("Operation is canceled"));
            ctsAsPause.Token.Register(() => Console.WriteLine("Operation is not paused"));
            //PauseTokenSource pts = new PauseTokenSource();

            Searcher schr = new Searcher() { StringRegex = "PCSer", cancelToken = cts.Token, pauseCancelToken = ctsAsPause };

            Task.Run(() => 
            {
                while (true)
                {
                    Console.WriteLine("Print something: ");
                    var command = Console.ReadLine();
                    if (command == "p" | command == "P")
                    {
                        schr.IsWaiting = !schr.IsWaiting;
                    }
                    //pts.IsPaused = !pts.IsPaused;
                    else if (command == "c" | command == "C")
                    {
                        cts.Cancel();
                        //schr.pauseCancelToken.Cancel();
                        //pts.IsPaused = !pts.IsPaused;
                    }
                }
            });
            

            //Searcher schr = new Searcher() { StringRegex = "PCSer", cancelToken = cts.Token, pauseToken = pts };
            schr.StartSearchAsync(@"F:\Config").Wait();
            //Task.Run(() => schr.StartSearch(@"F:\Config")).Wait();

            //schr.ObservableListFiles[0].Print();
        }
    }
}
