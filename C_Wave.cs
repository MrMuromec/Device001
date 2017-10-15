using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.ComponentModel;

namespace Device001  
{
    public class C_Wave
    {
        public System.ApplicationException V_Error { get; private set;} // Под ошибки

        public Device001.Port.C_ParameterGrid V_ParameterGrid { get; private set; } // Используемая решётка

        private int V_OneShift;
        /// <summary>
        /// Шаг
        /// </summary>
        public int Fv_OneShift
        {
            get
            {
                return V_OneShift;
            }
            set
            {
                if (value <= (V_ParameterGrid.V_Max - V_ParameterGrid.V_Min))
                    V_OneShift = value;
                else
                {
                    V_Error = new ApplicationException("Недопустимый шаг ( " + value.ToString() + " > " + (V_ParameterGrid.V_Max - V_ParameterGrid.V_Min).ToString() + " )");
                    throw V_Error;
                }
            }
        }
        private int V_Wave;
        /// <summary>
        /// Длина волны
        /// </summary>
        public int Fv_wave
        {
            get
            {
                return V_Wave;
            }
            set
            {
                if (F_WaveValidity(value))
                    V_Wave = value;    
                else
                {
                    V_Error = new ApplicationException("Волна вышла за границы диапазона");
                    throw V_Error;
                }
            }
        }
        public static C_Wave operator ++(C_Wave v_Wave)
        {
            return new C_Wave(v_Wave.V_ParameterGrid, v_Wave.Fv_wave + v_Wave.V_OneShift);
        }
        public static C_Wave operator --(C_Wave v_Wave)
        {
            return new C_Wave(v_Wave.V_ParameterGrid, v_Wave.Fv_wave - v_Wave.V_OneShift);
        }
        /// <summary>
        /// Волна
        /// </summary>
        /// <param name="V_ParameterGrid">используемая решётка</param>
        public C_Wave(Device001.Port.C_ParameterGrid V_ParameterGrid)
        {
            this.V_ParameterGrid = V_ParameterGrid;
            Fv_wave = V_ParameterGrid.V_Min;
        }
        /// <summary>
        /// Волна
        /// </summary>
        /// <param name="V_ParameterGrid">используемая решётка</param>
        /// <param name="v_wave">длина волны</param>
        public C_Wave(Device001.Port.C_ParameterGrid V_ParameterGrid, int v_wave)
        {
            this.V_ParameterGrid = V_ParameterGrid;
            Fv_wave = v_wave;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(int v_wave)
        {
            return (V_ParameterGrid.V_Min <= v_wave) && (v_wave < V_ParameterGrid.V_Max);
        }
    }
}
