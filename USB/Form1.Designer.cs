namespace USB
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_getUsbInfo = new System.Windows.Forms.Button();
            this.txt_UsbInfo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_getAllinfo = new System.Windows.Forms.Button();
            this.btn_linkUsb = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_getUsbInfo
            // 
            this.btn_getUsbInfo.Location = new System.Drawing.Point(312, 27);
            this.btn_getUsbInfo.Name = "btn_getUsbInfo";
            this.btn_getUsbInfo.Size = new System.Drawing.Size(75, 23);
            this.btn_getUsbInfo.TabIndex = 1;
            this.btn_getUsbInfo.Text = "button2";
            this.btn_getUsbInfo.UseVisualStyleBackColor = true;
            this.btn_getUsbInfo.Click += new System.EventHandler(this.btn_getUsbInfo_Click);
            // 
            // txt_UsbInfo
            // 
            this.txt_UsbInfo.Location = new System.Drawing.Point(12, 56);
            this.txt_UsbInfo.Multiline = true;
            this.txt_UsbInfo.Name = "txt_UsbInfo";
            this.txt_UsbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_UsbInfo.Size = new System.Drawing.Size(776, 382);
            this.txt_UsbInfo.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(231, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_getAllinfo
            // 
            this.btn_getAllinfo.Location = new System.Drawing.Point(393, 27);
            this.btn_getAllinfo.Name = "btn_getAllinfo";
            this.btn_getAllinfo.Size = new System.Drawing.Size(75, 23);
            this.btn_getAllinfo.TabIndex = 1;
            this.btn_getAllinfo.Text = "button3";
            this.btn_getAllinfo.UseVisualStyleBackColor = true;
            this.btn_getAllinfo.Click += new System.EventHandler(this.btn_getAllinfo_Click);
            // 
            // btn_linkUsb
            // 
            this.btn_linkUsb.Location = new System.Drawing.Point(475, 26);
            this.btn_linkUsb.Name = "btn_linkUsb";
            this.btn_linkUsb.Size = new System.Drawing.Size(75, 23);
            this.btn_linkUsb.TabIndex = 4;
            this.btn_linkUsb.Text = "link";
            this.btn_linkUsb.UseVisualStyleBackColor = true;
            this.btn_linkUsb.Click += new System.EventHandler(this.btn_linkUsb_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_linkUsb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_UsbInfo);
            this.Controls.Add(this.btn_getAllinfo);
            this.Controls.Add(this.btn_getUsbInfo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_getUsbInfo;
        private System.Windows.Forms.TextBox txt_UsbInfo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btn_getAllinfo;
        private System.Windows.Forms.Button btn_linkUsb;
    }
}

