using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSVLint.Tools;
using Kbg.NppPluginNET;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace CsvQuery.PluginInfrastructure
{
    public class SettingsBase
    {
        private const int DEFAULT_WIDTH = 350;
        private const int DEFAULT_HEIGHT = 450;

        private static readonly string IniFilePath;

        /// <summary> Delegate for update events </summary>
        public delegate void RepositoryEventHandler(object sender, SettingsChangedEventArgs e);

        /// <summary> Raised before settings has been changed, allowing listeners to cancel the change </summary>
        public event RepositoryEventHandler ValidateChanges;

        /// <summary> Raised after a setting has been changed </summary>
        public event RepositoryEventHandler SettingsChanged;

        /// <summary> Overridable event logic </summary>
        protected virtual void OnSettingsChanged(object sender, SettingsChangedEventArgs e)
        {
            SettingsChanged?.Invoke(sender, e);
        }

        /// <summary> Overridable event logic </summary>
        protected virtual bool OnValidateChanges(object sender, SettingsChangedEventArgs e)
        {
            ValidateChanges?.Invoke(sender, e);
            return !e.Cancel;
        }

        static SettingsBase()
        {
            // Figure out default N++ config file path
            // Path is usually -> .\Users\<username>\AppData\Roaming\Notepad++\plugins\config\
            var sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            var configDirectory = sbIniFilePath.ToString();
            IniFilePath = Path.Combine(configDirectory, Main.PluginName + ".ini");
        }

        /// <summary>
        /// By default loads settings from the default N++ config folder
        /// </summary>
        /// <param name="loadFromFile"> If false will not load anything and have default values set </param>
        public SettingsBase(bool loadFromFile = true)
        {
            // Set defaults
            foreach (var propertyInfo in GetType().GetProperties())
            {
                if (propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault() is DefaultValueAttribute def)
                {
                    propertyInfo.SetValue(this, def.Value, null);
                }
            }
            if (loadFromFile) ReadFromIniFile();
        }

        /// <summary>
        /// Reads all (existing) settings from an ini-file
        /// </summary>
        /// <param name="filename">File to write to (default is N++ plugin config)</param>
        public void ReadFromIniFile(string filename = null)
        {
            filename = filename ?? IniFilePath;
            if (!File.Exists(filename)) return;

            // Load all sections from file
            var loaded = GetType().GetProperties()
                .Select(x => ((CategoryAttribute)x.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault())?.Category ?? "General")
                .Distinct()
                .ToDictionary(section => section, section => GetKeys(filename, section));

            //var loaded = GetKeys(filename, "General");
            foreach (var propertyInfo in GetType().GetProperties())
            {
                var category = ((CategoryAttribute)propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), false).FirstOrDefault())?.Category ?? "General";
                var name = propertyInfo.Name;
                if (loaded.ContainsKey(category) && loaded[category].ContainsKey(name) && !string.IsNullOrEmpty(loaded[category][name]))
                {
                    var rawString = loaded[category][name];
                    var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                    if (converter.IsValid(rawString))
                    {
                        propertyInfo.SetValue(this, converter.ConvertFromString(rawString), null);
                    }
                }
            }
        }

        /// <summary>
        /// Saves all settings to an ini-file, under "General" section
        /// </summary>
        /// <param name="filename">File to write to (default is N++ plugin config)</param>
        public void SaveToIniFile(string filename = null)
        {
            filename = filename ?? IniFilePath;
            var dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            // Win32.WritePrivateProfileSection (that NppPlugin uses) doesn't work well with non-ASCII characters. So we roll our own.
            using (var fp = new StreamWriter(filename, false, Encoding.UTF8))
            {
                fp.WriteLine("; {0} settings file", Main.PluginName);

                foreach (var section in GetType()
                    .GetProperties()
                    .GroupBy(x => ((CategoryAttribute)x.GetCustomAttributes(typeof(CategoryAttribute), false)
                                        .FirstOrDefault())?.Category ?? "General"))
                {
                    fp.WriteLine(Environment.NewLine + "[{0}]", section.Key);
                    foreach (var propertyInfo in section.OrderBy(x => x.Name))
                    {
                        if (propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute description)
                            fp.WriteLine("; " + description.Description.Replace(Environment.NewLine, Environment.NewLine + "; "));
                        var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                        fp.WriteLine("{0}={1}", propertyInfo.Name, converter.ConvertToInvariantString(propertyInfo.GetValue(this, null)));
                    }
                }
            }
        }

        /// <summary>
        /// Read a section from an ini-file
        /// </summary>
        /// <param name="iniFile">Path to ini-file</param>
        /// <param name="category">Section to read</param>
        private Dictionary<string, string> GetKeys(string iniFile, string category)
        {
            var buffer = new byte[8 * 1024];

            Win32.GetPrivateProfileSection(category, buffer, buffer.Length, iniFile);
            var tmp = Encoding.UTF8.GetString(buffer).Trim('\0').Split('\0');
            return tmp.Select(x => x.Split(new[] { '=' }, 2))
                .Where(x => x.Length == 2)
                .ToDictionary(x => x[0], x => x[1]);
        }

        /// <summary>
        /// Opens a window that edits all settings
        /// </summary>
        public void ShowDialog()
        {
            // We bind a copy of this object and only apply it after they click "Ok"
            var copy = (Settings)MemberwiseClone();

            var dialog = new Form
            {
                Text = "Settings",
                ClientSize = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT),
                MinimumSize = new Size(250, 250),
                ShowIcon = false,
                AutoScaleMode = AutoScaleMode.Font,
                AutoScaleDimensions = new SizeF(6F, 13F),
                Controls =
            {
                new Button
                {
                    Name = "Cancel",
                    Text = "&Cancel",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Size = new Size(75, 23),
                    Location = new Point(DEFAULT_WIDTH - 75 - 13, DEFAULT_HEIGHT - 23 - 13),
                    UseVisualStyleBackColor = true
                },
                new Button
                {
                    Name = "Ok",
                    Text = "&Ok",
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Size = new Size(75, 23),
                    Location = new Point(DEFAULT_WIDTH - 75 - 13 - 81, DEFAULT_HEIGHT - 23 - 13),
                    UseVisualStyleBackColor = true
                },
                new PropertyGrid
                {
                    Name = "Grid",
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    Location = new Point(13, 13),
                    Size = new Size(DEFAULT_WIDTH - 13 - 13, DEFAULT_HEIGHT - 55),
                    AutoScaleMode = AutoScaleMode.Font,
                    AutoScaleDimensions = new SizeF(6F,13F),
                    SelectedObject = copy
                }
            }
            };

            dialog.Controls["Cancel"].Click += (a, b) => dialog.Close();
            dialog.Controls["Ok"].Click += (a, b) =>
            {
                var changesEventArgs = new SettingsChangedEventArgs((Settings)this, copy);
                if (!changesEventArgs.Changed.Any())
                {
                    dialog.Close();
                    return;
                }
                var acceptable = OnValidateChanges(dialog, changesEventArgs);
                if (!acceptable)
                {
                    dialog.Close();
                    return;
                }
                copy.SaveToIniFile();

                // Copy all changed settings to this
                foreach (var propertyInfo in GetType().GetProperties())
                {
                    var oldValue = propertyInfo.GetValue(this, null);
                    var newValue = propertyInfo.GetValue(copy, null);
                    if (!oldValue.Equals(newValue))
                        propertyInfo.SetValue(this, newValue, null);
                }
                OnSettingsChanged(dialog, changesEventArgs);
                dialog.Close();
            };

            dialog.ShowDialog();
        }

        /// <summary> Opens the config file directly in Notepad++ </summary>
        public void OpenFile()
        {
            if (!File.Exists(IniFilePath)) SaveToIniFile();
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DOOPEN, 0, IniFilePath);
        }
    }
}