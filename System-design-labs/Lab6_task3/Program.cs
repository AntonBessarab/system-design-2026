using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Lab6_task3
{
    public class FileManagerForm : Form
    {
        private TreeView treeView;
        private ListView listView;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Запускаємо нашу форму
            Application.Run(new FileManagerForm());
        }

        public FileManagerForm()
        {
            // Налаштування головного вікна
            Text = "Мій Файловий Менеджер";
            Size = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            // Розділювач між лівою і правою панеллю
            Splitter splitter = new Splitter { Dock = DockStyle.Left, Width = 3 };

            // Ліва панель (Дерево папок)
            treeView = new TreeView { Dock = DockStyle.Left, Width = 250 };
            treeView.BeforeExpand += TreeView_BeforeExpand;
            treeView.AfterSelect += TreeView_AfterSelect;

            // Права панель (Список файлів)
            listView = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true };
            listView.Columns.Add("Ім'я", 300);
            listView.Columns.Add("Розмір", 100);
            listView.Columns.Add("Тип", 100);
            listView.Columns.Add("Дата зміни", 150);

            // Додаємо елементи на форму (порядок важливий!)
            Controls.Add(listView);
            Controls.Add(splitter);
            Controls.Add(treeView);

            // Завантажуємо початкові дані (диски)
            LoadDrives();
        }

        private void LoadDrives()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    TreeNode node = new TreeNode(drive.Name) { Tag = drive.RootDirectory.FullName };
                    node.Nodes.Add("..."); // Тимчасовий вузол, щоб з'явився плюсик для розгортання
                    treeView.Nodes.Add(node);
                }
            }
        }

        // Ця подія спрацьовує, коли користувач натискає "+" біля папки
        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "...")
            {
                e.Node.Nodes.Clear();
                LoadDirectories(e.Node);
            }
        }

        private void LoadDirectories(TreeNode parentNode)
        {
            try
            {
                string path = (string)parentNode.Tag;
                foreach (string dir in Directory.GetDirectories(path))
                {
                    TreeNode node = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                    node.Nodes.Add("...");
                    parentNode.Nodes.Add(node);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ігноруємо системні папки до яких Windows не дає доступ
            }
        }

        // Ця подія спрацьовує, коли користувач клікає на папку (щоб показати файли справа)
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadFiles((string)e.Node.Tag);
        }

        private void LoadFiles(string path)
        {
            listView.Items.Clear();
            try
            {
                // Спочатку виводимо папки
                foreach (string dir in Directory.GetDirectories(path))
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    ListViewItem item = new ListViewItem(di.Name);
                    item.SubItems.Add("");
                    item.SubItems.Add("Папка");
                    item.SubItems.Add(di.LastWriteTime.ToString("g"));
                    listView.Items.Add(item);
                }

                // Потім виводимо файли
                foreach (string file in Directory.GetFiles(path))
                {
                    FileInfo fi = new FileInfo(file);
                    ListViewItem item = new ListViewItem(fi.Name);

                    // Рахуємо розмір в КБ
                    long sizeKb = fi.Length / 1024;
                    item.SubItems.Add(sizeKb == 0 ? "1 KB" : sizeKb + " KB");

                    item.SubItems.Add(fi.Extension);
                    item.SubItems.Add(fi.LastWriteTime.ToString("g"));
                    listView.Items.Add(item);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ігноруємо якщо немає доступу
            }
        }
    }
}