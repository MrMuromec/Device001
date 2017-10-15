using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_Waves
    {
        private C_Wave V_WaveStatic;

        private C_Wave V_WaveDynamic;
        private C_Wave V_WaveMinDynamic;
        private C_Wave V_WaveMaxDynamic;
        
        public C_Waves()
        {
            V_WaveStatic = new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]);

            V_WaveDynamic = new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]);
            V_WaveMinDynamic = new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]);
            V_WaveMaxDynamic = new C_Wave(Port.C_ParameterListsD02.F_NumGridGet()[0]);
        }
    }
}
