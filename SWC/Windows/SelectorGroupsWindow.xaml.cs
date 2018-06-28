using AngleSharp.Html;
using System.Collections;
using System.Collections.Generic;
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

        internal Link Link { get; set; }
        bool AllDefaultSelectors { get; set; }
        bool AllCustomSelectors { get; set; }
        public ObservableCollection<string> DefaultSelectors { get; set; }
        public ObservableCollection<string> CustomSelectors { get; set; }
        IList DefaultSelectorsSelected;
        IList CustomSelectorsSelected;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Link;

            DefaultSelectors = new ObservableCollection<string>(typeof(TagNames).GetFields().Select(field => field.Name).ToList());
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

        private void LibDefaultSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListBox)sender).ItemsSource = DefaultSelectors;
        }

        private void LibCustomSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            ((ListBox)sender).ItemsSource = CustomSelectors;
        }

        private void BtnExportSelectorGroup_Click(object sender, RoutedEventArgs e)
        {
            Windows.ExportSelectorGroup exportSelectorGroup = new Windows.ExportSelectorGroup()
            {
                DataContext = (Link.SelectorGroup)((Button)sender).DataContext
            };

            exportSelectorGroup.ShowDialog();
        }

        private void BtnExportSelectorGroups_Click(object sender, RoutedEventArgs e)
        {
            Windows.ExportSelectorGroups exportSelectorGroups = new Windows.ExportSelectorGroups()
            {
                DataContext = ((Link)((Button)sender).DataContext).SelectorGroups
            };

            exportSelectorGroups.ShowDialog();
        }

        private void BtnAddSelectors_Click(object sender, RoutedEventArgs e)
        {
            Link.SelectorGroup selectorGroup = (Link.SelectorGroup)((Button)sender).DataContext;

            if (AllDefaultSelectors & DefaultSelectors != null && DefaultSelectors.Count > 0)
            {
                foreach (var selector in DefaultSelectors)
                {
                    selectorGroup.Selectors.Add(new Link.Selector(selector));
                }
            }
            else if(DefaultSelectorsSelected != null && DefaultSelectorsSelected.Count > 0)
            {
                foreach (var selector in DefaultSelectorsSelected)
                {
                    selectorGroup.Selectors.Add(new Link.Selector((string)selector));
                }
            }

            if (AllCustomSelectors & CustomSelectors != null && CustomSelectors.Count > 0)
            {
                foreach (var selector in CustomSelectors)
                {
                    selectorGroup.Selectors.Add(new Link.Selector(selector));
                }
            }
            else if(CustomSelectorsSelected != null && CustomSelectorsSelected.Count > 0)
            {
                foreach (var selector in CustomSelectorsSelected)
                {
                    selectorGroup.Selectors.Add(new Link.Selector((string)selector));
                }       
            }
        }

        private void ChkSelectAllDefaultSelectors_Checked(object sender, RoutedEventArgs e)
        {
            AllDefaultSelectors = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkSelectAllCustomSelectors_Checked(object sender, RoutedEventArgs e)
        {
            AllCustomSelectors = (bool)((CheckBox)sender).IsChecked;
        }

        private void LibDefaultSelectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DefaultSelectorsSelected = ((ListBox)sender).SelectedItems;
        }

        private void LibCustomSelectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomSelectorsSelected = ((ListBox)sender).SelectedItems;
        }
    }
}