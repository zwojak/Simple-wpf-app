using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Forms;
using MenuItem = System.Windows.Controls.MenuItem;

namespace WpfApp3
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Open(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog()
            {
                Description = "Select directory to open"
            };
            DialogResult result = dlg.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                treeView.Items.Clear();
                DirectoryInfo dir = new DirectoryInfo(dlg.SelectedPath);
                var root = InsertDir(dir);
                treeView.Items.Add(root);
            }
        }

        private TreeViewItem InsertDir(DirectoryInfo dir)
        {
            var root = new TreeViewItem
            {
                Header = dir.Name,
                Tag = dir.FullName
            };
            root.ContextMenu = new System.Windows.Controls.ContextMenu();
            var menuItem1 = new MenuItem { Header = "Create" };
            menuItem1.Click += new RoutedEventHandler(MenuItemCreateClick);
            root.ContextMenu.Items.Add(menuItem1);
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                root.Items.Add(InsertDir(subdir));
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                root.Items.Add(InsertFile(file));
            }
            return root;
        }

        private TreeViewItem InsertFile(FileInfo file)
        {
            var item = new TreeViewItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            item.ContextMenu = new System.Windows.Controls.ContextMenu();
            var menuItem1 = new MenuItem { Header = "Open" };
            var menuItem2 = new MenuItem { Header = "Delete" };
            menuItem1.Click += new RoutedEventHandler(MenuItemOpenClick);
            menuItem2.Click += new RoutedEventHandler(MenuItemDeleteClick);
            item.ContextMenu.Items.Add(menuItem1);
            item.ContextMenu.Items.Add(menuItem2);
            return item;
        }

        private void MenuItemOpenClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            string content = File.ReadAllText((string)item.Tag);
            scrollViewer.Content = new TextBlock() { Text = content };
        }

        private void MenuItemCreateClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            string path = (string)item.Tag;
            DialogWindow dialog = new DialogWindow(path);
            dialog.ShowDialog();
            if (dialog.IsCorrect())
            {
                if (dialog.IsFile())
                {
                    FileInfo file = new FileInfo(dialog.GetPath());
                    item.Items.Add(InsertFile(file));
                }
                else
                {
                    DirectoryInfo dir = new DirectoryInfo(dialog.GetPath());
                    item.Items.Add(InsertDir(dir));
                }
            }
        }

        private void MenuItemDeleteClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)treeView.SelectedItem;
            File.Delete((string)item.Tag);
            TreeViewItem parent = (TreeViewItem)item.Parent;
            parent.Items.Remove(item);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
