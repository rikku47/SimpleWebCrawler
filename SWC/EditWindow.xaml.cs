using System.Windows;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        private Group group;
        

        internal Group Group { get => group; set => group = value; }
       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Group;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
