using System;

namespace SWC.Classes
{
    public class Item
    {
        public Item(Detail details)
        {
            Details = details;
            CreationDate = DateTime.Now;
        }

        public Detail Details { get; set; }
        public DateTime CreationDate { get; }
    }
}
