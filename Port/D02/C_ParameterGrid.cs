using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001.Port
{
    public class C_ParameterGrid
    {
        public int V_Min { get; private set; }
        public int V_Max { get; private set; }
        public int V_NumberStrokes { get; private set; }
        /// <summary>
        /// Параметры решётки
        /// </summary>
        /// <param name="v_FSLenght"> Диапазон волн (верхняя и ниженяя граница)</param>
        /// <param name="v_NumberStrokes"> Число штрихов решётки </param>
        public C_ParameterGrid(int[] v_FSLenght, int v_NumberStrokes)
        {
            V_Min = v_FSLenght.Min();
            V_Max = v_FSLenght.Max();
            V_NumberStrokes = v_NumberStrokes;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) 
                return false;

            C_ParameterGrid V_ = (C_ParameterGrid)obj;
            return ((this.V_Max == V_.V_Max) && (this.V_Min == V_.V_Min) && (this.V_NumberStrokes == V_.V_NumberStrokes));
        }
        /*
        public static bool operator ==(C_ParameterGrid v1, C_ParameterGrid v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(C_ParameterGrid v1, C_ParameterGrid v2)
        {
            return v1.Equals(v2);
        }
         * */

    }
}
