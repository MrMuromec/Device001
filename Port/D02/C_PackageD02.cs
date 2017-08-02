using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;

namespace Device001.Port
{
    public class C_PackageD02
    {
        Package.C_32 V_Float;

        /// <summary>
        /// Разбирает пачки
        /// </summary>
        /// <param name="v_byte">пачка на разбор</param>
        public void F_Parse(byte[] v_byte)
        {
            V_Float = new Device001.Port.Package.C_32();
            V_Float.b0 = v_byte[1];
            V_Float.b1 = v_byte[0];
            V_Float.b2 = v_byte[3];
            V_Float.b3 = v_byte[2];
        }
        /// <summary>
        /// Перевернутое число с плавающей точкой
        /// </summary>
        /// <returns></returns>
        public float F_GetFloat32()
        {
            return V_Float.F32;
        }
        /// <summary>
        /// Плавающее число 4-х байтовое, с двоичным порядком; от представления, принятого в других языках, отличается перестановкой певого и второго слова.
        /// </summary>
        /// <param name="v_Float"> Число для переворота</param>
        /// <returns></returns>
        public static byte[] F_СonversionFloat32(float v_Float)
        {
            Package.C_32 v_F = new Package.C_32();
            v_F.F32 = v_Float;
            return new byte[] { v_F.b1, v_F.b0, v_F.b3, v_F.b2 };
        }
    }
}
