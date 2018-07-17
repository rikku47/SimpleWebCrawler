using AngleSharp.Html;
using SWC.Classes;
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

        CheckBox ChkDefaultSelectors { get; set; }
        CheckBox ChkCustomSelectors { get; set; }
        ListBox LibDefaultSelectors { get; set; }
        ListBox LibCustomSelectors { get; set; }
        public ObservableCollection<string> DefaultSelectors { get; set; }
        public ObservableCollection<string> CustomSelectors { get; set; }
        string[] Selectors { get; set; }
        SelectorGroup SelectorGroup { get; set; }
        TextBox TextBox { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSelectors = new ObservableCollection<string>(typeof(TagNames).GetFields().Select(field => field.Name).ToList());

            //if (tcSelectorGroups != null && tcSelectorGroups.Items.Count > 0)
            //{
            //    tcSelectorGroups.SelectedIndex = 0;
            //}
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
                    ((Link)((TextBox)sender).DataContext).SelectorGroups.Add(new SelectorGroup(selectorGroup));
                }
            }
        }

        private void TxtSelectors_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Selectors = TextBox.Text.Split(';');

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                AddSelectorsToSelectorGroup(Selectors, SelectorGroup);
                AddSelectorsToCustomSelectors(Selectors, CustomSelectors);

                TextBox.Text = "";
            }
        }

        private void BtnShowResultDetails_Click(object sender, RoutedEventArgs e)
        {
            ResultDetailsWindow resultDetailsWindow = new ResultDetailsWindow()
            {
                Result = (DateEntry)((Button)sender).DataContext
            };

            resultDetailsWindow.Show();
        }

        private void LibDefaultSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            LibDefaultSelectors = (ListBox)sender;
            LibDefaultSelectors.ItemsSource = DefaultSelectors;
        }

        private void LibCustomSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            LibCustomSelectors = (ListBox)sender;
            LibCustomSelectors.ItemsSource = CustomSelectors;
        }

        private void BtnExportSelectorGroup_Click(object sender, RoutedEventArgs e)
        {
            Windows.ExportSelectorGroup exportSelectorGroup = new Windows.ExportSelectorGroup()
            {
                DataContext = (SelectorGroup)((Button)sender).DataContext,
                SelectorGroup = (SelectorGroup)((Button)sender).DataContext
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
            AddSelectorsToSelectorGroup(LibDefaultSelectors.SelectedItems, SelectorGroup);

            AddSelectorsToSelectorGroup(LibCustomSelectors.SelectedItems, SelectorGroup);

            AddSelectorsToSelectorGroup(Selectors, SelectorGroup);

            AddSelectorsToCustomSelectors(Selectors, CustomSelectors);

            TextBox.Text = "";
        }

        private void AddSelectorsToSelectorGroup(IList selectors, SelectorGroup selectorGroup)
        {
            if (selectors != null && selectors.Count > 0)
            {
                foreach (var selector in selectors)
                {
                    if (!string.IsNullOrEmpty(selector.ToString()) && SelectorGroup.Selectors.Any(s => s.CSSSelector.Equals(selector)) == false)
                    {
                        SelectorGroup.Selectors.Add(new Selector(selector.ToString()));
                    }
                }
            }
        }

        private void AddSelectorsToCustomSelectors(string[] selectors, ObservableCollection<string> customSelectors)
        {
            if (selectors != null && selectors.Length > 0)
            {
                foreach (var selector in selectors)
                {
                    if (!string.IsNullOrEmpty(selector) && customSelectors.Any(s => s.Equals(selector)) == false)
                    {
                        customSelectors.Add(selector);
                    }
                }
            }
        }

        private void ChkSelectAllDefaultSelectors_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var item in LibDefaultSelectors.ItemsSource)
                {
                    LibDefaultSelectors.SelectedItems.Add(item);
                }
            }
        }

        private void ChkSelectAllDefaultSelectors_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                LibDefaultSelectors.SelectedItems.Clear();
            }
        }

        private void ChkSelectAllCustomSelectors_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var item in LibCustomSelectors.ItemsSource)
                {
                    LibCustomSelectors.SelectedItems.Add(item);
                }
            }
        }

        private void ChkSelectAllCustomSelectors_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                LibCustomSelectors.SelectedItems.Clear();
            }
        }

        private void TcSelectorGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectorGroup = (SelectorGroup)((TabControl)sender).SelectedItem;
        }

        private void TxtSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox = (TextBox)sender;
        }

        private void LibDefaultSelectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).IsMouseOver || ((ListBox)sender).IsKeyboardFocusWithin)
            {
                if (((ListBox)sender).SelectedItems.Count < ((ListBox)sender).Items.Count)
                {
                    ChkDefaultSelectors.IsChecked = false;
                }
                else
                {
                    ChkDefaultSelectors.IsChecked = true;
                }
            }
        }

        private void LibCustomSelectors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ListBox)sender).IsMouseOver || ((ListBox)sender).IsKeyboardFocusWithin)
            {
                if (((ListBox)sender).SelectedItems.Count < ((ListBox)sender).Items.Count)
                {
                    ChkCustomSelectors.IsChecked = false;
                }
                else
                {
                    ChkCustomSelectors.IsChecked = true;
                }
            }
        }

        private void ChkSelectAllCustomSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            ChkCustomSelectors = (CheckBox)sender;
        }

        private void ChkSelectAllDefaultSelectors_Loaded(object sender, RoutedEventArgs e)
        {
            ChkDefaultSelectors = (CheckBox)sender;
        }

        private void TcSelectorGroups_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var temp = e.OriginalSource;

            if (sender is TabControl tcsg && e.OriginalSource is SelectorGroup sg)
            {
                if (e.Key == System.Windows.Input.Key.Delete)
                {
                    MessageBoxResult result = MessageBox.Show("Wollen Sie den/die  Datensatz/Datensätze endgültig löschen?", null, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        //((Link)DataContext).SelectorGroups.RemoveAt(tcSelectorGroups.SelectedIndex);

                        MessageBox.Show("Datensatz gelöscht.");
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (sender is TabControl tcsl && e.OriginalSource is Selector se)
            {
                if (e.Key == System.Windows.Input.Key.Delete)
                {
                    MessageBoxResult result = MessageBox.Show("Wollen Sie den/die  Datensatz/Datensätze endgültig löschen?", null, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        //((Link)DataContext).SelectorGroups[tcSelectorGroups.SelectedIndex].Selectors.RemoveAt(0);

                        MessageBox.Show("Datensatz gelöscht.");
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}