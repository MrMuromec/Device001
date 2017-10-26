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
using System.Windows.Shapes;

namespace Device001
{
    /// <summary>
    /// Логика взаимодействия для W_Measurements.xaml
    /// </summary>
    public partial class W_Measurements : Window
    {
        private C_Logic V_Logic;

        public W_Measurements(C_Logic v_Logic, List<string> v_StrokesGrid1, List<string> v_StrokesGrid2, List<string> v_NumShift, List<string> v_NumSpeed, string[] v_OperatingMode, string[] v_TypeMeasurement)
        {
            InitializeComponent();

            V_Logic = v_Logic;

            foreach (var v_GridParameter in v_StrokesGrid1)
                CB_StrokesGrid1.Items.Add(v_GridParameter);
            //CB_StrokesGrid1.SelectedIndex = V_Logic.Fv_SelectedOptionsOfMeasument["StrokesGrid1"];

            foreach (var v_GridParameter in v_StrokesGrid2)
                CB_StrokesGrid2.Items.Add(v_GridParameter);
            //CB_StrokesGrid2.SelectedIndex = V_Logic.Fv_SelectedOptionsOfMeasument["StrokesGrid2"];

            foreach (var v_shift in v_NumShift)
                CB_NumShift.Items.Add(v_shift);
            CB_NumShift.SelectedIndex = V_Logic.Fv_NumShift;

            foreach (var v_speed in v_NumSpeed)
                CB_NumSpeed.Items.Add(v_speed);
            CB_NumSpeed.SelectedIndex = V_Logic.Fv_NumSpeed;

            foreach (var v_mode in v_OperatingMode)
                CB_OperatingMode.Items.Add(v_mode);
            CB_OperatingMode.SelectedIndex = V_Logic.Fv_NumOperatingMode;

            foreach (var v_type in v_TypeMeasurement)
                CB_TypeMeasurement.Items.Add(v_type);
            CB_TypeMeasurement.SelectedIndex = V_Logic.Fv_NumTypeMeasurement;

            V_Logic.Event_CloseException += async () => { this.Close(); }; // Принудительное закрытие (используется при ошибках)

            /*
this.TB_MonochromatorStop.DataContext = (V_WaveStatic = new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]));

CB_StrokesGrid1.SelectionChanged += async (s, e1) => { if (CB_TypeMeasurement.SelectedIndex == 0) V_WaveStatic.V_ParameterGrid = Port.C_ParameterListsD02.F_NumGridGet()[CB_StrokesGrid1.SelectedIndex]; };
CB_StrokesGrid2.SelectionChanged += async (s, e1) => { if (CB_TypeMeasurement.SelectedIndex == 1) V_WaveStatic.V_ParameterGrid = Port.C_ParameterListsD02.F_NumGridGet()[CB_StrokesGrid2.SelectedIndex]; };
 * */

            
        }
        private void B_D01_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD01(); // Настройка 1 уст.
        }
        private void B_D02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD02(); // Настройка 2 уст.
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

        private void CB_TypeMeasurement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_TypeMeasurement.SelectedIndex == 0)
                T_MonochromatorStop.Text = "Длина волны возбуждения";
            else
                T_MonochromatorStop.Text = "Длина волны эмиссии";
        }
    }
}
