using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<string> CustomSelectors { get; set; }

        private void BtnCrawl_Click(object sender, RoutedEventArgs e)
        {
            foreach (var group in Groups)
            {
                foreach (var link in group.Links)
                {
                    CalculateTotalSelectorsOfALink(link);

                    link.CrawlSelectors();
                }
            }
        }

        private void ReportProgress(ProgressCrawl progress)
        {
            btnCrawl.IsEnabled = true; 
        }

        private void ReadCustomSelectors(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String line = sr.ReadToEnd();
                    CustomSelectors = JsonConvert.DeserializeObject<ObservableCollection<string>>(line);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void WriteCustomSelectors()
        {
            string customSelectorsJSON = JsonConvert.SerializeObject(CustomSelectors);

            using (var sw = new StreamWriter("customSelectors.json"))
            {
                sw.WriteLine(customSelectorsJSON);
            }
        }

        private void ReadGroups(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    String line = sr.ReadToEnd();
                    Groups = JsonConvert.DeserializeObject<ObservableCollection<Group>>(line);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void WriteGroups()
        {
            string groupsJSON = JsonConvert.SerializeObject(Groups);

            using (var sw = new StreamWriter("groups.json"))
            {
                sw.WriteLine(groupsJSON);
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string customSelectorsPathWithFile = appPath + "\\customSelectors.json";
            string groupsPathWithFile = appPath + "\\groups.json";
            string exportPath = appPath + "\\export";

            if (File.Exists(customSelectorsPathWithFile))
            {
                ReadCustomSelectors(customSelectorsPathWithFile);
            }

            if (File.Exists(groupsPathWithFile))
            {
                ReadGroups(groupsPathWithFile);
            }

            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            if (Groups == null)
            {
                Groups = new ObservableCollection<Group>();
            }

            tcGroups.ItemsSource = Groups;

            if (tcGroups != null && tcGroups.Items.Count > 0)
            {
                tcGroups.SelectedIndex = 0;

                CalculateTotalSelectorsOfALinkOfAllGroups();
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WriteCustomSelectors();
            WriteGroups();
        }

        private void BtnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            if (new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Multiselect = true,
                Filter = "html files (*.htm, *html) | *.htm; *html"
            }.ShowDialog() == true)
            {
                try
                {
                    Stream[] myStream = null;
                    if ((myStream = new OpenFileDialog
                    {
                        InitialDirectory = "c:\\",
                        Multiselect = true,
                        Filter = "html files (*.htm, *html) | *.htm; *html"
                    }.OpenFiles()) != null)
                    {
                        //using (myStream)
                        {
                            foreach (var stream in myStream)
                            {
                                var temp = stream;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Datei(en) konnte(n) nicht eingelesen werden: " + ex.Message);
                }
            }
        }

        private void MiInfo_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void MiClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MiOption_Click(object sender, RoutedEventArgs e)
        {
            OptionWindow optionWindow = new OptionWindow();

            optionWindow.Show();
        }

        private void BtnReadSelectors_Click(object sender, RoutedEventArgs e)
        {
            string[] selectors = txtSelectors.Text.Split(';');

            if (CustomSelectors == null)
            {
                CustomSelectors = new ObservableCollection<string>();
            }

            foreach (var selector in selectors)
            {
                if (!selector.Equals(""))
                    CustomSelectors.Add(selector);
            }
        }

        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Möchten Sie diesen Datensatz wirklich löschen?");
        }

        private void TcGroups_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void DataGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void TxtGroups_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ObservableCollection<Group> tempGroups = new ObservableCollection<Group>();

            string[] groups = txtGroups.Text.Split(';');

            foreach (var group in groups)
            {
                tempGroups.Add(new Group(group));
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtGroups.Text = "";

                foreach (var group in tempGroups)
                {
                    Groups.Add(group);
                }
            }

            if (Groups.Count > 0 && Groups[0] == null)
                Groups = null;

            tcGroups.ItemsSource = Groups;
        }

        private void TxtLinks_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ObservableCollection<Link> tempLinks = new ObservableCollection<Link>();

            string[] links = ((TextBox)e.OriginalSource).Text.Split(';');

            foreach (var link in links)
            {
                tempLinks.Add(new Link(link));
            }

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((TextBox)sender).Text = "";

                foreach (var link in tempLinks)
                {
                    ((Group)((TextBox)sender).DataContext).Links.Add(link);
                }
            }
        }

        private void BtnShowSelectorGroups_Click(object sender, RoutedEventArgs e)
        {
            EditSelectorsWindow editSelectorsWindow = new EditSelectorsWindow()
            {
                DataContext = (Link)((Button)sender).DataContext,
                CustomSelectors = CustomSelectors
            };

            editSelectorsWindow.ShowDialog();

            CalculateTotalSelectorsOfALink((Link)editSelectorsWindow.DataContext);
        }

        private void CalculateTotalSelectorsOfALinkOfAllGroups()
        {
            foreach (var group in Groups)
            {
                foreach (var link in group.Links)
                {
                    link.TotalSelectors = 0;

                    foreach (var selectorGroup in link.SelectorGroups)
                    {
                        link.TotalSelectors = link.TotalSelectors + selectorGroup.Selectors.Count;
                    }
                }
            }
        }

        private void CalculateTotalSelectorsOfALinkOfAGroup(Group group)
        {
            foreach (var link in group.Links)
            {
                link.TotalSelectors = 0;

                foreach (var selectorGroup in link.SelectorGroups)
                {
                    link.TotalSelectors = link.TotalSelectors + selectorGroup.Selectors.Count;
                }
            }
        }

        private void CalculateTotalSelectorsOfALink(Link link)
        {
            link.TotalSelectors = 0;

            foreach (var selectorGroup in link.SelectorGroups)
            {
                link.TotalSelectors = link.TotalSelectors + selectorGroup.Selectors.Count;
            }
        }

        private void CheckBox_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }

        private void CheckBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }

        private void ChkTrim_Checked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).IsTrim = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkTrim_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).IsTrim = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkCrawl_Checked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).IsCrawl = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkCrawl_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).IsCrawl = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkText_Checked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).CrawlText = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkText_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).CrawlText = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkInnerHTML_Checked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).CrawlInnerHTML = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkInnerHTML_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).CrawlInnerHTML = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkOuterHTML_Checked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).CrawlOuterHTML = (bool)((CheckBox)sender).IsChecked;
        }

        private void ChkOuterHTML_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Link)((CheckBox)sender).DataContext).CrawlOuterHTML = (bool)((CheckBox)sender).IsChecked;
        }
    }
}