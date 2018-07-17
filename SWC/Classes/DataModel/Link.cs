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

namespace SWC
{
    public class Link : INotifyPropertyChanged
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

        public ObservableCollection<SelectorGroup> SelectorGroups { get; set; }

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
            DateEntry dateEntry = null;

            if (selector.DateEntries.Count == 0 || DateTime.Today > selector.DateEntries[(selector.DateEntries.Count - 1)].CreationDateOnly)
            {
                dateEntry = new DateEntry();

                App.Current.Dispatcher.Invoke(delegate
                {
                    selector.DateEntries.Add(dateEntry);
                });
            }
            else if (selector.DateEntries.Count > 0 && DateTime.Today == selector.DateEntries[(selector.DateEntries.Count - 1)].CreationDateOnly)
            {
                    dateEntry = selector.DateEntries[(selector.DateEntries.Count - 1)];
            }

            if (dateEntry != null)
            {
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

                if (cells.Length > 0)
                {
                    var result = new Result();

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
                        result.FootPrintsAResult.Add(new FootPrintOfAResult(text, innerHTML, outerHTML));

                        //var item = new FootPrintOfAResult(text, innerHTML, outerHTML);
                    }

                    App.Current.Dispatcher.Invoke(delegate
                    {
                        dateEntry.Results.Add(result);
                    });
                }
            }
        }
        public void CallSelectorGroupsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectorGroups_CollectionChanged(sender, e);
        }
    }
}
