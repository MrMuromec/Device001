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
        private C_CommandD01 V_Command_D01;
        private W_Port1 V_w_D01 = null;
        private C_CommandD02 V_Command_D02;
        private W_Port1 V_w_D02 = null;

        public C_Logic()
        {
            V_Command_D01 = new C_CommandD01("",C_PortOptions.F_GetStopBits()[1],C_PortOptions.F_GetParity()[0],C_PortOptions.F_GetBaudRate()[0]);
            V_Command_D02 = new C_CommandD02("", C_PortOptions.F_GetStopBits()[1], C_PortOptions.F_GetParity()[0], C_PortOptions.F_GetBaudRate()[0]);
        }
        public void F_WindowPort_D01()
        {
            V_w_D01 = new W_Port1(V_Command_D01.F_Value_PortName, V_Command_D01.F_Value_StopBits, V_Command_D01.F_Value_Parity, V_Command_D01.F_Value_BaudRate);
            V_w_D01.Show();
        }
        public void F_WindowPort_D02()
        {
            V_w_D02 = new W_Port1(V_Command_D02.F_Value_PortName, V_Command_D02.F_Value_StopBits, V_Command_D02.F_Value_Parity, V_Command_D02.F_Value_BaudRate);
            V_w_D02.Show();
        }
        /// <summary>
        /// Обработка ошибок
        /// </summary>
        /// <param name="v_error">Ошибка</param>
        private void F_MyException(Exception v_error)
        {
            MessageBox.Show(v_error.Message + " " + v_error.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    
        }
        /// <summary>
        /// Подключение
        /// </summary>
        public void F_Measurement_On_()
        {
            try
            {
                //
                V_Command_D01.F_PortRun(100);
                V_Command_D01.Event_End_D01 += F_Measurement_End_D01;

                V_Command_D02.F_PortRun(100);
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
            }
        }
        /// <summary>
        /// Запуск измерения с параметрами
        /// </summary>
        public void F_Measurement_()
        {
            try
            {
                //
                V_Command_D01.F_Measurement_Run_D01(0x00);
                V_Command_D02.F_Measurement_Run_D02();
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
            }
        }
        public void F_Measurement_End_D01()
        {
            //
        }
        /// <summary>
        /// Выключение
        /// </summary>
        public void F_Measurement_Off_()
        {
            try
            {
                //
                V_Command_D01.F_PortStop(100);
                V_Command_D01.Event_End_D01 -= F_Measurement_End_D01;

                V_Command_D02.F_PortStop(100);
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
            }
        }
    }
}
