﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using FileTransferWpf.Data.Entity;
using FileTransferWpf.Tools.Entity;
using FileTransferWpfApp.UserInterfaceClasses.UserWindows;
using NLog;


namespace FileTransferWpf.Tools
{
    public class CommonSettings
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public bool Visible { get; set; }

        #region
        private static CommonSettings? instance;
        public static CommonSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = new CommonSettings();
                return instance;
            }
        }
        #endregion

        public List<DirectorySettings> Directories { get; set; } = new List<DirectorySettings>();

        [XmlIgnore]
        public static string CurrentDirectory => AppDomain.CurrentDomain.BaseDirectory;

        [XmlIgnore]
        public static string CurrentFile => Assembly.GetExecutingAssembly().Location;

        public static async Task<bool> LoadSettings()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(CommonSettings));

                FileInfo fileInfo = new FileInfo(CurrentFile);

                string settingsFilePath = fileInfo.Directory + $"\\{Path.GetFileNameWithoutExtension(fileInfo.FullName)}Settings.xml";

                if (!File.Exists(settingsFilePath))
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        MessageBox.Show($"Файла настроек [{settingsFilePath}] не существует. Давайте создадим его");
                    });

                    DataWarehouse.AddAndUpdateLogInterface(new ScreenLog($"Файла настроек [{settingsFilePath}] не существует. Давайте создадим его",
                        DataWarehouse.ImportanceLogs.medium));

                    DirectorySettingsWindow directorySettingsWindow = new DirectorySettingsWindow();

                    directorySettingsWindow.ShowDialog();

                    instance = new CommonSettings()
                    {
                        Visible = true,

                        Directories = new List<DirectorySettings>()
                        {
                            new DirectorySettings()
                            {
                                DeviceName = "test",

                                FileFilterMask = "*.*",

                                MoveFromPath = @"C:\Users\RemAd\Documents\testpapka",

                                MoveToPaths = new string[] { @"C:\Users\RemAd\Documents\testpapka1" }
                            }
                        }
                    };
                    using (TextWriter writer = new StreamWriter(settingsFilePath))
                    {
                        xmlSerializer.Serialize(writer, instance);
                    }

                    return false;
                }
                else
                {
                    DataWarehouse.AddAndUpdateLogInterface(new ScreenLog($"Файл настроек [{settingsFilePath}] найден. Чтение настроек",
                        DataWarehouse.ImportanceLogs.low));

                    using (Stream reader = new FileStream(settingsFilePath, FileMode.Open))
                    {
                        instance = (CommonSettings)xmlSerializer.Deserialize(reader);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return false;
            }
        }

    }
}
