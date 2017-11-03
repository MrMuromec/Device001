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
        public event D_CloseException Event_CloseException;

        W_Port1 V_w_D01; // Окно настроек 1 устр.
        W_Port1 V_w_D02; // Окно настроек 2 устр.

        W_Correction V_w_correction;

        W_Measurements V_WindowMeasument;

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
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D02 != null && V_w_D02.Activate()) V_w_correction.Close(); };

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
            //
            if (Event_CloseException != null)
                Event_CloseException();
        }
        /// <summary>
        /// Подключение
        /// </summary>
        public bool F_Measurement_On_()
        {
            try
            {
                //V_Command_D01.F_PortRun(100);
                //MessageBox.Show(V_Command_D02.Fv_PortName, "", MessageBoxButton.OKCancel);
                V_Command_D02.F_PortRun(100);
                //MessageBox.Show("Подключн " + V_Command_D02.Fv_PortName, "", MessageBoxButton.OKCancel);
                return true;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
                return false;
            }
        }
        /// <summary>
        /// Выключение
        /// </summary>
        public bool F_Measurement_Off_()
        {
            try
            {
                //V_Command_D01.F_PortStop(100);
                V_Command_D02.F_PortStop(100);
                return true;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
                return false;
            }
        }

        /// <summary>
        /// Запуск измерения с параметрами
        /// </summary>
        /// <param name="v_GridNumbersFirst"> I – монохр. номер решетки </param>
        /// <param name="v_GridNumbersSecond"> II – монохр. номер решетки </param>
        /// <param name="v_Type">  Тип корекции 0 - счетчик; 1 - репера </param>
        /// <param name="v_first">Начало диапазона</param>
        /// <param name="v_second">Конец диапазона</param>
        /// <param name="v_NummberShift">Номер шага</param>
        /// <param name="v_NumberSpeed">Номер скорости</param>
        /// <param name="v_ModeScan">Режим сканирования: 0 – I монохр, 1 – II монохр, 2 - параллельно</param>
        /// <param name="v_NoMove">Положение несканирующего монохр</param>
        /// <param name="v_Long">Длина массива</param>
        /// <param name="v_StrokesGridNum1"> Число штрихов решётки</param>
        /// <param name="v_StrokesGridNum2"> Число штрихов решётки</param>
        public void F_Measurement_(byte v_GridNumbersFirst, byte v_GridNumbersSecond, byte v_Type, float v_first, float v_second, byte v_NummberShift, byte v_NumberSpeed, List<byte> v_ModeScan, float v_NoMove, List<byte> v_Long, int v_StrokesGridNum1, int v_StrokesGridNum2)
        {
            
            if (V_w_correction == null || !V_w_correction.Activate())
            {
                V_w_correction = new W_Correction();
                V_w_correction.Event_UseCorrection += async (v_Correction) => 
                {
                    try
                    {
                        //MessageBox.Show("1", "", MessageBoxButton.OKCancel);

                        V_Command_D02.F_Com_Connection(100);
                        
                        //MessageBox.Show("Конект", "", MessageBoxButton.OKCancel);

                        V_Command_D02.F_Com_CorrectionType(v_Type, 100);
                        V_Command_D02.F_Com_Correction(v_Correction[1], v_Correction[0],600);

                        //MessageBox.Show("корекция", "", MessageBoxButton.OKCancel);

                        //V_Command_D02.F_Com_Scan(0, 500);
                        //V_Command_D02.F_Com_Grid();
                        V_Command_D02.F_Com_MonochromatorType(v_StrokesGridNum1, 0, 100);// Правки команды
                        V_Command_D02.F_Com_MonochromatorType(v_StrokesGridNum2, 1, 100);// Правки команды

                        MessageBox.Show("решётки", "", MessageBoxButton.OKCancel);

                        //V_Command_D02.F_Com_WaveLength(2, 250);
                        V_Command_D02.F_Com_WaveLength(0, v_NoMove, 100);
                        MessageBox.Show("не сканирующий", "", MessageBoxButton.OKCancel);
                        for (float v_Run = v_first; v_Run <= v_second; v_Run += (float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift])
                        {
                            V_Command_D02.F_Com_WaveLength(1, v_Run, 100);
                            Thread.Sleep((int)((float)Device001.Port.C_ParameterListsD02.F_ShiftGet()[v_NummberShift] / (Device001.Port.C_ParameterListsD02.F_SpeedGet()[v_NumberSpeed] / 60)));
                        }

                        //V_Command_D01.F_Command_Reset();
                        //V_Command_D01.F_Command_PMT(v_PMT);
                        /*
                        V_Command_D02.F_Com_Scan(0);

                        //MessageBox.Show("2", "", MessageBoxButton.OKCancel);

                        V_Command_D02.F_Com_MonochromatorType(v_StrokesGridNum1, 0);// Правки команды
                        V_Command_D02.F_Com_MonochromatorType(v_StrokesGridNum2, 1);// Правки команды

                        //MessageBox.Show("3", "", MessageBoxButton.OKCancel);

                        V_Command_D02.F_Com_CorrectionType(v_Type);
                        V_Command_D02.F_Com_Correction(v_First, v_Second);

                        V_Command_D02.F_Com_Grid(v_GridNumbersFirst, v_GridNumbersSecond);

                        //MessageBox.Show("4", "", MessageBoxButton.OKCancel);
                        */
                        //MessageBox.Show("5", "", MessageBoxButton.OKCancel);
                        //V_Command_D02.F_Com_OptionsScan(v_first, v_second, v_NummberShift, v_NumberSpeed, v_ModeScan, v_NoMove);

                        //V_Command_D01.F_Command_Request();
                    }
                    catch (ApplicationException v_error)
                    {
                        F_MyException(v_error);
                    }
                    catch (SystemException v_error)
                    {
                        F_MyException(v_error);
                    }
                    catch (Exception v_error)
                    {
                        F_MyException(v_error);
                    }
                };
                V_w_correction.Show();
            }
        }
    }
}
