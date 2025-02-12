﻿using System;
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
using FileTransferWpfApp.Model.ModelSettings;
using FileTransferWpfApp.ViewModel;

namespace FileTransferWpfApp.View.UserView
{
    /// <summary>
    /// Interaction logic for DirectorySettingsWindow.xaml
    /// </summary>
    public partial class DirectorySettingsWindow : Window
    {
        private CommonSettings _commonSettings;

        private DirectorySettings _directorySettings;

        private StackPanel _stackPanelInputPaths;

        int counter = 0;

        public DirectorySettingsWindow()
        {
            InitializeComponent();

            DataContext = new ApplicationViewModel();

            _commonSettings = CommonSettings.Instance;

            _directorySettings = new DirectorySettings();
        }
        private void GeneratePathInputs(object sender, RoutedEventArgs e)
        {
            if (NumericFieldValidation(PathCountTextBox.Text)) 
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

        private void DeviceNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            _directorySettings.DeviceName = textBox.Text;
        }

        private void DeviceNameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.DeviceName += e.Text;
        }

        private void SourceFilePathTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.MoveFromPath = e.Text;
        }
        private void SourceFilePathTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            _directorySettings.MoveFromPath = e.Text;
        }

        private void FileFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _directorySettings.FileFilterMask = e.AddedItems[0]?.ToString();
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var countInputPaths = _stackPanelInputPaths.Children.Count;

            _directorySettings.MoveToPaths = new string[countInputPaths];

            for (int i = 0; i < countInputPaths; i++)
            {
                TextBox textBox = _stackPanelInputPaths.Children[i] as TextBox;

                if (textBox != null) 
                {
                    if (string.IsNullOrEmpty(textBox.Text) &&
                        Directory.Exists(textBox.Text))
                    {
                        _directorySettings.MoveToPaths[i] = textBox.Text;
                    }
                    else 
                    {
                        MessageBox.Show("Путь не существует или к нему нет доступа!\n" +
                            "Проверьте правильность ввода");
                    }
                }
            }
            //foreach (TextBox textBox in _stackPanelInputPaths.Children)
            //{
            //    string path = textBox.Text;

            //    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            //    {
                    
            //        _commonSettings.Directories.Add(_directorySettings.MoveToPaths);
            //    }
            //    else
            //    {
            //        MessageBox.Show($"Путь '{path}' не является корректным или не существует.");
            //        // Дополнительная логика обработки некорректного пути
            //    }
            //}
            // Дополнительная логика после проверки всех путей


            //_commonSettings.Directories.Add(_directorySettings);
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
        public bool NumericFieldValidation(string text)
        {
            if (text != string.Empty)
            {
                //Related regular expression
                if (!Regex.IsMatch(text, @"^\d+$")
                    || string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("Проверьте правильность ввода числа перед генерацией!");

                    text = String.Empty;

                    return false;
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                MessageBox.Show("Что-то не так с обьектом TextBox, он равен Empty");

                return false;
            }
        }
    }
}
