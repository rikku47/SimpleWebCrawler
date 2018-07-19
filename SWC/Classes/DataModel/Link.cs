using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using SWC.Classes;
using SWC.Classes.DataModel;
using AngleSharp.Parser.Html;
using System.Text;
using System.Threading;

namespace SWC
{
    public class Link : INotifyPropertyChanged
    {
        private IDocument _iDoucument;
        private DateTime _iDocumentCrawl;
        private bool _isCrawl;
        private int _totalSelectors;
        private bool _isAdressFound;
        private bool _isEncodingFound;
        private bool _isTotalSelectorsFound;
        private bool _isCreationDateFound;
        private bool _isFilterOut;

        public event PropertyChangedEventHandler PropertyChanged;

        public Link(string adress)
        {
            Adress = adress;
            Encoding = "";
            IsTrim = true;
            IsCrawl = true;
            CrawlText = true;
            SelectorGroups = new ObservableCollection<SelectorGroup>();
            SelectorGroups.CollectionChanged += SelectorGroups_CollectionChanged;
            CreationDate = DateTime.Now;
        }

        private void SelectorGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (sender is ObservableCollection<Selector>)
            {
                TotalSelectors++;
            }
            else if (sender is ObservableCollection<SelectorGroup>)
            {
                TotalSelectors += ((ObservableCollection<SelectorGroup>)sender)[(((ObservableCollection<SelectorGroup>)sender).Count - 1)].Selectors.Count;

                foreach (var selectorGroup in (ObservableCollection<SelectorGroup>)sender)
                {
                    selectorGroup.Link = this;

                    foreach (var selector in selectorGroup.Selectors)
                    {
                        selector.Link = this;

                    }
                }
            }
        }

        public string Adress { get; set; }

        [JsonIgnore]
        public IDocument IDoucument
        {
            get => _iDoucument;
            set
            {
                _iDoucument = value;
            }
        }

        public DateTime IDocumentCrawl
        {
            get => _iDocumentCrawl;
            set => _iDocumentCrawl = value;
        }

        [JsonIgnore]
        public bool IsAdressFound
        {
            get => _isAdressFound;
            set
            {
                _isAdressFound = value;
                Changed(nameof(IsAdressFound));
            }
        }

        public string Encoding { get; set; }

        [JsonIgnore]
        public bool IsEncodingFound
        {
            get => _isEncodingFound;
            set
            {
                _isEncodingFound = value;
                Changed(nameof(IsEncodingFound));
            }
        }

        public bool IsTrim { get; set; }

        public bool IsCrawl
        {
            get
            {
                return _isCrawl;
            }
            set
            {
                _isCrawl = value;
                Changed(nameof(IsCrawl));
            }
        }

        public bool CrawlText { get; set; }

        public bool CrawlInnerHTML { get; set; }

        public bool CrawlOuterHTML { get; set; }

        public ObservableCollection<SelectorGroup> SelectorGroups { get; }

        [JsonIgnore]
        public int TotalSelectors
        {
            get => _totalSelectors;
            set
            {
                _totalSelectors = value;
                Changed(nameof(TotalSelectors));
            }
        }

        [JsonIgnore]
        public bool IsTotalSelectorsFound
        {
            get => _isTotalSelectorsFound;
            set
            {
                _isTotalSelectorsFound = value;
                Changed(nameof(IsTotalSelectorsFound));
            }
        }

        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public bool IsCreationDateFound
        {
            get => _isCreationDateFound;
            set
            {
                _isCreationDateFound = value;
                Changed(nameof(IsCreationDateFound));
            }
        }

        [JsonIgnore]
        public bool IsFilterOut
        {
            get => _isFilterOut;
            set
            {
                _isFilterOut = value;
                Changed(nameof(IsFilterOut));
            }
        }

        [JsonIgnore]
        public Group Group { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Crawl(CancellationToken ct)
        {
            if(IsCrawl)
            {
                foreach (var selectorGroup in SelectorGroups)
                {
                    selectorGroup.CrawlAsync(ct);
                }
            }
        }

        public void CallSelectorGroupsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectorGroups_CollectionChanged(sender, e);
        }
    }
}
