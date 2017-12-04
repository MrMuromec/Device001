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

        private C_ParametorGrid.S_ParameterGrid V_ParameterGrid; // Используемая решётка

        public C_ParametorGrid.S_ParameterGrid Fv_ParameterGrid
        {
            get
            {
                return V_ParameterGrid;
            }
            set
            {
                V_ParameterGrid = value;
                if (!F_WaveValidity(Fv_wave, (C_ParametorGrid.S_ParameterGrid)value))
                    V_Error = new ApplicationException("Волна (" + Fv_wave + ") в не диапазона решётки (" + value.Fv_Min + " , " + value.Fv_Max + "). Смените решётку или измените длину волны. " + F_GridNew());
                else
                    V_Error = null;
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
                V_Wave = value;
                if (!F_WaveValidity(value))
                    V_Error = new ApplicationException("Волна (" + value + ") в не диапазона решётки (" + this.Fv_ParameterGrid.Fv_Min + " , " + this.Fv_ParameterGrid.Fv_Max + "). Смените решётку или измените длину волны. " + F_GridNew());
                else
                    V_Error = null;
            }
        }
        /*
        public void F_NewWaveAndGrid(double v_NewWave)
        {
            V_Wave = v_NewWave;
            foreach (var v_grid in C_ParametorGrid.F_GridGet())
                if (F_WaveValidity(v_grid))
                    Fv_ParameterGrid = v_grid;
        }
         * */
        private string F_GridNew() // Дать нормальное имя
        {
            foreach (var v_grid in C_ParametorGrid.F_GridGet())
                if (F_WaveValidity(v_grid))
                    return "Подходящая решётка: " + v_grid.Fv_NumberStrokes.ToString() + " штр/мм.";
            return "Подходящих решёток нет";
        }
        /// <summary>
        /// Волна, с учётом длины волны
        /// </summary>
        /// <param name="v_ParameterGrid">используемая решётка</param>
        /// <param name="v_wave">длина волны</param>
        public C_Wave(C_ParametorGrid.S_ParameterGrid v_ParameterGrid, double v_wave)
        {
            V_Wave = v_wave;
            V_ParameterGrid = v_ParameterGrid;
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity()
        {
            return (Fv_ParameterGrid.Fv_Min <= Fv_wave) && (Fv_wave < Fv_ParameterGrid.Fv_Max);
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(double v_wave)
        {
            return (Fv_ParameterGrid.Fv_Min <= v_wave) && (v_wave < Fv_ParameterGrid.Fv_Max);
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(C_ParametorGrid.S_ParameterGrid v_ParameterGrid)
        {
            return (v_ParameterGrid.Fv_Min <= Fv_wave) && (Fv_wave < v_ParameterGrid.Fv_Max);
        }
        /// <summary>
        /// Возращвет true, если волна в допустимом диапазоне 
        /// </summary>
        public bool F_WaveValidity(double v_wave, C_ParametorGrid.S_ParameterGrid v_ParameterGrid)
        {
            return (v_ParameterGrid.Fv_Min <= v_wave) && (v_wave < v_ParameterGrid.Fv_Max);
        }
    }
}
