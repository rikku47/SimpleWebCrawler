using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using Microsoft.Win32;
using Newtonsoft.Json;

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

        Config config = null;
        ObservableCollection<Group> groups;
        ObservableCollection<string> customSelectors;

        private void btnCrawl_Click(object sender, RoutedEventArgs e)
        {
            //var progress = new Progress<ProgressCrawl>(ReportProgress);

            foreach (var group in groups)
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
            config = new Config();

            string configJSON = JsonConvert.SerializeObject(config);

            using (var sw = new StreamWriter("config.json"))
            {
                sw.WriteLine(configJSON);
            }
        }

        private void WriteConfig()
        {
            string configJSON = JsonConvert.SerializeObject(config);

            using (var sw = new StreamWriter("config.json"))
            {
                sw.WriteLine(configJSON);
            }
        }

        private void LoadConfig()
        {

            string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(pathIncludeAssembly);

            try
            {
                using (StreamReader sr = new StreamReader(path + "\\config.json"))
                {
                    String line = sr.ReadToEnd();
                    config = JsonConvert.DeserializeObject<Config>(line);
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
                    customSelectors = JsonConvert.DeserializeObject<ObservableCollection<string>>(line);
                    libCustomSelectors.ItemsSource = customSelectors;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void WriteCustomSelectors()
        {
            string customSelectorsJSON = JsonConvert.SerializeObject(customSelectors);

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
                    config = JsonConvert.DeserializeObject<Config>(line);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void WriteGroups()
        {

        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string pathIncludeAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(pathIncludeAssembly);

            if (File.Exists(path + "\\config.json"))
            {
                LoadConfig();
            }
            else
            {
                CreateConfig();
            }

            libDefaultSelectors.ItemsSource = typeof(TagNames).GetFields().Select(field => field.Name).ToList();

            tcGroups.ItemsSource = groups;


            ReadCustomSelectors();
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            WriteConfig();
            WriteCustomSelectors();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResultWindow resultWindow = new ResultWindow
            {
                Link = (Link)((Button)sender).DataContext
            };

            resultWindow.Show();
        }

        private void btnOpenFiles_Click(object sender, RoutedEventArgs e)
        {
            Stream[] myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "html files (*.htm, *html) | *.htm; *html";
            //openFileDialog.FilterIndex = 1;

            if(openFileDialog.ShowDialog() == true)
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

        }

        private void MiImport_Click(object sender, RoutedEventArgs e)
        {

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

        private void btnReadLinks_Click(object sender, RoutedEventArgs e)
        {
            string groupName = txtGroupName.Text;

            string[] links = txtLinks.Text.Split(';');

            if(links.Length > 0 && ((bool)chkSelectAllDefaultSelectors.IsChecked || libDefaultSelectors.SelectedItems.Count > 0 || libCustomSelectors.SelectedItems.Count > 0))
            {
                Group group = new Group(groupName);

                foreach (var linkAdress in links)
                {
                    Link link = new Link(linkAdress, (bool)chkSelectAllDefaultSelectors.IsChecked, libDefaultSelectors.SelectedItems, libCustomSelectors.SelectedItems);

                    group.Links.Add(link);
                }

                if (groups == null)
                {
                    groups = new ObservableCollection<Group>();
                }

                groups.Add(group);

                tcGroups.ItemsSource = groups;
            }
        }

        private void btnReadSelectors_Click(object sender, RoutedEventArgs e)
        {
            string[] selectors = txtSelectors.Text.Split(';');

            if (customSelectors == null)
            {
                customSelectors = new ObservableCollection<string>();
            }

            foreach (var selector in selectors)
            {
                customSelectors.Add(selector);
            }

            libCustomSelectors.ItemsSource = customSelectors;
        }
    }
}
