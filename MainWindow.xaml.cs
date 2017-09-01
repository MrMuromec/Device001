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
        private C_Logic V_Logic = new C_Logic();

        private W_Port1 V_w_D01;
        private bool V_flagD01 = false;

        private W_Port1 V_w_D02;
        private bool V_flagD02 = false;

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Установка настроек
        /// </summary>
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
        /// <summary>
        /// Принудительное закрытие (используется при ошибках)
        /// </summary>
        public void F_CloseException()
        {
            this.Close();
        }
        /// <summary>
        /// Настройка 1 уст.
        /// </summary>
        private void B_D01_Click(object sender, RoutedEventArgs e)
        {
            if (!V_flagD01)
            {
                V_w_D01 = new W_Port1(V_Logic.F_GetPortD01(), "Настройки D01");
                V_w_D01.Closed += V_w_D01_Closed;
                V_flagD01 = true;
                V_w_D01.Show();
            }
            else
                V_w_D01.Activate();
        }
        /// <summary>
        /// Сброс флага 1 уст.
        /// </summary>
        void V_w_D01_Closed(object sender, EventArgs e)
        {
            V_flagD01 = false;
        }
        /// <summary>
        /// Настройка 2 уст.
        /// </summary>
        private void B_D02_Click(object sender, RoutedEventArgs e)
        {
            if (!V_flagD02)
            {
                V_w_D02 = new W_Port1(V_Logic.F_GetPortD02(), "Настройки D02");
                V_w_D02.Closed += V_w_D02_Closed;
                V_flagD02 = true;
                V_w_D02.Show();
            }
            else
                V_w_D02.Activate();
        }
        /// <summary>
        /// Сброс флага 2 уст.
        /// </summary>
        void V_w_D02_Closed(object sender, EventArgs e)
        {
            V_flagD02 = false;
        }
        /// <summary>
        /// Старт
        /// </summary>
        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            if (V_Logic.F_Measurement_On_())
                //V_Logic.F_Measurement_();
                ;
        }
    }
}
