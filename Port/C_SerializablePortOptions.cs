using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;

namespace Device001.Port
{
    [Serializable]
    public class C_SerializablePortOptions
    {
        private string V_PortName = "";
        private int[] V_Options = new int[3];

        public C_SerializablePortOptions(string v_PortName, StopBits v_StopBits, int v_BaudRate, Parity v_Parity)
        {
            Fv_PortName = v_PortName;
            Fv_StopBits = v_StopBits;
            Fv_BaudRate = v_BaudRate;
            Fv_Parity = v_Parity;
        }
        /// <summary>
        /// Установка имени порта
        /// </summary>
        public string Fv_PortName
        {
            get { return V_PortName; }
            set { V_PortName = value; }
        }
        /// <summary>
        /// Число стоповых битов
        /// </summary>
        public StopBits Fv_StopBits
        {
            get { return C_PortOptions.F_GetStopBits()[V_Options[0]]; }
            set { V_Options[0] = Array.FindIndex(C_PortOptions.F_GetStopBits(), x => x == value); }
        }
        /// <summary>
        /// Скорость передачи
        /// </summary>
        public int Fv_BaudRate
        {
            get { return C_PortOptions.F_GetBaudRate()[V_Options[1]]; }
            set { V_Options[1] = Array.FindIndex(C_PortOptions.F_GetBaudRate(), x => x == value); }
        }
        /// <summary>
        /// Чётность
        /// </summary>
        public Parity Fv_Parity
        {
            get { return C_PortOptions.F_GetParity()[V_Options[2]]; }
            set { V_Options[2] = Array.FindIndex(C_PortOptions.F_GetParity(), x => x == value); }
        }
    }
}
