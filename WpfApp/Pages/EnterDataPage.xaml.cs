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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EnterDataPage.xaml
    /// </summary>
    public partial class EnterDataPage : Page
    {
        public EnterDataPage(int solve, int enter, int n)
        {
            InitializeComponent();
            List<Classes.DataClass> data = new List<Classes.DataClass>();
            for( int i=0; i < n; i++ )
            {
                data.Add(new Classes.DataClass { Id = i + 1 });
            }
            dtData.ItemsSource = data;
        }
    }
}
