using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.ComponentModel;

namespace Device001  
{
    public class C_Wave : IDataErrorInfo
    {
        public int V_Wave { get; set; }
        public Device001.Port.C_ParameterGrid V_ParameterGrid;

        public C_Wave (Device001.Port.C_ParameterGrid V_ParameterGrid)
        {
            this.V_ParameterGrid = V_ParameterGrid;
        }
        public string this[string columnName]
        {
            get
            {
                string v_error = String.Empty;
                int v_min = V_ParameterGrid.V_FSLenght[0];
                int v_max = V_ParameterGrid.V_FSLenght[1];
                switch (columnName)
                {
                    case "V_Wave":
                        if ((v_min > V_Wave) || (V_Wave > v_max))
                            v_error = "Недопустимая длина волны. Заданная длина волны не принадлежит интервалу [" + v_min.ToString() + ".." + v_max.ToString() + "]";
                        break;
                    case "V_ParameterGrid":
                        break;
                }
                return v_error;
            }
            /*
            set
            {
                switch (columnName)
                {
                    case "V_Wave":
                        break;
                    case "V_ParameterGrid":
                        int v_min = V_ParameterGrid.V_FSLenght[0];
                        int v_max = V_ParameterGrid.V_FSLenght[1];
                        if (v_min > V_Wave)
                            V_Wave = v_min;
                        if (V_Wave > v_max) 
                            V_Wave = v_max;
                        break;
                }
            }
             * */
        }
        public string Error
        {
            get { throw new NotImplementedException(); }
        }
        public Device001.Port.C_ParameterGrid F_GetParameterGrid()
        {
            return V_ParameterGrid;
        }
    }
}
