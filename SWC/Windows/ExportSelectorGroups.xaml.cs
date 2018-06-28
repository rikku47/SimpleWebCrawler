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
            foreach (var selectorGroup in (ObservableCollection<Link.SelectorGroup>)((Button)sender).DataContext)
            {
                ExportFunctions.ExportSelectorGroupToCSV(selectorGroup);
            }
        }
    }
}
