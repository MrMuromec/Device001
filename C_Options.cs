using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_Options : C_Waves
    {
        public C_Options F_Copy()
        {
            C_Options V_op = new C_Options();

            V_op.Fv_NumOperatingMode = this.Fv_NumOperatingMode;
            V_op.Fv_NumShift = this.Fv_NumShift;
            V_op.Fv_NumSpeed = this.Fv_NumTypeMeasurement;
            V_op.Fv_NumTypeMeasurement = this.Fv_NumTypeMeasurement;

            return V_op;
        }
        private string[] V_OperatingMode = new string[] { "Сигналы", "Спектры" }; // Формат данных
        /// <summary>
        /// Форматы данных
        /// </summary>
        public string[] F_GetOperatingMode()
        {
            return V_OperatingMode;
        }

        private string[] V_TypeMeasurement = new string[] { "Возбуждение", "Эмиссия" }; // Режим
        /// <summary>
        /// Режимы данных
        /// </summary>
        public string[] F_GetTypeMeasurement()
        {
            return V_TypeMeasurement;
        }

        private int[] V_OptionsOfMeasument = new int[] { 0, 0, 0 }; // Номера выбранны настроек

        /// <summary>
        /// Скорость
        /// </summary>
        public double Fv_Speed
        {
            get
            {
                return Port.C_ParameterListsD02.F_SpeedGet()[V_OptionsOfMeasument[0]];
            }
            set
            {
                if (Port.C_ParameterListsD02.F_SpeedGet().Contains(value))
                    V_OptionsOfMeasument[0] = Port.C_ParameterListsD02.F_SpeedGet().FindIndex(x => x == value);
            }
        }
        /// <summary>
        /// Номер Скорости
        /// </summary>
        public int Fv_NumSpeed
        {
            get
            {
                return V_OptionsOfMeasument[0];
            }
            set
            {
                if ((Port.C_ParameterListsD02.F_SpeedGet().Count() > value) && (value >= 0))
                    V_OptionsOfMeasument[0] = value;
            }
        }
        /// <summary>
        /// Формат данных
        /// </summary>
        public string Fv_OperatingMode
        {
            get
            {
                return V_OperatingMode[V_OptionsOfMeasument[1]];
            }
            /*
            set
            {                
                if (V_OperatingMode.Contains(value))
                    V_OptionsOfMeasument[1] = Array.FindIndex(V_OperatingMode, x => x==value);
            }
             * */
        }
        /// <summary>
        /// Номер Формата данных
        /// </summary>
        public int Fv_NumOperatingMode
        {
            get
            {
                return V_OptionsOfMeasument[1];
            }
            set
            {
                if ((V_OperatingMode.Count() > value) && (value >= 0))
                    V_OptionsOfMeasument[1] = value;
            }
        }
        /// <summary>
        /// Режим
        /// </summary>
        public string Fv_TypeMeasurement
        {
            get
            {
                return V_TypeMeasurement[V_OptionsOfMeasument[2]];
            }
            /*
            set
            {
                if (V_TypeMeasurement.Contains(value))
                    V_OptionsOfMeasument[2] = Array.FindIndex(V_TypeMeasurement, x => x == value);
            }
             * */
        }
        /// <summary>
        /// другой Режим
        /// </summary>
        public string Fv_OtherTypeMeasurement
        {
            get
            {
                return V_TypeMeasurement[Math.Abs(V_OptionsOfMeasument[2]-1)];
            }
            /*
            set
            {
                if (V_TypeMeasurement.Contains(value))
                    V_OptionsOfMeasument[2] = Array.FindIndex(V_TypeMeasurement, x => x == value);
            }
             * */
        }
        /// <summary>
        /// Номер Режима
        /// </summary>
        public int Fv_NumTypeMeasurement
        {
            get
            {
                return V_OptionsOfMeasument[2];
            }
            set
            {
                if ((V_TypeMeasurement.Count() > value) && (value >= 0))
                    V_OptionsOfMeasument[2] = value;
            }
        }
    }
}
