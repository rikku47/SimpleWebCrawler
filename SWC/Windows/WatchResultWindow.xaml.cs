using SWC.Classes;
using System.Windows;
using System.Windows.Controls;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für ResultDetailsWindow.xaml
    /// </summary>
    public partial class ResultDetailsWindow : Window
    {
        public ResultDetailsWindow()
        {
            InitializeComponent();
        }

        internal Result Result { get; set; } = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Result;
        }

        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            WatchResultWindow watchResultWindow = new WatchResultWindow()
            {
                Item = (Item)((Button)sender).DataContext
            };

            watchResultWindow.Show();
        }
    }
}
