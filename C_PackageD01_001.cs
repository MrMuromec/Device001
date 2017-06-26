using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;

namespace Device001
{
    [StructLayoutAttribute(LayoutKind.Explicit, Size = 4)]
    public class C_Int32
    {
        [FieldOffsetAttribute(3)]
        public byte b3;
        [FieldOffsetAttribute(2)]
        public byte b2;
        [FieldOffsetAttribute(1)]
        public byte b1;
        [FieldOffsetAttribute(0)]
        public byte b0;
        [FieldOffsetAttribute(0)]
        public Int32 I32;
    }
    /// <summary>
    /// Класс для разбора пачек данных с устройства 01
    /// </summary>
    public class C_PackageD01_001
    {
        C_Int32[] INT32;
        /// <summary>
        /// Разбирает пачки
        /// </summary>
        /// <param name="_byte">пачка на разбор</param>
        public void _Parse(byte[] _byte)
        {
            INT32 = new C_Int32[3] { new C_Int32(), new C_Int32(), new C_Int32() };
            for (int i = 0; i < 3; i++)
            {
                if ((byte)(_byte[2 + i * 3] & 0x80) == 0x00) // Положительноли?
                    INT32[i].b3 = (byte)(0x00);
                else
                    INT32[i].b3 = (byte)(0xFF);
                INT32[i].b0 = _byte[0 + i * 3];
                INT32[i].b1 = _byte[1 + i * 3];
                INT32[i].b2 = _byte[2 + i * 3];
            }
        }
        /// <summary>
        /// От 1 изделия три 32-битных числа со знаком в доп. коде.
        /// </summary>
        /// <param name="N">
        /// 0 - величина сигнала ФЭУ
        /// 1 - величина сигнала опорного канала
        /// 2 - величина сигнала зонда</param>
        /// <returns>Int32.MinValue - код ошибки</returns>
        public Int32 _GetInt32(byte N)
        {
            if (N < 3)
                return INT32[N].I32;
            else
                return Int32.MinValue;
        }
    }
}
