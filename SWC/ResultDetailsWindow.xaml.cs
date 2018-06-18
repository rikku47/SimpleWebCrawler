using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private Link.Selector.Result _result = null;

        internal Link.Selector.Result Result { get => _result; set => _result = value; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Result;
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            WatchResultWindow watchResultWindow = new WatchResultWindow()
            {
                Item = (Link.Selector.Result.Item)((Button)sender).DataContext
            };

            watchResultWindow.Show();
        }
    }
}
