using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SWC
{
    class Group : INotifyPropertyChanged
    {
        private string _name = "";
        private readonly ObservableCollection<Link> _links;
        private readonly DateTime _creationDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public Group(string name)
        {
            Name = name;
            _links = new ObservableCollection<Link>();
            _creationDate = DateTime.Now;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Changed(nameof(Name));
            }
        }
        public ObservableCollection<Link> Links { get => _links; }
        public DateTime CreationDate { get => _creationDate; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
