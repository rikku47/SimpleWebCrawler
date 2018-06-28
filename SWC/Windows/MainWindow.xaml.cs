using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        string AppPath { get; set; }
        public Config Config { get; set; }
        internal ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<string> CustomSelectors { get; set; }

        private void BtnCrawl_Click(object sender, RoutedEventArgs e)
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

        private void ReadConfig(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
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
            AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string configPath = AppPath + "\\config.json";
            string customSelectorsPath = AppPath + "\\customSelectors.json";
            string groupsPath = AppPath + "\\groups.json";
            string exportPath = AppPath + "\\export";

            if (File.Exists(configPath))
            {
                ReadConfig(configPath);
            }
            else
            {
                CreateConfig();
            }

            if (File.Exists(customSelectorsPath))
            {
                ReadCustomSelectors(customSelectorsPath);
            }

            if (File.Exists(groupsPath))
            {
                ReadGroups(groupsPath);
            }

            if(!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            if(Groups == null)
            {
                Groups = new ObservableCollection<Group>();
            }

            tcGroups.ItemsSource = Groups;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WriteConfig();
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

        private void MiFiles_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MiExport_Click(object sender, RoutedEventArgs e)
        {
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

        private void MiEdit_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow()
            {
                Group = (Group)((MenuItem)sender).DataContext
            };
            editWindow.ShowDialog();
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
                Link = (Link)((Button)sender).DataContext,
                CustomSelectors = CustomSelectors
            };

            editSelectorsWindow.ShowDialog();
        }
    }
}