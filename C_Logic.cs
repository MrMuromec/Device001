using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Windows;
//
using Device001.Port;
using System.Threading;

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
        private C_Calibration V_Calibration = new C_Calibration("AddressOfCalibrationFile.dat");
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
        private bool V_D01_OnOff = true;
        /// <summary>
        /// Управление 2 устройством
        /// </summary>
        private C_CommandD02 V_Command_D02;
        private bool V_D02_OnOff = false;

        public delegate void D_CloseException();
        public event D_CloseException E_CloseException;

        private W_Port1 V_w_D01; // Окно настроек 1 устр.
        private W_Port1 V_w_D02; // Окно настроек 2 устр.

        private W_Correction V_w_correction; // Окно настроек коррекции

        private W_Measurements V_WindowMeasument; // Окно измерений (основное окно)

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
                "OptionsD01.dat");
            V_Command_D01.F_LoadOptions();

            V_Command_D02 = new C_CommandD02(
                "",
                C_PortOptions.F_GetStopBits()[1],
                C_PortOptions.F_GetParity()[2],
                C_PortOptions.F_GetBaudRate()[3],
                "OptionsD02.dat");
            V_Command_D02.F_LoadOptions();

            V_WindowMeasument = new W_Measurements(this,
                Fv_Options.F_GetOperatingMode(),
                Fv_Options.F_GetTypeMeasurement());

            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D01 != null && V_w_D01.Activate()) V_w_D01.Close(); };
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D02 != null && V_w_D02.Activate()) V_w_D02.Close(); };
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_correction != null && V_w_correction.Activate()) V_w_correction.Close(); };

            V_WindowMeasument.Show();
        }
        /// <summary>
        /// Запуск настроек 1 уст.
        /// </summary>
        public void F_OpenWD01()
        {
            if (V_w_D01==null || !V_w_D01.Activate())
            {
                V_w_D01 = new W_Port1((Device001.Port.C_MyPort)V_Command_D01, "Настройки D01");
                V_w_D01.Event_UseSettings += async (v_Port) => { V_Command_D01.F_SetAndSaveOptions(v_Port); };
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
                V_w_D02.Event_UseSettings += async (v_Port) => { V_Command_D02.F_SetAndSaveOptions(v_Port); };
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
        /// Успешное подключение и ккоррекция
        /// </summary>
        public event D_MeasurementOn E_MeasurementOnSuccess;        
        /// <summary>
        /// Подключение
        /// </summary>
        public bool F_Measurement_On_()
        {
            try
            {
                if (V_D01_OnOff) V_Command_D01.F_PortRun(100);
                if (V_D02_OnOff) V_Command_D02.F_PortRun(100);
                if (E_MeasurementOnSuccess != null)
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
                if (V_D01_OnOff) V_Command_D01.F_PortStop(100);
                if (V_D02_OnOff) V_Command_D02.F_Com_Stop(100);
                if (V_D02_OnOff) V_Command_D02.F_PortStop(100);

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
                    if (F_Measurement_On_())
                    {
                        if (V_D02_OnOff) V_Command_D02.F_Com_Connection(200);

                        //if (V_D02_OnOff)V_Command_D02.F_Com_Control(0, 200);

                        if (V_D02_OnOff) V_Command_D02.F_Com_CorrectionType(0, 200);
                        if (V_D02_OnOff) V_Command_D02.F_Com_Correction(v_Correction[1], v_Correction[0], 600);

                        if (V_D02_OnOff) V_Command_D02.F_Com_MonochromatorType(0, 200);
                        if (V_D02_OnOff) V_Command_D02.F_Com_MonochromatorType(1, 200);

                        //if (V_D02_OnOff) V_Command_D02.F_Com_Scan(0, 200);

                        if (E_MeasurementOnAndCorrectionSuccess != null)
                            E_MeasurementOnAndCorrectionSuccess();

                    }
                };
                V_w_correction.Show();
            }
        }
        public void F_GoWave(byte v_Monochromator = 0, float v_WaveLength = 0, Int32 v_TimeToSleep = 500)
        {
            //if (V_D02_OnOff) V_Command_D02.F_Com_WaveLength(v_Monochromator, v_WaveLength, 100);
            if (V_D02_OnOff) V_Command_D02.F_Com_ReachWavelength(v_Monochromator, v_WaveLength, 200);
            //if (V_D02_OnOff) V_Command_D02.F_Com_ReachWavelength(v_WaveLength, 100);
        }

        public delegate void D_MeasurementNew(int V_PMTOut, int v_ReferenceOut, int v_ProbeOut);
        /// <summary>
        /// Успешное подключение и ккоррекция
        /// </summary>
        public event D_MeasurementNew E_MeasurementNew;

        /// <summary>
        /// Установка напряжения ФЭУ + измерение
        /// </summary>
        /// <param name="v_PMT">Установка напряжения ФЭУ. 0xFF соответствует 1250 В.</param>
        public void F_GoPMT(double v_PMT)
        {
            if (V_D01_OnOff) V_Command_D01.F_Command_Reset();
            if (V_D01_OnOff) V_Command_D01.F_Command_PMT((byte)(255 * (v_PMT / 1250)));
            F_Request();
        }

        public void F_Request()
        {
            if (V_D01_OnOff) V_Command_D01.F_Command_Request();
            if (V_D01_OnOff)
                if (E_MeasurementNew != null)
                    E_MeasurementNew(V_Command_D01.F_Measurement_D01(0), V_Command_D01.F_Measurement_D01(1), V_Command_D01.F_Measurement_D01(2));
            //MessageBox.Show("величина сигнала ФЭУ (" + V_Command_D01.F_Measurement_D01(0).ToString() + "), величина сигнала опорного канала (" + V_Command_D01.F_Measurement_D01(1).ToString() + "), величина сигнала зонда (" + V_Command_D01.F_Measurement_D01(2).ToString() + "),");
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
                if (V_D01_OnOff) V_Command_D01.F_Command_Reset();
                if (V_D01_OnOff) V_Command_D01.F_Command_PMT((byte)(255 * (v_PMT / 1250)));

                if (V_D02_OnOff) F_GoWave((byte)v_MonochromatorSelected, v_NoMove, 100);
                for (float v_Run = v_first; v_Run <= v_second; v_Run += (float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift])
                {
                    if (V_D02_OnOff) F_GoWave((byte)(1 - v_MonochromatorSelected), v_Run, 100);
                    Thread.Sleep((int)((float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift] / (Device001.Port.C_ParameterListsD02.F_SpeedGet()[v_NumberSpeed] / 60)));
                    F_Request();
                }
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
    }
}
