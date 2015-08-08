using System;
using System.Linq;
using System.ServiceProcess;

namespace EnvironmentSwitcher.Utilities
{
    class ServiceUtilities
    {
        public static bool IsServiceInstalled(string serviceName)
        {
            var services = ServiceController.GetServices();
            return services.Any(service => service.ServiceName == serviceName);
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

            service.Start();
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            var service = new ServiceController(serviceName);
            var timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

            service.Stop();
            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        }
    }
}
