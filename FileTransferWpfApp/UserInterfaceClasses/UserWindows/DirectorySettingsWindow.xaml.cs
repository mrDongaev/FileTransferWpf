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
using System.Reflection;
using System.Text.RegularExpressions;

namespace FileTransferWpfApp.UserInterfaceClasses.UserWindows
{
    /// <summary>
    /// Interaction logic for DirectorySettingsWindow.xaml
    /// </summary>
    public partial class DirectorySettingsWindow : Window
    {
        private CommonSettings _commonSettings;

        private DirectorySettings _directorySettings;

        private StackPanel _stackPanelInputPaths;

        public DirectorySettingsWindow()
        {
            _commonSettings = CommonSettings.Instance;

            _directorySettings = new DirectorySettings();

            InitializeComponent();

        }
        private void GeneratePathInputs(object sender, RoutedEventArgs e)
        {
            if (NumericFieldValidation(sender, e)) 
            {
                // Очистим существующие элементы в ScrollViewer
                DynamicPathInputsScrollViewer.Content = null;

                // Создаем новый StackPanel для хранения элементов
                _stackPanelInputPaths = new StackPanel();

                var pathcount = int.Parse(PathCountTextBox.Text);

                _directorySettings.MoveToPaths = new string[pathcount];

                // Создание новых элементов и добавление их в StackPanel
                for (int i = 0; i < pathcount; i++)
                {
                    TextBox newPathTextBox = new TextBox();

                    newPathTextBox.Margin = new Thickness(0, 5, 0, 5);

                    newPathTextBox.TextChanged += ChangingTextInGeneratedPaths;

                    _stackPanelInputPaths.Children.Add(newPathTextBox);
                }

                // Устанавливаем StackPanel как содержимое ScrollViewer
                DynamicPathInputsScrollViewer.Content = _stackPanelInputPaths;
            };
        }

        private void DeviceNameTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.DeviceName = e.Text;
        }

        private void SourceFilePathTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.MoveFromPath = e.Text;
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ChangingTextInGeneratedPaths(object sender, TextChangedEventArgs e) 
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null) 
            {
                var index = (_stackPanelInputPaths.Children.IndexOf(textBox));

                _directorySettings.MoveToPaths[index] += textBox.Text;
            }
        }
        public bool NumericFieldValidation(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                string input = textBox.Text;

                //Related regular expression
                if (!Regex.IsMatch(input, @"^\d+$")
                    || string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Проверьте правильность ввода числа перед генерацией!");

                    textBox.Text = String.Empty;

                    return false;
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                MessageBox.Show("Что-то не так с обьектом TextBox, он равен NULL");

                return false;
            }
        }
    }
}
