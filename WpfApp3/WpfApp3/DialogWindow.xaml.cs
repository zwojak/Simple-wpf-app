using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Logika interakcji dla klasy DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private string path;
        private string name;
        private bool correct;
        public DialogWindow(string path)
        {
            InitializeComponent();
            this.path = path;
            correct = false;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if((bool)fileRButton.IsChecked && !Regex.IsMatch(fileName.Text, "^[a-zA-Z0-9_~-]{1,8}\\.(txt|php|html)$"))
            {
                System.Windows.MessageBox.Show("Wrong name!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            if(!(bool)fileRButton.IsChecked && !(bool)directoryRButton.IsChecked)
            {
                System.Windows.MessageBox.Show("Choose the type of the file!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            name = fileName.Text;
            path += "\\" + name;
            if ((bool)fileRButton.IsChecked)
            {
                File.Create(path);
            }
            else
            {
                Directory.CreateDirectory(path);
            }
            correct = true;
            Close();
        }

        public bool IsFile()
        {
            if ((bool)fileRButton.IsChecked) return true;
            else return false;
        }

        public bool IsCorrect()
        {
            if (correct) return true;
            else return false;
        }


        public string GetPath()
        {
            return path;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
