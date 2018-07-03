using System;
using System.Collections.ObjectModel;

namespace SWC.Classes
{
    class Selector
    {
        public Selector(string cssselector)
        {
            CSSSelector = cssselector;
            IsTrim = true;
            Crawl = true;
            CrawlText = true;
            ScriptPath = "";
            ScriptFile = "";
            Results = new ObservableCollection<Result>();
            CreationDate = DateTime.Now;
        }

        public string CSSSelector { get; }
        public bool IsTrim { get; set; }
        public bool Crawl { get; set; }
        public bool CrawlText { get; set; }
        public bool CrawlInnerHTML { get; set; }
        public bool CrawlOuterHTML { get; set; }
        public bool ScriptActivate { get; set; }
        public string ScriptPath { get; set; }
        public string ScriptFile { get; set; }
        public bool Export { get; set; }
        public ObservableCollection<Result> Results { get; }
        public DateTime CreationDate { get; }
    }
}
