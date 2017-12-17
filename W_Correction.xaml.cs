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
    /// Логика взаимодействия для W_Correction.xaml
    /// </summary>
    public partial class W_Correction : Window
    {
        public delegate void D_UseCorrection(float[] v_Correction);
        public event D_UseCorrection Event_UseCorrection;
        public W_Correction()
        {
            InitializeComponent();
        }

        private void B_UseCorrection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Event_UseCorrection != null)
                    {
                        float[] v_Monohromators = new float[] { float.Parse(TB_Monochromator1.Text.Replace(',', '.'), CultureInfo.InvariantCulture), float.Parse(TB_Monochromator2.Text.Replace(',', '.'), CultureInfo.InvariantCulture) };
                        if ((C_ParametorGrid.F_GridGet()[0].Fv_Min <= v_Monohromators.Min()) && (v_Monohromators.Max() <= C_ParametorGrid.F_GridGet()[0].Fv_Max))
                        {
                            B_UseCorrection.IsEnabled = !B_UseCorrection.IsEnabled;
                            Event_UseCorrection(v_Monohromators);
                            this.Close();
                        }
                        else
                            MessageBox.Show("Показания счётчика за границами рабочего диапазона [ " + C_ParametorGrid.F_GridGet()[0].Fv_Min.ToString() + " ; " + C_ParametorGrid.F_GridGet()[0].Fv_Max.ToString() + " ]", "Ошибка данных");
                    }
            }
            catch (System.FormatException v_Ex)
            {
                MessageBox.Show(v_Ex.Message, "Ошибка данных");
            }
        }
    }
}
