using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Device001
{
    /// <summary>
    /// Калибровка для расчётов
    /// </summary>
    public class C_Calibration02
    {
        public S_Function V_Excitation , V_Emission;

        private BinaryFormatter V_formatter = new BinaryFormatter();  // Формат 
        /// <summary>
        /// Калибровка для расчётов
        /// </summary>
        public C_Calibration02()
        {
            try
            {
                using (FileStream fs = new FileStream("Excitation.dat", FileMode.Open))  // Подумать насчт исключений
                {
                    V_Excitation = (S_Function)V_formatter.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                V_Excitation = new S_Function(new double[3] { 0, 0, 0 }, 0, 1);
            }
            try
            {
                using (FileStream fs = new FileStream("Emission.dat", FileMode.Open))  // Подумать насчт исключений
                {
                    V_Emission = (S_Function)V_formatter.Deserialize(fs);
                }
            }
            catch (FileNotFoundException)
            {
                V_Emission = new S_Function(new double[3] { 0, 0, 0 }, 0, 0);
            }
        }
        /// <summary>
        /// Установка калибровки для расчётов
        /// </summary>
        public void F_SetCalibration02(C_Calibration02 v_NewCalibration02)
        {
            V_Emission = v_NewCalibration02.V_Emission;
            V_Excitation = v_NewCalibration02.V_Excitation;

            using (FileStream fs = new FileStream("Excitation.dat", FileMode.OpenOrCreate)) // Подумать насчёт исключений
            {
                V_formatter.Serialize(fs, V_Excitation);
            }
            using (FileStream fs = new FileStream("Emission.dat", FileMode.OpenOrCreate)) // Подумать насчёт исключений
            {
                V_formatter.Serialize(fs, V_Emission);
            }
        }

        [Serializable]
        public struct S_Function
        {
            private double[] V_Coefficients;
            /// <summary>
            /// Коэффиценты многочлена
            /// </summary>
            public double[] Fv_Coefficients
            {
                get
                {
                    return V_Coefficients;
                }
            }
            private double V_Height;
            /// <summary>
            /// Ширина шели
            /// </summary>
            public double Fv_Height
            {
                get
                {
                    return V_Height;
                }
            }
            private double V_Coefficient;
            /// <summary>
            /// Коэффицент
            /// </summary>
            public double Fv_Coefficient
            {
                get
                {
                    return V_Coefficient;
                }
            }
            /*
            public static bool operator ==(S_Function v1, S_Function v2)
            {
                return ((v1.V_Coefficient == v2.V_Coefficient) && (v1.V_Height == v2.V_Height) && (v1.V_Coefficients[0] == v2.V_Coefficients[0]) && (v1.V_Coefficients[1] == v2.V_Coefficients[1]) && (v1.V_Coefficients[2] == v2.V_Coefficients[2]));
            }
            public static bool operator !=(S_Function v1, S_Function v2)
            {
                return !((v1.V_Coefficient == v2.V_Coefficient) && (v1.V_Height == v2.V_Height) && (v1.V_Coefficients[0] == v2.V_Coefficients[0]) && (v1.V_Coefficients[1] == v2.V_Coefficients[1]) && (v1.V_Coefficients[2] == v2.V_Coefficients[2]));
            }
             * */
            /// <summary>
            /// структура для функции вычисления плотности потоков возбуждения/эмиссии 
            /// </summary>
            /// <param name="v_Coefficients">Коэффиценты полинома</param>
            /// <param name="v_Height">Ширины щели</param>
            /// <param name="v_Coefficient">Коэффициента коррекции</param>
            public S_Function(double[] v_Coefficients, double v_Height, double v_Coefficient)
            {
                V_Coefficients = v_Coefficients;
                V_Height = v_Height;
                V_Coefficient = v_Coefficient;
            }
            /// <summary>
            /// структура под параметры для вычесления члена многочлена
            /// </summary>
            private struct S_ParamOfCoefficient
            {
                public double V_Coefficient;
                public double V_WaveLength;
                public int V_Pow;
                /// <summary>
                /// Параметры для вычесления члена многочлена
                /// </summary>
                /// <param name="v_Coefficient">Коэффицент</param>
                /// <param name="v_WaveLength">Длина волны</param>
                /// <param name="v_Pow">Степень</param>
                public S_ParamOfCoefficient(double v_Coefficient, double v_WaveLength, int v_Pow)
                {
                    V_Coefficient = v_Coefficient;
                    V_WaveLength = v_WaveLength;
                    V_Pow = v_Pow;
                }
            }
            /// <summary>
            ///  Плотность потоков возбуждения/эмиссии 
            /// </summary>
            public double F_SpectralDensitiesOfFlows(double v_Photodetector, double v_WaveLength)
            {
                double V_Sum = 0;
                for (int i = 0; i < V_Coefficients.Count(); ++i)
                    V_Sum += Task.Factory.StartNew(V_param => 
                    { 
                        S_ParamOfCoefficient v_p = (S_ParamOfCoefficient)V_param;
                        return v_p.V_Coefficient * Math.Pow(v_p.V_WaveLength, v_p.V_Pow);
                    }, new S_ParamOfCoefficient(V_Coefficients[i], v_WaveLength, i)).Result;
                return (((V_Sum) * v_Photodetector * V_Coefficient) / V_Height);
                //return (((V_Coefficients[0] + V_Coefficients[1] * v_WaveLength + V_Coefficients[2] * v_WaveLength * v_WaveLength) * v_Photodetector * V_Coefficient) / V_Height);
            }
        }
    }
}
