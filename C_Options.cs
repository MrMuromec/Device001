﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device001
{
    public class C_Options : C_Waves
    {
        // Создание копии с выделением памяти
        public C_Options F_Copy()
        {
            C_Options V_op = new C_Options();

            V_op.Fv_NumOperatingMode = this.Fv_NumOperatingMode;
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
        /// <summary>
        /// Длина волны не статичного монохраматора
        /// </summary>
        public override C_Wave V_WaveDynamic
        {
            get
            {
                if (Fv_NumTypeMeasurement == 0)
                    return V_WaveFirst;
                else
                    return V_WaveSecond;
            }
        }
        /// <summary>
        /// Длина волны статичного монохраматора
        /// </summary>
        public override C_Wave V_WaveStatic
        {
            get
            {
                if (Fv_NumTypeMeasurement == 1)
                    return V_WaveFirst;
                else
                    return V_WaveSecond;
            }
        }
    }
}
