using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using SWC.Classes;
using AngleSharp.Parser.Html;
using System.Text;

namespace SWC
{
    internal class Link : INotifyPropertyChanged
    {
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
            CreationDate = DateTime.Now;
        }

        public string Adress { get; }

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

        public DateTime CreationDate { get; }

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

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CrawlSelectors()
        {
            if (IsCrawl)
            {
                var config = Configuration.Default.WithDefaultLoader();

                var document = await BrowsingContext.New(config).OpenAsync(Adress);

                Encoding = document.CharacterSet;

                foreach (var selectorGroup in SelectorGroups)
                {
                    if (selectorGroup.IsCrawl)
                    {
                        foreach (var selector in selectorGroup.Selectors)
                        {
                            if (selector.Crawl)
                            {
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
                                Task.Factory.StartNew(() =>
                                {
                                    ExtractDatas(document, selector);
                                });
#pragma warning restore CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
                            }
                        }
                    }
                }
            }
        }

        private void ExtractDatas(IDocument document, Selector selector)
        {
            var result = new Result();
            Detail details;
            IDocument documentFromScript = null;

            if (selector.ScriptActivate)
            {
                var process = new Process();
                var processStartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    FileName = "cmd.exe",
                    WorkingDirectory = selector.ScriptPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };
                process.StartInfo = processStartInfo;
                process.Start();
                process.StandardInput.WriteLine("node " + selector.ScriptFile);

                bool isClosed = false;
                StringBuilder outp = new StringBuilder();

                while (isClosed == false && !process.StandardOutput.EndOfStream)
                {
                    outp.Append(process.StandardOutput.ReadLine());

                    if (process.StandardOutput.ReadLine().Contains("exit"))
                    {
                        process.Close();
                        isClosed = true;
                    }
                }

                HtmlParser parser = new HtmlParser();
                documentFromScript = parser.Parse(outp.ToString());
            }

            IHtmlCollection<IElement> cells = null;

            if (documentFromScript != null)
            {
                cells = documentFromScript.QuerySelectorAll(selector.CSSSelector);
            }
            else
            {
                cells = document.QuerySelectorAll(selector.CSSSelector);
            }

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

                details = new Detail(text, innerHTML, outerHTML);

                var item = new Item(details);

                result.Items.Add(item);
            }

            App.Current.Dispatcher.Invoke(delegate
            {
                selector.Results.Add(result);
            });
        }
    }
}