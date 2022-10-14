using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Receiver
{
    public partial class Receiver : Form
    {
        public Receiver()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();

        }
        Socket reveiver;
        IPEndPoint ipep;
        public void Connect()
        {
            ipep = new IPEndPoint(IPAddress.Any, 9999);
            reveiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress ip = IPAddress.Parse("224.0.0.1");
            reveiver.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Any));
            try
            {
                    reveiver.Bind(ipep);
            }
            catch
            {
                ipep = new IPEndPoint(IPAddress.Any, 9999);
                reveiver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start();
                    }
                }
                catch (Exception e)
                {

                }
            });
            listen.IsBackground = true;
            listen.Start();
        }
        public void Receive()
        {
            while (true)
            {
                byte[] recv = new byte[1024];
                reveiver.Receive(recv);
                string str = Encoding.UTF8.GetString(recv, 0, recv.Length);
                listView1.Items.Add(new ListViewItem() { Text = str });
            }
            
        }
        private void Receiver_FormClosed(object sender, FormClosedEventArgs e)
        {
            reveiver.Close();
        }
    }
}
