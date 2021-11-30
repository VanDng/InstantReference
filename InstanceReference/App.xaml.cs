using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace InstanceReference
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow = window;

            var sourceManager = new SourceManager();
            sourceManager.Initialize(Global.Constant.LookupSourceFilePath);

            var displayManager = new DisplayManager(window.tabContainer);
            displayManager.Initialize(sourceManager.Sources);

            var lookupManager = new LookupManager();
            lookupManager.Intialize(sourceManager.Sources);
            lookupManager.OnLookupCompleted += async (lookupresult) =>
            {
                _ = Task.Run(() => displayManager.ShowLookupResult(lookupresult));
            };

            var dataTrigger = new DataTrigger();
            dataTrigger.OnDataArrived += (text) =>
            {
                lookupManager.Lookup(text);
            };

            var clipboard = new ClipboardWatcher();
            clipboard.OnTextArrived += (text) =>
            {
                dataTrigger.Push(text.Value);
            };

            window.Closed += (s, e) =>
            {
                lookupManager.Dispose();
                displayManager.Dispose();
            };
        }
    }
}
