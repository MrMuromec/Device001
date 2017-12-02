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

        private Device001.Port.C_ParameterGrid V_ParameterGrid = null; // Используемая решётка

        public Device001.Port.C_ParameterGrid Fv_ParameterGrid
        {
            get
            {
                return V_ParameterGrid;
            }
            set
            {
                if (F_WaveValidity(Fv_wave,value))
                    V_ParameterGrid = value;
                else
                {
                    V_Error = new ApplicationException("Волна (" + Fv_wave + ") в не диапазона решётки (" + value.V_Min + " , " + value.V_Max + "). Смените решётку или измените длину волны. " + F_GridNew());
                    throw V_Error;
                }
            }
        }

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
                    //V_Error = new ApplicationException("Волна вышла за границы диапазона");
                    V_Error = new ApplicationException("Волна (" + value + ") в не диапазона решётки (" + this.Fv_ParameterGrid.V_Min + " , " + this.Fv_ParameterGrid.V_Max + "). Смените решётку или измените длину волны. " + F_GridNew());
                    throw V_Error;
                }
            }
        }
        public void F_NewWaveAndGrid(double v_NewWave)
        {
            V_Wave = v_NewWave;
            foreach (var v_grid in Device001.Port.C_ParameterListsD02.F_NumGridGet())
                if (F_WaveValidity(v_grid))
                    Fv_ParameterGrid = v_grid;
        }
        private string F_GridNew()
        {
            foreach (var v_grid in Device001.Port.C_ParameterListsD02.F_NumGridGet())
                if (F_WaveValidity(v_grid))
                    return "Подходящая решётка: " + v_grid.V_NumberStrokes.ToString() + " штр/мм.";
            return "Подходящих решёток нет";
        }
        /// <summary>
        /// Волна, с учётом длины волны
        /// </summary>
        /// <param name="v_ParameterGrid">используемая решётка</param>
        /// <param name="v_wave">длина волны</param>
        public C_Wave(Device001.Port.C_ParameterGrid v_ParameterGrid, double v_wave)
        {
            Fv_wave = v_wave;
            Fv_ParameterGrid = v_ParameterGrid;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity()
        {
            if (Fv_ParameterGrid != null)
                return (Fv_ParameterGrid.V_Min <= Fv_wave) && (Fv_wave < Fv_ParameterGrid.V_Max);
            else
                return true;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(double v_wave)
        {
            if (Fv_ParameterGrid != null)
                return (Fv_ParameterGrid.V_Min <= v_wave) && (v_wave < Fv_ParameterGrid.V_Max);
            else
                return true;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(Device001.Port.C_ParameterGrid v_ParameterGrid)
        {
            if (Fv_ParameterGrid != null)
                return (v_ParameterGrid.V_Min <= Fv_wave) && (Fv_wave < v_ParameterGrid.V_Max);
            else
                return true;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(double v_wave, Device001.Port.C_ParameterGrid v_ParameterGrid)
        {
            if (Fv_ParameterGrid != null)
                return (v_ParameterGrid.V_Min <= v_wave) && (v_wave < v_ParameterGrid.V_Max);
            else
                return true;
        }
    }
}
