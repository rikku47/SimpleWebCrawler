using System.Windows;
using System.Windows.Controls;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        private bool _loaded = false; 

        public OptionWindow()
        {
            InitializeComponent();
        }

        private void CboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_loaded && ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.Equals("Englisch"))
            {
                Properties.Settings.Default.Language = "en-US";
            }
            else if(_loaded)
            {
                Properties.Settings.Default.Language = "de-DE";
            }

            Properties.Settings.Default.Save();
        }

        private void CboLanguage_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Language.Equals("en-US"))
            {
                ((ComboBox)sender).SelectedItem = cboIEnglish;
            }
            else
            {
                ((ComboBox)sender).SelectedItem = cboIGerman;
            }

            _loaded = true;
        }
    }
}
