using System;
using System.Collections.ObjectModel;

namespace SWC.Classes
{
    class SelectorGroup
    {
        public SelectorGroup(string name)
        {
            Name = name;
            IsTrim = true;
            IsCrawl = true;
            CrawlText = true;
            Selectors = new ObservableCollection<Selector>();
            CreationDate = DateTime.Now;
        }

        public string Name { get; set; }
        public bool IsTrim { get; set; }
        public bool IsCrawl { get; set; }
        public bool CrawlText { get; set; }
        public bool CrawlInnerHTML { get; set; }
        public bool CrawlOuterHTML { get; set; }
        public bool Export { get; set; }
        public ObservableCollection<Selector> Selectors { get; }
        public DateTime CreationDate { get; }
    }
}
