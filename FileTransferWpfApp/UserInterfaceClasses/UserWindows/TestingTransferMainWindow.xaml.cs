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
    /// Логика взаимодействия для TestingTransferMainWindow.xaml
    /// </summary>
    public partial class TestingTransferMainWindow : Window
    {
        public TestingTransferMainWindow()
        {
            InitializeComponent();
        }

        private void ListBItemSettingsBlockerPrevMouseLBut(object sender, MouseButtonEventArgs e)
        {
            var blockerSettingsWindow = new BlockerSettingsWindow();

            blockerSettingsWindow.Show();
        }
    }
}
