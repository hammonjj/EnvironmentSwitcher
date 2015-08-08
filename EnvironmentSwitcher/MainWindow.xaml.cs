using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

using EnvironmentSwitcher.Utilities;

namespace EnvironmentSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string SmProxyServiceName = "AVSM";
        private const string AdAgentServiceName = "AVADAgent";
        private const string SessionManagerServiceName = "AVSMProxySvc";

        private readonly RegistryKey _wow6432RegistryKey;

        public MainWindow()
        {
            InitializeComponent();

            _wow6432RegistryKey = Registry.LocalMachine.GetOrCreateRegistryKey(
                @"Software\Wow6432Node\Aventura", true);

            LoadRegistryKeys();
        }

        private void LoadRegistryKeys()
        {
            TxtActiveBuild.Text = _wow6432RegistryKey.GetValueAsString("Path");
            TxtBoxLogHost.Text = _wow6432RegistryKey.GetValueAsString("logHost");

            using (var datastoreKey = _wow6432RegistryKey.GetOrCreateRegistryKey(@"Datastore", false))
            {
                TxtBoxSqlDatabase.Text = datastoreKey.GetValueAsString("sqlDatabase");
                TxtBoxConnectionString.Text = datastoreKey.GetValueAsString("ConnectionStr");
            }

            using (var sessionManagerKey = _wow6432RegistryKey.GetOrCreateRegistryKey(@"SessionManager", false))
            {
                TxtBoxSessionManagerPort.Text = sessionManagerKey.GetValueAsString("port");
            }

            using (var agentsKey = _wow6432RegistryKey.GetOrCreateRegistryKey(@"Agents", false))
            {
                TxtBoxSessionManagerAddress.Text = agentsKey.GetValueAsString("SessionManager");
            }
        }

        private void ApplyActiveBuildClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtActiveBuild.Text))
            {
                MessageBox.Show("Build path can not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(TxtActiveBuild.Text))
            {
                MessageBox.Show("Directory does not exist", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //Change root directory
            _wow6432RegistryKey.SetValue("Path", TxtActiveBuild.Text);

            //Stop, change location and start service
            EditServiceLocation(AdAgentServiceName, TxtActiveBuild.Text + @"ADAgent\ADAgent.exe");
            EditServiceLocation(SmProxyServiceName, TxtActiveBuild.Text + @"AVSMProxySvc\AVSMProxySvc.exe");
            EditServiceLocation(SessionManagerServiceName, TxtActiveBuild.Text + @"SessionManager\TISessionManager.exe");

            //Save path for future switches
        }

        private static void EditServiceLocation(string serviceName, string newLocation)
        {
            if (!ServiceUtilities.IsServiceInstalled(serviceName)) { return; }

            try
            {
                ServiceUtilities.StopService(serviceName, 10000);

                using(var serviceKey = Registry.LocalMachine.GetOrCreateRegistryKey(
                    @"System\CurrentControlSet\Services\" + serviceName, true))
                {
                    serviceKey.SetValue("ImagePath", newLocation);
                }

                ServiceUtilities.StartService(serviceName, 10000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error editing " + serviceName + "service: " + ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ApplyRegistryKeysClicked(object sender, RoutedEventArgs e)
        {
            _wow6432RegistryKey.SetValue("logHost", TxtBoxLogHost.Text);

            using (var datastoreKey = _wow6432RegistryKey.GetOrCreateRegistryKey(@"Datastore", true))
            {
                datastoreKey.SetValue("sqlDatabase", TxtBoxSqlDatabase.Text);
                datastoreKey.SetValue("ConnectionStr", TxtBoxConnectionString.Text);
            }

            using (var sessionManagerKey = _wow6432RegistryKey.GetOrCreateRegistryKey(@"SessionManager", true))
            {
                sessionManagerKey.SetValue("port", TxtBoxSessionManagerPort.Text);
            }

            using (var agentsKey = _wow6432RegistryKey.GetOrCreateRegistryKey(@"Agents", true))
            {
                agentsKey.SetValue("SessionManager", TxtBoxSessionManagerAddress.Text);
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _wow6432RegistryKey.Close();
        }
    }
}