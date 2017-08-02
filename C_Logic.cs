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
        private C_CommandD01 V_CommandD01 = new C_CommandD01();

        private void F_MyException(Exception v_error)
        {
            MessageBox.Show(v_error.Message + " " + v_error.StackTrace, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //          
            
        }
        public void F_Measurement_On_()
        {
            try
            {
                //
                V_CommandD01.F_PortRun(100);
                V_CommandD01.Event_End_D01 += F_Measurement_End_D01;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
            }
        }
        public void F_Measurement_()
        {
            try
            {
                //
                V_CommandD01.F_MeasurementRun_D01(0x00);
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
        public void F_Measurement_Off_()
        {
            try
            {
                //
                V_CommandD01.F_PortStop(100);
                V_CommandD01.Event_End_D01 -= F_Measurement_End_D01;
            }
            catch (ApplicationException v_error)
            {
                F_MyException(v_error);
            }
        }
    }
}
