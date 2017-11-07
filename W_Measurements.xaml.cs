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
//
using ZedGraph;
using System.Windows.Forms;

namespace Device001
{
    /// <summary>
    /// Логика взаимодействия для W_Measurements.xaml
    /// </summary>
    public partial class W_Measurements : Window
    {
        private C_Logic V_Logic;
        private ZedGraphControl V_ZGC = new ZedGraph.ZedGraphControl();

        public W_Measurements(C_Logic v_Logic, string[] v_OperatingMode, string[] v_TypeMeasurement)
        {
            InitializeComponent();

            V_Logic = v_Logic;

            foreach (var v_shift in Device001.Port.C_ParameterListsD02.F_ShiftGet())
                CB_NumShift.Items.Add(v_shift.ToString() + " нм");
            CB_NumShift.SelectedIndex = V_Logic.Fv_Options.Fv_NumShift;
            CB_NumShift.SelectionChanged += async (s, e1) => { F_NewOptions(); };

            foreach (var v_speed in Device001.Port.C_ParameterListsD02.F_SpeedGet())
                CB_NumSpeed.Items.Add(v_speed.ToString() + " нм/мин");
            CB_NumSpeed.SelectedIndex = V_Logic.Fv_Options.Fv_NumSpeed;
            CB_NumShift.SelectionChanged += async (s, e1) => { F_NewOptions(); };

            foreach (var v_mode in v_OperatingMode)
                CB_OperatingMode.Items.Add(v_mode);
            CB_OperatingMode.SelectedIndex = V_Logic.Fv_Options.Fv_NumOperatingMode;

            foreach (var v_type in v_TypeMeasurement)
                CB_TypeMeasurement.Items.Add(v_type);
            CB_TypeMeasurement.SelectionChanged += async (s, e1) => 
            {
                if (CB_TypeMeasurement.SelectedIndex == 0)
                {
                    T_MonochromatorStaticOrDynamic.Text = "Длина волны возбуждения";
                    TB_MonochromatorStaticOrDynamic.Text = V_Logic.Fv_Options.V_WaveStatic.Fv_wave.ToString("F3");
                    TB_MonochromatorMin.Text = V_Logic.Fv_Options.V_WaveDynamic.Fv_wave.ToString("F3");
                }             
                else
                {
                    T_MonochromatorStaticOrDynamic.Text = "Длина волны эмиссии";
                    TB_MonochromatorStaticOrDynamic.Text = V_Logic.Fv_Options.V_WaveDynamic.Fv_wave.ToString("F3");
                    TB_MonochromatorMin.Text = V_Logic.Fv_Options.V_WaveStatic.Fv_wave.ToString("F3");
                }
                F_NewOptions();
            };
            CB_TypeMeasurement.SelectedIndex = V_Logic.Fv_Options.Fv_NumTypeMeasurement;
            if (CB_TypeMeasurement.SelectedIndex == 0)
                TB_MonochromatorMax.Text = (V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid.V_Max - V_Logic.Fv_Options.Fv_Shift).ToString("F3");
            else
                TB_MonochromatorMax.Text = (V_Logic.Fv_Options.V_WaveStatic.Fv_ParameterGrid.V_Max - V_Logic.Fv_Options.Fv_Shift).ToString("F3");

            TB_MonochromatorStaticOrDynamic.LostFocus += async (s, e1) => { F_NewOptions(); };
            TB_MonochromatorMin.LostFocus += async (s, e1) => { F_NewOptions(); };

            V_Logic.E_CloseException += async () => { this.Close(); };
            V_Logic.E_MeasurementOnSuccess += async () => { Gr_ButtonStartOrStop.IsEnabled = Gr_OptionsD01.IsEnabled = Gr_OptionsD02.IsEnabled = true; B_D01.IsEnabled = B_D02.IsEnabled = false; };
            V_Logic.E_MeasurementOffSuccess += async () => { B_Stop.IsEnabled = false; B_Start.IsEnabled = true; };

            WinFH_Paint = new System.Windows.Forms.Integration.WindowsFormsHost();           
        }
        private void B_D01_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD01(); // Настройка 1 уст.
        }
        private void B_D02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD02(); // Настройка 2 уст.
        }

        private void F_NewOptions ()
        {
            B_Start.IsEnabled = true;

            V_Logic.Fv_Options.Fv_Shift = Device001.Port.C_ParameterListsD02.F_ShiftGet()[CB_NumShift.SelectedIndex];
            V_Logic.Fv_Options.Fv_Speed = Device001.Port.C_ParameterListsD02.F_SpeedGet()[CB_NumSpeed.SelectedIndex];

            if (CB_TypeMeasurement.SelectedIndex == 0)
                try
                {
                    V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStaticOrDynamic.Text.Replace('.', ','));
                    V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorMin.Text.Replace('.', ','));
                }
                catch (ApplicationException)
                {
                    B_Start.IsEnabled = false;
                }
                catch (System.FormatException)
                {
                    B_Start.IsEnabled = false;
                }
            else
                try
                {
                    V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStaticOrDynamic.Text.Replace('.', ','));
                    V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorMin.Text.Replace('.', ','));
                }
                catch (ApplicationException)
                {
                    B_Start.IsEnabled = false;
                }
                catch (System.FormatException)
                {
                    B_Start.IsEnabled = false;
                }
            //F_Paint();
        }

        /// <summary>
        /// Старт
        /// </summary>
        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_Measurement_(
                (float)double.Parse(TB_MonochromatorMin.Text.Replace('.', ',')),
                (float)double.Parse(TB_MonochromatorMax.Text.Replace('.', ',')),
                (byte)CB_NumShift.SelectedIndex,
                (byte)CB_NumSpeed.SelectedIndex,
                (float)double.Parse(TB_MonochromatorStaticOrDynamic.Text.Replace('.', ',')),
                CB_TypeMeasurement.SelectedIndex,
                double.Parse(TB_PMT.Text.Replace('.', ','))
                );
        }
        /// <summary>
        /// Стоп
        /// </summary>
        private void B_Stop_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_Measurement_Off_();
        }

        private void B_Correction_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_Correction();
        }

        private void B_WaveSattic_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_GoWave((byte)CB_TypeMeasurement.SelectedIndex, (float)double.Parse(TB_MonochromatorStaticOrDynamic.Text.Replace('.', ',')));
        }

        private void B_Dynamic_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_GoWave((byte)(1 - CB_TypeMeasurement.SelectedIndex), (float)double.Parse(TB_MonochromatorStaticOrDynamic.Text.Replace('.', ',')));
        }

        private void B_PMT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                V_Logic.F_GoPMT(double.Parse(TB_PMT.Text.Replace('.', ',')));
            }
            catch (System.FormatException v_Ex)
            {
                System.Windows.MessageBox.Show(v_Ex.Message, "Ошибка данных");
            }
        }

        /*
        /// <summary>
        /// Рисование
        /// </summary>
        private void F_Paint()
        {
            GraphPane pane = V_ZGC.GraphPane;

            pane.CurveList.Clear();

            PointPairList list = new PointPairList();

            double xmin = -50;
            double xmax = 50;
            for (double x = xmin; x <= xmax; x += 0.01)
            {
                list.Add(x, f(x));
            }

            LineItem myCurve = pane.AddCurve("Sinc", list, System.Drawing.Color.Blue, SymbolType.None);

            V_ZGC.AxisChange();
            V_ZGC.Invalidate();

            WinFH_Paint.Child = V_ZGC;
        }
        /// <summary>
        /// Тестовая кривая
        /// </summary>
        private double f(double x)
        {
            if (x == 0)
            {
                return 1;
            }

            return Math.Sin(x) / x;
        }
        */
    }
}
