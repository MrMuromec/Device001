using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_Waves
    {
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

        private C_Wave[] V_Waves;
        /// <summary>
        /// Длина волны постоянного монохроматора
        /// </summary>
        public C_Wave V_WaveStatic
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
        /// Длина волны переменного монохраматора
        /// </summary>
        public C_Wave V_WaveDynamic
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
        public C_Waves()
        {
            var v_OptionsGrids = Port.C_ParameterListsD02.F_NumGridGet();
            V_Waves = new C_Wave[]
            {
                new C_Wave(v_OptionsGrids[0],v_OptionsGrids[0].V_Min),
                new C_Wave(v_OptionsGrids[0],v_OptionsGrids[0].V_Min),
            };
        }
    }
}
