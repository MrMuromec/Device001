﻿using System;
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
        public W_Port1()
        {
            InitializeComponent();
        }
        public W_Port1(string V_NamePort, StopBits V_StopBits, Parity V_Parity, int V_BaudRate, string V_NameDeice)
        {
            InitializeComponent();

            this.Title = V_NameDeice;

            foreach (var v_n in C_PortOptions.F_GetPortNames())
                CB_NamePort.Items.Add(v_n);
            if (C_PortOptions.F_GetPortNames().Count() != 0)
                CB_NamePort.SelectedIndex = 0;
            if (C_PortOptions.F_GetPortNames().Contains(V_NamePort))
                CB_NamePort.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetPortNames(), x => x == V_NamePort);

            foreach (var v_sb in C_PortOptions.F_GetStopBits())
                CB_StopBits.Items.Add(v_sb);
            if (C_PortOptions.F_GetStopBits().Contains(V_StopBits))
                CB_StopBits.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetStopBits(), x => x == V_StopBits);

            foreach (var v_p in C_PortOptions.F_GetParity())
                CB_Parity.Items.Add(v_p);
            if (C_PortOptions.F_GetParity().Contains(V_Parity))
                CB_Parity.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetParity(), x => x == V_Parity);

            foreach (var v_br in C_PortOptions.F_GetBaudRate())
                CB_BaudRate.Items.Add(v_br);
            if (C_PortOptions.F_GetBaudRate().Contains(V_BaudRate))
                CB_BaudRate.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetBaudRate(), x => x == V_BaudRate);
        }

        private void B_UpdatePorts_Click(object sender, RoutedEventArgs e) // Обновление портов переделать!
        {
            string v_NamePort = "";
            int v_index;
            if ((v_index = CB_NamePort.SelectedIndex) != -1) 
                v_NamePort = CB_NamePort.Items[v_index].ToString();
            foreach (var v_n in C_PortOptions.F_GetPortNames())
                CB_NamePort.Items.Add(v_n);
            if (C_PortOptions.F_GetPortNames().Count() != 0)
                CB_NamePort.SelectedIndex = 0;
            if (C_PortOptions.F_GetPortNames().Contains(v_NamePort))
                CB_NamePort.SelectedIndex = Array.FindIndex(C_PortOptions.F_GetPortNames(), x => x == v_NamePort);
        }
    }
}
