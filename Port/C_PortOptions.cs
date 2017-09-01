using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;

namespace Device001.Port
{
    /// <summary>
    /// Класс доступных настроек порта
    /// </summary>
    public static class C_PortOptions
    {
        /// <summary>
        /// Массив доступных потров
        /// </summary>
        public static string[] F_GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
        /// <summary>
        /// Массив стоп бит
        /// </summary>
        public static StopBits[] F_GetStopBits()
        {
            return new StopBits[] { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
        }
        /// <summary>
        /// Преобразование
        /// </summary>
        public static StopBits F_StopBits(string V_StopBit)
        {
            return F_GetStopBits().First(x => x.ToString() == V_StopBit);
        }
        /// <summary>
        /// Преобразование
        /// </summary>
        public static string F_StopBits(StopBits V_StopBit)
        {
            return V_StopBit.ToString();
        }
        /// <summary>
        /// Массив паритетов
        /// </summary>
        public static Parity[] F_GetParity()
        {
            return new Parity[] { Parity.Even, Parity.Mark, Parity.None, Parity.Odd, Parity.Space };
        }
        /// <summary>
        /// Преобразование
        /// </summary>
        public static Parity F_Parity(string V_Parity)
        {
            return F_GetParity().First(x => x.ToString() == V_Parity);
        }
        /// <summary>
        /// Преобразование
        /// </summary>
        public static string F_Parity(Parity V_Parity)
        {
            return V_Parity.ToString();
        }
        /// <summary>
        /// Массив скоростей
        /// </summary>
        public static int[] F_GetBaudRate()
        {
            return new int[] { 2400, 4800, 9600, 19200, 38400, 57600, 115200, 128000, 256000, 410800, 921600 };
        }
        /// <summary>
        /// Преобразование
        /// </summary>
        public static int F_BaudRate(string V_BaudRate)
        {
            return F_GetBaudRate().First(x => x.ToString() == V_BaudRate);
        }
        /// <summary>
        /// Преобразование
        /// </summary>
        public static string F_BaudRate(int V_BaudRate)
        {
            return V_BaudRate.ToString();
        }
    }
}
