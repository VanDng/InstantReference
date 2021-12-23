using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
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
            Global.Setup.Install();

            var configFile = Global.Constant.ConfigurationFilePath;
            if (File.Exists(configFile))
            {
                Global.ConfigurationManager.Load(configFile);
            }
            else
            {
                Global.ConfigurationManager.Save(configFile);
            }

            MainWindow window = new MainWindow();
            window.ChangeVisibility(Visibility.Hidden);

            var triggerWindow = new TriggerWindow();
            triggerWindow.Show();
            triggerWindow.OnTriggered += (w) =>
            {
                window.ChangeVisibility(Visibility.Visible);
            };
            triggerWindow.Closed += (o, s) =>
            {
                window.Close();
            };

            window.Owner = triggerWindow;
            window.Show();

            Exit += (o,e) => Global.ConfigurationManager.Save(Global.Constant.ConfigurationFilePath);
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow = window;

            var sourceManager = new SourceManager();
            sourceManager.Initialize(Global.Constant.LookupSourceFilePath);

            var displayManager = new DisplayManager(window.tabContainer);
            displayManager.Initialize(sourceManager.Sources);
            displayManager.OnMouseDown += (m) =>
            {
                if (m.RightButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    window.Dispatcher.Invoke(() =>
                    {
                        window.ChangeVisibility(Visibility.Hidden);
                    });
                }
            };

            var lookupManager = new LookupManager();
            lookupManager.Intialize(sourceManager.Sources);
            lookupManager.OnLookupCompleted += (lookupresult) =>
            {
                displayManager.ShowLookupResult(lookupresult);
                triggerWindow.SetStatus(TriggerStatus.LookupCompleted);
            };

            var dataTrigger = new DataTrigger();
            dataTrigger.OnDataArrived += (text) =>
            {
                triggerWindow.SetStatus(TriggerStatus.LookupInProgress);
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
