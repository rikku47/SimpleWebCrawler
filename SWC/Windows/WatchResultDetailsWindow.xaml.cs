using SWC.Classes;
using System.Windows;

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

        private Item _item = null;

        internal Item Item { get => _item; set => _item = value; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Item;
        }
    }
}
