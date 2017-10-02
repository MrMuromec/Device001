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

namespace Device001
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private C_Logic V_Logic;

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Установка настроек
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            V_Logic = new C_Logic();
            this.Close();
        }
    }
}
