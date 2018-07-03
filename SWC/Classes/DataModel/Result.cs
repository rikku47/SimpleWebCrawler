using System;
using System.Collections.ObjectModel;

namespace SWC.Classes
{
    class Result
    {
        public Result()
        {
            Items = new ObservableCollection<Item>();
            CreationDate = DateTime.Now;
        }

        public ObservableCollection<Item> Items { get; }
        public DateTime CreationDate { get; }
    }
}
