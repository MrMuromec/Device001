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
    public class C_Logic : C_Waves
    {
        private Dictionary<string, string> V_PortD01_Options = new Dictionary<string, string>
            {
                {"NamePort",""},
                {"StopBits",C_PortOptions.F_GetStopBits()[1].ToString()},
                {"Parity",C_PortOptions.F_GetParity()[2].ToString()},
                {"BaudRate",C_PortOptions.F_GetBaudRate()[3].ToString()}
            }; // Настройки 1 устройства
        private Dictionary<string, string> V_PortD02_Options = new Dictionary<string, string>
            {
                {"NamePort",""},
                {"StopBits",C_PortOptions.F_GetStopBits()[1].ToString()},
                {"Parity",C_PortOptions.F_GetParity()[2].ToString()},
                {"BaudRate",C_PortOptions.F_GetBaudRate()[3].ToString()}
            }; // Настройки 2 устройства

        private C_CommandD01 V_Command_D01; // Управление 1 устройством
        private C_CommandD02 V_Command_D02; // Управление 2 устройством

        private List<string> V_OperatingMode = new List<string>() { "Сигналы", "Спектры" }; // Формат данных
        private List<string> V_TypeMeasurement = new List<string>() { "Возбуждение", "Эмиссия" }; // Режим

        private Dictionary<string, int> V_SelectedOptionsOfMeasument = new Dictionary<string, int>
            {
                {"StrokesGrid1",0},
                {"StrokesGrid2",0},
                {"NumShift",0},
                {"NumSpeed",0},
                {"OperatingMode",0},
                {"TypeMeasurement",0},
            }; // Выбранные настройки

        public Dictionary<string, int> Fv_SelectedOptionsOfMeasument
        {
            get
            {
                return V_SelectedOptionsOfMeasument;
            }
            set
            {
                // Добавить блокировку
                if ((0 <= value["StrokesGrid1"]) && (value["StrokesGrid1"]< Port.C_ParameterListsD02.F_NumGridGet().Count()) &&
                    (0 <= value["StrokesGrid2"]) && (value["StrokesGrid2"]< Port.C_ParameterListsD02.F_NumGridGet().Count()) &&
                    (0 <= value["NumShift"]) && (value["NumShift"]< Port.C_ParameterListsD02.F_NumShiftGet().Count()) &&
                    (0 <= value["NumSpeed"]) && (value["NumSpeed"]< Port.C_ParameterListsD02.F_NumSpeedGet().Count()) &&
                    (0 <= value["OperatingMode"]) && (value["OperatingMode"]< V_OperatingMode.Count()) &&
                    (0 <= value["TypeMeasurement"]) && (value["TypeMeasurement"]< V_TypeMeasurement.Count()))
                    V_SelectedOptionsOfMeasument = value;
            }
        }

        public delegate void D_CloseException();
        public event D_CloseException Event_CloseException;

        W_Port1 V_w_D01; // Окно настроек 1 устр.
        W_Port1 V_w_D02; // Окно настроек 2 устр.

        W_Measurements V_WindowMeasument;

        /// <summary>
        /// Настройки при загрузке
        /// </summary>
        public C_Logic()
        {
            V_Command_D01 = new C_CommandD01(
                V_PortD01_Options["NamePort"],
                C_PortOptions.F_StopBits(V_PortD01_Options["StopBits"]),
                C_PortOptions.F_Parity(V_PortD01_Options["Parity"]),
                C_PortOptions.F_BaudRate(V_PortD01_Options["BaudRate"]));
 
            V_Command_D02 = new C_CommandD02(
                V_PortD02_Options["NamePort"],
                C_PortOptions.F_StopBits(V_PortD02_Options["StopBits"]),
                C_PortOptions.F_Parity(V_PortD02_Options["Parity"]),
                C_PortOptions.F_BaudRate(V_PortD02_Options["BaudRate"]));

            V_WindowMeasument = new W_Measurements(this,
                Port.C_ParameterListsD02.F_NumGridGet().ConvertAll(v_options => v_options.V_NumberStrokes.ToString() + " штр./мм."),
                Port.C_ParameterListsD02.F_NumGridGet().ConvertAll(v_options => v_options.V_NumberStrokes.ToString() + " штр./мм."),
                Port.C_ParameterListsD02.F_NumShiftGet().ConvertAll(v_options => v_options.ToString() + " нм"),
                Port.C_ParameterListsD02.F_NumSpeedGet().ConvertAll(v_options => v_options.ToString() + " нм/мин"),
                V_OperatingMode,
                V_TypeMeasurement);

            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D01 != null && V_w_D01.Activate()) V_w_D01.Close(); };
            V_WindowMeasument.Closed += async (s, e1) => { if (V_w_D02 != null && V_w_D02.Activate()) V_w_D02.Close(); };

            V_WindowMeasument.Show();
        }
        /// <summary>
        /// Запуск настроек 1 уст.
        /// </summary>
        public void F_OpenWD01()
        {
            if (V_w_D01==null || !V_w_D01.Activate())
            {
                V_w_D01 = new W_Port1(V_PortD01_Options, "Настройки D01");
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
                V_w_D02 = new W_Port1(V_PortD02_Options, "Настройки D02");
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
                V_Command_D01.F_PortRun(100);
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
        /// Выключение
        /// </summary>
        public bool F_Measurement_Off_()
        {
            try
            {
                V_Command_D01.F_PortStop(100);
                V_Command_D02.F_PortStop(100);
                return true;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
                return false;
            }
        }

        /*
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
/// <param name="v_StrokesGridNum1"> Число штрихов решётки</param>
/// <param name="v_StrokesGridNum2"> Число штрихов решётки</param>
public bool F_Measurement_(byte v_GridNumbersFirst, byte v_GridNumbersSecond, byte v_Type, float v_First, float v_Second, float v_first, float v_second, byte v_NummberShift, byte v_NumberSpeed, List<byte> v_ModeScan, float v_NoMove, List<byte> v_Long,int v_StrokesGridNum1,int v_StrokesGridNum2)
{
    try
    {
        V_Command_D02.F_Com_Connection();

        V_Command_D01.F_Command_Reset();
        //V_Command_D01.F_Command_PMT(v_PMT);

        V_Command_D02.F_Com_Scan(0);
        V_Command_D02.F_Com_MonochromatorType(v_StrokesGridNum1, 0);// Правки команды
        V_Command_D02.F_Com_MonochromatorType(v_StrokesGridNum2, 1);// Правки команды
        V_Command_D02.F_Com_CorrectionType(v_Type);
        V_Command_D02.F_Com_Correction(v_First, v_Second);
        V_Command_D02.F_Com_Grid(v_GridNumbersFirst, v_GridNumbersSecond);
        V_Command_D02.F_Com_OptionsScan(v_first, v_second, v_NummberShift, v_NumberSpeed, v_ModeScan, v_NoMove);

        V_Command_D01.F_Command_Request();
        return true;
    }
    catch (ApplicationException v_error)
    {
        F_MyException(v_error);
        return false;
    }
}
 * */
    }
}
