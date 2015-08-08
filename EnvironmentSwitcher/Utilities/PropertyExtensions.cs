using System;
using System.Windows;

namespace EnvironmentSwitcher.Utilities
{
    public class PropertyExtensions
    {
        //Registry Key Property
        public static readonly DependencyProperty RegistryKeyProperty = DependencyProperty.RegisterAttached(
            "RegistryKey",
            typeof(string), 
            typeof(PropertyExtensions), 
            new FrameworkPropertyMetadata(null));

        public static string GetRegistryKey(UIElement element)
        {
            if (element == null) { throw new ArgumentNullException("element"); }
            return (string)element.GetValue(RegistryKeyProperty);
        }

        public static void SetRegistryKey(UIElement element, string value)
        {
            if (element == null) { throw new ArgumentNullException("element"); }
            element.SetValue(RegistryKeyProperty, value);
        }

        //Service Property
        public static readonly DependencyProperty ServiceProperty = DependencyProperty.RegisterAttached(
            "Service",
            typeof(string),
            typeof(PropertyExtensions),
            new FrameworkPropertyMetadata(null));

        public static string GetService(UIElement element)
        {
            if (element == null) { throw new ArgumentNullException("element"); }
            return (string)element.GetValue(ServiceProperty);
        }

        public static void SetService(UIElement element, string value)
        {
            if (element == null) { throw new ArgumentNullException("element"); }
            element.SetValue(ServiceProperty, value);
        }
    }
}
