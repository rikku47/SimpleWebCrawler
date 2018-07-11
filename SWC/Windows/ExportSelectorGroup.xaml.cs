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

        public SelectorGroup SelectorGroup { get; set; }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            SelectorGroup selectorGroup = (SelectorGroup)((Button)sender).DataContext;

            ExportFunctions.ExportSelectorGroupToCSV(selectorGroup);
        }

        private void ChkAllExport_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var selector in SelectorGroup.Selectors)
                {
                    selector.Export = true;
                }
            }
        }

        private void ChkAllExport_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var selector in SelectorGroup.Selectors)
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
    }
}
