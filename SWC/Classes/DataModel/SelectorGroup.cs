using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SWC.Classes
{
    public class SelectorGroup : INotifyPropertyChanged
    {
        private bool _exportAllSelectors;

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectorGroup(string name)
        {
            Name = name;
            IsTrim = true;
            IsCrawl = true;
            CrawlText = true;
            ExportAllSelectors = false;
            Selectors = new ObservableCollection<Selector>();
            CreationDate = DateTime.Now;
        }

        public string Name { get; set; }
        public bool IsTrim { get; set; }
        public bool IsCrawl { get; set; }
        public bool CrawlText { get; set; }
        public bool CrawlInnerHTML { get; set; }
        public bool CrawlOuterHTML { get; set; }
        public bool ExportSelectorGroup { get; set; }
        public bool ExportAllSelectors
        {
            get => _exportAllSelectors;
            set
            {
                _exportAllSelectors = value;
                Changed(nameof(ExportAllSelectors));
            }
        }
        public ObservableCollection<Selector> Selectors { get; }
        public DateTime CreationDate { get; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
