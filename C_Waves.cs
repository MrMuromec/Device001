using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_Waves
    {
        private C_Wave[] V_Waves;
        /// <summary>
        /// Длина волны посточнного монохроматора
        /// </summary>
        private C_Wave V_WaveStatic
        {
            get
            {
                return V_Waves[0];
            }
            set
            {
                V_Waves[0] = value;
            }
        }
        /// <summary>
        /// Длина волны переменного монохраматора
        /// </summary>
        private C_Wave V_WaveDynamic
        {
            get
            {
                return V_Waves[1];
            }
            set
            {
                V_Waves[1] = value;
            }
        }
        /// <summary>
        /// Минимальная длина волны переменнного монохроматора
        /// </summary>
        private C_Wave V_WaveMinDynamic
        {
            get
            {
                return V_Waves[2];
            }
            set
            {
                V_Waves[2] = value;
            }
        }
        /// <summary>
        /// Максимальная длина волны переменного монохроматора
        /// </summary>
        private C_Wave V_WaveMaxDynamic
        {
            get
            {
                return V_Waves[3];
            }
            set
            {
                V_Waves[3] = value;
            }
        }       
        public C_Waves()
        {
            V_Waves = new C_Wave[]
            {
                new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]),
                new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]),
                new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]),
                new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0])
            };
        }
    }
}
