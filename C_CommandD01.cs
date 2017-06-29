using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Collections.Concurrent;
using System.Threading;

namespace Device001
{
    /// <summary>
    /// Класс команд для 1 устройства
    /// </summary>
    public class C_CommandD01 : C_MyPort
    {
        private byte[] V_CommandReset = { 0x12, 0x00 }; // Сброс
        private byte[] V_CommandPMT = { 0x13, 0x02 }; // ФЭУ
        private byte[] V_CommandRequest = { 0x12, 0x01 }; // Запрос

        /// <summary>
        /// Поток для разбора
        /// </summary>
        private void T_Decoder()
        {
            byte V_byte; // Загруженный байт
            byte[] _pack = new byte[9]; // Пачка
            byte _n = 0; // Счётчик  
            bool _logPack = false; // Проверка на безопасность
            Thread.Sleep(0);
            while (true)
            {
                if (V_StakIn.TryPop(out V_byte))
                {
                    if ((V_CommandReset[1] == V_byte) && !_logPack)
                    {
                        _logPack = false;
                        //if (_SuccessReset != null)_SuccessReset();
                        continue;
                    }
                    if ((V_CommandPMT[1] == V_byte) && !_logPack)
                    {
                        _logPack = false;
                        //if (_SuccessPMT != null)_SuccessPMT();
                        continue;
                    }
                    if ((V_CommandRequest[1] == V_byte) && !_logPack)
                    {
                        _n = 0;
                        _logPack = true;
                        continue;
                    }
                    if (_logPack == true)
                    {
                        _pack[_n] = V_byte;
                        _n++;
                        if (_n == 9)
                        {
                            //_D._Parse(_pack);
                            _logPack = false;
                            //if (_SuccessRequest != null)_SuccessRequest();
                        }
                        continue;
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
