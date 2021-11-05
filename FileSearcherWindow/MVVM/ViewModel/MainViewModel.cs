using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;
using FileSearcherWindow.Model;
using FileSearcherWindow.Model.Pause;
using FileSearcherWindow.Core;
using System.Windows.Forms;
using System.Windows;



namespace FileSearcherWindow.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        //CancellationTokenSource cts = new CancellationTokenSource();
        //PauseTokenSource pts = new PauseTokenSource();
        private Visibility _visiblePlay = Visibility.Visible;
        private Visibility _visiblePause = Visibility.Collapsed;

        private bool _isBusy;
        public Visibility VisiblePlay { get { return _visiblePlay; } set { _visiblePlay = value; OnPropertyChanged(); } }
        public Visibility VisiblePause { get { return _visiblePause; } set { _visiblePause = value; OnPropertyChanged(); } }
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public Searcher SearcherModel { get; set; }

        public MainViewModel()
        {
            //SearcherModel = new Searcher() { cancelToken = cts.Token, pauseToken = pts };
            SearcherModel = new Searcher();
            SelectDirectoryCommand = new RelayCommand((par) => SearcherModel.SelectDirectory());
            //PlaySearch = new RelayCommand((par) => SearcherModel.StartSearchAsync(SearcherModel.StartedPath).Wait());
            PlaySearch = new AsyncCommand<string>(ExecuteAsyncSearch, CanExecuteAsync);
            //PlaySearch.CanExecuteChanged += (sender, e) => cts.Cancel();
            PauseSearchingCommand = new RelayCommand((par) => PauseSearching());
            StopSearchingCommand = new RelayCommand((par) => StopSearching());
        }

        public RelayCommand SelectDirectoryCommand { get; set; }
        public RelayCommand PauseSearchingCommand { get; set; }
        public RelayCommand StopSearchingCommand { get; set; }
        //public RelayCommand PlaySearch { get; set; }
        public IAsyncCommand<string> PlaySearch { get; private set; }
        
        private async Task ExecuteAsyncSearch(string startedPath)
        {
            if (SearcherModel.IsWaiting)
            {
                Console.WriteLine("Снять с паузы");
                SearcherModel.IsWaiting = !SearcherModel.IsWaiting;
                SearcherModel.Timing.timer.IsEnabled = !SearcherModel.Timing.timer.IsEnabled;
                VisiblePlay = Visibility.Collapsed;
                VisiblePause = Visibility.Visible;
                return;
            }
            try
            {
                VisiblePlay = Visibility.Collapsed;
                VisiblePause = Visibility.Visible;
                IsBusy = true;
                await SearcherModel.StartSearchAsync(startedPath);
                Console.WriteLine("Выход из блока try");
            }
            finally
            {
                VisiblePlay = Visibility.Visible;
                VisiblePause = Visibility.Collapsed;
                IsBusy = false;
                Console.WriteLine("!!!!!!stop here");
                //SearcherModel.Timing.Stop();
            }
        }
        private bool CanExecuteAsync(string par = null)
        {
            return !IsBusy;
        }

        private void PauseSearching()
        {
            SearcherModel.IsWaiting = !SearcherModel.IsWaiting;
            SearcherModel.Timing.timer.IsEnabled = !SearcherModel.Timing.timer.IsEnabled;
            VisiblePlay = Visibility.Visible;
            VisiblePause = Visibility.Collapsed;
        }

        private void StopSearching()
        {
            VisiblePlay = Visibility.Visible;
            SearcherModel.cancelToken.Cancel();
        }
    }
}
