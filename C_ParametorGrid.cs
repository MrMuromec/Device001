using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_ParametorGrid
    {
        /// <summary>
        /// Набор решёток
        /// </summary>
        public static List<S_ParameterGrid> F_GridGet()
        {
            return new List<S_ParameterGrid> { 
            new S_ParameterGrid (new int[] { 200, 500 },3000), 
            new S_ParameterGrid (new int[] { 400, 1000 },1500) };
        }
        public struct S_ParameterGrid
        {
            private int V_Min;
            public int Fv_Min
            {
                get
                {
                    return V_Min;
                }
                private set
                {
                    V_Min = value;
                }
            }
            private int V_Max;
            public int Fv_Max
            {
                get
                {
                    return V_Max;
                }
                private set
                {
                    V_Max = value;
                }
            }
            private int V_NumberStrokes;
            public int Fv_NumberStrokes
            {
                get
                {
                    return V_NumberStrokes;
                }
                private set
                {
                    V_NumberStrokes = value;
                }
            }
            /// <summary>
            /// Параметры решётки
            /// </summary>
            /// <param name="v_FSLenght"> Диапазон волн (верхняя и ниженяя граница)</param>
            /// <param name="v_NumberStrokes"> Число штрихов решётки </param>
            public S_ParameterGrid(int[] v_FSLenght, int v_NumberStrokes)
            {
                V_Max = 0;
                V_Min = 0;
                V_NumberStrokes = 0;

                Fv_Min = v_FSLenght.Min();
                Fv_Max = v_FSLenght.Max();
                Fv_NumberStrokes = v_NumberStrokes;
            }
            public override bool Equals(object obj)
            {
                if (obj.GetType() != this.GetType())
                    return false;

                S_ParameterGrid V_ = (S_ParameterGrid)obj;
                return ((this.Fv_Max == V_.Fv_Max) && (this.Fv_Min == V_.Fv_Min) && (this.Fv_NumberStrokes == V_.Fv_NumberStrokes));
            }
        }
    }
}
