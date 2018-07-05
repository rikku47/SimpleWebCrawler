using SWC.Classes;
using System.Windows;
using System.Windows.Controls;

namespace SWC.Windows
{
    /// <summary>
    /// Interaktionslogik für ExportSelectorGroup.xaml
    /// </summary>
    public partial class ExportSelectorGroup : Window
    {
        public ExportSelectorGroup()
        {
            InitializeComponent();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            SelectorGroup selectorGroup = (SelectorGroup)((Button)sender).DataContext;

            ExportFunctions.ExportSelectorGroupToCSV(selectorGroup);
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
            ((CheckBox)sender).DataContext = DataContext;
        }
    }
}
