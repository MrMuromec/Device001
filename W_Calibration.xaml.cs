using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//
using System.Globalization;

namespace Device001
{
    /// <summary>
    /// Interaction logic for W_Calibration.xaml
    /// </summary>
    public partial class W_Calibration : Window
    {
        public W_Calibration()
        {
            InitializeComponent();
        }
        private C_Calibration02 V_Calibration02;
        public W_Calibration(C_Calibration02 v_Calibration02)
        {
            V_Calibration02 = v_Calibration02;

            InitializeComponent();

            TB_A1.Text = V_Calibration02.V_Excitation.Fv_Coefficients[0].ToString(CultureInfo.InvariantCulture);
            TB_A2.Text = V_Calibration02.V_Excitation.Fv_Coefficients[1].ToString(CultureInfo.InvariantCulture);
            TB_A3.Text = V_Calibration02.V_Excitation.Fv_Coefficients[2].ToString(CultureInfo.InvariantCulture);
            TB_HeightExcitation.Text = V_Calibration02.V_Excitation.Fv_Height.ToString(CultureInfo.InvariantCulture);

            TB_A4.Text = V_Calibration02.V_Emission.Fv_Coefficients[0].ToString(CultureInfo.InvariantCulture);
            TB_A5.Text = V_Calibration02.V_Emission.Fv_Coefficients[1].ToString(CultureInfo.InvariantCulture);
            TB_A6.Text = V_Calibration02.V_Emission.Fv_Coefficients[2].ToString(CultureInfo.InvariantCulture);
            TB_HeightEmission.Text = V_Calibration02.V_Emission.Fv_Height.ToString(CultureInfo.InvariantCulture);
            TB_.Text = V_Calibration02.V_Emission.Fv_Coefficient.ToString(CultureInfo.InvariantCulture);
        }
        public delegate void D_UseCalibration02(C_Calibration02 v_Calibration02);
        public event D_UseCalibration02 Event_UseCalibration02;

        private void B_UseCalibration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                V_Calibration02.V_Excitation = new C_Calibration02.S_Function(
                    new double[] {
                        double.Parse(TB_A1.Text.Replace(',', '.'),CultureInfo.InvariantCulture),
                        double.Parse(TB_A2.Text.Replace(',', '.'),CultureInfo.InvariantCulture),
                        double.Parse(TB_A3.Text.Replace(',', '.'),CultureInfo.InvariantCulture)},
                    double.Parse(TB_HeightExcitation.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                    1);
                
                V_Calibration02.V_Emission = new C_Calibration02.S_Function(
                    new double[] {
                        double.Parse(TB_A4.Text.Replace(',', '.'),CultureInfo.InvariantCulture),
                        double.Parse(TB_A5.Text.Replace(',', '.'),CultureInfo.InvariantCulture),
                        double.Parse(TB_A6.Text.Replace(',', '.'),CultureInfo.InvariantCulture)},
                    double.Parse(TB_HeightExcitation.Text.Replace(',', '.'), CultureInfo.InvariantCulture),
                    double.Parse(TB_.Text.Replace(',', '.'), CultureInfo.InvariantCulture));

                if (Event_UseCalibration02 != null)
                    Event_UseCalibration02(V_Calibration02);

                this.Close();
            }
            catch (System.FormatException v_Ex)
            {
                MessageBox.Show(v_Ex.Message, "Ошибка данных");
            }
        }
    }
}
