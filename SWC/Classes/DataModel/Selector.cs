using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SWC.Classes
{
    public class Selector : INotifyPropertyChanged
    {
        private bool _export;

        public event PropertyChangedEventHandler PropertyChanged;

        public Selector(string cssselector)
        {
            CSSSelector = cssselector;
            IsTrim = true;
            Crawl = true;
            CrawlText = true;
            ScriptPath = "";
            ScriptFile = "";
            DateEntries = new ObservableCollection<DateEntry>();
            CreationDate = DateTime.Now;
        }

        public string CSSSelector { get; set; }
        public bool IsTrim { get; set; }
        public bool Crawl { get; set; }
        public bool CrawlText { get; set; }
        public bool CrawlInnerHTML { get; set; }
        public bool CrawlOuterHTML { get; set; }
        public bool ScriptActivate { get; set; }
        public string ScriptPath { get; set; }
        public string ScriptFile { get; set; }
        public bool Export
        {
            get => _export;
            set
            {
                _export = value;
                Changed(nameof(Export));
            }
        }
        public ObservableCollection<DateEntry> DateEntries { get; set; }
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public SelectorGroup SelectorGroup { get; set; }
        [JsonIgnore]
        public Link Link { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
