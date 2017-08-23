using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.Runtime.InteropServices;

namespace Device001.Port.Package
{
    /// <summary>
    /// Класс для работы с памятью
    /// </summary>
    [StructLayoutAttribute(LayoutKind.Explicit, Size = 4)]
    public class C_32
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
        public float F32;
        [FieldOffsetAttribute(0)]
        public Int32 I32;
    }
}
