namespace FTPClient.Library.Controls
{
    partial class FileBrowser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pAddress = new System.Windows.Forms.Panel();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.pButton = new System.Windows.Forms.Panel();
            this.btnNewFolder = new System.Windows.Forms.Button();
            this.btnShowLog = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRename = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.listView = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.chType = new System.Windows.Forms.ColumnHeader();
            this.chSize = new System.Windows.Forms.ColumnHeader();
            this.chDateModified = new System.Windows.Forms.ColumnHeader();
            this.chPermission = new System.Windows.Forms.ColumnHeader();
            this.loadingControls1 = new FTPClient.Library.Controls.LoadingControls();
            this.pAddress.SuspendLayout();
            this.pButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pAddress
            // 
            this.pAddress.Controls.Add(this.tbPath);
            this.pAddress.Controls.Add(this.btnRefresh);
            this.pAddress.Controls.Add(this.btnBack);
            this.pAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.pAddress.Dock = System.Windows.Forms.DockStyle.Top;
            this.pAddress.Location = new System.Drawing.Point(0, 0);
            this.pAddress.Name = "pAddress";
            this.pAddress.Size = new System.Drawing.Size(768, 30);
            this.pAddress.TabIndex = 0;
            this.pAddress.Click += new System.EventHandler(this.pAddress_Click);
            // 
            // tbPath
            // 
            this.tbPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPath.Location = new System.Drawing.Point(32, 7);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(703, 16);
            this.tbPath.TabIndex = 5;
            this.tbPath.Text = "/";
            this.tbPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPath_KeyDown);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRefresh.Location = new System.Drawing.Point(738, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(30, 30);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnBack
            // 
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Segoe MDL2 Assets", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnBack.Location = new System.Drawing.Point(0, 0);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(30, 30);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pButton
            // 
            this.pButton.Controls.Add(this.btnNewFolder);
            this.pButton.Controls.Add(this.btnShowLog);
            this.pButton.Controls.Add(this.btnDelete);
            this.pButton.Controls.Add(this.btnRename);
            this.pButton.Controls.Add(this.btnDownload);
            this.pButton.Controls.Add(this.btnUpload);
            this.pButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.pButton.Location = new System.Drawing.Point(0, 30);
            this.pButton.Name = "pButton";
            this.pButton.Size = new System.Drawing.Size(768, 30);
            this.pButton.TabIndex = 1;
            // 
            // btnNewFolder
            // 
            this.btnNewFolder.FlatAppearance.BorderSize = 0;
            this.btnNewFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnNewFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnNewFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewFolder.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNewFolder.Location = new System.Drawing.Point(200, 0);
            this.btnNewFolder.Name = "btnNewFolder";
            this.btnNewFolder.Size = new System.Drawing.Size(100, 30);
            this.btnNewFolder.TabIndex = 0;
            this.btnNewFolder.Text = " New folder";
            this.btnNewFolder.UseVisualStyleBackColor = true;
            this.btnNewFolder.Click += new System.EventHandler(this.btnNewFolder_Click);
            // 
            // btnShowLog
            // 
            this.btnShowLog.FlatAppearance.BorderSize = 0;
            this.btnShowLog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnShowLog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnShowLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowLog.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnShowLog.Location = new System.Drawing.Point(500, 0);
            this.btnShowLog.Name = "btnShowLog";
            this.btnShowLog.Size = new System.Drawing.Size(100, 30);
            this.btnShowLog.TabIndex = 0;
            this.btnShowLog.Text = " Show log";
            this.btnShowLog.UseVisualStyleBackColor = true;
            this.btnShowLog.Visible = false;
            this.btnShowLog.Click += new System.EventHandler(this.btnShowLog_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnDelete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDelete.Location = new System.Drawing.Point(400, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 30);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = " Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRename
            // 
            this.btnRename.FlatAppearance.BorderSize = 0;
            this.btnRename.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnRename.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnRename.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRename.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRename.Location = new System.Drawing.Point(300, 0);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(100, 30);
            this.btnRename.TabIndex = 0;
            this.btnRename.Text = " Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.FlatAppearance.BorderSize = 0;
            this.btnDownload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnDownload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDownload.Location = new System.Drawing.Point(100, 0);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 30);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = " Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.FlatAppearance.BorderSize = 0;
            this.btnUpload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.btnUpload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpload.Font = new System.Drawing.Font("Segoe MDL2 Assets", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnUpload.Location = new System.Drawing.Point(0, 0);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 30);
            this.btnUpload.TabIndex = 0;
            this.btnUpload.Text = " Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chType,
            this.chSize,
            this.chDateModified,
            this.chPermission});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 60);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(768, 407);
            this.listView.TabIndex = 2;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ItemActivate += new System.EventHandler(this.listView_ItemActivate);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 240;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 100;
            // 
            // chSize
            // 
            this.chSize.Text = "Size";
            this.chSize.Width = 100;
            // 
            // chDateModified
            // 
            this.chDateModified.Text = "Date modified";
            this.chDateModified.Width = 150;
            // 
            // chPermission
            // 
            this.chPermission.Text = "Permission";
            this.chPermission.Width = 100;
            // 
            // loadingControls1
            // 
            this.loadingControls1.BackColor = System.Drawing.Color.White;
            this.loadingControls1.DarkMode = false;
            this.loadingControls1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadingControls1.Location = new System.Drawing.Point(0, 60);
            this.loadingControls1.Name = "loadingControls1";
            this.loadingControls1.Size = new System.Drawing.Size(768, 407);
            this.loadingControls1.TabIndex = 1;
            // 
            // FileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.listView);
            this.Controls.Add(this.loadingControls1);
            this.Controls.Add(this.pButton);
            this.Controls.Add(this.pAddress);
            this.Name = "FileBrowser";
            this.Size = new System.Drawing.Size(768, 467);
            this.pAddress.ResumeLayout(false);
            this.pAddress.PerformLayout();
            this.pButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pAddress;
        private System.Windows.Forms.Panel pButton;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chDateModified;
        private System.Windows.Forms.ColumnHeader chPermission;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnNewFolder;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnShowLog;
        private System.Windows.Forms.Button btnDelete;
        private LoadingControls loadingControls1;
    }
}
