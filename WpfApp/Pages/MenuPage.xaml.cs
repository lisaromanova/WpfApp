using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Чтение данных из файла
        /// </summary>
        /// <returns>Путь к файлу с данными</returns>
        string ReadDataFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //фильтрация выбора файла с расширением .txt
            ofd.Filter = "Текстовые файлы (*.txt)|*.txt";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        /// <summary>
        /// Проверка данных в файле
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>True - данные верны, False - данные неверные</returns>
        bool CheckDataFromFile(string path)
        {
            string[] dataLines = File.ReadAllLines(path);
            //если в файле всего одна запись
            if(dataLines.Length <= 1)
            {
                return false;
            }
            for (int i = 0; i < dataLines.Length - 1; i++)
            {
                //разделение данных по пробелу
                string[] dataLine = dataLines[i].Split(' ');
                //если количество данных по строке не равно 4
                if(dataLine.Length != 4)
                {
                    return false;
                }
                //проверка на дробные числа
                foreach(string s in dataLine)
                {
                    if (!Regex.IsMatch(s, "^\\d+([.]\\d+)?$"))
                    {
                        return false;
                    }
                    else
                    {
                        if(Convert.ToDouble(s) <= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            //проверка суммарной каллорийности
            if (!Regex.IsMatch(dataLines[dataLines.Length - 1], "^\\d+([.]\\d+)?$"))
            {
                return false;
            }
            else
            {
                if (Convert.ToDouble(dataLines[dataLines.Length - 1]) <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            if(cbEnterData.SelectedIndex != -1)
            {
                if(cbSolution.SelectedIndex != -1)
                {
                    switch (cbEnterData.SelectedIndex)
                    {
                        case 0:
                        case 1:
                            if (Regex.IsMatch(tbN.Text, "^\\d+$") && Convert.ToInt32(tbN.Text) >= 1 && Convert.ToInt32(tbN.Text) <= 10)
                            {
                                Classes.FrameClass.frmMain.Navigate(new EnterDataPage(cbSolution.SelectedIndex, cbEnterData.SelectedIndex, Convert.ToInt32(tbN.Text), null));
                            }
                            else
                            {
                                MessageBox.Show("Введите целое число больше 0 и меньше 10!", "Количество видов продукции", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            break;
                        case 2:
                            try
                            {
                                string path = ReadDataFromFile();
                                if (CheckDataFromFile(path))
                                {
                                    Classes.FrameClass.frmMain.Navigate(new EnterDataPage(cbSolution.SelectedIndex, cbEnterData.SelectedIndex, 0, path));
                                }
                                else
                                {
                                    MessageBox.Show("Неверные данные в файле", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Ошибка выбора файла!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            break;
                        default:
                            MessageBox.Show("Ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Выберите способ решения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите способ ввода данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
