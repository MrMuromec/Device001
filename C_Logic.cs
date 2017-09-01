using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Windows;
//
using Device001.Port;

namespace Device001
{
    public class C_Logic
    {
        private Dictionary<string, string> V_PortD01; // Настройки 1 устройства
        private Dictionary<string, string> V_PortD02; // Настройки 2 устройства

        private C_CommandD01 V_Command_D01; // Управление 1 устройством
        private C_CommandD02 V_Command_D02; // Управление 2 устройством

        public delegate void D_CloseException();
        public event D_CloseException Event_CloseException;

        /// <summary>
        /// Настройки пи загрузке
        /// </summary>
        public C_Logic()
        {
            V_PortD01 = new Dictionary<string, string>
            {
                {"NamePort",""},
                {"StopBits",C_PortOptions.F_GetStopBits()[1].ToString()},
                {"Parity",C_PortOptions.F_GetParity()[2].ToString()},
                {"BaudRate",C_PortOptions.F_GetBaudRate()[3].ToString()}
            };

            V_Command_D01 = new C_CommandD01(
                V_PortD01["NamePort"],
                C_PortOptions.F_StopBits(V_PortD01["StopBits"]),
                C_PortOptions.F_Parity(V_PortD01["Parity"]),
                C_PortOptions.F_BaudRate(V_PortD01["BaudRate"]));

            V_PortD02 = new Dictionary<string, string>
            {
                {"NamePort",""},
                {"StopBits",C_PortOptions.F_GetStopBits()[1].ToString()},
                {"Parity",C_PortOptions.F_GetParity()[2].ToString()},
                {"BaudRate",C_PortOptions.F_GetBaudRate()[3].ToString()}
            };

            V_Command_D02 = new C_CommandD02(
                V_PortD02["NamePort"],
                C_PortOptions.F_StopBits(V_PortD02["StopBits"]),
                C_PortOptions.F_Parity(V_PortD02["Parity"]),
                C_PortOptions.F_BaudRate(V_PortD02["BaudRate"]));

        }
        /// <summary>
        /// Словарь с настройками к 1 уст.
        /// </summary>
        public Dictionary<string, string> F_GetPortD01()
        {
            return V_PortD01;
        }
        /// <summary>
        /// Словарь с настройками к 2 уст.
        /// </summary>
        public Dictionary<string, string> F_GetPortD02()
        {
            return V_PortD02;
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
                //
                V_Command_D01.F_PortRun(100);
                V_Command_D01.Event_End_D01 += F_Measurement_End_D01;

                V_Command_D02.F_PortRun(100);
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
        /// <param name="v_First"> Положение II монохроматора (нм.)</param>
        /// <param name="v_Second"> Положение I монохроматора введенное оператором в случае коррекции по счетчику или произвольные числа при коррекции по реперам</param>
        /// <param name="v_first">Начало диапазона</param>
        /// <param name="v_second">Конец диапазона</param>
        /// <param name="v_NummberShift">Номер шага</param>
        /// <param name="v_NumberSpeed">Номер скорости</param>
        /// <param name="v_ModeScan">Режим сканирования: 0 – I монохр, 1 – II монохр, 2 - параллельно</param>
        /// <param name="v_NoMove">Положение несканирующего монохр</param>
        /// <param name="v_Long">Длина массива</param>
        public bool F_Measurement_(byte v_GridNumbersFirst, byte v_GridNumbersSecond, byte v_Type, float v_First, float v_Second, float v_first, float v_second, byte v_NummberShift, byte v_NumberSpeed, List<byte> v_ModeScan, float v_NoMove, List<byte> v_Long)
        {
            try
            {
                // Все команды должны вызываться от суда для синхронизации
                //V_Command_D01.F_Measurement_Run_D01(0x00);
                //V_Command_D02.F_Measurement_Run_D02(v_GridNumbersFirst ,v_GridNumbersSecond ,v_Type ,v_First ,v_Second ,v_first,v_second,v_NummberShift,v_NumberSpeed,v_ModeScan,v_NoMove,v_Long);
                return true;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
                return false;
            }
        }
        private void F_Measurement_End_D01()
        {
            //
        }
        /// <summary>
        /// Выключение
        /// </summary>
        public bool F_Measurement_Off_()
        {
            try
            {
                //
                V_Command_D01.F_PortStop(100);
                V_Command_D01.Event_End_D01 -= F_Measurement_End_D01;

                V_Command_D02.F_PortStop(100);
                return true;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
                return false;
            }
        }
    }
}
