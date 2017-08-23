using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO.Ports;
using System.Collections.Concurrent;
using System.Threading;

namespace Device001.Port
{
    /// <summary>
    /// Класс команд для 1 устройства
    /// </summary>
    public class C_CommandD01 : C_MyPort
    {
        private C_PackageD01 V_PackageD01 = new C_PackageD01();     
        private static Mutex V_WaitOfContinuation = new Mutex(false);// Примитив синхронизации для блокировки
        // Для обозначения конца измерения
        public delegate void D_MeasurementEnd();
        public event D_MeasurementEnd Event_End_D01;

        public C_CommandD01()
        {

        }
        public C_CommandD01(string V_NamePort, StopBits V_StopBits, Parity V_Parity, int V_BaudRate)
            : base(V_NamePort, V_StopBits, V_Parity, V_BaudRate)
        {

        }
        /// <summary>
        /// Измерения от 1 блока три 32-битных числа со знаком в доп. коде.
        /// </summary>
        /// <param name="v_N">
        /// 0 - величина сигнала ФЭУ
        /// 1 - величина сигнала опорного канала
        /// 2 - величина сигнала зонда</param>
        public Int32 F_Measurement_D01(byte v_N)
        {
            return V_PackageD01.F_GetInt32(v_N);
        }

        /// <summary>
        /// Разбор ответа
        /// </summary>
        /// <param name="v_bytes"> Данные команды </param>
        /// <param name="v_N"> Кол-во ожидаемых байт </param>
        /// <param name="v_TimeToSleepOfRequest"> Время ожидания повторного запроса </param>
        /// <param name="v_MaximumRequests"> Максимальное количесво повторных запросов </param>
        private void F_ComIn_Decoder(out byte[] v_bytes, int v_N, Int32 v_TimeToSleepOfRequest = 100, int v_MaximumRequests = 5)
        {
            System.ApplicationException v_Error;
            byte v_byte;
            if (--v_N > 0)
                v_bytes = new byte[v_N];
            else
                v_bytes = null;
            for (int i = 0; i < v_N; ++i)
            {
                if (F_Request(out v_byte, F_QueueInTryDequeue, v_MaximumRequests, v_TimeToSleepOfRequest))
                    v_bytes[i] = v_byte;
                else
                {
                    v_Error = new ApplicationException("Неожиданный конец ответа (Ошибка от блока 1)");
                    throw v_Error;
                }
            }
        }
        /// <summary>
        /// Проверка ответа
        /// </summary>
        /// <param name="v_DuteByte"> Байт подтверждения </param>
        /// <param name="v_TimeToSleepOfRequest"> Время ожидания повторного запроса </param>
        /// <param name="v_MaximumRequests"> Максимальное количесво повторных запросов </param>
        /// <returns> Результат: true - успех </returns>
        private void F_ComIn(byte v_DuteByte ,Int32 v_TimeToSleepOfRequest = 100, int v_MaximumRequests = 5)
        {
            System.ApplicationException v_Error;
            byte v_byte;
            // Начало разбора
            if (F_Request(out v_byte, F_QueueInTryDequeue, v_MaximumRequests, v_TimeToSleepOfRequest))
                switch (v_byte)
                {
                    case 0xA8:
                        v_Error = new ApplicationException("Ошибка команды (Ошибка от блока 1)");
                        throw v_Error;
                    case 0xFB:
                        v_Error = new ApplicationException("Превышен лимит ожидания команды (Ошибка от блока 1)");
                        throw v_Error;
                    default:
                        if (v_DuteByte != v_byte)
                        {
                            v_Error = new ApplicationException("Нет потверждения команды (Ошибка от блока 1)");
                            throw v_Error;
                        }
                        break;
                }
            else
            {
                v_Error = new ApplicationException("Нет ответа для проверки (" + v_MaximumRequests * v_TimeToSleepOfRequest + " мс) (Ошибка от блока 1)");
                throw v_Error;
            }
        }

        /// <summary>
        /// Сброс блокировки по началу приёма
        /// </summary>
        private void F_InAdd()
        {
            V_WaitOfContinuation.ReleaseMutex();
        }
        /// <summary>
        /// Запуск измерения
        /// </summary>
        /// <param name="v_PMT"> ФЭУ </param>
        public void F_Measurement_Run_D01(byte v_PMT)
        {
            Event_InAdd += F_InAdd;

            F_Command_Reset();
            F_Command_PMT(v_PMT);
            F_Command_Request();

            if (Event_End_D01 != null)
                Event_End_D01();

            Event_InAdd -= F_InAdd;
        }
        /// <summary>
        /// Команда - Сброс
        /// </summary>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        private void F_Command_Reset(Int32 v_TimeToSleep = 500)
        {
            F_PortWrite(new byte[] { 0x12, 0x00 });
            V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            F_ComIn((byte)0x00);
        }
        /// <summary>
        /// Команда - ФЭУ
        /// </summary>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        private void F_Command_PMT(byte v_PMT ,Int32 v_TimeToSleep = 500)
        {
            F_PortWrite(new byte[] { 0x13, 0x02, v_PMT });
            V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            F_ComIn((byte)0x02);
        }
        /// <summary>
        /// Команда - Запрос
        /// </summary>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        private void F_Command_Request(Int32 v_TimeToSleep = 500)
        {         
            F_PortWrite(new byte[] { 0x12, 0x01 });
            V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            byte[] v_bytes;
            F_ComIn((byte)0x01);
            F_ComIn_Decoder(out v_bytes, 6);
            V_PackageD01.F_Parse(v_bytes);
        }
    }
}
