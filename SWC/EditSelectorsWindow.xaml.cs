using System.Collections.ObjectModel;
using System.Windows;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für EditSelectorsWindow.xaml
    /// </summary>
    public partial class EditSelectorsWindow : Window
    {
        public EditSelectorsWindow()
        {
            InitializeComponent();
        }

        private Link link;
        private ObservableCollection<string> customSelectors;

        internal Link Link { get => link; set => link = value; }
        public ObservableCollection<string> CustomSelectors { get => customSelectors; set => customSelectors = value; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Link;
        }
    }
}
