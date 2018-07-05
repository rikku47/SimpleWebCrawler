using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace SWC
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(SWC.Properties.Settings.Default.Language);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SWC.Properties.Settings.Default.Language);

            base.OnStartup(e);
        }
    }
}
