using Microsoft.Win32;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp.Classes;

namespace WpfApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnterDataPage.xaml
    /// </summary>
    public partial class EnterDataPage : Page
    {
        /// <summary>
        /// Спиок в данными видов продукции
        /// </summary>
        List<DataClass> data;

        /// <summary>
        /// Решение
        /// </summary>
        int solve;

        /// <summary>
        /// Конструктор для ручного ввода и генерации случайным образом
        /// </summary>
        /// <param name="solve">Решение</param>
        /// <param name="enter">Ввод данных</param>
        /// <param name="n">Количество видов продукции</param>
        public EnterDataPage(int solve, int enter, int n)
        {
            InitializeComponent();
            this.solve = solve;
            data = new List<DataClass>();
            Random rnd = new Random();
            //цикл по видам продукции
            for (int i = 0; i < n; i++)
            {
                //если генерация случайным образом
                if (enter == 1)
                {
                    data.Add(new DataClass 
                    { 
                        Id = i + 1, 
                        Weight = rnd.Next(100, 1000), 
                        Calories = rnd.Next(1, 1000), 
                        MaxCount = rnd.Next(1, 50) 
                    });
                }
                else
                {
                    data.Add(new DataClass
                    {
                        Id = i + 1
                    });
                }
            }
            //если генерация случайным образом
            if (enter == 1)
            {
                dtData.IsReadOnly = true;
                tbMaxCallor.Text = rnd.Next(1000, 10000).ToString();
                tbMaxCallor.IsReadOnly = true;
            }
            dtData.ItemsSource = data;
        }

        /// <summary>
        /// Конструктор для восстановления данных из файла
        /// </summary>
        /// <param name="solve">Решение</param>
        /// <param name="path">Путь к файлу с данными</param>
        public EnterDataPage(int solve, string path)
        {
            InitializeComponent();
            this.solve = solve;
            data = new List<DataClass>();
            //чтение строк с данными
            string[] dataLines = File.ReadAllLines(path);
            for (int i = 0; i < dataLines.Length - 1; i++)
            {
                //разделение строки по пробелам
                string[] dataLine = dataLines[i].Split(' ');
                DataClass dataClass = new DataClass()
                {
                    Id = i + 1,
                    Weight = Convert.ToDouble(dataLine[1]),
                    Calories = Convert.ToDouble(dataLine[2]),
                    MaxCount = Convert.ToInt32(dataLine[3])
                };
                data.Add(dataClass);
            }
            dtData.ItemsSource = data;
            tbMaxCallor.Text = dataLines[dataLines.Length - 1];
        }

        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <returns></returns>
        bool CheckData()
        {
            foreach (DataClass dataClass in data)
            {
                if (dataClass.Weight < 1 ||
                    dataClass.Calories < 1 ||
                    dataClass.MaxCount < 1)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Сохранение данных в файл
        /// </summary>
        void SaveDataToFile()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            //фильтрация файлов с расширением .txt
            saveFile.Filter = "Текстовые файлы (*.txt)|*.txt";
            saveFile.ShowDialog();
            //формирование строки с данными
            string str = "";
            foreach(DataClass dataClass in data)
            {
                str += $"{dataClass.Id} {dataClass.Weight} {dataClass.Calories} {dataClass.MaxCount}\n";
            }
            str += tbMaxCallor.Text;
            string path = saveFile.FileName;
            //запись строки в файл
            File.WriteAllText(path, str);
            MessageBox.Show("Данные успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                if (Regex.IsMatch(tbMaxCallor.Text, "^\\d+([.]\\d+)?$"))
                {
                    try
                    {
                        MessageBoxResult result = MessageBox.Show("Вы хотите сохранить данные в файл?", "Сохранение данных", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            SaveDataToFile();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка сохранения данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        switch (solve)
                        {
                            case 0:
                                FrameClass.frmMain.Navigate(new ResultPage(IteratingOverTheSetOfAcceptableSolutions.Method(data, Convert.ToDouble(tbMaxCallor.Text))));
                                break;
                            case 1:
                                FrameClass.frmMain.Navigate(new ResultPage(SimplexMethod.Solve(data.Count, data, Convert.ToDouble(tbMaxCallor.Text))));
                                break;
                            default:
                                MessageBox.Show("Ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Введите минимальную каллорийность корректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Данные введены некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frmMain.GoBack();
        }
    }
}
