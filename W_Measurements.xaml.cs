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

        public W_Measurements(C_Logic v_Logic)
        {
            InitializeComponent();

            V_Logic = v_Logic;

            foreach (var v_type in V_Logic.Fv_Options.F_GetTypeMeasurement())
                CB_TypeMeasurement.Items.Add(v_type);

            foreach (C_ParametorGrid.S_ParameterGrid v_grid in C_ParametorGrid.F_GridGet())
            {
                CB_MonochromatorDynamicGrid.Items.Add(v_grid.Fv_NumberStrokes);
                CB_MonochromatorStaticGrid.Items.Add(v_grid.Fv_NumberStrokes);
            }

            CB_TypeMeasurement.SelectionChanged += async (s, e1) => 
            {
                if (CB_TypeMeasurement.SelectedIndex == 0)
                    T_MonochromatorStaticOrDynamic.Text = "Длина волны возбуждения";
                else
                    T_MonochromatorStaticOrDynamic.Text = "Длина волны эмиссии";
                V_Logic.Fv_Options.Fv_NumTypeMeasurement = CB_TypeMeasurement.SelectedIndex;

                TB_MonochromatorStatic.Text = V_Logic.Fv_Options.V_WaveStatic.Fv_wave.ToString("F3", CultureInfo.InvariantCulture);
                TB_MonochromatorDynamic.Text = V_Logic.Fv_Options.V_WaveDynamic.Fv_wave.ToString("F3", CultureInfo.InvariantCulture);
                TB_MonochromatorMinDynamic.Text = V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid.Fv_Min.ToString("F3", CultureInfo.InvariantCulture);

                CB_MonochromatorDynamicGrid.SelectedIndex = C_ParametorGrid.F_GridGet().FindIndex(x => x.Equals(V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid));
                CB_MonochromatorStaticGrid.SelectedIndex = C_ParametorGrid.F_GridGet().FindIndex(x => x.Equals(V_Logic.Fv_Options.V_WaveStatic.Fv_ParameterGrid));
            };
            CB_TypeMeasurement.SelectedIndex = V_Logic.Fv_Options.Fv_NumTypeMeasurement;

            CB_MonochromatorDynamicGrid.LostFocus += async (s, e1) => { F_NewOptions(); }; // Применение новых настроек
            CB_MonochromatorStaticGrid.SelectionChanged += async (s, e1) => { F_NewOptions(); }; // Применение новых настроек
            TB_MonochromatorStatic.LostFocus += async (s, e1) => { F_NewOptions(); }; // Применение новых настроек
            TB_MonochromatorDynamic.LostFocus += async (s, e1) => { F_NewOptions(); }; // Применение новых настроек
            TB_MonochromatorMinDynamic.LostFocus += async (s, e1) => { F_NewOptions(); }; // Применение новых настроек

            V_Logic.E_CloseException += async () => { this.Close(); }; // Закрытие из за ошибок

            V_Logic.E_MeasurementOnSuccess += async () =>
            {
                B_Correction.IsEnabled = B_Free.IsEnabled = B_Save.IsEnabled = B_Stop.IsEnabled = TB_Name.IsEnabled = Gr_OptionsD01.IsEnabled = true;
                B_Сalibration02.IsEnabled = B_On.IsEnabled = B_D01.IsEnabled = B_D02.IsEnabled = false;
            }; // Блокировка/Активация элементов интерфейса при успешном подключении

            V_Logic.E_MeasurementCorrectionSuccess += async () => 
            {
                B_Start.IsEnabled = B_WaveSattic.IsEnabled = B_Dynamic.IsEnabled = Gr_OptionsD01.IsEnabled = Gr_OptionsD02.IsEnabled = true;
                B_D01.IsEnabled = B_D02.IsEnabled = B_On.IsEnabled = false;
            }; // Блокировка/Активация элементов интерфейса при успешной корекции

            V_Logic.E_MeasurementOffSuccess += async () =>
            {
                B_Сalibration02.IsEnabled = B_D01.IsEnabled = B_D02.IsEnabled = B_On.IsEnabled = true;
                B_Correction.IsEnabled = B_Free.IsEnabled = B_Start.IsEnabled = B_Save.IsEnabled = B_WaveSattic.IsEnabled = B_Dynamic.IsEnabled = B_Stop.IsEnabled = TB_Name.IsEnabled = Gr_OptionsD02.IsEnabled = Gr_OptionsD01.IsEnabled = false;
            }; // Блокировка/Активация элементов интерфейса при успешном отключении

            V_Logic.E_MeasurementNew += async (int V_PMTOut, int v_ReferenceOut, int v_ProbeOut, double v_OutExcitation, double v_OutEmission, double v_WaveDynamic, double v_WaveStatic, C_Calibration02 v_Calibration02) => 
            { 
                TB_NumberRequest.Text = (int.Parse(TB_NumberRequest.Text) + 1).ToString(); TB_PMTOut.Text = V_PMTOut.ToString(); 
                TB_ReferenceOut.Text = v_ReferenceOut.ToString(); 
                TB_ProbeOut.Text = v_ProbeOut.ToString();

                TB_OutEmission.Text = v_OutEmission.ToString("e3");
                TB_OutExcitation.Text = v_OutExcitation.ToString("e3"); 
            };

            WinFH_Paint = new System.Windows.Forms.Integration.WindowsFormsHost();           
        }
        /// <summary>
        /// Настройка 1 уст.
        /// </summary>
        private void B_D01_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD01(); // Настройка 1 уст.
        }
        /// <summary>
        /// Настройка 2 уст.
        /// </summary>
        private void B_D02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_OpenWD02();
        }
        object v_Key = new object();
        /// <summary>
        /// Обновление настроек
        /// </summary>
        private void F_NewOptions ()
        {
            lock (v_Key)
            {
                B_Start.IsEnabled = true;
                try
                {
                    V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStatic.Text, CultureInfo.InvariantCulture);
                    V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorDynamic.Text, CultureInfo.InvariantCulture);
                    V_Logic.Fv_Options.V_WaveDynamic.Fv_ParameterGrid = C_ParametorGrid.F_GridGet()[CB_MonochromatorDynamicGrid.SelectedIndex];
                    V_Logic.Fv_Options.V_WaveStatic.Fv_ParameterGrid = C_ParametorGrid.F_GridGet()[CB_MonochromatorStaticGrid.SelectedIndex];
                }
                catch (ApplicationException e)
                {
                    B_Start.IsEnabled = false;
                }
                catch (System.FormatException)
                {
                    B_Start.IsEnabled = false;
                }
            }

            //F_Paint();
        }

        /// <summary>
        /// Старт
        /// </summary>
        private void B_Start_Click(object sender, RoutedEventArgs e)
        {
            double v_PMT = 0;
            double v_MinWave = 200;
            int k = 0;
            try
            {
                if (V_Logic.Fv_Options.V_WaveStatic.V_Error != null)
                    MessageBox.Show(V_Logic.Fv_Options.V_WaveStatic.V_Error.Message, "Недопустимая длина волны");
                else
                    if (V_Logic.Fv_Options.V_WaveDynamic.V_Error != null)
                        MessageBox.Show(V_Logic.Fv_Options.V_WaveDynamic.V_Error.Message, "Недопустимая длина волны");
                    else
                    {
                        v_PMT = double.Parse(TB_PMT.Text, CultureInfo.InvariantCulture);
                        k = int.Parse(TB_k.Text, CultureInfo.InvariantCulture);
                        v_MinWave = double.Parse(TB_MonochromatorMinDynamic.Text, CultureInfo.InvariantCulture);
                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                            (ThreadStart)delegate()
                            {
                                V_Logic.F_Measurement_(1, k, TB_Name.Text, v_PMT, v_MinWave);
                            });
                    }
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
        private void B_On_Click(object sender, RoutedEventArgs e)
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
            try
            {
                V_Logic.Fv_Options.V_WaveStatic.Fv_wave = double.Parse(TB_MonochromatorStatic.Text, CultureInfo.InvariantCulture);
                if (V_Logic.Fv_Options.V_WaveStatic.V_Error == null)
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                        (ThreadStart)delegate() { V_Logic.F_GoWave(true); });
                else
                    MessageBox.Show(V_Logic.Fv_Options.V_WaveStatic.V_Error.Message, "Недопустимая длина волны");
            }
            catch (System.FormatException v_error)
            {
                MessageBox.Show(v_error.Message, "Неизвестная длина волны");
            }
        }
        /// <summary>
        /// Выход на длину волны динамичного монохроматора
        /// </summary>
        private void B_Dynamic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                V_Logic.Fv_Options.V_WaveDynamic.Fv_wave = double.Parse(TB_MonochromatorDynamic.Text, CultureInfo.InvariantCulture);
                if (V_Logic.Fv_Options.V_WaveDynamic.V_Error == null)
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
                        (ThreadStart)delegate() { V_Logic.F_GoWave(false); });
                else
                    MessageBox.Show(V_Logic.Fv_Options.V_WaveDynamic.V_Error.Message, "Недопустимая длина волны");
            }
            catch (System.FormatException v_error)
            {
                MessageBox.Show(v_error.Message, "Неизвестная длина волны");
            }
        }
        /// <summary>
        /// Установка напряжения ФЕУ
        /// </summary>
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
        /// <summary>
        /// Калбровка
        /// </summary>
        private void B_Сalibration02_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_NewCalibration02();
        }
        /// <summary>
        /// Сохранение измерений
        /// </summary>
        private void B_Save_Click(object sender, RoutedEventArgs e)
        {
            V_Logic.F_SaveMessurements();
        }
        /// <summary>
        /// Множественная отправка
        /// </summary>
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
        /// <summary>
        /// Сброс записанных данных
        /// </summary>
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
