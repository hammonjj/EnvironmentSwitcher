using Microsoft.Win32;

namespace EnvironmentSwitcher.Utilities
{
    public static class RegistryExtensions
    {
        public static RegistryKey GetOrCreateRegistryKey(this RegistryKey root, string path, bool writeAcess)
        {
            return root.OpenSubKey(path, writeAcess) ?? root.CreateSubKey(path);
        }
    }
}
