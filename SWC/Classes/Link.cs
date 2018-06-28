using AngleSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SWC
{
    internal class Link
    {
        public Link(string adress)
        {
            Adress = adress;
            Encoding = "";
            IsTrim = true;
            IsCrawl = true;
            CrawlText = true;
            SelectorGroups = new ObservableCollection<SelectorGroup>();
            CreationDate = DateTime.Now;
        }

        public string Adress { get; }
        public string Encoding { get; set; }
        public bool IsTrim { get; set; }
        public bool IsCrawl { get; set; }
        public bool CrawlText { get; set; }
        public bool CrawlInnerHTML { get; set; }
        public bool CrawlOuterHTML { get; set; }
        public ObservableCollection<SelectorGroup> SelectorGroups { get; }
        public DateTime CreationDate { get; }

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
            public SelectorGroup(string name)
            {
                Name = name;
                Crawl = true;
                CrawlText = true;
                Selectors = new ObservableCollection<Selector>();
                CreationDate = DateTime.Now;
            }

            public string Name { get; set; }
            public bool Crawl { get; set; }
            public bool CrawlText { get; set; }
            public bool CrawlInnerHTML { get; set; }
            public bool CrawlOuterHTML { get; set; }
            public bool Export { get; set; }
            public ObservableCollection<Selector> Selectors { get; }
            public DateTime CreationDate { get; }
        }

        public class Selector
        {
            public Selector(string cssselector)
            {
                CSSSelector = cssselector;
                Crawl = true;
                CrawlText = true;
                ScriptPath = "";
                Results = new ObservableCollection<Result>();
                CreationDate = DateTime.Now;
            }

            public string CSSSelector { get; }
            public bool Crawl { get; set; }
            public bool CrawlText { get; set; }
            public bool CrawlInnerHTML { get; set; }
            public bool CrawlOuterHTML { get; set; }
            public bool ScriptActivate { get; set; }
            public string ScriptPath { get; set; }
            public bool Export { get; set; }
            public ObservableCollection<Result> Results { get; }
            public DateTime CreationDate { get; }

            public class Result
            {
                public Result()
                {
                    Items = new ObservableCollection<Item>();
                    CreationDate = DateTime.Now;
                }

                public ObservableCollection<Item> Items { get; }
                public DateTime CreationDate { get; }

                public class Item
                {
                    public Item(Detail details)
                    {
                        Details = details;
                        CreationDate = DateTime.Now;
                    }

                    public Detail Details { get; set; }
                    public DateTime CreationDate { get; }

                    public class Detail
                    {
                        public Detail()
                        {
                            Text = "";
                            InnerHTML = "";
                            OuterHTML = "";
                            CreationDate = DateTime.Now;
                        }

                        public Detail(string text) : this()
                        {
                            Text = text;
                        }

                        public Detail(string text, string innerHTML) : this(text)
                        {
                            InnerHTML = innerHTML;
                        }

                        public Detail(string text, string innerHTML, string outerHTML) : this(text, innerHTML)
                        {
                            OuterHTML = outerHTML;
                        }

                        public string Text { get; set; }
                        public string InnerHTML { get; set; }
                        public string OuterHTML { get; set; }
                        public DateTime CreationDate { get; }
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
                    var result = new Selector.Result();
                    Selector.Result.Item.Detail details;

                    if (selector.ScriptActivate)
                    {
                        var process = new Process();
                        var processStartInfo = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = @"node C:\Users\RacOO\Desktop\example.js"
                        };
                        process.StartInfo = processStartInfo;
                        process.Start();
                    }
                    else
                    {
                        var cells = document.QuerySelectorAll(selector.CSSSelector);

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

                            details = new Selector.Result.Item.Detail(text, innerHTML, outerHTML);

                            var item = new Selector.Result.Item(details);

                            result.Items.Add(item);
                        }
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