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
            Link.SelectorGroup selectorGroup = (Link.SelectorGroup)((Button)sender).DataContext;

            ExportFunctions.ExportSelectorGroupToCSV(selectorGroup);
        }
    }
}
