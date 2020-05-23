namespace CSVLint.Tools
{
    using Kbg.NppPluginNET;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;

    public class SettingsChangedEventArgs : CancelEventArgs
    {
        public SettingsChangedEventArgs(Settings oldSettings, Settings newSettings)
        {
            OldSettings = oldSettings;
            NewSettings = newSettings;
            Changed = new HashSet<string>();
            foreach (var propertyInfo in typeof(Settings).GetProperties())
            {
                var oldValue = propertyInfo.GetValue(oldSettings, null);
                var newValue = propertyInfo.GetValue(newSettings, null);
                if (!oldValue.Equals(newValue))
                {
                    Trace.TraceInformation($"Setting {propertyInfo.Name} has changed");
                    Changed.Add(propertyInfo.Name);
                }
            }
        }

        public HashSet<string> Changed { get; }
        public Settings OldSettings { get; }
        public Settings NewSettings { get; }
    }
}