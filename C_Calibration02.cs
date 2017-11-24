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
    public class C_Calibration02
    {
        public S_Function V_Excitation , V_Emission;

        private BinaryFormatter V_formatter = new BinaryFormatter();  // Формат 

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
            public double[] Fv_Coefficients
            {
                get
                {
                    return V_Coefficients;
                }
            }
            private double V_Height;
            public double Fv_Height
            {
                get
                {
                    return V_Height;
                }
            }
            private double V_Coefficient;
            public double Fv_Coefficient
            {
                get
                {
                    return V_Coefficient;
                }
            }

            public static bool operator ==(S_Function v1, S_Function v2)
            {
                return ((v1.V_Coefficient == v2.V_Coefficient) && (v1.V_Height == v2.V_Height) && (v1.V_Coefficients[0] == v2.V_Coefficients[0]) && (v1.V_Coefficients[1] == v2.V_Coefficients[1]) && (v1.V_Coefficients[2] == v2.V_Coefficients[2]));
            }
            public static bool operator !=(S_Function v1, S_Function v2)
            {
                return !((v1.V_Coefficient == v2.V_Coefficient) && (v1.V_Height == v2.V_Height) && (v1.V_Coefficients[0] == v2.V_Coefficients[0]) && (v1.V_Coefficients[1] == v2.V_Coefficients[1]) && (v1.V_Coefficients[2] == v2.V_Coefficients[2]));
            }

            public S_Function(double[] v_Coefficients, double v_Height, double v_Coefficient)
            {
                V_Coefficients = v_Coefficients;
                V_Height = v_Height;
                V_Coefficient = v_Coefficient;
            }
            public double F_SpectralDensitiesOfFlows(double v_Photodetector, double v_WaveLength)
            {
                return (((V_Coefficients[0] * v_WaveLength * v_WaveLength + V_Coefficients[1] * v_WaveLength + V_Coefficients[2]) * v_Photodetector * V_Coefficient) / V_Height);
            }
        }
    }
}
