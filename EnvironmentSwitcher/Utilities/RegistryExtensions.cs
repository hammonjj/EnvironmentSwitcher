using System;
using Microsoft.Win32;

namespace EnvironmentSwitcher.Utilities
{
    public static class RegistryExtensions
    {
        public static string GetValueAsString(this RegistryKey root, string value)
        {
            var valueKey = root.GetValue(value);
            return valueKey == null ? "" : Convert.ToString(valueKey);
        }

        public static RegistryKey GetOrCreateRegistryKey(this RegistryKey root, string path, bool writeAcess)
        {
            return root.OpenSubKey(path, writeAcess) ?? root.CreateSubKey(path);
        }
    }
}
