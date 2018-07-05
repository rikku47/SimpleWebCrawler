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

        private void ChkExport_Checked(object sender, RoutedEventArgs e)
        {
            ((Selector)((CheckBox)sender).DataContext).Export = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkExport_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Selector)((CheckBox)sender).DataContext).Export = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkAllExport_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var selector in ((SelectorGroup)((CheckBox)sender).DataContext).Selectors)
            {
                selector.Export = true;
            }
        }

        private void ChkAllExport_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var selector in ((SelectorGroup)((CheckBox)sender).DataContext).Selectors)
            {
                selector.Export = false;
            }
        }

        private void ChkAllExport_Loaded(object sender, RoutedEventArgs e)
        {
            ((CheckBox)sender).DataContext = tcSelectorGroups.SelectedItem;
        }
    }
}
