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
        private W_Port1 V_w_D01;
        private bool V_flagD01 = false;
        private C_CommandD02 V_Command_D02;
        private W_Port1 V_w_D02;
        private bool V_flagD02 = false;

        public delegate void D_CloseException();
        public event D_CloseException Event_CloseException;

        public C_Logic()
        {
            V_Command_D01 = new C_CommandD01("",C_PortOptions.F_GetStopBits()[1],C_PortOptions.F_GetParity()[2],C_PortOptions.F_GetBaudRate()[3]);
            V_Command_D02 = new C_CommandD02("", C_PortOptions.F_GetStopBits()[1], C_PortOptions.F_GetParity()[2], C_PortOptions.F_GetBaudRate()[3]);
        }
        public void F_WindowPort_D01()
        {
            if (!V_flagD01)
            {
                V_w_D01 = new W_Port1(V_Command_D01.F_Value_PortName, V_Command_D01.F_Value_StopBits, V_Command_D01.F_Value_Parity, V_Command_D01.F_Value_BaudRate, "D01");
                V_w_D01.Closed += V_w_D01_Closed;
                V_flagD01 = true;
                V_w_D01.Show();
            }
            else
                V_w_D01.Activate();
        }

        void V_w_D01_Closed(object sender, EventArgs e)
        {
            V_flagD01 = false;
        }
        public void F_WindowPort_D02()
        {
            if (!V_flagD02)
            {
                V_w_D02 = new W_Port1(V_Command_D02.F_Value_PortName, V_Command_D02.F_Value_StopBits, V_Command_D02.F_Value_Parity, V_Command_D02.F_Value_BaudRate, "D02");
                V_w_D02.Closed += V_w_D02_Closed;
                V_flagD02 = true;
                V_w_D02.Show();
            }
            else
                V_w_D02.Activate();

        }
        void V_w_D02_Closed(object sender, EventArgs e)
        {
            V_flagD02 = false;
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
        public bool F_Measurement_()
        {
            try
            {
                //
                V_Command_D01.F_Measurement_Run_D01(0x00);
                V_Command_D02.F_Measurement_Run_D02();
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
