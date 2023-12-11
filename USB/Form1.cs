using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btn_getUsbInfo_Click(object sender, EventArgs e)
        {
            this.txt_UsbInfo.Clear();
            List<string> Ulist =  USBHelper.GetAllUsbDevInfo();
            foreach (string s in Ulist)
            {
                this.txt_UsbInfo.AppendText(s+Environment.NewLine);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.txt_UsbInfo.Clear();
            List<string> Uplist = USBHelper.GetAllInfo();
            foreach(string s in Uplist)
                this.txt_UsbInfo.AppendText(s + Environment.NewLine);
        }

        private void btn_getAllinfo_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            USBTool.GetAllUsbDevice(ref list);
            foreach (string s in list)
            {
                this.txt_UsbInfo.AppendText(s + Environment.NewLine);
            }
        }

        private void btn_linkUsb_Click(object sender, EventArgs e)
        {
            USBTool uTool = new USBTool();
            uTool.OpenUsbDevice(0x0471,0x8E92);
            uTool.Send("FORMFEED" + Environment.NewLine);
        }
    }
}
