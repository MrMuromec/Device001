using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;

namespace Device001
{
    /// <summary>
    /// Класс для работы с портом
    /// </summary>
    public class C_MyPort
    {
        /// <summary>
        /// Порт с настройками
        /// </summary>
        public volatile C_MyPortOptions V_Port = new C_MyPortOptions();
        /// <summary>
        /// Данные с порта
        /// </summary>
        public ConcurrentStack<byte> V_StakIn = new ConcurrentStack<byte>();

        /// <summary>
        /// Подлючение порта
        /// </summary>
        /// <param name="V_TimeToOpen">Срок ожидания открытия порта</param>
        /// <param name="V_Error">Возращаемое сообщение об ошибке</param>
        /// <returns>true - успешное завершение, false -  ошибка</returns>
        public bool F_PortRun(int V_TimeToOpen, out string V_Error)
        { 
            try
            {
                if (!V_Port.IsOpen)
                {
                    if (C_MyPortOptions.F_GetPortNames().Contains(V_Port.PortName))
                    {
                        V_Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        V_Port.Open();
                        if (!V_Port.IsOpen)
                            Thread.Sleep(V_TimeToOpen);
                        if (V_Port.IsOpen)
                        {
                            V_Error = "";
                            return true;
                        }
                        else
                        {
                            V_Error = "Не удалось подключиться. Превышено время подключения порта.";
                            return false;
                        }
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
        /// Отключение порта
        /// </summary>
        /// <param name="V_TimeToStop">Срок ожидания закрытия порта</param>
        /// <param name="V_Error">Возращаемое сообщение об ошибке</param>
        /// <returns>true - успешное завершение, false -  ошибка</returns>
        public bool F_PortStop(int V_TimeToStop, out string V_Error)
        { 
            try
            {
                if (V_Port.IsOpen)
                {
                    V_Port.Close();
                    V_Port.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                    if (V_Port.IsOpen)
                        Thread.Sleep(V_TimeToStop);
                    if (!V_Port.IsOpen)
                    {
                        V_Error = "";
                        return true;
                    }
                    else
                    {
                        V_Error = "Не удалось отключиться. Превышено время отключения порта.";
                        return false;
                    }
                }
                else
                {
                    V_Error = "Порт уже был отключен"; // Порт уже отвалился
                    return false;
                }
            }
            catch (Exception Error)
            {
                V_Error = Error.Message;
                return false;
            }
        }
        /// <summary>
        /// Чтение с порта по событию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(0);
            byte[] V_IN = new byte[V_Port.BytesToRead];
            V_Port.Read(V_IN, 0, V_IN.Length);
            foreach (byte v_in in V_IN)
                V_StakIn.Push(v_in);
        }
    }
}
