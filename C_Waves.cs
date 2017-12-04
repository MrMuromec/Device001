using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_Waves
    {
        /*
        private int V_NumShift = 0;
        /// <summary>
        /// Шаг
        /// </summary>
        public double Fv_Shift
        {
            get
            {
                return Port.C_ParameterListsD02.F_ShiftGet()[V_NumShift];
            }
            set
            {
                if (Port.C_ParameterListsD02.F_ShiftGet().Contains(value))
                    V_NumShift = Port.C_ParameterListsD02.F_ShiftGet().FindIndex(x => x == value);
            }
        }
        /// <summary>
        /// Номер Шага
        /// </summary>
        public int Fv_NumShift
        {
            get
            {
                return V_NumShift;
            }
            set
            {
                if ((Port.C_ParameterListsD02.F_ShiftGet().Count() > value) && (value >= 0))
                    V_NumShift = value;
            }
        }
        */
        private C_Wave[] V_Waves;
        /// <summary>
        /// Длина волны 1 монохроматора
        /// </summary>
        public C_Wave V_WaveFirst
        {
            get
            {
                return V_Waves[0];
            }
            private set
            {
                V_Waves[0] = value;
            }
        }
        /// <summary>
        /// Длина волны 2 монохраматора
        /// </summary>
        public C_Wave V_WaveSecond
        {
            get
            {
                return V_Waves[1];
            }
            private set
            {
                V_Waves[1] = value;
            }
        }
        /// <summary>
        /// Длина волны статичного монохраматора
        /// </summary>
        public virtual C_Wave V_WaveStatic
        {
            get
            {
                return V_WaveFirst;
            }
        }
        /// <summary>
        /// Длина волны не статичного монохраматора
        /// </summary>
        public virtual C_Wave V_WaveDynamic
        {
            get
            {
                return V_WaveSecond;
            }
        }
        public C_Waves()
        {
            var v_OptionsGrids = C_ParametorGrid.F_GridGet();
            V_Waves = new C_Wave[]
            {
                new C_Wave(v_OptionsGrids[0],v_OptionsGrids[0].Fv_Min),
                new C_Wave(v_OptionsGrids[0],v_OptionsGrids[0].Fv_Min),
            };
        }
    }
}
