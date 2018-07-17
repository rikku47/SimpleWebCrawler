using AngleSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SWC.Classes
{
    public class SelectorGroup : INotifyPropertyChanged
    {
        private bool _exportAllSelectors;
        private DateTime _startTimeSelectorGroup;
        private KeyValuePair<int, string> _startTimeSelectorGroupHour;
        private KeyValuePair<int, string> _startTimeSelectorGroupMinute;
        private KeyValuePair<int, string> _startTimeSelectorGroupSecond;
        private DateTime _endTimeSelectorGroup;
        private KeyValuePair<int, string> _endTimeSelectorGroupHour;
        private KeyValuePair<int, string> _endTimeSelectorGroupMinute;
        private KeyValuePair<int, string> _endTimeSelectorGroupSecond;
        private long _interval;

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
            StartTimeSelectorGroupHour = new KeyValuePair<int, string>(0, "00");
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
        private bool Lock { get; set; }
        public ObservableCollection<Selector> Selectors { get; }
        public DateTime StartTimeSelectorGroup
        {
            get => _startTimeSelectorGroup;
            set => _startTimeSelectorGroup = value;
        }
        public KeyValuePair<int, string> StartTimeSelectorGroupHour
        {
            get => _startTimeSelectorGroupHour;
            set => _startTimeSelectorGroupHour = value;
        }
        public KeyValuePair<int, string> StartTimeSelectorGroupMinute
        {
            get => _startTimeSelectorGroupMinute;
            set => _startTimeSelectorGroupMinute = value;
        }
        public KeyValuePair<int, string> StartTimeSelectorGroupSecond
        {
            get => _startTimeSelectorGroupSecond;
            set => _startTimeSelectorGroupSecond = value;
        }
        public DateTime EndTimeSelectorGroup
        {
            get => _endTimeSelectorGroup;
            set => _endTimeSelectorGroup = value;
        }
        public KeyValuePair<int, string> EndTimeSelectorGroupHour
        {
            get => _endTimeSelectorGroupHour;
            set => _endTimeSelectorGroupHour = value;
        }
        public KeyValuePair<int, string> EndTimeSelectorGroupMinute
        {
            get => _endTimeSelectorGroupMinute;
            set => _endTimeSelectorGroupMinute = value;
        }
        public KeyValuePair<int, string> EndTimeSelectorGroupSecond
        {
            get => _endTimeSelectorGroupSecond;
            set => _endTimeSelectorGroupSecond = value;
        }
        public long Interval
        {
            get => _interval;
            set
            {
                if(value < 0)
                {
                    value = 0;
                }
                _interval = value;
            }
        }
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public Link Link { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async System.Threading.Tasks.Task CrawlAsync()
        {
            DateTime dtStart = StartTimeSelectorGroup;

            DateTime dtTest = dtStart.AddHours(Convert.ToDouble(StartTimeSelectorGroupHour.Value));

            if (IsCrawl)
            {
                if (true)
                {
                    foreach (var selector in Selectors)
                    {
                        if (selector.IsCrawl & Lock == false)
                        {
                            var config = Configuration.Default.WithDefaultLoader();

                            Link.IDoucument = await BrowsingContext.New(config).OpenAsync(Link.Adress);

                            Link.Encoding = Link.IDoucument.CharacterSet;

                            Lock = true;
                        }
                        selector.Crawl();
                    }

                    Lock = false;
                }
            }
        }
    }
}
