using FileTransferWpfApp.Tools;
using System.Windows;
using System.Windows.Controls;

namespace FileTransferWpfApp.UserInterfaceClasses.UserWindows
{
    public partial class BlockerSettingsWindow : Window
    {
        BlockerSettings blockerSettings;

        public BlockerSettingsWindow()
        {
            InitializeComponent();

            blockerSettings = new BlockerSettings();
        }

        private void FilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                blockerSettings.FilePath += textBox.Text;
            }
        }

        private void TimeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                blockerSettings.Time += textBox.Text;
            }
        }

        private void FileExtensionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                blockerSettings.FileExtension = selectedItem.Content.ToString();
            }
        }
        private void FileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                blockerSettings.FileName = textBox.Text;
            }
        }

        private void FilePathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string filePath = textBox.Text;
            }
        }

        private void TimeInputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
