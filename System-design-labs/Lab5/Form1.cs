using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    // 3. Опис класу Student згідно з вимогами
    public class Student
    {
        // 4 властивості різних типів, одна з яких - колекція (List)
        public string FullName { get; set; }
        public int Age { get; set; }
        public bool IsEnrolled { get; set; }
        public List<int> Grades { get; set; }

        // Конструктор 1 (за замовчуванням)
        public Student()
        {
            FullName = "Unknown";
            Age = 0;
            IsEnrolled = false;
            Grades = new List<int>();
        }

        // Конструктор 2 (з параметрами)
        public Student(string fullName, int age, bool isEnrolled, List<int> grades)
        {
            FullName = fullName;
            Age = age;
            IsEnrolled = isEnrolled;
            Grades = grades;
        }

        // 3 методи
        public void Study()
        {
            // логіка навчання
        }

        public void PassExam()
        {
            // логіка здачі іспиту
        }

        public double GetAverageGrade()
        {
            return 0.0;
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Обробник натискання кнопки, яку ми додали на форму
        private void button1_Click(object sender, EventArgs e)
        {
            // Створення об'єкта з тестовими даними українською
            Student student = new Student("Іван Петренко", 20, true, new List<int> { 90, 85, 95 });

            ShowPropertiesInTreeView(student, treeView1);
        }

        private void ShowPropertiesInTreeView(object obj, TreeView tv)
        {
            tv.Nodes.Clear();

            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            // Кореневий вузол українською
            TreeNode rootNode = new TreeNode($"Об'єкт: {type.Name}");
            tv.Nodes.Add(rootNode);

            foreach (PropertyInfo prop in properties)
            {
                object value = prop.GetValue(obj);
                string valueStr = value != null ? value.ToString() : "null";

                if (value is IEnumerable enumerable && !(value is string))
                {
                    List<string> items = new List<string>();
                    foreach (var item in enumerable)
                    {
                        items.Add(item.ToString());
                    }
                    valueStr = "[" + string.Join(", ", items) + "]";
                }

                // Дочірні вузли українською
                TreeNode propNode = new TreeNode($"Властивість: {prop.Name}");
                propNode.Nodes.Add($"Значення: {valueStr}");
                propNode.Nodes.Add($"Тип: {prop.PropertyType.Name}");

                rootNode.Nodes.Add(propNode);
            }

            tv.ExpandAll();
        }
    }
}
