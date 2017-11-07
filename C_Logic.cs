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

        /// <summary>
        /// Управление 1 устройством
        /// </summary>
        private C_CommandD01 V_Command_D01;
        /// <summary>
        /// Управление 2 устройством
        /// </summary>
        private C_CommandD02 V_Command_D02;

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
        
        /// <summary>
        /// Подключение
        /// </summary>
        public bool F_Measurement_On_()
        {
            try
            {
                //V_Command_D01.F_PortRun(100);
                V_Command_D02.F_PortRun(100);
                return true;
            }
            catch (ApplicationException v_Ex)
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
                //V_Command_D01.F_PortStop(100);
                V_Command_D02.F_Com_Stop(100);
                V_Command_D02.F_PortStop(100);

                if (E_MeasurementOffSuccess != null)
                    E_MeasurementOffSuccess();
            }
            catch (ApplicationException v_Ex)
            {
                F_MyException(v_Ex);
            }
        }

        public delegate void D_MeasurementOn();
        /// <summary>
        /// Успешное подключение
        /// </summary>
        public event D_MeasurementOn E_MeasurementOnSuccess;

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
                        V_Command_D02.F_Com_Connection(100);

                        V_Command_D02.F_Com_Control(0, 100);

                        V_Command_D02.F_Com_CorrectionType(0, 100);
                        V_Command_D02.F_Com_Correction(v_Correction[1], v_Correction[0], 600);

                        V_Command_D02.F_Com_MonochromatorType(0, 100);
                        V_Command_D02.F_Com_MonochromatorType(1, 100);

                        if (E_MeasurementOnSuccess != null)
                            E_MeasurementOnSuccess();
                    }
                };
                V_w_correction.Show();
            }
        }
        public void F_GoWave(byte v_Monochromator = 0, float v_WaveLength = 0)
        {
            V_Command_D02.F_Com_WaveLength(v_Monochromator, v_WaveLength, 500);
        }
        /// <summary>
        /// Установка напряжения ФЭУ + измерение
        /// </summary>
        /// <param name="v_PMT">Установка напряжения ФЭУ. 0xFF соответствует 1250 В.</param>
        public void F_GoPMT(double v_PMT)
        {
            V_Command_D01.F_Command_Reset();
            V_Command_D01.F_Command_PMT((byte)(255*(v_PMT/1250)));
            V_Command_D01.F_Command_Request();
            MessageBox.Show("величина сигнала ФЭУ (" + V_Command_D01.F_Measurement_D01(0).ToString() + "), величина сигнала опорного канала (" + V_Command_D01.F_Measurement_D01(1).ToString() + "), величина сигнала зонда (" + V_Command_D01.F_Measurement_D01(2).ToString() + "),");
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
                //V_Command_D01.F_Command_Reset();
                //V_Command_D01.F_Command_PMT((byte)(255*(v_PMT/1250)));

                V_Command_D02.F_Com_WaveLength((byte)v_MonochromatorSelected, v_NoMove, 100);
                for (float v_Run = v_first; v_Run <= v_second; v_Run += (float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift])
                {
                    V_Command_D02.F_Com_WaveLength((byte)(1 - v_MonochromatorSelected), v_Run, 100);
                    Thread.Sleep((int)((float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift] / (Device001.Port.C_ParameterListsD02.F_SpeedGet()[v_NumberSpeed] / 60)));
                    //V_Command_D01.F_Command_Request();
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
