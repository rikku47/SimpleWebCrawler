using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SWC
{
    class Group : INotifyPropertyChanged
    {
        private string _name = "";

        public event PropertyChangedEventHandler PropertyChanged;

        public Group(string name)
        {
            Name = name;
            Links = new ObservableCollection<Link>();
            CreationDate = DateTime.Now;
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
        public ObservableCollection<Link> Links { get; }
        public DateTime CreationDate { get; }

        private void Changed(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool Equals(object obj)
        {
            var group = obj as Group;
            return group != null &&
                   EqualityComparer<ObservableCollection<Link>>.Default.Equals(Links, group.Links);
        }

        public override int GetHashCode()
        {
            return 1817216190 + EqualityComparer<ObservableCollection<Link>>.Default.GetHashCode(Links);
        }
    }
}
