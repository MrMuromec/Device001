using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using Excel = Microsoft.Office.Interop.Excel; // Переобозначение
using System.Globalization;
//
using Device001.Port;

namespace Device001
{
    public class C_Logic 
    {
        private C_Options V_Options = new C_Options();
        /// <summary>
        /// Настройки
        /// </summary>
        public C_Options Fv_Options
        {
            get
            {
                return V_Options;
            }
            set
            {
                V_Options = value;
            }
        }
        private C_Calibration V_Calibration = new C_Calibration();
        /// <summary>
        /// Калибровка
        /// </summary>
        public C_Calibration Fv_Calibration
        {
            get
            {
                return V_Calibration;
            }
            set
            {
                V_Calibration = value;
            }
        }
        /// <summary>
        /// Управление 1 устройством
        /// </summary>
        private C_CommandD01 V_Command_D01;
        private W_Port1 V_w_D01; // Окно настроек для 1 устр.
        /// <summary>
        /// Управление 2 устройством
        /// </summary>
        private C_CommandD02 V_Command_D02;
        private W_Port1 V_w_D02; // Окно настроек для 2 устр.

        public delegate void D_CloseException();
        public event D_CloseException E_CloseException;

        private W_Correction V_w_correction; // Окно настроек коррекции

        private W_Measurements V_WindowMeasument; // Окно измерений (основное окно)

        private W_Calibration V_w_Calibration; // Окно калибровки
        private C_Calibration02 V_Calibration02 = new C_Calibration02();

        private TimerCallback tm;
        private System.Threading.Timer timer;

        private int V_MonochromatorSelected;
        private float V_Run;
        private float V_RunShift;
        /// <summary>
        /// Настройки при загрузке
        /// </summary>
        public C_Logic()
        {
            V_Command_D01 = new C_CommandD01(
                "",
                C_PortOptions.F_GetStopBits()[1],
                C_PortOptions.F_GetParity()[2],
                C_PortOptions.F_GetBaudRate()[3],
                "OptionsD01.dat",
                true);
            V_Command_D01.F_LoadOptions();

            V_Command_D02 = new C_CommandD02(
                "",
                C_PortOptions.F_GetStopBits()[1],
                C_PortOptions.F_GetParity()[2],
                C_PortOptions.F_GetBaudRate()[3],
                "OptionsD02.dat",
                true);
            V_Command_D02.F_LoadOptions();

            V_WindowMeasument = new W_Measurements(this,
                Fv_Options.F_GetOperatingMode(),
                Fv_Options.F_GetTypeMeasurement());

            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D01 != null && V_w_D01.Activate()) V_w_D01.Close(); };
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D02 != null && V_w_D02.Activate()) V_w_D02.Close(); };
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_correction != null && V_w_correction.Activate()) V_w_correction.Close(); };
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_Calibration != null && V_w_Calibration.Activate()) V_w_Calibration.Close(); };

            //F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 3, 1, "qwerty");

            V_WindowMeasument.Show();
        }
        /// <summary>
        /// Запуск окна калибровки
        /// </summary>
        public void F_NewCalibration02()
        {
            if (V_w_Calibration == null || !V_w_Calibration.Activate())
            {
                V_w_Calibration = new W_Calibration(V_Calibration02);
                V_w_Calibration.Event_UseCalibration02 += async (v_NewCalibration02) => { V_Calibration02.F_SetCalibration02(v_NewCalibration02); };
                V_w_Calibration.Show();
            }
        }

        /// <summary>
        /// Запуск настроек 1 уст.
        /// </summary>
        public void F_OpenWD01()
        {
            if (V_w_D01==null || !V_w_D01.Activate())
            {
                V_w_D01 = new W_Port1((Device001.Port.C_MyPort)V_Command_D01, "Настройки D01");
                V_w_D01.Event_UseSettings += async (v_Port) => { await V_Command_D01.F_SetAndSaveOptions(v_Port); };
                V_w_D01.Show();
            }
        }
        /// <summary>
        /// Запуск настроек 2 уст.
        /// </summary>
        public void F_OpenWD02()
        {
            if (V_w_D02 == null || !V_w_D02.Activate())
            {
                V_w_D02 = new W_Port1((Device001.Port.C_MyPort)V_Command_D02, "Настройки D02");
                V_w_D02.Event_UseSettings += async (v_Port) => { await V_Command_D02.F_SetAndSaveOptions(v_Port); };
                V_w_D02.Show();
            }
        }
        /// <summary>
        /// Обработка ошибок
        /// </summary>
        /// <param name="v_error">Ошибка</param>
        private void F_MyException(Exception v_error)
        {
            MessageBox.Show(v_error.Message + " " + v_error.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            if (E_CloseException != null)
                E_CloseException();
        }

        public delegate void D_MeasurementOn();
        /// <summary>
        /// Успешное подключение
        /// </summary>
        public event D_MeasurementOn E_MeasurementOnSuccess;        
        /// <summary>
        /// Подключение
        /// </summary>
        public bool F_Measurement_On_()
        {
            try
            {
                if (V_Command_D01.V_OnOff) 
                    V_Command_D01.F_PortRun(100);
                if (V_Command_D02.V_OnOff)
                {
                    V_Command_D02.F_PortRun(100);
                    F_Correction();
                }
                if ((V_Command_D01.V_OnOff) && (E_MeasurementOnSuccess != null))
                    E_MeasurementOnSuccess();
                return true;
            }
            catch (Exception v_Ex)
            {
                F_MyException(v_Ex);
                return false;
            }
        }

        public delegate void D_MeasurementOff();
        /// <summary>
        /// Успешное выключение
        /// </summary>
        public event D_MeasurementOff E_MeasurementOffSuccess;
        /// <summary>
        /// Выключение
        /// </summary>
        public void F_Measurement_Off_()
        {
            try
            {
                if (V_Command_D01.V_OnOff) V_Command_D01.F_PortStop(100);
                if (V_Command_D02.V_OnOff) V_Command_D02.F_ComOut_211_Stop(100);
                if (V_Command_D02.V_OnOff) V_Command_D02.F_PortStop(100);
                if (timer!=null)
                {
                    timer.Dispose();
                    timer = null;
                }

                if (E_MeasurementOffSuccess != null)
                    E_MeasurementOffSuccess();
            }
            catch (ApplicationException v_Ex)
            {
                F_MyException(v_Ex);
            }
        }

        public delegate void D_MeasurementOnAndCorrection();
        /// <summary>
        /// Успешное подключение и ккоррекция
        /// </summary>
        public event D_MeasurementOnAndCorrection E_MeasurementOnAndCorrectionSuccess;

        /// <summary>
        /// Коррекция
        /// </summary>
        public void F_Correction()
        {
            if (V_w_correction == null || !V_w_correction.Activate())
            {
                V_w_correction = new W_Correction();
                V_w_correction.Event_UseCorrection += async (v_Correction) => 
                {
                    try
                    {
                        if (V_Command_D02.V_OnOff)
                        {
                        V_Command_D02.F_ComOut_Connection(100);

                        //V_Command_D02.F_Com_Control(0, 200);

                        V_Command_D02.F_ComOut_204_CorrectionType(0, 200);
                        V_Command_D02.F_ComOut_205_Correction(v_Correction[1], v_Correction[0], 600);

                        V_Command_D02.F_ComOut_214_MonochromatorType(0, 100);
                        V_Command_D02.F_ComOut_214_MonochromatorType(1, 100);

                        V_Command_D02.F_ComOut_206_Grid(0, 0);
                        //V_Command_D02.F_ComOut_215_ReplacementGrid(1);

                        //V_Command_D02.F_Com_Scan(0, 200);

                        if (E_MeasurementOnAndCorrectionSuccess != null)
                            E_MeasurementOnAndCorrectionSuccess();
                        }
                    }
                    catch (ApplicationException v_Ex)
                    {
                        F_MyException(v_Ex);
                    }

                };
                V_w_correction.Show();
            }
        }
        /// <summary>
        /// Выйти на длину волны
        /// </summary>
        /// <param name="v_Monochromator">номер монохроматора</param>
        /// <param name="v_WaveLength">длина волны</param>
        /// <param name="v_TimeToSleep">время на выполнение</param>
        public void F_GoWave(byte v_Monochromator = 0, float v_WaveLength = 0, Int32 v_TimeToSleep = 100)
        {
            try
            {
                if (V_Command_D02.V_OnOff) V_Command_D02.F_ComOut_200_WaveLength(v_Monochromator, v_WaveLength, v_TimeToSleep);
                //if (V_Command_D02.V_OnOff) V_Command_D02.F_ComOut_220_ReachWavelength(v_Monochromator, v_WaveLength, v_TimeToSleep);
                //if (V_Command_D02.V_OnOff) V_Command_D02.F_Com_ReachWavelength(v_Monochromator, v_WaveLength, v_TimeToSleep);
                //if (V_Command_D02.V_OnOff) V_Command_D02.F_Com_ReachWavelength(v_WaveLength, v_TimeToSleep);
            }
            catch (ApplicationException v_Ex)
            {
                F_MyException(v_Ex);
            }
        }
        /// <summary>
        /// Установка напряжения ФЭУ + измерение
        /// </summary>
        /// <param name="v_PMT">Установка напряжения ФЭУ. 0xFF соответствует 1250 В.</param>
        public void F_GoPMT(double v_PMT)
        {
            try
            {
                if (V_Command_D01.V_OnOff) 
                    if (/*V_Command_D01.F_Command_Reset()*/ true)
                        if (V_Command_D01.F_Command_PMT((byte)(255 * (v_PMT / 1250))))
                            F_Request();
            }
            catch (ApplicationException v_Ex)
            {
                F_MyException(v_Ex);
            }

        }
        public delegate void D_MeasurementNew(int V_PMTOut, int v_ReferenceOut, int v_ProbeOut, double v_OutExcitation, double v_OutEmission);
        /// <summary>
        /// Новое измерение
        /// </summary>
        public event D_MeasurementNew E_MeasurementNew;
        private int V_NumberRequest;
        /// <summary>
        /// Обновление данных
        /// </summary>
        public void F_Request()
        {
            try
            {
                if (V_Command_D01.V_OnOff)
                    if (V_Command_D01.F_Command_Request() && (E_MeasurementNew != null))
                    {
                        E_MeasurementNew(
                            V_Command_D01.F_Measurement_D01(0), 
                            V_Command_D01.F_Measurement_D01(1),
                            V_Command_D01.F_Measurement_D01(2),
                            V_Calibration02.V_Excitation.F_SpectralDensitiesOfFlows(V_Command_D01.F_Measurement_D01(2), double.Parse(V_WindowMeasument.TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture)),
                            V_Calibration02.V_Emission.F_SpectralDensitiesOfFlows(V_Command_D01.F_Measurement_D01(0), double.Parse(V_WindowMeasument.TB_MonochromatorMin.Text, CultureInfo.InvariantCulture)));
                        ++V_NumberRequest;
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 1, V_NumberRequest, System.DateTime.Now.ToLongTimeString());
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 2 , V_NumberRequest, V_Command_D01.F_Measurement_D01(0).ToString());
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 3 , V_NumberRequest, V_Command_D01.F_Measurement_D01(1).ToString());
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 4 , V_NumberRequest, V_Command_D01.F_Measurement_D01(2).ToString());


                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 6, V_NumberRequest, V_WindowMeasument.TB_MonochromatorStaticOrDynamic.Text);
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 7, V_NumberRequest, V_Calibration02.V_Excitation.F_SpectralDensitiesOfFlows(V_Command_D01.F_Measurement_D01(2), double.Parse(V_WindowMeasument.TB_MonochromatorStaticOrDynamic.Text, CultureInfo.InvariantCulture)).ToString());
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 9, V_NumberRequest, V_WindowMeasument.TB_MonochromatorMin.Text);
                        F_ExelSet(Environment.CurrentDirectory + @"\" + "Measument.xlsx", "Measument", 10, V_NumberRequest, V_Calibration02.V_Emission.F_SpectralDensitiesOfFlows(V_Command_D01.F_Measurement_D01(0), double.Parse(V_WindowMeasument.TB_MonochromatorMin.Text, CultureInfo.InvariantCulture)).ToString());
                    }
            }
            catch (ApplicationException v_Ex)
            {
                F_MyException(v_Ex);
            }
        }
        /// <summary>
        /// Запуск измерения с параметрами
        /// </summary>
        /// <param name="v_first">Начало диапазона</param>
        /// <param name="v_second">Конец диапазона</param>
        /// <param name="v_NummberShift">Номер шага</param>
        /// <param name="v_NumberSpeed">Номер скорости</param>
        /// <param name="v_NoMove">Положение несканирующего монохр</param>
        /// <param name="v_MonochromatorSelected">Выбранный монохраматор</param>
        /// <param name="v_PMT">Установка напряжения ФЭУ. 0xFF соответствует 1250 В.</param>
        public void F_Measurement_(float v_first, float v_second, byte v_NummberShift, byte v_NumberSpeed, float v_NoMove, int v_MonochromatorSelected, double v_PMT)
        {
            try
            {
                if (V_Command_D01.V_OnOff) V_Command_D01.F_Command_PMT((byte)(255 * (v_PMT / 1250)));
                if (V_Command_D02.V_OnOff) F_GoWave((byte)v_MonochromatorSelected, v_NoMove, 100);

                V_MonochromatorSelected = (byte)(1 - v_MonochromatorSelected);

                V_Run = v_first;
                V_RunShift = (float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift];

                tm = new TimerCallback(Count);
                // создаем таймер
                timer = new System.Threading.Timer(
                    tm, 
                    this, 
                    0,
                    (int)((float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift] / (Device001.Port.C_ParameterListsD02.F_SpeedGet()[v_NumberSpeed] / 60)));
                /*
                if (V_Command_D02.V_OnOff) F_GoWave((byte)v_MonochromatorSelected, v_NoMove, 100);
                for (float v_Run = v_first; v_Run <= v_second; v_Run += (float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift])
                {
                    if (V_Command_D02.V_OnOff) F_GoWave((byte)(1 - v_MonochromatorSelected), v_Run, 100);
                    Thread.Sleep(0);
                    F_Request();
                }
                 * */
            }
            catch (ApplicationException v_Ex)
            {
                F_MyException(v_Ex);
            }
            catch (Exception v_Ex)
            {
                F_MyException(v_Ex);
            }
        }
        /// <summary>
        /// Метод вызываемый по таймеру
        /// </summary>
        /// <param name="obj"></param>
        public static void Count(object obj)
        {
            C_Logic v_logic = ((C_Logic)obj);
            if (v_logic.V_Command_D01.V_OnOff) 
                v_logic.F_Request();            
            if (v_logic.V_Command_D02.V_OnOff)
                v_logic.F_GoWave((byte)(v_logic.V_MonochromatorSelected), v_logic.V_Run, 100);
            v_logic.V_Run += v_logic.V_RunShift;
        }

        private object _missingObj = System.Reflection.Missing.Value;
        private Excel.Application _ObjExcel = null; // Приложение
        private Excel.Workbook _ObjWorkBook = null; // Книга
        private Excel.Worksheet _ObjWorkSheet = null; // Листы

        private bool F_ExelSet(string v_ExcelName, string v_SheetsName, int v_Colum, int v_Row, string v_var)
        {
            // Создаём приложение
            _ObjExcel = new Excel.Application();
            // Открываем книгу                                                                                                                                                        
            //_ObjWorkBook = _ObjExcel.Workbooks.Open(v_ExcelName,0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            _ObjWorkBook = _ObjExcel.Workbooks.Open(v_ExcelName);
            for (int i = 1; i <= _ObjWorkBook.Sheets.Count; i++)
                if (v_SheetsName == ((Excel.Worksheet)_ObjWorkBook.Sheets[i]).Name)
                    _ObjWorkSheet = _ObjWorkBook.Sheets[i];

            _ObjWorkSheet.Cells[v_Row,v_Colum] = v_var;

            _ObjWorkBook.Save();

            return F_ExelFree();
        }
        private bool F_ExelFree()
        {
            // Уборка неуправляемого мусора
            if (_ObjWorkBook != null)
                _ObjWorkBook.Close(false, _missingObj, _missingObj);
            if (_ObjExcel != null)
            {
                _ObjExcel.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_ObjExcel);
            }
            _ObjWorkBook = null;
            _ObjExcel = null;
            _ObjWorkSheet = null;
            System.GC.Collect();
            return true;
        }
        ~C_Logic()
        {
            F_ExelFree();
        }
    }
}
