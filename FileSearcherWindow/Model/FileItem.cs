using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using FileSearcherWindow.Core;

namespace FileSearcherWindow.Model
{
    public class FileItem
    {
        public string Title { get; set; }
        public ObservableCollection<FileItem> Items { get; set; }
        public FileItem(string title)
        {
            Title = title;
            Items = new ObservableCollection<FileItem>();
        }
        //public void AddItem(FileItem item) { Items.Add(item); }
        public void Print()
        {
            if (Items != null)
            {
                Console.Write($"Files in {Title}: ");
                foreach (var item in Items)
                {
                    Console.Write($"{item.Title} ");
                };
                Console.WriteLine("\n");
                foreach (var item in Items)
                {
                    item.Print();
                }
            }

        }
    }
}
