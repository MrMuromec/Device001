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
    }
}
