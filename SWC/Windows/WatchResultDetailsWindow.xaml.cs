using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für WatchResultWindow.xaml
    /// </summary>
    public partial class WatchResultWindow : Window
    {
        public WatchResultWindow()
        {
            InitializeComponent();
        }

        private Link.Selector.Result.Item _item = null;

        internal Link.Selector.Result.Item Item { get => _item; set => _item = value; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Item;
        }
    }
}
