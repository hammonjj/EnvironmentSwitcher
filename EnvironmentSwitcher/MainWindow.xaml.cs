using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Microsoft.Win32;
using UPDLog.Utilities;

namespace EnvironmentSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string AdAgentServiceName = "";
        private const string SmProxyServiceName = "";
        private const string SessionManagerServiceName = "";

        private readonly RegistryKey _wow6432RegistryKey;

        public MainWindow()
        {
            InitializeComponent();

            _wow6432RegistryKey = Registry.LocalMachine.GetOrCreateRegistryKey(
                @"Software\Wow6432Node\Aventura", false);


            LoadActiveBuild();
            LoadRegistryKeys();
        }

        private void LoadActiveBuild()
        {
            var activePathKey = _wow6432RegistryKey.GetValue("Path");
            
            if (activePathKey == null) { return; }
            TxtActiveBuild.Text = Convert.ToString(activePathKey);
        }

        private void LoadRegistryKeys()
        {
            TxtBoxConnectionString.Text = "";
            TxtBoxLogHost.Text = "";
            TxtBoxPath.Text = "";
            TxtBoxSessionManagerAddress.Text = "";
            TxtBoxSessionManagerPort.Text = "";
            TxtBoxSqlDatabase.Text = "";
        }

        private void ChangeActiveBuild()
        {
            if (string.IsNullOrEmpty(TxtActiveBuild.Text))
            {
                //Build path can not be empty
                return;
            }

            if (!Directory.Exists(TxtActiveBuild.Text))
            {
                //Directory does not exist
                return;
            }

            //Change active build registry key
            _wow6432RegistryKey.SetValue("Path", TxtActiveBuild.Text);

            //Check which services exist

            //Stop services

            //Change service location(s)

            //Start services
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _wow6432RegistryKey.Close();
        }

        private static void StartService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                // ...
            }
        }

        private static void StopService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            try
            {
                var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch
            {
                // ...
            }
        }

        private static bool IsServiceInstalled(string serviceName)
        {
            var services = ServiceController.GetServices();
            return services.Any(service => service.ServiceName == serviceName);
        }
    }
}
