using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;
using System.Threading;

namespace Device001
{
    /// <summary>
    /// Класс для работы с ком портом
    /// </summary>
    public class C_MyPort
    {
        private volatile SerialPort V_Port = new SerialPort(); // Порт (COM порт, настройки далее в проге)
        /// <summary>
        /// Подлючение порта
        /// </summary>
        /// <param name="V_NamePort">Системное имя порта</param>
        /// <param name="V_StopBits">Количество стоп битов</param>
        /// <param name="V_Parity">Паритет</param>
        /// <param name="V_BaudRate">Скорость</param>
        /// <param name="V_ReadTimeout">Срок ожидания операции чтения</param>
        /// <param name="V_WriteTimeout">Срок ожидания операции записи</param>
        /// <param name="V_TimeToOpen">Срок ожидания открытия порта</param>
        /// <param name="V_Error">Возращаемое сообщение об ошибке</param>
        /// <returns>true - успешное завершение, false -  ошибка</returns>
        public bool F_PortRun(string V_NamePort, StopBits V_StopBits, Parity V_Parity, int V_BaudRate, int V_ReadTimeout, int V_WriteTimeout, int V_TimeToOpen, out string V_Error)
        {
            V_Error = "";
            try
            {
                if (!V_Port.IsOpen)
                {
                    if (F_GetPortNames().Contains(V_NamePort))
                    {
                        V_Port.PortName = V_NamePort;
                        V_Port.StopBits = V_StopBits;
                        V_Port.Parity = V_Parity;
                        V_Port.BaudRate = V_BaudRate;
                        V_Port.ReadTimeout = V_ReadTimeout;
                        V_Port.WriteTimeout = V_WriteTimeout;
                        //_Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        V_Port.Open();
                        if (!V_Port.IsOpen)
                        {
                            Thread.Sleep(V_TimeToOpen);
                            V_Error = "Не удалось подключиться. Превышено время подключения порта.";
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        V_Error = "Прибор не найден. Обновите список портов.";
                        return false;
                    }
                }
                else
                {
                    V_Error = "Порт уже был подключен, отключите его";
                    return false;
                }
            }
            catch (Exception Error)
            {
                V_Error = "Проверте настройки порта. " + Error.Message;
                return false;
            }
        }
        /// <summary>
        /// Массив доступных потров
        /// </summary>
        public string[] F_GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
        /// <summary>
        /// Массив стоп битоа
        /// </summary>
        public StopBits[] F_GetStopBits()
        {
            return new StopBits[] { StopBits.None, StopBits.One, StopBits.OnePointFive, StopBits.Two };
        }
        /// <summary>
        /// Массив паритетов
        /// </summary>
        public Parity[] F_GetParity()
        {
            return new Parity[] { Parity.Even, Parity.Mark, Parity.None, Parity.Odd, Parity.Space};
        }
        /// <summary>
        /// Массив скоростей
        /// </summary>
        public int[] F_GetBaudRate()
        {
            return new int[] { 2400, 4800, 9600, 19200, 38400, 57600, 115200, 128000, 256000, 410800, 921600};
        }
    }
}
