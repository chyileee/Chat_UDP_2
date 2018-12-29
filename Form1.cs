//https://youtu.be/ObcGBT4ZWEU
//C# Tutorial 78: How to make a Chat Program in C# Part 1234
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
namespace Chat_Client_App
{
    public partial class Form1 : Form
    {
        Socket sck;
        EndPoint epLocal, epRemote;
        Boolean show_flag = false;
        public Form1()
        {
            InitializeComponent();
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            textLocalIP.Text = GetLocalIP();
            textFriendsIP.Text = GetLocalIP();
            textLocalPort.Text = "8000";
            textFriendsPort.Text = "8001";
        }

        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {
                if(show_flag == false)
                {
                    string str1 = sck.RemoteEndPoint.ToString();
                    //MessageBox.Show(str1);
                    listMessage.Items.Add("RemoteEndPoint: " + str1);
                    show_flag = true;
                }
                int size = sck.EndReceiveFrom(aResult, ref epRemote);
                if (size > 0)
                {
                    byte[] receiveData = new byte[1464];
                    receiveData = (byte[])aResult.AsyncState;
                    ASCIIEncoding eEncoding = new ASCIIEncoding();
                    string receiveMessage = eEncoding.GetString(receiveData);
                    //listMessage.Items.Add("Friends: " + receiveMessage);
                    listMessage.Items.Add("Rx: " + receiveMessage);
                }
                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//Start
        {
            try
            {
                epLocal = new IPEndPoint(IPAddress.Parse(textLocalIP.Text), Convert.ToInt32(textLocalPort.Text));
                sck.Bind(epLocal);

                epRemote = new IPEndPoint(IPAddress.Parse(textFriendsIP.Text), Convert.ToInt32(textFriendsPort.Text));
                sck.Connect(epRemote);
                
                byte[] buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);

                button1.Text = "Connected";
                button1.Enabled = false;
                button2.Enabled = true;
                textMessage.Focus();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void textFriendsPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ex;
            ex = textLocalPort.Text;
            textLocalPort.Text = textFriendsPort.Text;// "8001";
            textFriendsPort.Text = ex;// "8000";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                sck.Disconnect(true);
                if (sck.Connected)
                    listMessage.Items.Add("We're still connnected");
                else
                    listMessage.Items.Add("We're disconnected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void button2_Click(object sender, EventArgs e)//Send 
        {
            try
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] msg = new byte[1500];
                msg = enc.GetBytes(textMessage.Text);
 
                sck.Send(msg);

                //listMessage.Items.Add("You:" + textMessage.Text);
                listMessage.Items.Add("Tx: " + textMessage.Text);
                //textMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }


        }
    }
}
