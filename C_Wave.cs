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

        private double V_Wave; // Длина волны
        /// <summary>
        /// Длина волны
        /// </summary>
        public double Fv_wave
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
        /// <summary>
        /// Волна, конструктор поумолчанию
        /// </summary>
        /// <param name="V_ParameterGrid">используемая решётка</param>
        public C_Wave(Device001.Port.C_ParameterGrid V_ParameterGrid)
        {
            this.V_ParameterGrid = V_ParameterGrid;
            Fv_wave = V_ParameterGrid.V_Min;
        }
        /// <summary>
        /// Волна, с учётом длины волны
        /// </summary>
        /// <param name="V_ParameterGrid">используемая решётка</param>
        /// <param name="v_wave">длина волны</param>
        public C_Wave(Device001.Port.C_ParameterGrid V_ParameterGrid, double v_wave)
        {
            this.V_ParameterGrid = V_ParameterGrid;
            Fv_wave = v_wave;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(double v_wave)
        {
            return (V_ParameterGrid.V_Min <= v_wave) && (v_wave < V_ParameterGrid.V_Max);
        }
    }
}
