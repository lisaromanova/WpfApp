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
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void btnSolve_Click(object sender, RoutedEventArgs e)
        {
            if(cbEnterData.SelectedIndex != -1)
            {
                if(cbSolution.SelectedIndex != -1)
                {
                    if (Regex.IsMatch(tbN.Text, "^\\d+$") && Convert.ToInt32(tbN.Text) >= 1)
                    {
                        Classes.FrameClass.frmMain.Navigate(new EnterDataPage(cbSolution.SelectedIndex, cbEnterData.SelectedIndex, Convert.ToInt32(tbN.Text)));
                    }
                    else
                    {
                        MessageBox.Show("Введите целое число больше 0!", "Количество видов продукции", MessageBoxButton.OK, MessageBoxImage.Error);
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
