using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Device;
using System.Device.Location;
using System.Net;
using System.IO;

namespace FC
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_GetLocation_Click(object sender, EventArgs e)
        {
            System.Net.IPHostEntry myEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            string ipAddress = myEntry.AddressList[0].ToString();
            this.Text = ipAddress;
        }
        public string GetInerIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
               if (ip.AddressFamily.ToString() == "InterNetwork")
               {
                 localIP = ip.ToString();
                  break;
                }
            }
            return localIP;
        }
        public string GetOutIp()
        {
            string direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
              direction = stream.ReadToEnd();
             }
            int first = direction.IndexOf("Address:") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);
            return direction;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = $@"";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetOutIp());
        }

        private void eventLog1_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }

        private void btn_OpenBrowswer_Click(object sender, EventArgs e)
        {
            Browswer bs = new Browswer();
            bs.Show();
        }
    }
}
