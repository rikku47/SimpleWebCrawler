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
    }
}
