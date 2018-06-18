using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html;

namespace SWC
{
    class Link
    {
        private string _adress;
        private string _encoding;
        private bool _allDefaultSelectors;
        private bool _isTrim;
        private bool _crawl;
        private bool _crawlDefault;
        private bool _crawlDefaultText;
        private bool _crawlDefaultInnerHTML;
        private bool _crawlDefaultOuterHTML;
        private bool _crawlCustom;
        private bool _crawlCustomText;
        private bool _crawlCustomInnerHTML;
        private bool _crawlCustomOuterHTML;
        private Selector _selectors;
        private DateTime _creationDate;

        private Link()
        {
            _isTrim = true;
            _isTrim = true;
            _crawl = true;
            _crawlDefault = true;
            _crawlDefaultText = true;
            _crawlDefaultInnerHTML = true;
            _crawlDefaultOuterHTML = true;
            _crawlCustom = true;
            _crawlCustomText = true;
            _crawlCustomInnerHTML = true;
            _crawlCustomOuterHTML = true;
            _selectors = new Selector();

            _creationDate = DateTime.Now;
        }

        private Link(string adress) : this()
        {
            _adress = adress;
        }

        public Link(string adress, bool allDefaultSelectors, IList defaultSelectors) : this(adress)
        {
            _selectors.DefaultSelectors = new ObservableCollection<Selector.DefaultSelector>();

            if (allDefaultSelectors)
            {
                foreach (var defaultSelector in typeof(TagNames).GetFields().Select(field => field.Name).ToList())
                {
                    _selectors.DefaultSelectors.Add(new Selector.DefaultSelector(defaultSelector));
                }
            }
            else
            {
                foreach (string defaultSelector in defaultSelectors)
                {
                    _selectors.DefaultSelectors.Add(new Selector.DefaultSelector(defaultSelector));
                }
            }
        }

        public Link(string adress, bool allDefaultSelectors, IList defaultSelectors, IList customSelectors) : this(adress, allDefaultSelectors, defaultSelectors)
        {
            _selectors.CustomSelectors = new ObservableCollection<Selector.CustomSelector>();

            foreach (string defaultSelector in customSelectors)
            {
                _selectors.CustomSelectors.Add(new Selector.CustomSelector(defaultSelector));
            }
        }

        public string Adress { get => _adress; set => _adress = value; }
        public string Encoding { get => _encoding; set => _encoding = value; }
        public bool IsTrim { get => _isTrim; set => _isTrim = value; }
        public bool IsCrawl { get => _crawl; set => _crawl = value; }
        public bool IsCrawlDefault { get => _crawlDefault; set => _crawlDefault = value; }
        public bool IsCrawlDefaultText { get => _crawlDefaultText; set => _crawlDefaultText = value; }
        public bool IsCrawlDefaultInnerHTML { get => _crawlDefaultInnerHTML; set => _crawlDefaultInnerHTML = value; }
        public bool IsCrawlDefaultOuterHTML { get => _crawlDefaultOuterHTML; set => _crawlDefaultOuterHTML = value; }
        public bool IsCrawlCustom { get => _crawlCustom; set => _crawlCustom = value; }
        public bool IsCrawlCustomText { get => _crawlCustomText; set => _crawlCustomText = value; }
        public bool IsCrawlCustomInnerHTML { get => _crawlCustomInnerHTML; set => _crawlCustomInnerHTML = value; }
        public bool IsCrawlCustomOuterHTML { get => _crawlCustomOuterHTML; set => _crawlCustomOuterHTML = value; }
        //public bool AllDefaultSelectors { get => _allDefaultSelectors; set => _allDefaultSelectors = value; }
        public Selector Selectors { get => _selectors; set => _selectors = value; }
        public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }

        public class Selector
        {
            private ObservableCollection<DefaultSelector> _defaultSelectors;
            private ObservableCollection<CustomSelector> _customSelectors;

            public ObservableCollection<DefaultSelector> DefaultSelectors { get => _defaultSelectors; set => _defaultSelectors = value; }
            public ObservableCollection<CustomSelector> CustomSelectors { get => _customSelectors; set => _customSelectors = value; }

            public class DefaultSelector
            {
                private string _cssselector;
                private DateTime _creationDate;
                private ObservableCollection<Result> _results;

                public DefaultSelector(string cssselector)
                {
                    _cssselector = cssselector;
                    _results = new ObservableCollection<Result>();
                    _creationDate = DateTime.Now;
                }

                public string CSSSelector { get => _cssselector; set => _cssselector = value; }
                public ObservableCollection<Result> Results { get => _results; set => _results = value; }
                public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }
            }

            public class CustomSelector
            {
                private string _cssselector;
                private DateTime _creationDate;
                private ObservableCollection<Result> _results;

                public CustomSelector(string cssselector)
                {
                    _cssselector = cssselector;
                    _results = new ObservableCollection<Result>();
                    _creationDate = DateTime.Now;
                }

                public string CSSSelector { get => _cssselector; set => _cssselector = value; }
                public ObservableCollection<Result> Results { get => _results; set => _results = value; }
                public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }
            }

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

                if (IsCrawlDefault && (IsCrawlDefaultText || IsCrawlDefaultInnerHTML || IsCrawlDefaultOuterHTML))
                {
                    foreach (var selector in Selectors.DefaultSelectors)
                    {
                        var cells = document.QuerySelectorAll(selector.CSSSelector);

                        var result = new Selector.Result();

                        foreach (var cell in cells)
                        {
                            string text = "";
                            string innerHTML = "";
                            string outerHTML = "";

                            if (IsCrawlDefaultText)
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

                            if (IsCrawlDefaultInnerHTML)
                            {
                                innerHTML = cell.InnerHtml;
                            }

                            if (IsCrawlDefaultOuterHTML)
                            {
                                outerHTML = cell.OuterHtml;
                            }

                            Selector.Result.Item.Detail details = new Selector.Result.Item.Detail(text, innerHTML, outerHTML);

                            var item = new Selector.Result.Item(details);

                            result.Items.Add(item);
                        }

                        selector.Results.Add(result);
                    }
                }

                if (IsCrawlCustom && (IsCrawlCustomText || IsCrawlCustomInnerHTML || IsCrawlCustomOuterHTML))
                {
                    foreach (var selector in Selectors.CustomSelectors)
                    {
                        var cells = document.QuerySelectorAll(selector.CSSSelector);

                        var result = new Selector.Result();

                        foreach (var cell in cells)
                        {
                            string text = "";
                            string innerHTML = "";
                            string outerHTML = "";

                            if (IsCrawlDefaultText)
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

                            if (IsCrawlDefaultInnerHTML)
                            {
                                innerHTML = cell.InnerHtml;
                            }

                            if (IsCrawlDefaultOuterHTML)
                            {
                                outerHTML = cell.OuterHtml;
                            }

                            Selector.Result.Item.Detail details = new Selector.Result.Item.Detail(text, innerHTML, outerHTML);

                            var item = new Selector.Result.Item(details);

                            result.Items.Add(item);
                        }

                        selector.Results.Add(result);
                    }
                }
            }
        }
    }
}
