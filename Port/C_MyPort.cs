using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Globalization;

namespace Device001.Port
{

    /// <summary>
    /// Класс для работы с портом
    /// </summary>
    public class C_MyPort
    {     
        public bool V_OnOff { get; set; }// Разрешение на работу

        private string V_FileName = ""; // Названия файла для сохранения настроек
        private BinaryFormatter V_formatter = new BinaryFormatter();  // Формат 
        private C_SerializablePortOptions V_SerializableOptions; // Для сохранения настроек

        private ConcurrentQueue<byte> V_QueueIn = new ConcurrentQueue<byte>(); // Данные с порта
        private volatile SerialPort V_Port = new SerialPort(); // Порт

        public delegate bool D_Request(out byte v_byte); // Формат запроса

        public delegate void D_InAdd();
        /// <summary>
        /// Событие получения данных
        /// </summary>
        public event D_InAdd E_InAdd;

        /// <summary>
        /// Конструктор
        /// </summary>
        public C_MyPort(string v_NamePort, StopBits v_StopBits, Parity v_Parity, int v_BaudRate, string v_FileName, bool v_OnOff)
        {
            V_Port.DataBits = 8; // сколько битов
            V_Port.ReceivedBytesThreshold = 1; // сколько байтов
            Fv_PortName = v_NamePort;
            Fv_StopBits = v_StopBits;
            Fv_Parity = v_Parity;
            Fv_BaudRate = v_BaudRate;
            V_FileName = v_FileName;
            V_Port.ReadTimeout = 200;
            V_Port.WriteTimeout = 200;

            V_OnOff = v_OnOff;

            F_WriteTextAsync(null, Fv_PortName + " <> ");
        }

        #region F_PortGetSetOptions
        /// <summary>
        /// Установака и сохранение новых значений
        /// </summary>
        public void F_SetAndSaveOptions(C_MyPort v_PortOptions)
        {
            V_SerializableOptions.Fv_BaudRate = Fv_BaudRate = v_PortOptions.Fv_BaudRate;
            V_SerializableOptions.Fv_Parity = Fv_Parity = v_PortOptions.Fv_Parity;
            V_SerializableOptions.Fv_PortName = Fv_PortName = v_PortOptions.Fv_PortName;
            V_SerializableOptions.Fv_StopBits = Fv_StopBits = v_PortOptions.Fv_StopBits;

            V_SerializableOptions.Fv_OnOff = V_OnOff = v_PortOptions.V_OnOff;

            using (FileStream fs = new FileStream(V_FileName, FileMode.OpenOrCreate)) // Подумать насчёт исключений
            {
                V_formatter.Serialize(fs, V_SerializableOptions);
            }
        }
        /// <summary>
        /// Загрузка значений
        /// </summary>
        public void F_LoadOptions()
        {
            try
            {
                using (FileStream fs = new FileStream(V_FileName, FileMode.Open))  // Подумать насчт исключений
                {
                    V_SerializableOptions = (C_SerializablePortOptions)V_formatter.Deserialize(fs);
                    Fv_BaudRate = V_SerializableOptions.Fv_BaudRate;
                    Fv_Parity = V_SerializableOptions.Fv_Parity;
                    Fv_PortName = V_SerializableOptions.Fv_PortName;
                    Fv_StopBits = V_SerializableOptions.Fv_StopBits;

                    V_OnOff = V_SerializableOptions.Fv_OnOff;
                }
            }
            catch (FileNotFoundException)
            {
                V_SerializableOptions = new C_SerializablePortOptions(Fv_PortName, Fv_StopBits, Fv_BaudRate, Fv_Parity, V_OnOff);
            }
        }
        /// <summary>
        /// Установка имени порта
        /// </summary>
        public string Fv_PortName
        {
            get { return V_Port.PortName; }
            set 
            { 
                if ((!V_Port.IsOpen) && (C_PortOptions.F_GetPortNames().Contains(value))) 
                    V_Port.PortName = value; 
            }
        }
        /// <summary>
        /// Число стоповых битов
        /// </summary>
        public StopBits Fv_StopBits
        {
            get { return V_Port.StopBits; }
            set 
            { 
                if ((!V_Port.IsOpen) && (C_PortOptions.F_GetStopBits().Contains(value)))
                    V_Port.StopBits = value; 
            }
        }
        /// <summary>
        /// Скорость передачи
        /// </summary>
        public int Fv_BaudRate
        {
            get { return V_Port.BaudRate; }
            set 
            { 
                if ((!V_Port.IsOpen) && (C_PortOptions.F_GetBaudRate().Contains(value)))
                    V_Port.BaudRate = value; 
            }
        }
        /// <summary>
        /// Чётность
        /// </summary>
        public Parity Fv_Parity
        {
            get { return V_Port.Parity; }
            set 
            { 
                if ((!V_Port.IsOpen) &&(C_PortOptions.F_GetParity().Contains(value))) 
                    V_Port.Parity = value; 
            }
        }
        #endregion

        #region F_PortRunOrStop
        /// <summary>
        /// Подлючение порта
        /// </summary>
        /// <param name="V_TimeToOpen">Срок ожидания открытия порта</param>
        public void F_PortRun(int V_TimeToOpen)
        {
            System.ApplicationException v_Error;
            if (!V_Port.IsOpen)
            {
                if (C_PortOptions.F_GetPortNames().Contains(V_Port.PortName))
                {
                    V_Port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    // Вероятны исключения!
                    V_Port.Open();                    
                    if (!V_Port.IsOpen)
                        Thread.Sleep(V_TimeToOpen);
                    if (!V_Port.IsOpen)
                    {
                        v_Error = new ApplicationException("Не удалось подключиться. Превышено время подключения порта.");
                        throw v_Error;
                    }
                }
                else
                {
                    v_Error = new ApplicationException("Прибор не найден. Обновите список портов. (" +  V_Port.PortName +  ")");
                    throw v_Error;
                }
            }
            else
            {
                v_Error = new ApplicationException("Порт уже был подключен, отключите его");
                throw v_Error;
            }
        }

        /// <summary>
        /// Отключение порта
        /// </summary>
        /// <param name="V_TimeToStop">Срок ожидания закрытия порта</param>
        public void F_PortStop(int V_TimeToStop)
        {
            System.ApplicationException v_Error;
            if (V_Port.IsOpen)
            {
                V_Port.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                // Вероятны исключения!
                V_Port.Close();
                if (V_Port.IsOpen)
                    Thread.Sleep(V_TimeToStop);
                if (V_Port.IsOpen)
                {
                    v_Error = new ApplicationException("Не удалось отключиться. Превышено время отключения порта.");
                    throw v_Error;
                }
            }
            else
            {
                v_Error = new ApplicationException("Порт уже был отключен"); // Порт уже отвалился
                throw v_Error;
            }
        }
        #endregion

        #region F_PortReadOrWrite
        /// <summary>
        /// Пишет в порт
        /// </summary>
        /// <param name="v_Out">Для записи</param>
        protected void F_PortWrite(byte[] v_Out)
        {
            // Вероятны исключения!
            V_Port.Write(v_Out, 0, v_Out.Count());
            F_WriteTextAsync(v_Out, Fv_PortName + " на ");
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
            // Вероятны исключения!
            V_Port.Read(V_IN, 0, V_IN.Length);
            foreach (byte v_in in V_IN)
                V_QueueIn.Enqueue(v_in);
            F_WriteTextAsync(V_IN,Fv_PortName + " от ");
            if (E_InAdd != null)
                E_InAdd();
        }
        /// <summary>
        /// Считывает байт из очереди с удалением
        /// </summary>
        /// <param name="V_byte">считанный байт</param>
        /// <returns>true - если считался байт, false - если считать не удалось</returns>
        protected bool F_QueueInTryDequeue(out byte V_byte)
        {
            return V_QueueIn.TryDequeue(out V_byte);
        }
        /// <summary>
        /// Считывает байт из очереди без удаления
        /// </summary>
        /// <param name="V_byte">считанный байт</param>
        /// <returns>true - если считался байт, false - если считать не удалось</returns>
        protected bool F_QueueInTryPeek(out byte V_byte)
        {
            return V_QueueIn.TryPeek(out V_byte);
        }
        /// <summary>
        /// Чтение из очереди данных пришедших с порта
        /// </summary>
        /// <param name="v_byte"> Считанный байт </param>
        /// <param name="v_Request"> Способ чтения (с/без удаления) </param>
        /// <param name="v_MaximumRequests"> Максимальное количесво повторных запросов </param>
        /// <param name="v_TimeToSleepOfRequest"> Время ожидания повторного запроса </param>
        /// <returns> true - если байт считан</returns>
        public bool F_Request(out byte v_byte, D_Request v_Request, int v_MaximumRequests = 5, Int32 v_TimeToSleepOfRequest = 100)
        {
            bool v_Result;
            for (; !(v_Result = v_Request.Invoke(out v_byte)) && v_MaximumRequests > 0; Thread.Sleep(v_TimeToSleepOfRequest), --v_MaximumRequests)
                ;
            return v_Result;
        }
        #endregion

        public static async void F_WriteTextAsync(byte[] v_bytes, string v_str)
        {
            string v_buteStr = "";
            if (v_bytes != null)
                foreach (byte v_byte in v_bytes)
                    v_buteStr += " " + v_byte.ToString(CultureInfo.InvariantCulture);
            try
            {
                using (StreamWriter outputFile = new StreamWriter("InOut.txt", true))
                {
                    await outputFile.WriteAsync(v_str + v_buteStr + Environment.NewLine);
                    outputFile.Dispose();
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                Thread.Sleep(10);
            }
        }
    }
}
