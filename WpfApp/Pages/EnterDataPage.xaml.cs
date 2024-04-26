using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для EnterDataPage.xaml
    /// </summary>
    public partial class EnterDataPage : Page
    {
        List<Classes.DataClass> data;
        int solve, n;
        public EnterDataPage(int solve, int enter, int n)
        {
            InitializeComponent();
            this.solve = solve;
            this.n = n;
            data = new List<Classes.DataClass>();
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                if (enter == 1)
                {
                    data.Add(new Classes.DataClass 
                    { 
                        Id = i + 1, 
                        Weight = rnd.Next(1, 1000), 
                        Calories = rnd.Next(1, 1000), 
                        MaxCount = rnd.Next(1, 50) 
                    });
                }
                else
                {
                    data.Add(new Classes.DataClass
                    {
                        Id = i + 1
                    });
                }
            }

            if(enter == 1)
            {
                dtData.IsReadOnly = true;
                tbMaxCallor.Text = rnd.Next(1000, 10000).ToString();
                tbMaxCallor.IsReadOnly = true;
            }
            dtData.ItemsSource = data;
        }

        bool CheckData()
        {
            foreach(Classes.DataClass dataClass in data)
            {
                if (dataClass.Weight <= 0 ||
                    dataClass.Calories <= 0 ||
                    dataClass.MaxCount <= 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            if (CheckData())
            {
                if (Regex.IsMatch(tbMaxCallor.Text, "^\\d+([.]\\d+)?$"))
                {
                    if(solve == 0)
                    {
                        Classes.FrameClass.frmMain.Navigate(new ResultPage(Classes.SimplexMethod.Solve(n, data, Convert.ToDouble(tbMaxCallor.Text), solve)));
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
            Classes.FrameClass.frmMain.GoBack();
        }
    }
}
