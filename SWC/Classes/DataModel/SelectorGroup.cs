using Newtonsoft.Json;
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
            Selectors.CollectionChanged += Selectors_CollectionChanged;
            CreationDate = DateTime.Now;
        }

        private void Selectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var selector in (ObservableCollection<Selector>)sender)
            {
                selector.SelectorGroup = this;
                //selector.SelectorGroup.CallLink(sender, e);
                if (selector.SelectorGroup.Link != null)
                {
                    selector.SelectorGroup.Link.CallSelectorGroupsCollectionChanged(sender, e);
                }
            }
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
        public ObservableCollection<Selector> Selectors { get; set; }
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public Link Link { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
