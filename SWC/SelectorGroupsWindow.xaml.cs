using AngleSharp.Html;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

        private void TxtSelectorGroups_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ObservableCollection<string> tempSelectorGroups = new ObservableCollection<string>();

            string[] lselectorGroups = ((TextBox)e.OriginalSource).Text.Split(';');

            foreach (var selectorGroup in lselectorGroups)
            {
                tempSelectorGroups.Add(selectorGroup);
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((TextBox)sender).Text = "";

                foreach (var selectorGroup in tempSelectorGroups)
                {
                    ((Link)((TextBox)sender).DataContext).SelectorGroups.Add(new Link.SelectorGroup(selectorGroup));
                }
            }
        }

        private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void TxtSelectors_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ObservableCollection<string> tempSelectors = new ObservableCollection<string>();

            string[] selectors = ((TextBox)e.OriginalSource).Text.Split(';');

            foreach (var selector in selectors)
            {
                tempSelectors.Add(selector);
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((TextBox)sender).Text = "";

                foreach (var tempSelector in tempSelectors)
                {
                    ((Link.SelectorGroup)((TextBox)sender).DataContext).Selectors.Add(new Link.Selector(tempSelector));
                }
            }
        }

        private void BtnShowResultDetails_Click(object sender, RoutedEventArgs e)
        {
            ResultDetailsWindow resultDetailsWindow = new ResultDetailsWindow()
            {
                Result = (Link.Selector.Result)((Button)sender).DataContext
            };

            resultDetailsWindow.Show();
        }

        private void ContentPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            //libDefaultSelectors.ItemsSource = typeof(TagNames).GetFields().Select(field => field.Name).ToList();
        }

        private void LibDefaultSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListBox)sender).ItemsSource = typeof(TagNames).GetFields().Select(field => field.Name).ToList();
        }

        private void LibCustomSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListBox)sender).ItemsSource = typeof(TagNames).GetFields().Select(field => field.Name).ToList();
        }
    }
}
