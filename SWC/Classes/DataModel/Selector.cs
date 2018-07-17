using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using SWC.Classes.DataModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SWC.Classes
{
    public class Selector : INotifyPropertyChanged
    {
        private bool _export;
        private bool _isCrawl;

        public event PropertyChangedEventHandler PropertyChanged;

        public Selector(string cssselector)
        {
            CSSSelector = cssselector;
            IsTrim = true;
            IsCrawl = true;
            CrawlText = true;
            ScriptPath = "";
            ScriptFile = "";
            DateEntries = new ObservableCollection<DateEntry>();
            StartTimeSelector = DateTime.Today;
            EndTimeSelector = DateTime.MaxValue;
            Interval = 3600;
            CreationDate = DateTime.Now;
        }

        public string CSSSelector { get; set; }
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
        public DateTime StartTimeSelector { get; set; }
        public int StartTimeSelectorHour { get; set; }
        public int StartTimeSelectorMinute { get; set; }
        public int StartTimeSelectorSecond { get; set; }
        public DateTime EndTimeSelector { get; set; }
        public int EndTimeSelectorHour { get; set; }
        public int EndTimeSelectorMinute { get; set; }
        public int EndTimeSelectorSecond { get; set; }
        public long Interval { get; set; }
        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public SelectorGroup SelectorGroup { get; set; }
        [JsonIgnore]
        public Link Link { get; set; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Crawl()
        {
            if (IsCrawl)
            {
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
                Task.Factory.StartNew(() =>
                {
                    ExtractDatas(Link.IDoucument, this);
                });
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
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
    }
}
