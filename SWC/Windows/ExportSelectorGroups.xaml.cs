using SWC.Classes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SWC.Windows
{
    /// <summary>
    /// Interaktionslogik für ExportSelectorGroups.xaml
    /// </summary>
    public partial class ExportSelectorGroups : Window
    {
        private SelectorGroup SelectorGroup { get; set; }

        public ExportSelectorGroups()
        {
            InitializeComponent();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selectorGroup in (ObservableCollection<SelectorGroup>)((Button)sender).DataContext)
            {
                ExportFunctions.ExportSelectorGroupToCSV(selectorGroup);
            }
        }

        private void ChkAllExport_Loaded(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).DataContext = tcSelectorGroups.SelectedItem;
        }

        private void ChkAllExport_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var selector in ((SelectorGroup)((CheckBox)sender).DataContext).Selectors)
                {
                    selector.Export = true;
                }
            }
        }

        private void ChkAllExport_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var selector in ((SelectorGroup)((CheckBox)sender).DataContext).Selectors)
                {
                    selector.Export = false;
                }
            }
        }

        private void ChkExport_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                ((Selector)((CheckBox)sender).DataContext).Export = (bool)((CheckBox)sender).IsChecked;

                if (CheckIfEverySelectorsIsChecked(SelectorGroup))
                {
                    SelectorGroup.ExportAllSelectors = true;
                }
            }
        }

        private void ChkExport_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                ((Selector)((CheckBox)sender).DataContext).Export = (bool)((CheckBox)sender).IsChecked;

                SelectorGroup.ExportAllSelectors = false;
            }
        }

        private bool CheckIfEverySelectorsIsChecked(SelectorGroup selectorGroup)
        {
            foreach (var selector in selectorGroup.Selectors)
            {
                if (!selector.Export)
                    return false;
            }
            return true;
        }

        private void TcSelectorGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectorGroup = (SelectorGroup)((TabControl)sender).SelectedItem;
        }
    }
}
