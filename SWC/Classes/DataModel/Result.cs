using System;
using System.Collections.ObjectModel;

namespace SWC.Classes
{
    public class Result
    {
        public Result()
        {
            Items = new ObservableCollection<Item>();
            ResultsCount = 1337;
            CreationDate = DateTime.Now;
        }

        public ObservableCollection<Item> Items { get; }
        public int ResultsCount { get; set; }
        public DateTime CreationDate { get; }
    }
}
