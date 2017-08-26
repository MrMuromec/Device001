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
        C_Logic V_Logic = new C_Logic();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CB_OperatingMode.Items.Add("Сигналы");
            CB_OperatingMode.Items.Add("Спектры");
            CB_OperatingMode.SelectedIndex = 0;

            CB_TypeMeasurement.Items.Add("Возбуждение");
            CB_TypeMeasurement.Items.Add("Эмиссия");
            CB_TypeMeasurement.SelectedIndex = 0 ;

            V_Logic.Event_CloseException += F_CloseException;
        }

        public void F_CloseException()
        {
            this.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        private void B_D01_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_WindowPort_D01();
        }
        /// <summary>
        /// 
        /// </summary>
        private void B_D02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_WindowPort_D02();
        }

        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            if (V_Logic.F_Measurement_On_())
                V_Logic.F_Measurement_();
        }
    }
}
