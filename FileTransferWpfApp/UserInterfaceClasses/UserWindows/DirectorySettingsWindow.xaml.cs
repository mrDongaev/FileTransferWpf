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

namespace FileTransferWpfApp.UserInterfaceClasses.UserWindows
{
    /// <summary>
    /// Interaction logic for DirectorySettingsWindow.xaml
    /// </summary>
    public partial class DirectorySettingsWindow : Window
    {
        public DirectorySettingsWindow()
        {
            InitializeComponent();
        }

        private void IncreasePathCount(object sender, RoutedEventArgs e)
        {
            int currentCount = int.Parse(PathCountTextBox.Text);

            PathCountTextBox.Text = (currentCount + 1).ToString();
        }

        private void DecreasePathCount(object sender, RoutedEventArgs e)
        {
            int currentCount = int.Parse(PathCountTextBox.Text);

            if (currentCount > 1)
            {
                PathCountTextBox.Text = (currentCount - 1).ToString();
            }
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

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
