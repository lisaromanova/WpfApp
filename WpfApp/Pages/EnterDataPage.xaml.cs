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
using System.Windows.Markup;
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
        /// Конструктор для ручного ввода и генерации основного объема данных случайным образом
        /// </summary>
        /// <param name="solve">Решение</param>
        /// <param name="enter">Ввод данных</param>
        /// <param name="n">Количество видов продукции</param>
        /// <param name="path">Путь к файлу</param>
        public EnterDataPage(int solve, int enter, int n, string path)
        {
            InitializeComponent();
            this.solve = solve;
            data = new List<DataClass>();
            switch (enter)
            {
                case 0:
                    ManualDataEntry(n);
                    break;
                case 1:
                    GenerateDataRandom(n);
                    break;
                case 2:
                    ConvertDataFromFile(path);
                    break;
                default:
                    MessageBox.Show("Ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            dtData.ItemsSource = data;
        }

        /// <summary>
        /// Генерация основного объема данных случайным образом
        /// </summary>
        /// <param name="n">Количество видов продуктов</param>
        void GenerateDataRandom(int n)
        {
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                data.Add(new DataClass
                {
                    Id = i + 1,
                    Weight = rnd.Next(100, 1000),
                    Calories = rnd.Next(1, 1000),
                    MaxCount = rnd.Next(1, 50)
                });
            }
            dtData.IsReadOnly = true;
            tbMaxCallor.Text = rnd.Next(1000, 10000).ToString();
            tbMaxCallor.IsReadOnly = true;
        }

        /// <summary>
        /// Ручной ввод данных
        /// </summary>
        /// <param name="n">Количество видов продукции</param>
        void ManualDataEntry(int n)
        {
            for (int i = 0; i < n; i++)
            {
                data.Add(new DataClass
                {
                    Id = i + 1
                });
            }
        }

        /// <summary>
        /// Преобразование данных из файла в список
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        void ConvertDataFromFile(string path)
        {
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
            dtData.IsReadOnly = true;
            tbMaxCallor.IsReadOnly = true;
        }

        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <returns>True - данные верны, False - данные неверны</returns>
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
            string strData = "";
            foreach(DataClass dataClass in data)
            {
                strData += $"{dataClass.Id} {dataClass.Weight} {dataClass.Calories} {dataClass.MaxCount}\n";
            }
            strData += tbMaxCallor.Text;
            string path = saveFile.FileName;
            //запись строки в файл
            File.WriteAllText(path, strData);
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
                        MessageBoxResult mgResult = MessageBox.Show("Вы хотите сохранить данные в файл?", "Сохранение данных", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mgResult == MessageBoxResult.Yes)
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
                                FrameClass.frmMain.Navigate(new ResultPage(IteratingMethod.Solve(data, Convert.ToDouble(tbMaxCallor.Text))));
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
