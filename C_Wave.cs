using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.ComponentModel;

namespace Device001  
{
    public class C_Wave //: IDataErrorInfo
    {
        private Device001.Port.C_ParameterGrid V_ParameterGrid; // Используемая решётка

        private int V_Wave;
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
            }
        }
        /// <summary>
        /// Волна
        /// </summary>
        /// <param name="V_ParameterGrid">используемая решётка</param>
        /// <param name="v_min">true - если длина волны минимальная для решётки</param>
        public C_Wave(Device001.Port.C_ParameterGrid V_ParameterGrid, bool v_min = true)
        {
            this.V_ParameterGrid = V_ParameterGrid;
            if (v_min)
                Fv_wave = V_ParameterGrid.V_FSLenght[0];
            else
                Fv_wave = V_ParameterGrid.V_FSLenght[1];
        }
        /// <summary>
        /// Параметры решётки
        /// </summary>
        public Device001.Port.C_ParameterGrid F_GetParameterGrid()
        {
            return V_ParameterGrid;
        }
        public bool F_WaveValidity (int v_wave) // Возращвет true, если волна в допустимом диапазоне 
        {
            int v_min = V_ParameterGrid.V_FSLenght[0];
            int v_max = V_ParameterGrid.V_FSLenght[1];
            return (v_min <= v_wave) && (v_wave < v_max);
        }


        /*
public delegate void V_DelegateNew();
public event V_DelegateNew V_NewGrid;
public Device001.Port.C_ParameterGrid V_ParameterGrid
{
    get
    {
        return V_ParameterGrid_;
    }
    set
    {
        V_ParameterGrid_ = value;
        if (V_NewGrid != null)
            V_NewGrid();
    }
}
 * */
    }
}
