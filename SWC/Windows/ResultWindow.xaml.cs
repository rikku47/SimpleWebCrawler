using System.Windows;
using System.Windows.Controls;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();
        }

        //private Link _link = null;

        //internal Link Link { get => _link; set => _link = value; }

        private void miInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //DataContext = Link;
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            ResultDetailsWindow resultDetailsWindow = new ResultDetailsWindow()
            {
                //Result = (Link.Selector.Result)((Button)sender).DataContext
            };

            resultDetailsWindow.Show();
        }
    }
}
