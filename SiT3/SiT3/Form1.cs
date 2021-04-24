using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiT3
{
    public partial class Form1 : Form
    {
        private string link = null;
        
        private Socket active = null;
        private Socket passive = null;

        private byte[] bytes = null;
        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
            active = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            passive = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            active.Connect("127.0.0.1", 21);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bytes = new byte[256];
            var bytes2 = new byte[1024];

            if (textBox1.Text != "") active.Send(Encoding.ASCII.GetBytes(textBox1.Text + "\r\n"));
            if (passive.Connected) _ = passive.Receive(bytes2);

            _ = active.Receive(bytes);

            var rcv = Encoding.ASCII.GetString(bytes);

            if (rcv.Substring(0, 3) == "227") openPassive(rcv);

            label1.Text += rcv + "\n";
            label2.Text = Encoding.ASCII.GetString(bytes2);
        }

        private void openPassive(string rcv)
        {
            rcv = rcv.Substring(rcv.IndexOf('(') + 1, (rcv.IndexOf(')') - rcv.IndexOf('(') - 1));
            var strs = rcv.Split(',');

            passive.Connect(strs[0] + "." + strs[1] + "." + strs[2] + "." + strs[3], Convert.ToInt32(strs[4])*256 + Convert.ToInt32(strs[5]));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            link = textBox1.Text;
        }
    }
}
