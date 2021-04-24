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

namespace Browser
{
    public partial class Form1 : Form
    {
        private Socket socket = null;
        
        public string address = "http://localhost:5000/";
        public Uri uri = new Uri("http://localhost:5000/");

        public List<Uri> Uris = new List<Uri>();
        public int index = 0;

        public Form1()
        {
            InitializeComponent();

            webBrowser1.ScriptErrorsSuppressed = true;

            connectToUri(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.LocalPath != "blank")
            {
                uri = new Uri(address + e.Url.AbsolutePath.Substring(1, e.Url.AbsolutePath.Length - 1));

                connectToUri(true);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void connectToUri(bool inc)
        {
            if (inc)
            {
                Uris.Add(uri);
                index += 1;
            }

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(uri.Host, uri.Port);

            string str = "GET " + uri.PathAndQuery + " HTTP/1.1\r\n" + "Host: " + uri.Host + "\r\n\r\n";

            socket.Send(Encoding.ASCII.GetBytes(str));

            byte[] bytes = new byte[4096];

            _ = socket.Receive(bytes);

            var str1 = Encoding.ASCII.GetString(bytes);

            str1 = str1.Substring(str1.IndexOf('<'), str1.Length - 1 - str1.IndexOf('<'));

            webBrowser1.DocumentText = str1;
            socket.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (index > 1)
            {
                index -= 1;

                uri = Uris[index - 1];

                connectToUri(false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (index < Uris.Count)
            {
                index += 1;

                uri = Uris[index - 1];

                connectToUri(false);
            }
        }
    }
}
