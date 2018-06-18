using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWC
{
    class Group
    {
        private string _name = "";
        private ObservableCollection<Link> _links;
        private DateTime _creationDate;

        public Group(string name)
        {
            _name = name;
            _links = new ObservableCollection<Link>() ;
            _creationDate = DateTime.Now;
        }

        public string Name { get => _name; set => _name = value; }
        public DateTime CreationDate { get => _creationDate; set => _creationDate = value; }
        public ObservableCollection<Link> Links { get => _links; set => _links = value; }
    }
}
