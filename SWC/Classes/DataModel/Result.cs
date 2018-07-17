using System;
using System.Collections.ObjectModel;

namespace SWC.Classes.DataModel
{
    public class Result
    {
        public int ResultsCount { get; set; }
        public ObservableCollection<FootPrintOfAResult> FootPrintsAResult { get; set; }
        public DateTime CreationDate { get; set; }

        public Result()
        {
            FootPrintsAResult = new ObservableCollection<FootPrintOfAResult>();
            FootPrintsAResult.CollectionChanged += Items_CollectionChanged;
            CreationDate = DateTime.Now;
        }

        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<FootPrintOfAResult>)
            {
                ResultsCount++;
            }
            else
            {

            }
        }
    }
}
