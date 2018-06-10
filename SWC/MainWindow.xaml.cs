using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

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

        private void btnCrawl_Click(object sender, RoutedEventArgs e)
        {
            


            btnCrawl.IsEnabled = false;

            pgbProgress.Value = 0;

            var url = txtUrlOrFile.Text;

            var selectors = txtSelectors.Text;

            var outputPath = txtOutput.Text;

            config.lastUrlOrFile = url;

            config.outputPath = outputPath;

            pgbProgress.Maximum = 1;

            var progress = new Progress<ProgressCrawl>(ReportProgress);

            string text = "";

            try
            {
                text = File.ReadAllText(url);
            }
            catch (Exception)
            {

                //MessageBox.Show(ex.Message);
            }

            Crawl(url, selectors, outputPath, progress, text);
        }

        private static async Task Crawl(string url, string selectors, string outputPath, IProgress<ProgressCrawl> progress, string text)
        {
            var config = Configuration.Default.WithDefaultLoader();

            IDocument document = null;

            if (text.Length > 0)
            {
                var parser = new HtmlParser();

                document = parser.Parse(text);
            }
            else
            {
                document = await BrowsingContext.New(config).OpenAsync(url);
            }

            var cells = document.QuerySelectorAll(selectors);

            var output = cells.Select(m => m.TextContent);

            using (var sw = new StreamWriter(outputPath))
            {
                foreach (var line in output)
                {
                    sw.WriteLine(line);
                }
            }

            progress.Report(new ProgressCrawl { currentProgress = 1, totalProgress = 1 });
        }

        private void ReportProgress(ProgressCrawl progress)
        {
            pgbProgress.Value = progress.currentProgress;

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

            txtUrlOrFile.Text = config.lastUrlOrFile;
            txtSelectors.Text = config.lastSelectors;
            txtOutput.Text = config.outputPath;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            config.lastUrlOrFile = txtUrlOrFile.Text;
            config.lastSelectors = txtSelectors.Text;
            config.outputPath = txtOutput.Text;

            WriteConfig();
        }
    }
}
