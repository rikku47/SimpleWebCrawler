using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC.Classes.SearchModel
{
    public class GlobalSearch : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string[] _linksSearch;
        string[] _encodingsSearch;
        int _selectorsMin;
        int _selectorsMax;
        DateTime _dateTimeStart;
        DateTime _dateTimeEnd;

        public string[] LinksSearch { get => _linksSearch; set => _linksSearch = value; }
        public string[] EncodingsSearch { get => _encodingsSearch; set => _encodingsSearch = value; }
        public int SelectorsMin { get => _selectorsMin; set => _selectorsMin = value; }
        public int SelectorsMax { get => _selectorsMax; set => _selectorsMax = value; }
        public DateTime DateTimeStart { get => _dateTimeStart; set => _dateTimeStart = value; }
        public DateTime DateTimeEnd { get => _dateTimeEnd; set => _dateTimeEnd = value; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
