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
    /// Класс команд для 2 устройства
    /// </summary>
    public class C_CommandD02 : C_MyPort
    {
        //private static Mutex V_WaitOfContinuation = new Mutex(false);// Примитив синхронизации для блокировки

        public C_CommandD02(string V_NamePort, StopBits V_StopBits, Parity V_Parity, int V_BaudRate, string v_FileName)
            : base(V_NamePort, V_StopBits, V_Parity, V_BaudRate, v_FileName)
        {
            Event_InAdd += F_InAdd;
        }
        /// <summary>
        /// Подтверждение от устройства
        /// </summary>
        ///<param name="v_DuteByte"> Байт подтверждения </param>
        /// <param name="v_TimeToSleepOfRequest"> Время ожидания повторного запроса </param>
        /// <param name="v_MaximumRequests"> Максимальное количесво повторных запросов </param>
        private bool F_ComIn_Verification(byte v_DuteByte = (byte)'#', Int32 v_TimeToSleepOfRequest = 100, int v_MaximumRequests = 5)
        {
            System.ApplicationException v_Error;
            byte v_byte;
            // Начало разбора
            F_Request(out v_byte, F_QueueInTryDequeue, v_MaximumRequests, v_TimeToSleepOfRequest);
            switch (v_byte)
            {
                // Спросить про список ошибок
                default:
                    if (v_DuteByte != v_byte)
                    {
                        return false;
                        /*
                        v_Error = new ApplicationException("Нет потверждения приёма байта " + v_DuteByte.ToString() + " != " + v_byte.ToString() + " (Ошибка от блока 2)");
                        throw v_Error;
                         * */
                    }
                    else
                        return true;
                    break;
            }
        }
        /// <summary>
        /// Байтная запись с подтверждением
        /// </summary>
        /// <param name="v_byteOut"> Байт для записи, по умолчанию байт подтверждения </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        private void F_ComWrite_Verification(byte v_byteOut = (byte)'#', Int32 v_TimeToSleep = 500)
        {
            NotAnswer:
            F_PortWrite(new byte[] { v_byteOut });
            //V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            Thread.Sleep(v_TimeToSleep);
            if (!F_ComIn_Verification())
                goto NotAnswer;
        }
        /// <summary>
        /// Сброс блокировки по началу приёма
        /// </summary>
        private void F_InAdd()
        {
            //V_WaitOfContinuation.ReleaseMutex();
            //Thread.Sleep(v_TimeToSleep);
        }
        /// <summary>
        /// # - Подключение
        /// </summary>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_Connection(Int32 v_TimeToSleep = 500)
        {
            F_ComWrite_Verification(v_TimeToSleep:v_TimeToSleep);
        }

        /// <summary>
        /// 200 - Выйти на длину волны
        /// </summary>
        /// <param name="v_Monochromator"> Передает 1 байт: 0 - первый монохроматор, 1 – второй монохроматор, 2 – оба вместе </param>
        /// <param name="v_WaveLength"> Длина волны </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_WaveLength(byte v_Monochromator = 0, float v_WaveLength = 0, Int32 v_TimeToSleep = 500)
        {
            byte[][] v_bytes = new byte[2][];
            v_bytes[0] = new byte[] { (byte)0x80, v_Monochromator };
            v_bytes[1] = C_PackageD02.F_СonversionFloat32(v_WaveLength);

            NotAnswer1:

            foreach (byte[] v_bytesOut in v_bytes)
                foreach (byte v_byteOut in v_bytesOut)
                    F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
            //V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            Thread.Sleep(5 * v_TimeToSleep);
            
            if (!F_ComIn_Verification((byte)0x82)) // по выполнении блок передает команду 202 - готовность
                goto NotAnswer1;
        }
        /// <summary>
        /// 201 - Сканировать
        /// </summary>
        /// <param name="v_Type"> 1 – собственно сканирование, 2 - кинетика. Предположительно нумерация от 0??</param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_Scan(byte v_Type = 0, Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x81, v_Type };
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 204 - Тип коррекции
        /// </summary>
        /// <param name="v_Type">  0 - счетчик; 1 - репера </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_CorrectionType(byte v_Type = 0, Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x84, v_Type};
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 205 - Тип коррекции
        /// </summary>
        /// <param name="v_First"> Положение II монохроматора (нм.)</param>
        /// <param name="v_Second"> Положение I монохроматора введенное оператором в случае коррекции по счетчику или произвольные числа при коррекции по реперам</param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_Correction(float v_First = 0, float v_Second = 0, Int32 v_TimeToSleep = 500)
        {
            byte[][] v_bytes = new byte[3][];
            v_bytes[0] = new byte[] { (byte)0x85 };
            v_bytes[1] = C_PackageD02.F_СonversionFloat32(v_First);
            v_bytes[2] = C_PackageD02.F_СonversionFloat32(v_Second);
            NotAnswer2:
            foreach (byte[] v_bytesOut in v_bytes)
                foreach (byte v_byteOut in v_bytesOut)
                    F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
            //V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            Thread.Sleep(v_TimeToSleep);
            if (!F_ComIn_Verification((byte)0x82)) // по выполнении блок передает команду 202 - готовность
                goto NotAnswer2;
        }
        /// <summary>
        /// 206 - Номер решётки
        /// </summary>
        /// <param name="v_GridNumbersFirst"> I – монохр. номер решетки </param>
        /// <param name="v_GridNumbersSecond"> II – монохр. номер решетки </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_Grid(byte v_GridNumbersFirst = 0, byte v_GridNumbersSecond = 0, Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x86, v_GridNumbersFirst, v_GridNumbersSecond };
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 207 - Приёмник
        /// </summary>
        /// <param name="NumberOfReceiver"> Номер приемника </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_Receiver(byte NumberOfReceiver = 0, Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x87, NumberOfReceiver };
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 211 - Стоп, остановить эксперимент
        /// </summary>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_Stop(Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x89};
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 212 - ручное управление монохроматором с клавиатуры
        /// </summary>
        /// <param name="v_ControlCommand"> 1 - стоп, 2 - назад, 3 - вперед, 4 – закончить ручной режим, 5 - начать ручной режим, 6 - дать положение монохр.</param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_ManualMonochromatorControl(byte v_ControlCommand = 5, Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x8A, v_ControlCommand };
            NotAnswer3:
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
            //V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            Thread.Sleep(v_TimeToSleep);
            if (!F_ComIn_Verification((byte)0x82)) // по выполнении блок передает команду 202 - готовность
                goto NotAnswer3;
        }
        /// <summary>
        /// 214 - Тип монохр. и т.д.
        /// </summary>
        /// <param name="v_StrokesGridNum"> Число штрихов решётки</param>
        /// <param name="v_NumberMonochromator"> Номер монохраматора</param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_MonochromatorType(int v_StrokesGridNum, int v_NumberMonochromator,Int32 v_TimeToSleep = 500)
        {
            byte v_byteInfo = 0;
            
            if (v_NumberMonochromator == 1)
                v_byteInfo = 1;
            v_byteInfo <<= 5;
            if (true)//ДШИ-200
                ++v_byteInfo;
            v_byteInfo <<= 1;
            if (true) // мдр-41
                ++v_byteInfo;
            v_byteInfo <<= 1;
            if (false) // 3000
                ++v_byteInfo;
            
            byte[] v_bytes = new byte[] { (byte)0x8C, v_byteInfo }; // (какие значения в битах должны быть переданы) Уточнить!
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 215 - Смена реш.
        /// </summary>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_ReplacementGrid(Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x8D }; // (выбор решёток) Уточнить!
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 217 - скорость монохроматора в ручном режиме
        /// </summary>
        /// <param name="v_ControlCommand">  1 - быстрее, 0 - медленней </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_ManualMonochromatorSpeedControl(byte v_ControlCommand = 1, Int32 v_TimeToSleep = 500)
        {
            byte[] v_bytes = new byte[] { (byte)0x8F, v_ControlCommand };
            foreach (byte v_byteOut in v_bytes)
                F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
        /// <summary>
        /// 220 - выйти на длину волны без установки сменных элементов. 
        /// </summary>
        /// <param name="v_Wavelength"> Длина волны </param>
        /// <param name="v_TimeToSleep"> Время ожидания ответа на команду</param>
        public void F_Com_ReachWavelength(float v_Wavelength, Int32 v_TimeToSleep = 500)
        {
            byte[][] v_bytes = new byte[2][];
            v_bytes[0] = new byte[] { (byte)0x90 };
            v_bytes[1] = C_PackageD02.F_СonversionFloat32(v_Wavelength);
            NotAnswer4:
            foreach (byte[] v_bytesOut in v_bytes)
                foreach (byte v_byteOut in v_bytesOut)
                    F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
            //V_WaitOfContinuation.WaitOne(v_TimeToSleep);
            Thread.Sleep(v_TimeToSleep);
            if (!F_ComIn_Verification((byte)0x82)) // по выполнении блок передает команду 202 - готовность
                goto NotAnswer4;
        }
        /// <summary>
        /// 227 - параметры сканирования
        /// </summary>
        /// <param name="v_first">Начало диапазона</param>
        /// <param name="v_second">Конец диапазона</param>
        /// <param name="v_NummberShift">Номер шага</param>
        /// <param name="v_NumberSpeed">Номер скорости</param>
        /// <param name="v_ModeScan">Режим сканирования: 0 – I монохр, 1 – II монохр, 2 - параллельно</param>
        /// <param name="v_NoMove">Положение несканирующего монохр</param>
        /// <param name="v_TimeToSleep">Время ожидания ответа на команду</param>
        public void F_Com_OptionsScan(float v_first, float v_second, byte v_NummberShift, byte v_NumberSpeed, List<byte> v_ModeScan, float v_NoMove, Int32 v_TimeToSleep = 500)
        {
            byte[][] v_bytes = new byte[8][];
            v_bytes[0] = new byte[] { (byte)0x97 };
            if ( v_first >= v_second )
            {
                v_bytes[1] = C_PackageD02.F_СonversionFloat32(v_first);
                v_bytes[2] = C_PackageD02.F_СonversionFloat32(v_second);
                //v_bytes[7] = (Int16)((v_first-v_second)/);
            }
            else
            {
                v_bytes[2] = C_PackageD02.F_СonversionFloat32(v_first);
                v_bytes[1] = C_PackageD02.F_СonversionFloat32(v_second);
            }
            v_bytes[3] = new byte[] { v_NummberShift };
            v_bytes[4] = new byte[] { v_NumberSpeed };
            v_bytes[5] = v_ModeScan.ToArray() ;
            v_bytes[6] = C_PackageD02.F_СonversionFloat32(v_NoMove);
            foreach (byte[] v_bytesOut in v_bytes)
                foreach (byte v_byteOut in v_bytesOut)
                    F_ComWrite_Verification(v_byteOut, v_TimeToSleep);
        }
    }
}
