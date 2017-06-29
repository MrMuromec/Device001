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
using System.IO.Ports;

namespace Device001
{
    /// <summary>
    /// Логика взаимодействия для W_Port1.xaml
    /// </summary>
    public partial class W_Port1 : Window
    {
        public W_Port1()
        {
            InitializeComponent();
        }
        public W_Port1(string V_NamePort, StopBits V_StopBits, Parity V_Parity, int V_BaudRate)
        {
            foreach (var v_n in C_MyPortOptions.F_GetPortNames())
                CB_NamePort.Items.Add(v_n);
            if (C_MyPortOptions.F_GetPortNames().Count() != 0)
                CB_NamePort.SelectedIndex = 0;
            if (C_MyPortOptions.F_GetPortNames().Contains(V_NamePort))
                CB_NamePort.SelectedIndex = Array.FindIndex(C_MyPortOptions.F_GetPortNames(), x => x == V_NamePort);

            foreach (var v_sb in C_MyPortOptions.F_GetStopBits())
                CB_StopBits.Items.Add(v_sb);
            foreach (var v_p in C_MyPortOptions.F_GetParity())
                CB_Parity.Items.Add(v_p);
            foreach (var v_br in C_MyPortOptions.F_GetBaudRate())
                CB_BaudRate.Items.Add(v_br);
            InitializeComponent();
        }

        private void B_UpdatePorts_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
