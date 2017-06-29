﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;

namespace Device001
{
    /// <summary>
    /// Порт + настройки
    /// </summary>
    public class C_MyPortOptions : SerialPort
    {
        public C_MyPortOptions()
        {
            this.DataBits = 8; // сколько битов
            this.ReceivedBytesThreshold = 1; // сколько байтов
            //V_Port.PortName = V_NamePort;
            //V_Port.StopBits = V_StopBits;
            //V_Port.Parity = V_Parity;
            //V_Port.BaudRate = V_BaudRate;
            this.ReadTimeout = 50;
            this.WriteTimeout = 50;
        }
        /// <summary>
        /// Массив доступных потров
        /// </summary>
        public static string[] F_GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
        /// <summary>
        /// Массив стоп битоа
        /// </summary>
        public static StopBits[] F_GetStopBits()
        {
            return new StopBits[] { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
        }
        /// <summary>
        /// Массив паритетов
        /// </summary>
        public static Parity[] F_GetParity()
        {
            return new Parity[] { Parity.Even, Parity.Mark, Parity.None, Parity.Odd, Parity.Space };
        }
        /// <summary>
        /// Массив скоростей
        /// </summary>
        public static int[] F_GetBaudRate()
        {
            return new int[] { 2400, 4800, 9600, 19200, 38400, 57600, 115200, 128000, 256000, 410800, 921600 };
        }
    }
}