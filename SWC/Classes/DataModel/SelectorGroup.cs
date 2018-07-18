using AngleSharp;
using Newtonsoft.Json;
using SWC.Classes.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace SWC.Classes
{
    public class SelectorGroup : INotifyPropertyChanged
    {
        private bool _exportAllSelectors;
        private int _interval;

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
            DateTimeAutomation = new DateTimeAutomation();
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
        public DateTimeAutomation DateTimeAutomation { get; set; }
        
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public Link Link { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async System.Threading.Tasks.Task CrawlAsync()
        {
            if (IsCrawl)
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

        public void CrawlInterval(CancellationToken ct)
        {
            #region DateTimeStart
            DateTime dtStart = DateTimeAutomation.StartDate;

            TimeSpan tsStart = new TimeSpan(DateTimeAutomation.StartDateHour, Convert.ToInt32(DateTimeAutomation.StartDateMinute.Value), Convert.ToInt32(DateTimeAutomation.StartDateSecond.Value));

            DateTime finalDateStart = dtStart.Add(tsStart);
            #endregion

            #region DateTimeEnd
            DateTime dtEnd = DateTimeAutomation.EndDate;

            TimeSpan tsEnd = new TimeSpan(Convert.ToInt32(DateTimeAutomation.EndDateHour.Value), Convert.ToInt32(DateTimeAutomation.EndDateMinute.Value), Convert.ToInt32(DateTimeAutomation.EndDateSecond.Value));

            DateTime finalDateEnd = dtEnd.Add(tsEnd);
            #endregion

            #region DateTimeInterval
            DateTime dtStartInterval = DateTimeAutomation.StartDate;

            TimeSpan tsStartInterval = new TimeSpan(DateTimeAutomation.StartDateHour, Convert.ToInt32(DateTimeAutomation.StartDateMinute.Value), (Convert.ToInt32(DateTimeAutomation.StartDateSecond.Value) + DateTimeAutomation.Interval));

            DateTime finalDateStartInterval = dtStart.Add(tsStart);
            #endregion

            while (DateTime.Now < finalDateEnd)
            {
                if (!ct.IsCancellationRequested)
                {
                    if (DateTime.Now >= finalDateStartInterval)
                    {
                        CrawlAsync();

                        TimeSpan ts = new TimeSpan(0, 0, DateTimeAutomation.Interval);
                        finalDateStartInterval = finalDateStartInterval.Add(ts);
                    }
                }
            }
        }
    }
}
