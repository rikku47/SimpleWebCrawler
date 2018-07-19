using Newtonsoft.Json;
using SWC.Classes.DataModel;
using SWC.Classes.SearchModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace SWC
{
    public class Group : INotifyPropertyChanged
    {
        private string _name = "";
        private bool _isCrawl;

        public event PropertyChangedEventHandler PropertyChanged;

        public Group(string name)
        {
            Name = name;
            IsCrawl = true;
            Links = new ObservableCollection<Link>();
            Links.CollectionChanged += Links_CollectionChanged;
            Search = new Search();
            Automation = true;
            DateTimeAutomation = new DateTimeAutomation();
            CreationDate = DateTime.Now;
        }

        private void Links_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var link in (ObservableCollection<Link>)sender)
            {
                link.Group = this;
            }
            
               SetHighestAmountOfSelectorsOfALink();
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Changed(nameof(Name));
            }
        }
        public ObservableCollection<Link> Links { get; set; }
        public Search Search { get; set; }
        public bool Automation { get; set; }
        public DateTimeAutomation DateTimeAutomation { get; set; }
        public DateTime CreationDate { get; set; }

        public int HighestAmountOfSelectorsOfALink { get; set; }
        public bool IsCrawl { get => _isCrawl; set => _isCrawl = value; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetHighestAmountOfSelectorsOfALink()
        {
            foreach (var link in Links)
            {
                if(link.TotalSelectors > HighestAmountOfSelectorsOfALink)
                {
                    HighestAmountOfSelectorsOfALink = link.TotalSelectors;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var group = obj as Group;
            return group != null &&
                   EqualityComparer<ObservableCollection<Link>>.Default.Equals(Links, group.Links);
        }

        public override int GetHashCode()
        {
            return 1817216190 + EqualityComparer<ObservableCollection<Link>>.Default.GetHashCode(Links);
        }

        public void Crawl(CancellationToken ct)
        {
            if (IsCrawl)
            {
                foreach (var link in Links)
                {
                    link.Crawl(ct);
                }
            }
        }

        public void CrawlInterval(CancellationToken ct)
        {
            #region DateTimeStart
            DateTime dtStart = DateTimeAutomation.StartDate;

            TimeSpan tsStart = new TimeSpan(DateTimeAutomation.StartDateHour, DateTimeAutomation.StartDateMinute, DateTimeAutomation.StartDateSecond);

            DateTime finalDateStart = dtStart.Add(tsStart);
            #endregion

            #region DateTimeEnd
            DateTime dtEnd = DateTimeAutomation.EndDate;

            TimeSpan tsEnd = new TimeSpan(DateTimeAutomation.EndDateHour, DateTimeAutomation.EndDateMinute, DateTimeAutomation.EndDateSecond);

            DateTime finalDateEnd = dtEnd.Add(tsEnd);
            #endregion

            #region DateTimeInterval
            DateTime dtStartInterval = DateTimeAutomation.StartDate;

            TimeSpan tsStartInterval = new TimeSpan(DateTimeAutomation.StartDateHour, DateTimeAutomation.StartDateMinute, DateTimeAutomation.StartDateSecond + DateTimeAutomation.Interval);

            DateTime finalDateStartInterval = dtStart.Add(tsStart);
            #endregion

            while (DateTime.Now < finalDateEnd)
            {
                if (!ct.IsCancellationRequested)
                {
                    if (DateTime.Now >= finalDateStartInterval)
                    {
                        Crawl(ct);

                        TimeSpan ts = new TimeSpan(0, 0, DateTimeAutomation.Interval);
                        finalDateStartInterval = finalDateStartInterval.Add(ts);
                    }
                }
            }
        }
    }
}
