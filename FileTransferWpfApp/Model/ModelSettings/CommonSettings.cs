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
using FileTransferWpfApp.View.UserView;
using FileTransferWpfApp.Model.ModelLogs;
using NLog;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace FileTransferWpfApp.Model.ModelSettings
{
    public class CommonSettings
    {
        private static string settingsFilePath = string.Empty;

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static readonly XmlSerializer xmlSerializer = new(typeof(CommonSettings));

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
                FileInfo fileInfo = new FileInfo(CurrentFile);

                settingsFilePath = fileInfo.Directory + $"\\{Path.GetFileNameWithoutExtension(fileInfo.FullName)}Settings.xml";

                if (!File.Exists(settingsFilePath))
                {
                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        instance?.OnMessageToShow(PhraseModel.GetPhrase(0));
                    });

                    DataWarehouseModel.AddUILog(new UILogModel(PhraseModel.GetPhrase(0),
                        DataWarehouseModel.ImportanceLogs.medium));

                    instance = GetDefaultSettings();

                    DirectorySettingsWindow directorySettingsWindow = new DirectorySettingsWindow();

                    await Application.Current.Dispatcher.InvokeAsync(() => 
                    {
                        instance.OnWindowToShow(directorySettingsWindow);
                    });

                    WriteDefaultSettings();

                    return false;
                }
                else
                {
                    ReadSettings();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                return false;
            }
        }
        private static CommonSettings GetDefaultSettings() 
        {

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
                return instance;
        }
        private static void WriteDefaultSettings() 
        {
            using (TextWriter writer = new StreamWriter(settingsFilePath))
            {
                xmlSerializer.Serialize(writer, instance);
            }
        }
        private static void ReadSettings() 
        {
            DataWarehouseModel.AddUILog(new UILogModel(PhraseModel.GetPhrase(1),
                DataWarehouseModel.ImportanceLogs.low));

            using (Stream reader = new FileStream(settingsFilePath, FileMode.Open))
            {
                instance = (CommonSettings)xmlSerializer.Deserialize(reader);
            }
        }

        public event EventHandler<string> MessageToShow;

        public event EventHandler<Window> WindowToShow;

        private void OnMessageToShow(string message) 
        {
            MessageToShow?.Invoke(this, message);
        }
        private void OnWindowToShow(Window window) 
        {
            WindowToShow?.Invoke(this, window);
        }
    }
}
