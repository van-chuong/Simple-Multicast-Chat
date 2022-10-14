using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multicast
{
    public partial class Sender : Form
    {
        public Sender()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        Socket send;
        IPAddress ip;
        public void Connect()
        {
            send = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ip = IPAddress.Parse("224.0.0.1");
            send.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));
            send.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            IPEndPoint ipep = new IPEndPoint(ip, 9999);
            send.Connect(ipep);
            
        }
        public void SendMessage(string message)
        {

            byte[] senddata = Encoding.UTF8.GetBytes(message);
            send.Send(senddata, senddata.Length, SocketFlags.None);
        }

        private void Sender_FormClosed(object sender, FormClosedEventArgs e)
        {
            send.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPEndPoint endPoint = send.LocalEndPoint as IPEndPoint;
            string ipAddress = endPoint.Address.ToString();
            string mess = ipAddress + ": " + textBox1.Text.Trim();
            SendMessage(mess);
            listView1.Items.Add(new ListViewItem() { Text = mess });
        }
    }
}
