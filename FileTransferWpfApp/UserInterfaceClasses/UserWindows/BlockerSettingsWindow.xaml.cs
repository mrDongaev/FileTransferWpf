using System.Windows;
using System.Windows.Controls;

namespace FileTransferWpfApp.UserInterfaceClasses.UserWindows
{
    public partial class BlockerSettingsWindow : Window
    {
        public BlockerSettingsWindow()
        {
            InitializeComponent();
        }

        private void FileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string fileName = textBox.Text;
            }
        }

        private void FilePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string filePath = textBox.Text;
            }
        }

        private void TimeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string timeInput = textBox.Text;
            }
        }

        private void FileExtensionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedExtension = selectedItem.Content.ToString();
            }
        }
        private void FileNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string fileName = textBox.Text;
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
