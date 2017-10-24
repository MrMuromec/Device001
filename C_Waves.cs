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

        public int Fv_OneShift
        {
            get
            {
                return V_WaveDynamic.Fv_OneShift;
            }
            set
            {
                foreach (C_Wave v_W in V_Waves)
                    v_W.Fv_OneShift = value;
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
