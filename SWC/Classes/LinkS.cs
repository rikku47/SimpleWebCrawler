using AngleSharp;
using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC.Classes
{
    public class LinkS
    {
        private string _link;
        private List<LinkS> _linksSilbing;
        private int _level;
        private DateTime _creationDate;

        public LinkS(string link, int level)
        {
            _link = link;
            _linksSilbing = new List<LinkS>();
            _level = level;
            _creationDate = DateTime.Now;
        }

        public LinkS(string link, int level, List<LinkS> lnksSilbing) : this(link, level)
        {
            _linksSilbing = lnksSilbing;
        }

        public string Link { get => _link; set => _link = value; }
        public List<LinkS> LinksSilbing { get => _linksSilbing; set => _linksSilbing = value; }
        public int Level { get => _level; set => _level = value; }
        public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }

        public async Task GetLinksAsync()
        {
            //Task.Factory.StartNew(async () =>
            //{
                var config = Configuration.Default.WithDefaultLoader();

                var document = await BrowsingContext.New(config).OpenAsync(Link);

                var linksG = document.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().Select(a => a.Href).ToList();

                Level--;

                List<LinkS> linksTemp = new List<LinkS>();

                foreach (var linkS in linksG)
                {
                    LinkS linkTemp = new LinkS(linkS, Level);

                    LinksSilbing.Add(linkTemp);
                }

                //GetLinksAsync(LinksSilbing, Level);
            //});
        }
    }
}
