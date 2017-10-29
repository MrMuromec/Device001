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

        public W_Measurements(C_Logic v_Logic, string[] v_OperatingMode, string[] v_TypeMeasurement)
        {
            InitializeComponent();

            V_Logic = v_Logic;

            foreach (var v_mode in v_OperatingMode)
                CB_OperatingMode.Items.Add(v_mode);
            CB_OperatingMode.SelectedIndex = V_Logic.Fv_Options.Fv_NumOperatingMode;

            foreach (var v_type in v_TypeMeasurement)
                CB_TypeMeasurement.Items.Add(v_type);
            CB_TypeMeasurement.SelectedIndex = V_Logic.Fv_Options.Fv_NumTypeMeasurement;

            foreach (var v_GridParameter in Device001.Port.C_ParameterListsD02.F_NumGridGet())
            {
                CB_StrokesGrid1.Items.Add(v_GridParameter.V_NumberStrokes.ToString() + " штр./мм.");
                CB_StrokesGrid2.Items.Add(v_GridParameter.V_NumberStrokes.ToString() + " штр./мм.");
            }

            CB_StrokesGrid1.SelectedIndex = Device001.Port.C_ParameterListsD02.F_NumGridGet().FindIndex(x => x.V_NumberStrokes == V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid.V_NumberStrokes);
            CB_StrokesGrid2.SelectedIndex = Device001.Port.C_ParameterListsD02.F_NumGridGet().FindIndex(x => x.V_NumberStrokes == V_Logic.Fv_Options.V_WaveStatic.Fv_ParameterGrid.V_NumberStrokes);
            CB_StrokesGrid1.SelectionChanged += async (s, e1) => { F_NewOptions(); };
            CB_StrokesGrid2.SelectionChanged += async (s, e1) => { F_NewOptions(); };

            foreach (var v_shift in Device001.Port.C_ParameterListsD02.F_ShiftGet())
                CB_NumShift.Items.Add(v_shift.ToString() + " нм");
            CB_NumShift.SelectedIndex = V_Logic.Fv_Options.Fv_NumShift;
            CB_NumShift.SelectionChanged += async (s, e1) => { F_NewOptions(); };

            foreach (var v_speed in Device001.Port.C_ParameterListsD02.F_SpeedGet())
                CB_NumSpeed.Items.Add(v_speed.ToString() + " нм/мин");
            CB_NumSpeed.SelectedIndex = V_Logic.Fv_Options.Fv_NumSpeed;
            CB_NumShift.SelectionChanged += async (s, e1) => { F_NewOptions(); };

            

            V_Logic.Event_CloseException += async () => { this.Close(); };
        }

        private void B_D01_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD01(); // Настройка 1 уст.
        }
        private void B_D02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD02(); // Настройка 2 уст.
        }

        private void CB_TypeMeasurement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_TypeMeasurement.SelectedIndex == 0)
                T_MonochromatorStop.Text = "Длина волны возбуждения";
            else
                T_MonochromatorStop.Text = "Длина волны эмиссии";
        }

        private void F_NewOptions ()
        {
            B_Start.IsEnabled = !B_Stop.IsEnabled;
            try
            {
                V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid = Device001.Port.C_ParameterListsD02.F_NumGridGet()[CB_StrokesGrid1.SelectedIndex];
            }
            catch (ApplicationException v_error)
            {
                B_Start.IsEnabled = false;
            }
            try
            {
                V_Logic.Fv_Options.V_WaveStatic.Fv_ParameterGrid = Device001.Port.C_ParameterListsD02.F_NumGridGet()[CB_StrokesGrid2.SelectedIndex];
            }
            catch (ApplicationException v_error)
            {
                B_Start.IsEnabled = false;
            }

            V_Logic.Fv_Options.Fv_Shift = Device001.Port.C_ParameterListsD02.F_ShiftGet()[CB_NumShift.SelectedIndex];

            V_Logic.Fv_Options.Fv_Speed = Device001.Port.C_ParameterListsD02.F_SpeedGet()[CB_NumSpeed.SelectedIndex];
        }
        /// <summary>
        /// Старт
        /// </summary>
        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            if (V_Logic.F_Measurement_On_())
            {
                //V_Logic.F_Measurement_();
                B_Start.IsEnabled = false;
                B_Stop.IsEnabled = true;
            }
        }
        /// <summary>
        /// Стоп
        /// </summary>
        private void B_Stop_Click(object sender, RoutedEventArgs e)
        {
            B_Start.IsEnabled = true;
            B_Stop.IsEnabled = false;
        }
    }
}
