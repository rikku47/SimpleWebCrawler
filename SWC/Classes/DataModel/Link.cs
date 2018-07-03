using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using SWC.Classes;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using AngleSharp.Parser.Html;
using System.Text;

namespace SWC
{
    internal class Link : INotifyPropertyChanged
    {
        private int _totalSelectors;

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
        public string Encoding { get; set; }
        public bool IsTrim { get; set; }
        public bool IsCrawl { get; set; }
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
        public DateTime CreationDate { get; }

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
                process.OutputDataReceived += Process_OutputDataReceived;
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

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                int i = 10;
            }
        }
    }
}