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
//
using Device001.Port;

namespace Device001
{
    /// <summary>
    /// Логика взаимодействия для W_Port1.xaml
    /// </summary>
    public partial class W_Port1 : Window
    {
        public delegate void D_UseSettings(Device001.Port.C_MyPort v_Port);
        public event D_UseSettings Event_UseSettings;

        private Device001.Port.C_MyPort V_Port;
        public W_Port1()
        {
            InitializeComponent();
        }
        public W_Port1(Device001.Port.C_MyPort v_Port, string V_NameDeice)
        {
            InitializeComponent();

            this.Title = V_NameDeice;

            V_Port = v_Port;
            
            foreach (var v_n in C_PortOptions.F_GetPortNames())
                CB_NamePort.Items.Add(v_n);
            //CB_NamePort.Items.Add("COM2");
            if (C_PortOptions.F_GetPortNames().Count() != 0)
                CB_NamePort.SelectedIndex = 0;
            if (C_PortOptions.F_GetPortNames().Contains(v_Port.Fv_PortName))
                CB_NamePort.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetPortNames(), x => x == v_Port.Fv_PortName);
            
            foreach (var v_sb in C_PortOptions.F_GetStopBits())
                CB_StopBits.Items.Add(C_PortOptions.F_StopBits(v_sb));
            if (CB_StopBits.Items.Contains(C_PortOptions.F_StopBits(v_Port.Fv_StopBits)))
                CB_StopBits.SelectedIndex = CB_StopBits.Items.IndexOf(C_PortOptions.F_StopBits(v_Port.Fv_StopBits));

            foreach (var v_sb in C_PortOptions.F_GetParity())
                CB_Parity.Items.Add(C_PortOptions.F_Parity(v_sb));
            if (CB_Parity.Items.Contains(C_PortOptions.F_Parity(v_Port.Fv_Parity)))
                CB_Parity.SelectedIndex = CB_Parity.Items.IndexOf(C_PortOptions.F_Parity(v_Port.Fv_Parity));

            foreach (var v_sb in C_PortOptions.F_GetBaudRate())
                CB_BaudRate.Items.Add(C_PortOptions.F_BaudRate(v_sb));
            if (CB_BaudRate.Items.Contains(C_PortOptions.F_BaudRate(v_Port.Fv_BaudRate)))
                CB_BaudRate.SelectedIndex = CB_BaudRate.Items.IndexOf(C_PortOptions.F_BaudRate(v_Port.Fv_BaudRate));
        }

        private void B_UpdatePorts_Click(object sender, RoutedEventArgs e) // Обновление портов переделать!
        {
            
            string v_NamePort = "COM2";
            int v_index;
            if ((v_index = CB_NamePort.SelectedIndex) != -1) 
                v_NamePort = CB_NamePort.Items[v_index].ToString();
            CB_NamePort.Items.Clear();

            foreach (var v_n in C_PortOptions.F_GetPortNames())
                CB_NamePort.Items.Add(v_n);
            if (C_PortOptions.F_GetPortNames().Count() != 0)
                CB_NamePort.SelectedIndex = 0;
            if (C_PortOptions.F_GetPortNames().Contains(v_NamePort))
                CB_NamePort.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetPortNames(), x => x == v_NamePort);
        }

        private void B_UsePorts_Click(object sender, RoutedEventArgs e)
        {
            V_Port.Fv_BaudRate = C_PortOptions.F_GetBaudRate()[CB_BaudRate.SelectedIndex];
            V_Port.Fv_Parity = C_PortOptions.F_GetParity()[CB_Parity.SelectedIndex];
            int v_index;
            if ((v_index = CB_NamePort.SelectedIndex) != -1)
                V_Port.Fv_PortName = CB_NamePort.Items[CB_NamePort.SelectedIndex].ToString();
            V_Port.Fv_StopBits = C_PortOptions.F_GetStopBits()[CB_StopBits.SelectedIndex];
            if (Event_UseSettings != null)
                Event_UseSettings(V_Port);
        }
    }
}
