using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC.Classes
{
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
