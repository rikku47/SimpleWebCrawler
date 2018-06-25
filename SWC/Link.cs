using AngleSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SWC
{
    internal class Link
    {
        private readonly string _adress;
        private string _encoding;
        private bool _isTrim;
        private bool _crawl;
        private readonly ObservableCollection<SelectorGroup> _selectorGroups;
        private readonly DateTime _creationDate;

        public Link(string adress)
        {
            _adress = adress;
            Encoding = "";
            IsTrim = true;
            IsCrawl = true;
            _selectorGroups = new ObservableCollection<SelectorGroup>();

            //if (selectorGroupNames != null && selectorGroupNames.Count > 0)
            //    foreach (var selectorGroupName in selectorGroupNames)
            //    {
            //        if (!selectorGroupName.Equals(""))
            //            SelectorGroups.Add(new SelectorGroup(selectorGroupName));
            //    }

            //AddSelectorsToSelectorGroup(selectors);

            _creationDate = DateTime.Now;
        }

        public string Adress { get => _adress; }
        public string Encoding { get => _encoding; set => _encoding = value; }
        public bool IsTrim { get => _isTrim; set => _isTrim = value; }
        public bool IsCrawl { get => _crawl; set => _crawl = value; }
        public ObservableCollection<SelectorGroup> SelectorGroups { get => _selectorGroups; }
        public DateTime CreationDate { get => _creationDate; }

        private void AddSelectorsToSelectorGroup(IList<string> selectors = null)
        {
            if (selectors != null && selectors.Count > 0)
            {
                foreach (var selectorGroup in SelectorGroups)
                {
                    foreach (var selector in selectors)
                    {
                        selectorGroup.Selectors.Add(new Selector(selector));
                    }
                }
            }
        }

        public class SelectorGroup
        {
            private string _name;
            private bool _crawl;                      //Use it later for global configuration
            private bool _crawlText;               //Use it later for global configuration
            private bool _crawlInnerHTML;   //Use it later for global configuration
            private bool _crawlOuterHTML;  //Use it later for global configuration
            private ObservableCollection<Selector> _selectors;

            public SelectorGroup(string name)
            {
                _name = name;
                _selectors = new ObservableCollection<Selector>();
            }

            public string Name { get => _name; set => _name = value; }
            public bool Crawl { get => _crawl; set => _crawl = value; }
            public bool CrawlText { get => _crawlText; set => _crawlText = value; }
            public bool CrawlInnerHTML { get => _crawlInnerHTML; set => _crawlInnerHTML = value; }
            public bool CrawlOuterHTML { get => _crawlOuterHTML; set => _crawlOuterHTML = value; }
            public ObservableCollection<Selector> Selectors { get => _selectors; set => _selectors = value; }
        }

        public class Selector
        {
            private string _cssselector;
            private bool _crawl;
            private bool _crawlText;
            private bool _crawlInnerHTML;
            private bool _crawlOuterHTML;
            private ObservableCollection<Result> _results;
            private DateTime _creationDate;

            public Selector(string cssselector)
            {
                _cssselector = cssselector;
                _crawl = true;
                _crawlText = true;
                _results = new ObservableCollection<Result>();
                _creationDate = DateTime.Now;
            }

            public string CSSSelector { get => _cssselector; set => _cssselector = value; }
            public bool Crawl { get => _crawl; set => _crawl = value; }
            public bool CrawlText { get => _crawlText; set => _crawlText = value; }
            public bool CrawlInnerHTML { get => _crawlInnerHTML; set => _crawlInnerHTML = value; }
            public bool CrawlOuterHTML { get => _crawlOuterHTML; set => _crawlOuterHTML = value; }
            public ObservableCollection<Result> Results { get => _results; set => _results = value; }
            public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }

            public class Result
            {
                private ObservableCollection<Item> _items;
                private DateTime _creationDate;

                public Result()
                {
                    _creationDate = DateTime.Now;
                    _items = new ObservableCollection<Item>();
                }

                public ObservableCollection<Item> Items { get => _items; set => _items = value; }
                public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }

                public class Item
                {
                    private Detail _details;
                    private DateTime _creationDate;

                    public Item(Detail details)
                    {
                        _details = details;
                        _creationDate = DateTime.Now;
                    }

                    public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }
                    public Detail Details { get => _details; set => _details = value; }

                    public class Detail
                    {
                        private string _text;
                        private string _innerHTML;
                        private string _outerHTML;
                        private DateTime _creationDate;

                        public Detail()
                        {
                            _creationDate = DateTime.Now;
                        }

                        public Detail(string text) : this()
                        {
                            _text = text;
                        }

                        public Detail(string text, string innerHTML) : this(text)
                        {
                            _innerHTML = innerHTML;
                        }

                        public Detail(string text, string innerHTML, string outerHTML) : this(text, innerHTML)
                        {
                            _outerHTML = outerHTML;
                        }

                        public string Text { get => _text; set => _text = value; }
                        public string InnerHTML { get => _innerHTML; set => _innerHTML = value; }
                        public string OuterHTML { get => _outerHTML; set => _outerHTML = value; }
                        public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }
                    }
                }
            }
        }

        public async Task CrawlAsync()
        {
            if (IsCrawl)
            {
                var config = Configuration.Default.WithDefaultLoader();

                var document = await BrowsingContext.New(config).OpenAsync(Adress);

                Encoding = document.CharacterSet;

                foreach (var selectorGroup in SelectorGroups)
                {
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
                    Task.Factory.StartNew(() =>
                    {
                        ExtractDatas(document, selectorGroup.Selectors);
                    });
#pragma warning restore CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
                }
            }
        }

        private void ExtractDatas(AngleSharp.Dom.IDocument document, ObservableCollection<Selector> selectors)
        {
            foreach (var selector in selectors)
            {
                if (selector.Crawl)
                {
                    var cells = document.QuerySelectorAll(selector.CSSSelector);

                    var result = new Selector.Result();

                    foreach (var cell in cells)
                    {
                        string text = "";
                        string innerHTML = "";
                        string outerHTML = "";

                        if (selector.CrawlText)
                        {
                            if (IsTrim)
                            {
                                text = cell.TextContent.Trim();
                            }
                            else
                            {
                                text = cell.TextContent;
                            }
                        }

                        if (selector.CrawlInnerHTML)
                        {
                            innerHTML = cell.InnerHtml;
                        }

                        if (selector.CrawlOuterHTML)
                        {
                            outerHTML = cell.OuterHtml;
                        }

                        Selector.Result.Item.Detail details = new Selector.Result.Item.Detail(text, innerHTML, outerHTML);

                        var item = new Selector.Result.Item(details);

                        result.Items.Add(item);
                    }

                    App.Current.Dispatcher.Invoke(delegate
                    {
                        selector.Results.Add(result);
                    });
                }
            }
        }
    }
}