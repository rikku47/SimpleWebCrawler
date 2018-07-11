using AngleSharp;
using AngleSharp.Dom.Html;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SWC.Classes.SearchModel;

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
        GlobalSearch globalSearch { get; set; }

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

            if (CustomSelectors == null)
            {
                CustomSelectors = new ObservableCollection<string>();
            }

            libCustomSelectors.ItemsSource = CustomSelectors;

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

            libTest.DataContext = CustomSelectors;
            libTest.ItemsSource = CustomSelectors;
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

            foreach (var selector in selectors)
            {
                if (!selector.Equals("") && CustomSelectors.Any(s => s.Equals(selector)) == false)
                {
                    CustomSelectors.Add(selector);
                }
            }
        }

        private void MiDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Möchten Sie diesen Datensatz wirklich löschen?");
        }

        private void TcGroups_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is TabControl tc)
            {
                if (e.Key == System.Windows.Input.Key.Delete)
                {
                    MessageBoxResult result = MessageBox.Show("Wollen Sie die Gruppe endgültig löschen?", null, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        Groups.RemoveAt(tc.SelectedIndex);

                        WriteGroups();

                        MessageBox.Show("Datensatz gelöscht.");
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void TxtGroups_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string[] groups = txtGroups.Text.Split(';');

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                txtGroups.Text = "";

                foreach (var group in groups)
                {
                    if (!string.IsNullOrEmpty(group) && Groups.Any(g => g.Name.Equals(group)) == false)
                    {
                        Groups.Add(new Group(group));
                    }
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

            int countNewLinks = links.Length;

            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((TextBox)sender).Text = "";

                int countNewLinksAdded = 0;

                foreach (var link in links)
                {
                    if (((Group)((TextBox)sender).DataContext).Links.Any(l => l.Adress.Equals(link)) == false)
                    {
                        ((Group)((TextBox)sender).DataContext).Links.Add(new Link(link));

                        countNewLinksAdded++;
                    }
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

        private void CheckBox_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void CheckBox_SourceUpdated(object sender, DataTransferEventArgs e)
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

        private void TxtSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchInput = ((TextBox)sender).Text;

            Group group = new Group(((Group)tcGroups.SelectedItem).Name);

            foreach (var item in tcGroups.Items)
            {
                foreach (var link in ((Group)item).Links)
                {
                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        if (link.Adress.Contains(searchInput))
                        {
                            link.IsAdressFound = true;
                            link.IsFilterOut = false;
                        }
                        else
                        {
                            link.IsAdressFound = false;
                            link.IsFilterOut = true;
                        }

                        if (link.Encoding.Contains(searchInput))
                        {
                            link.IsEncodingFound = true;
                            link.IsFilterOut = false;
                        }
                        else
                        {
                            link.IsEncodingFound = false;
                            link.IsFilterOut = true;
                        }

                        if (link.Adress.Contains(searchInput))
                        {
                            link.IsAdressFound = true;
                            link.IsFilterOut = false;
                        }
                        else
                        {
                            link.IsAdressFound = false;
                            link.IsFilterOut = true;
                        }
                    }
                    else
                    {
                        link.IsAdressFound = false;
                        link.IsEncodingFound = false;
                        link.IsTotalSelectorsFound = false;
                        link.IsCreationDateFound = false;
                        link.IsFilterOut = false;
                    }
                }
            }



            //if (link.Encoding.Contains(searchInput))
            //{

            //}

            //if (link.TotalSelectors > 0 & link.TotalSelectors < 0)
            //{

            //}

            //foreach (var selectorGroup in link.SelectorGroups)
            //{
            //    foreach (var selector in selectorGroup.Selectors)
            //    {
            //        foreach (var result in selector.Results)
            //        {
            //            foreach (var item in result.Items)
            //            {
            //                if (item.Details.Text.Contains(searchInput))
            //                {

            //                }

            //                if (item.Details.InnerHTML.Contains(searchInput))
            //                {

            //                }

            //                if (item.Details.OuterHTML.Contains(searchInput))
            //                {

            //                }
            //            }
            //        }
            //    }
            //}
            //if(link.CreationDate > 0.0 & link.CreationDate < 0.0)
            //{

            //}
        }

        private void BtnGrabLinks_Click(object sender, RoutedEventArgs e)
        {
            string initLink = "http://www.mtv-grone.de";

            // -1 for infinity
            int level = 2;

            List<Classes.LinkS> _links = new List<Classes.LinkS>();

            _links.Add(new Classes.LinkS(initLink, level));

            DigIntoLinks(_links, level, _links, new HashSet<string>());
        }

        private static async System.Threading.Tasks.Task DigIntoLinks(List<Classes.LinkS> links, int level, List<Classes.LinkS> linksTest, HashSet<string> linksWithoutHir = null)
        {
            int count = 0;

            foreach (var link in links)
            {
                count++;

                if (link.Level > 0)
                {
                    var config = Configuration.Default.WithDefaultLoader();

                    var document = await BrowsingContext.New(config).OpenAsync(link.Link);

                    var linksG = document.QuerySelectorAll("a").OfType<IHtmlAnchorElement>().Select(a => a.Href).ToList();

                    --link.Level;

                    foreach (var linkS in linksG)
                    {
                        if (linksWithoutHir != null)
                        {
                            linksWithoutHir.Add(linkS);
                        }

                        link.LinksSilbing.Add(new Classes.LinkS(linkS, link.Level));
                    }

                    DigIntoLinks(link.LinksSilbing, link.Level, linksTest, linksWithoutHir);
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            Link link = new Link("www5678");

            var group = (Group)((Button)sender).DataContext;

            ((Button)sender).DataContext = null;
        }

        private void TxtSearchLink_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchInput = ((TextBox)sender).Text;

            Group group = new Group(((Group)tcGroups.SelectedItem).Name);

            foreach (var item in tcGroups.Items)
            {
                foreach (var link in ((Group)item).Links)
                {
                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        if (link.Adress.Contains(searchInput))
                        {
                            link.IsAdressFound = true;
                            link.IsFilterOut = false;
                        }
                        else
                        {
                            link.IsAdressFound = false;
                            link.IsFilterOut = true;
                        }
                    }
                    else
                    {
                        link.IsAdressFound = false;
                        link.IsFilterOut = false;
                    }
                }
            }
        }

        private void TxtSearchEncoding_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string searchInput = ((TextBox)sender).Text;

            Group group = new Group(((Group)tcGroups.SelectedItem).Name);

            foreach (var item in tcGroups.Items)
            {
                foreach (var link in ((Group)item).Links)
                {
                    if (!string.IsNullOrEmpty(searchInput))
                    {
                        if (link.Encoding.Contains(searchInput))
                        {
                            link.IsEncodingFound = true;
                            link.IsFilterOut = false;
                        }
                        else
                        {
                            link.IsEncodingFound = false;
                            link.IsFilterOut = true;
                        }
                    }
                    else
                    {
                        link.IsEncodingFound = false;
                        link.IsFilterOut = false;
                    }
                }
            }
        }

        private void ChkTest_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                foreach (var item in libTest.ItemsSource)
                {
                    libTest.SelectedItems.Add(item);
                }
            }

            e.Handled = true;
        }

        private void ChkTest_Unchecked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsFocused)
            {
                libTest.SelectedItems.Clear();
            }

            e.Handled = true;
        }

        private void DataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (sender is DataGrid dg)
            {
                DataGridRow dgr = (DataGridRow)(dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex));

                if (e.Key == System.Windows.Input.Key.Delete && !dgr.IsEditing)
                {
                    MessageBoxResult result = MessageBox.Show("Wollen Sie den/die  Datensatz/Datensätze endgültig löschen?", null, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Datensatz gelöscht.");
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void LibTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(((ListBox)sender).IsMouseOver || ((ListBox)sender).IsKeyboardFocusWithin)
            {
                if (((ListBox)sender).SelectedItems.Count < ((ListBox)sender).Items.Count)
                {
                    chkTest.IsChecked = false;
                }
                else
                {
                    chkTest.IsChecked = true;
                }
            }
        }
    }
}