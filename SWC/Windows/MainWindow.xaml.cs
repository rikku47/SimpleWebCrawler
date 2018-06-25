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

        private Config _config;
        private ObservableCollection<Group> _groups;
        private ObservableCollection<string> _customSelectors;

        public Config Config { get => _config; set => _config = value; }
        internal ObservableCollection<Group> Groups { get => _groups; set => _groups = value; }
        public ObservableCollection<string> CustomSelectors { get => _customSelectors; set => _customSelectors = value; }

        private void btnCrawl_Click(object sender, RoutedEventArgs e)
        {
            //var progress = new Progress<ProgressCrawl>(ReportProgress);

            foreach (var group in Groups)
            {
                foreach (var link in group.Links)
                {
                    //implement bool to crawl

                    link.CrawlAsync();
                }
            }
        }

        private void ReportProgress(ProgressCrawl progress)
        {
            btnCrawl.IsEnabled = true;
        }

        private void CreateConfig()
        {
            Config = new Config();

            string configJSON = JsonConvert.SerializeObject(Config);

            using (var sw = new StreamWriter("config.json"))
            {
                sw.WriteLine(configJSON);
            }
        }

        private void WriteConfig()
        {
            string configJSON = JsonConvert.SerializeObject(Config);

            using (var sw = new StreamWriter("config.json"))
            {
                sw.WriteLine(configJSON);
            }
        }

        private void ReadConfig()
        {
            string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(pathIncludeAssembly);

            try
            {
                using (StreamReader sr = new StreamReader(path + "\\config.json"))
                {
                    String line = sr.ReadToEnd();
                    Config = JsonConvert.DeserializeObject<Config>(line);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ReadCustomSelectors()
        {
            string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(pathIncludeAssembly);

            try
            {
                using (StreamReader sr = new StreamReader(path + "\\customSelectors.json"))
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

        private void ReadGroups()
        {
            string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(pathIncludeAssembly);

            try
            {
                using (StreamReader sr = new StreamReader(path + "\\groups.json"))
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

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(pathIncludeAssembly);

            if (File.Exists(path + "\\config.json"))
            {
                ReadConfig();
            }
            else
            {
                CreateConfig();
            }

            if (File.Exists(path + "\\customSelectors.json"))
            {
                ReadCustomSelectors();
            }

            if (File.Exists(path + "\\groups.json"))
            {
                ReadGroups();
            }

            tcGroups.ItemsSource = Groups;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WriteConfig();
            WriteCustomSelectors();
            WriteGroups();
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            Stream[] myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "html files (*.htm, *html) | *.htm; *html";
            //openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFiles()) != null)
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

        private void MiFiles_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MiExport_Click(object sender, RoutedEventArgs e)
        {
            ExportWindow exportWindow = new ExportWindow();

            exportWindow.ShowDialog();
        }

        private void MiImport_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow importWindow = new ImportWindow()
            {
                CustomSelectors = CustomSelectors,
                Group = (Group)((MenuItem)sender).DataContext
            };

            importWindow.ShowDialog();
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

        private void btnReadSelectors_Click(object sender, RoutedEventArgs e)
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
            //libCustomSelectors.ItemsSource = _customSelectors;
        }

        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Möchten Sie diesen Datensatz wirklich löschen?");
        }

        private void tcGroups_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void DataGrid_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void MiEdit_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow()
            {
                Group = (Group)((MenuItem)sender).DataContext
            };
            editWindow.ShowDialog();
        }

        private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        private void txtLinks_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void btnShowSelectorGroups_Click(object sender, RoutedEventArgs e)
        {
            EditSelectorsWindow editSelectorsWindow = new EditSelectorsWindow()
            {
                Link = (Link)((Button)sender).DataContext,
                CustomSelectors = CustomSelectors
            };

            editSelectorsWindow.ShowDialog();
        }
    }
}