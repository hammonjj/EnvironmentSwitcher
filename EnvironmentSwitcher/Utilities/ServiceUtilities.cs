using System;
using System.Linq;
using System.ServiceProcess;

namespace EnvironmentSwitcher.Utilities
{
    class ServiceUtilities
    {
        public static bool IsServiceInstalled(string serviceName, string server = "")
        {
            var services = string.IsNullOrEmpty(server) ? 
                ServiceController.GetServices() : 
                ServiceController.GetServices(server);
            
            return services.Any(service => service.ServiceName == serviceName);
        }

        public static void StartService(string serviceName, int timeoutMilliseconds, string server = "")
        {
            var service = string.IsNullOrEmpty(server) ? 
                new ServiceController(serviceName) : 
                new ServiceController(serviceName, server);
            
            service.Start();
            service.WaitForStatus(
                ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }

        public static void StopService(string serviceName, int timeoutMilliseconds, string server = "")
        {
            var service = string.IsNullOrEmpty(server) ? 
                new ServiceController(serviceName) : 
                new ServiceController(serviceName, server);
            
            service.Stop();
            service.WaitForStatus(
                ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(timeoutMilliseconds));
        }
    }
}
