using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC.Classes.SearchModel
{
    public class Search : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _linksSearch;
        string _encodingsSearch;
        int _selectorsMin;
        int _selectorsMax;
        DateTime _dateTimeStart;
        DateTime _dateTimeEnd;

        public Search()
        {
            _linksSearch = "www.chooseadomain.de; www.chooseanotherdomain.com";
            _encodingsSearch = "utf-8;utf-16";
            _selectorsMin = 0;
            _selectorsMax = 0;
            _dateTimeStart = DateTime.Today;
            _dateTimeEnd = DateTime.Today;
        }

        public string LinksSearch
        {
            get => _linksSearch;
            set
            {
                _linksSearch = value;
                Changed(nameof(LinksSearch));
            }
        }

        public string EncodingsSearch
        {
            get => _encodingsSearch;
            set
            {
                _encodingsSearch = value;
                Changed(nameof(EncodingsSearch));
            }
        }

        public int SelectorsMin
        {
            get => _selectorsMin;
            set
            {
                _selectorsMin = value;
                Changed(nameof(SelectorsMin));
            }
        }

        public int SelectorsMax
        {
            get => _selectorsMax;
            set
            {
                _selectorsMax = value;
                Changed(nameof(SelectorsMax));
            }
        }

        public DateTime DateTimeStart
        {
            get => _dateTimeStart;
            set
            {
                _dateTimeStart = value;
                Changed(nameof(DateTimeStart));
            }
        }

        public DateTime DateTimeEnd
        {
            get => _dateTimeEnd;
            set
            {
                _dateTimeEnd = value;
                Changed(nameof(DateTimeEnd));
            }
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
