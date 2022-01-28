namespace FTPClient
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConnectChange = new System.Windows.Forms.Button();
            this.lbServer = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.fileBrowser = new FTPClient.Library.Controls.FileBrowser();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnConnectChange);
            this.panel1.Controls.Add(this.lbServer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 34);
            this.panel1.TabIndex = 0;
            // 
            // btnConnectChange
            // 
            this.btnConnectChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnConnectChange.FlatAppearance.BorderSize = 0;
            this.btnConnectChange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnConnectChange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnConnectChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnectChange.Location = new System.Drawing.Point(539, 0);
            this.btnConnectChange.Name = "btnConnectChange";
            this.btnConnectChange.Size = new System.Drawing.Size(182, 34);
            this.btnConnectChange.TabIndex = 2;
            this.btnConnectChange.Text = "Connect/Change server";
            this.btnConnectChange.UseVisualStyleBackColor = false;
            this.btnConnectChange.Click += new System.EventHandler(this.btnConnectChange_Click);
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Location = new System.Drawing.Point(12, 10);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(196, 15);
            this.lbServer.TabIndex = 0;
            this.lbServer.Text = "Server: (unknown), User: (unknown)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnShowLog);
            this.panel2.Controls.Add(this.lbStatus);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 404);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(721, 29);
            this.panel2.TabIndex = 1;
            // 
            // btnShowLog
            // 
            this.btnShowLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShowLog.FlatAppearance.BorderSize = 0;
            this.btnShowLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnShowLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnShowLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowLog.Location = new System.Drawing.Point(611, 0);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(110, 29);
            this.btnShowLog.TabIndex = 2;
            this.btnShowLog.Text = "Show log";
            this.btnShowLog.UseVisualStyleBackColor = false;
            this.btnShowLog.Click += new System.EventHandler(this.btnShowLog_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(6, 7);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(26, 15);
            this.lbStatus.TabIndex = 0;
            this.lbStatus.Text = "Idle";
            // 
            // fileBrowser
            // 
            this.fileBrowser.BackColor = System.Drawing.Color.White;
            this.fileBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileBrowser.Enabled = false;
            this.fileBrowser.Location = new System.Drawing.Point(0, 34);
            this.fileBrowser.Name = "fileBrowser";
            this.fileBrowser.Path = "/";
            this.fileBrowser.Size = new System.Drawing.Size(721, 370);
            this.fileBrowser.TabIndex = 2;
            this.fileBrowser.TaskRequested += new System.EventHandler<FTPClient.Library.Controls.FileBrowserTaskEventArgs>(this.fileBrowser_TaskRequested);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(721, 433);
            this.Controls.Add(this.fileBrowser);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "FTP Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbServer;
        private System.Windows.Forms.Button btnConnectChange;
        private System.Windows.Forms.Button btnShowLog;
        private Library.Controls.FileBrowser fileBrowser;
    }
}
