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
//
using System.Globalization;
using System.IO;
using System.Threading;


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
                CB_NumShift.Items.Add(v_shift.ToString(CultureInfo.InvariantCulture) + " нм");
            CB_NumShift.SelectedIndex = V_Logic.Fv_Options.Fv_NumShift;
            CB_NumShift.SelectionChanged += async (s, e1) => { F_NewOptions(); };

            foreach (var v_speed in Device001.Port.C_ParameterListsD02.F_SpeedGet())
                CB_NumSpeed.Items.Add(v_speed.ToString(CultureInfo.InvariantCulture) + " нм/мин");
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
                    TB_MonochromatorStaticOrDynamic.Text = V_Logic.Fv_Options.V_WaveStatic.Fv_wave.ToString("F3", CultureInfo.InvariantCulture);
                    TB_MonochromatorMin.Text = V_Logic.Fv_Options.V_WaveDynamic.Fv_wave.ToString("F3", CultureInfo.InvariantCulture);
                }             
                else
                {
                    T_MonochromatorStaticOrDynamic.Text = "Длина волны эмиссии";
                    TB_MonochromatorStaticOrDynamic.Text = V_Logic.Fv_Options.V_WaveDynamic.Fv_wave.ToString("F3", CultureInfo.InvariantCulture);
                    TB_MonochromatorMin.Text = V_Logic.Fv_Options.V_WaveStatic.Fv_wave.ToString("F3", CultureInfo.InvariantCulture);
                }
                F_NewOptions();
            };
            CB_TypeMeasurement.SelectedIndex = V_Logic.Fv_Options.Fv_NumTypeMeasurement;
            if (CB_TypeMeasurement.SelectedIndex == 0)
                TB_MonochromatorMax.Text = (V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid.V_Max - V_Logic.Fv_Options.Fv_Shift).ToString("F3", CultureInfo.InvariantCulture);
            else
                TB_MonochromatorMax.Text = (V_Logic.Fv_Options.V_WaveStatic.Fv_ParameterGrid.V_Max - V_Logic.Fv_Options.Fv_Shift).ToString("F3", CultureInfo.InvariantCulture);

            TB_MonochromatorStaticOrDynamic.LostFocus += async (s, e1) => { F_NewOptions(); };
            TB_MonochromatorMin.LostFocus += async (s, e1) => { F_NewOptions(); };

            V_Logic.E_CloseException += async () => 
            { this.Close(); };

            V_Logic.E_MeasurementOnAndCorrectionSuccess += async () => 
            { Gr_ButtonStartOrStop.IsEnabled = Gr_OptionsD01.IsEnabled = Gr_OptionsD02.IsEnabled = true; B_D01.IsEnabled = B_D02.IsEnabled = false; B_Correction.IsEnabled = false; };

            V_Logic.E_MeasurementOnSuccess += async () =>
            { TB_Name.IsEnabled = Gr_ButtonStartOrStop.IsEnabled = Gr_OptionsD01.IsEnabled = true; B_D01.IsEnabled = B_D02.IsEnabled = false; B_Correction.IsEnabled = false; B_Stop.IsEnabled = true; };

            V_Logic.E_MeasurementOnSuccess += async () =>
            { Gr_OptionsD02.IsEnabled = true; B_Сalibration02.IsEnabled = false; B_Free.IsEnabled = B_Save.IsEnabled = true; }; // времено

            V_Logic.E_MeasurementOffSuccess += async () =>
            { TB_Name.IsEnabled = Gr_OptionsD02.IsEnabled = Gr_OptionsD01.IsEnabled = false; B_Start.IsEnabled = B_D01.IsEnabled = B_D02.IsEnabled = B_Correction.IsEnabled = true; };

            V_Logic.E_MeasurementOffSuccess += async () =>
            { B_Сalibration02.IsEnabled = true; B_Free.IsEnabled = B_Save.IsEnabled = false; }; // времено

            V_Logic.E_MeasurementNew += async (int V_PMTOut, int v_ReferenceOut, int v_ProbeOut, double v_OutExcitation, double v_OutEmission, double v_WaveDynamic, double v_WaveStatic, C_Calibration02 v_Calibration02) => 
            { TB_NumberRequest.Text = (int.Parse(TB_NumberRequest.Text) + 1).ToString(); TB_PMTOut.Text = V_PMTOut.ToString(); TB_ReferenceOut.Text = v_ReferenceOut.ToString(); TB_ProbeOut.Text = v_ProbeOut.ToString(); };

            V_Logic.E_MeasurementNew += async (int V_PMTOut, int v_ReferenceOut, int v_ProbeOut, double v_OutExcitation, double v_OutEmission, double v_WaveDynamic, double v_WaveStatic, C_Calibration02 v_Calibration02) =>
            { F_WriteTextAsync(System.DateTime.Now.ToLongTimeString() + " " + TB_PMTOut.Text + " " + /*TB_ReferenceOut.Text*/"" + " " + TB_ProbeOut.Text); };

            V_Logic.E_MeasurementNew += async (int V_PMTOut, int v_ReferenceOut, int v_ProbeOut, double v_OutExcitation, double v_OutEmission, double v_WaveDynamic, double v_WaveStatic, C_Calibration02 v_Calibration02) =>
            { TB_OutEmission.Text = v_OutEmission.ToString("e3"); TB_OutExcitation.Text = v_OutExcitation.ToString("e3"); };

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

        static async void F_WriteTextAsync(string v_str) // Временно
        {
            using (StreamWriter outputFile = new StreamWriter("D01out.txt", true))
            {
                await outputFile.WriteAsync(v_str + Environment.NewLine);
            }
        }
        /// <summary>
        /// Обновление настроек
        /// </summary>
        private void F_NewOptions ()
        {
            B_Start.IsEnabled = true;

            V_Logic.Fv_Options.Fv_Shift = Device001.Port.C_ParameterListsD02.F_ShiftGet()[CB_NumShift.SelectedIndex];
            V_Logic.Fv_Options.Fv_Speed = Device001.Port.C_ParameterListsD02.F_SpeedGet()[CB_NumSpeed.SelectedIndex];

            V_Logic.Fv_Options.Fv_NumTypeMeasurement = CB_TypeMeasurement.SelectedIndex;

            if (CB_TypeMeasurement.SelectedIndex == 0)
                try
                {
                    V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture);
                    V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorMin.Text, CultureInfo.InvariantCulture);
                }
                catch (ApplicationException e)
                {
                    MessageBox.Show(e.Message, "Недопустимая длина волны");
                    B_Start.IsEnabled = false;
                }
                catch (System.FormatException)
                {
                    B_Start.IsEnabled = false;
                }
            else
                try
                {
                    V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture);
                    V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorMin.Text, CultureInfo.InvariantCulture);
                }
                catch (ApplicationException e)
                {
                    MessageBox.Show(e.Message, "Недопустимая длина волны");
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
            try
            {
                V_Logic.F_Measurement_(
                    (float)double.Parse(TB_MonochromatorMin.Text, CultureInfo.InvariantCulture),
                    (float)double.Parse(TB_MonochromatorMax.Text, CultureInfo.InvariantCulture),
                    (byte)CB_NumShift.SelectedIndex,
                    (byte)CB_NumSpeed.SelectedIndex,
                    (float)double.Parse(TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture),
                    CB_TypeMeasurement.SelectedIndex,
                    double.Parse(TB_PMT.Text, CultureInfo.InvariantCulture));
                //V_Logic.F_GoPMT(double.Parse(TB_PMT.Text, CultureInfo.InvariantCulture));
            }
            catch (System.FormatException v_Ex)
            {
                System.Windows.MessageBox.Show(v_Ex.Message, "Ошибка данных");
            }
        }
        /// <summary>
        /// Стоп
        /// </summary>
        private void B_Stop_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (ThreadStart)delegate()
                {
                    V_Logic.F_Measurement_Off_();
                });   
        }
        /// <summary>
        /// Подключение
        /// </summary>
        private void B_Correction_Click(object sender, RoutedEventArgs e)
        {
            TB_NumberRequest.Text = (0).ToString();
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                (ThreadStart)delegate()
                {
                    V_Logic.F_Measurement_On_();
                });
        }
        /// <summary>
        /// Выход на длину волны статичного монохроматора
        /// </summary>
        private void B_WaveSattic_Click(object sender, RoutedEventArgs e)
        {
            bool v_key = true;
            try
            {
                V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture);
            }
            catch (ApplicationException V_MyException)
            {
                if (MessageBox.Show(V_MyException.Message + " Для подтверждения смены нажмите 'Да', для отмены 'Нет'.", "Недопустимая длина волны", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    V_Logic.Fv_Options.V_WaveStatic.F_NewWaveAndGrid(double.Parse(TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture));
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
                            V_Logic.F_GoWave(0);
                        });
                }
                else
                    v_key = false;

            }
            catch (System.FormatException)
            {
                v_key = false;
            }
            if (v_key)
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        V_Logic.F_GoWave(0);
                    });
        }
        /// <summary>
        /// Выход на длину волны динамичного монохроматора
        /// </summary>
        private void B_Dynamic_Click(object sender, RoutedEventArgs e)
        {
            bool v_key = true;
            try
            {
                V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorMin.Text, CultureInfo.InvariantCulture);
            }
            catch (ApplicationException V_MyException)
            {
                if (MessageBox.Show(V_MyException.Message + " Для подтверждения смены нажмите 'Да', для отмены 'Нет'.", "Недопустимая длина волны", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    V_Logic.Fv_Options.V_WaveDynamic.F_NewWaveAndGrid(double.Parse(TB_MonochromatorMin.Text, CultureInfo.InvariantCulture));
                }
                else
                    v_key = false;

            }
            catch (System.FormatException)
            {
                v_key = false;
            }
            if (v_key)
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (ThreadStart)delegate()
                    {
                        V_Logic.F_GoWave(1);
                    });
        }

        private void B_PMT_Click(object sender, RoutedEventArgs e)
        {
            double v_PMT = 0;
            try
            {
                v_PMT = double.Parse(TB_PMT.Text, CultureInfo.InvariantCulture);
                if ((0 < v_PMT) && (v_PMT<1250))
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
                            V_Logic.F_GoPMT(v_PMT, TB_Name.Text);
                        });
                else
                    System.Windows.MessageBox.Show("Ошибка напряжения", "Ошибка данных");
            }
            catch (System.FormatException v_Ex)
            {
                System.Windows.MessageBox.Show(v_Ex.Message, "Ошибка данных");
            }
        }

        private void B_Сalibration_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog() { };
            openFileDialog1.Filter = "Excel|*.xlsx;*.xls|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                V_Logic.Fv_Calibration.Fv_Address = openFileDialog1.FileName;
        }

        private void B_Сalibration02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_NewCalibration02();
        }

        private void B_Save_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_SaveMessurements();
        }

        private void B_PMTxk_Click(object sender, RoutedEventArgs e)
        {
            double v_PMT = 0;
            int k = 0;
            try
            {
                v_PMT = double.Parse(TB_PMT.Text, CultureInfo.InvariantCulture);
                k = int.Parse(TB_k.Text, CultureInfo.InvariantCulture);
                if ((0 < v_PMT) && (v_PMT < 1250))
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                        (ThreadStart)delegate()
                        {
                            for (; 0<k;--k )
                                V_Logic.F_GoPMT(v_PMT, TB_Name.Text);
                        });
                else
                    System.Windows.MessageBox.Show("Ошибка напряжения", "Ошибка данных");
            }
            catch (System.FormatException v_Ex)
            {
                System.Windows.MessageBox.Show(v_Ex.Message, "Ошибка данных");
            }
        }

        private void B_Free_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_free();
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
