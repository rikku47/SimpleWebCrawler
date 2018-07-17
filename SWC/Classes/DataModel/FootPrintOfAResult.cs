using System;

namespace SWC.Classes
{
    public class FootPrintOfAResult
    {
        public FootPrintOfAResult()
        {
            Text = "";
            InnerHTML = "";
            OuterHTML = "";
            CreationDate = DateTime.Now;
        }

        public FootPrintOfAResult(string text) : this()
        {
            Text = text;
        }

        public FootPrintOfAResult(string text, string innerHTML) : this(text)
        {
            InnerHTML = innerHTML;
        }

        public FootPrintOfAResult(string text, string innerHTML, string outerHTML) : this(text, innerHTML)
        {
            OuterHTML = outerHTML;
        }

        public string Text { get; set; }
        public string InnerHTML { get; set; }
        public string OuterHTML { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
