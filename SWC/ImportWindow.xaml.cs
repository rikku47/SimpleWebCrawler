using AngleSharp.Html;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für ImportWindow.xaml
    /// </summary>
    public partial class ImportWindow : Window
    {
        public ImportWindow()
        {
            InitializeComponent();
        }

        private Group group;
        private ObservableCollection<string> customSelectors;

        internal Group Group { get => group; set => group = value; }
        public ObservableCollection<string> CustomSelectors { get => customSelectors; set => customSelectors = value; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Group;

            libDefaultSelectors.ItemsSource = typeof(TagNames).GetFields().Select(field => field.Name).ToList();
            libCustomSelectors.ItemsSource = CustomSelectors;
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            string[] links = txtLinks.Text.Split(';');

            IList customSelectors;

            if ((bool)chkSelectAllCustomSelectors.IsChecked)
            {
                customSelectors = (IList)libCustomSelectors.ItemsSource;
            }
            else
            {
                customSelectors = libCustomSelectors.SelectedItems;
            }
           
            if (links.Length > 0 && ((bool)chkSelectAllDefaultSelectors.IsChecked || libDefaultSelectors.SelectedItems.Count > 0 || customSelectors.Count > 0))
            {
                foreach (var linkAdress in links)
                {
                    //Link link = new Link(linkAdress, (bool)chkSelectAllDefaultSelectors.IsChecked, libDefaultSelectors.SelectedItems, customSelectors);

                    //Group.Links.Add(link);
                }
            }
        }
    }
}
