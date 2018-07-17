using Newtonsoft.Json;
using SWC.Classes.DataModel;
using System;
using System.Collections.ObjectModel;

namespace SWC.Classes
{
    public class DateEntry
    {
        public ObservableCollection<Result> Results { get; set; }
        public DateTime CreationDateOnly { get; set; }
        public DateTime CreationDate { get; set; }

        public DateEntry()
        {
            Results = new ObservableCollection<Result>();
            CreationDateOnly = DateTime.Today;
            CreationDate = DateTime.Now;
        }
    }
}
