using FileTransferWpf.Tools;
using FileTransferWpfApp.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autofac;
using System.IO;

namespace FileTransferWpfApp.UserInterfaceClasses.UserWindows
{
    /// <summary>
    /// Interaction logic for DirectorySettingsWindow.xaml
    /// </summary>
    public partial class DirectorySettingsWindow : Window
    {
        private CommonSettings _commonSettings;

        private DirectorySettings _directorySettings;

        public DirectorySettingsWindow()
        {
            _commonSettings = CommonSettings.Instance;

            _directorySettings = new DirectorySettings();

            InitializeComponent();

        }
        private void GeneratePathInputs(object sender, RoutedEventArgs e)
        {
            // Очистим существующие элементы в ScrollViewer
            DynamicPathInputsScrollViewer.Content = null;

            // Создаем новый StackPanel для хранения элементов
            StackPanel stackPanel = new StackPanel();

            // Создание новых элементов и добавление их в StackPanel
            for (int i = 0; i < int.Parse(PathCountTextBox.Text); i++)
            {
                TextBox newPathTextBox = new TextBox();

                newPathTextBox.Margin = new Thickness(0, 5, 0, 5);

                stackPanel.Children.Add(newPathTextBox);
            }

            // Устанавливаем StackPanel как содержимое ScrollViewer
            DynamicPathInputsScrollViewer.Content = stackPanel;
        }

        private void DeviceNameTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.DeviceName = e.Text;
        }

        private void SourceFilePathTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.MoveFromPath = e.Text;
        }

        private void PathCountTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
