using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001.Port
{
    /// <summary>
    /// Класс для разбора пачек данных с устройства 01
    /// </summary>
    public class C_PackageD01
    {
        Package.C_32[] V_INT32;
        /// <summary>
        /// Разбирает пачки
        /// </summary>
        /// <param name="v_byte">пачка на разбор</param>
        public void F_Parse(byte[] v_byte)
        {
            V_INT32 = new Package.C_32[3] { new Package.C_32(), new Package.C_32(), new Package.C_32() };
            for (int i = 0; i < 3; i++)
            {
                if ((byte)(v_byte[2 + i * 3] & 0x80) == 0x00) // Положительноли?
                    V_INT32[i].b3 = (byte)(0x00);
                else
                    V_INT32[i].b3 = (byte)(0xFF);
                V_INT32[i].b0 = v_byte[0 + i * 3];
                V_INT32[i].b1 = v_byte[1 + i * 3];
                V_INT32[i].b2 = v_byte[2 + i * 3];
            }
        }
        /// <summary>
        /// От 1 блока три 32-битных числа со знаком в доп. коде.
        /// </summary>
        /// <param name="v_N">
        /// 0 - величина сигнала ФЭУ
        /// 1 - величина сигнала опорного канала
        /// 2 - величина сигнала зонда</param>
        /// <returns></returns>
        public Int32 F_GetInt32(byte v_N)
        {
            return V_INT32[v_N].I32;
        }
    }
}
