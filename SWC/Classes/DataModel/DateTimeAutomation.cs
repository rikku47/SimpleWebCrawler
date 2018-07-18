using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC.Classes.DataModel
{
    public class DateTimeAutomation : INotifyPropertyChanged
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private int _interval;

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                Changed(nameof(StartDate));
            }
        }
        public int StartDateHour { get; set; }
        public int StartDateMinute { get; set; }
        public int StartDateSecond { get; set; }
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                Changed(nameof(EndDate));
            }
        }
        public int EndDateHour { get; set; }
        public int  EndDateMinute { get; set; }
        public int  EndDateSecond { get; set; }
        public int Interval
        {
            get => _interval;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                _interval = value;
                Changed(nameof(Interval));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTimeAutomation()
        {
            StartDate = DateTime.Today;
            StartDateHour = 0;
            StartDateMinute = 0;
            StartDateSecond = 0;
            EndDate = DateTime.MaxValue;
            EndDateHour = 0;
            EndDateMinute = 0;
            EndDateSecond = 0;
        }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
