namespace Do_An_BM
{
    partial class frmAuditMonitor
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabAuditLog = new System.Windows.Forms.TabPage();
            this.btnVerifySignature = new System.Windows.Forms.Button();
            this.btnRefreshAudit = new System.Windows.Forms.Button();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvAuditLog = new System.Windows.Forms.DataGridView();
            this.tabFGA = new System.Windows.Forms.TabPage();
            this.btnRefreshFGA = new System.Windows.Forms.Button();
            this.dgvFGA = new System.Windows.Forms.DataGridView();
            this.tabVPD = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvPolicies = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblMANV = new System.Windows.Forms.Label();
            this.lblMAKH = new System.Windows.Forms.Label();
            this.lblUserType = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabFlashback = new System.Windows.Forms.TabPage();
            this.btnRestore = new System.Windows.Forms.Button();
            this.nudMinutes = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRecordID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabAuditLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLog)).BeginInit();
            this.tabFGA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFGA)).BeginInit();
            this.tabVPD.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPolicies)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabFlashback.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).BeginInit();
            this.SuspendLayout();
            //
            // tabControl1
            //
            this.tabControl1.Controls.Add(this.tabAuditLog);
            this.tabControl1.Controls.Add(this.tabFGA);
            this.tabControl1.Controls.Add(this.tabVPD);
            this.tabControl1.Controls.Add(this.tabFlashback);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tabControl1.Location = new System.Drawing.Point(12, 70);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1160, 500);
            this.tabControl1.TabIndex = 0;
            //
            // tabAuditLog
            //
            this.tabAuditLog.Controls.Add(this.btnVerifySignature);
            this.tabAuditLog.Controls.Add(this.btnRefreshAudit);
            this.tabAuditLog.Controls.Add(this.dtpEnd);
            this.tabAuditLog.Controls.Add(this.dtpStart);
            this.tabAuditLog.Controls.Add(this.label2);
            this.tabAuditLog.Controls.Add(this.label1);
            this.tabAuditLog.Controls.Add(this.dgvAuditLog);
            this.tabAuditLog.Location = new System.Drawing.Point(4, 25);
            this.tabAuditLog.Name = "tabAuditLog";
            this.tabAuditLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuditLog.Size = new System.Drawing.Size(1152, 471);
            this.tabAuditLog.TabIndex = 0;
            this.tabAuditLog.Text = "Audit Log";
            this.tabAuditLog.UseVisualStyleBackColor = true;
            //
            // btnVerifySignature
            //
            this.btnVerifySignature.Location = new System.Drawing.Point(950, 15);
            this.btnVerifySignature.Name = "btnVerifySignature";
            this.btnVerifySignature.Size = new System.Drawing.Size(180, 35);
            this.btnVerifySignature.TabIndex = 6;
            this.btnVerifySignature.Text = "Verify Signature";
            this.btnVerifySignature.UseVisualStyleBackColor = true;
            this.btnVerifySignature.Click += new System.EventHandler(this.btnVerifySignature_Click);
            //
            // btnRefreshAudit
            //
            this.btnRefreshAudit.Location = new System.Drawing.Point(750, 15);
            this.btnRefreshAudit.Name = "btnRefreshAudit";
            this.btnRefreshAudit.Size = new System.Drawing.Size(180, 35);
            this.btnRefreshAudit.TabIndex = 5;
            this.btnRefreshAudit.Text = "Làm mới";
            this.btnRefreshAudit.UseVisualStyleBackColor = true;
            this.btnRefreshAudit.Click += new System.EventHandler(this.btnRefreshAudit_Click);
            //
            // dtpEnd
            //
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(530, 20);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(180, 23);
            this.dtpEnd.TabIndex = 4;
            //
            // dtpStart
            //
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(230, 20);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(180, 23);
            this.dtpStart.TabIndex = 3;
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(450, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Đến ngày:";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(150, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Từ ngày:";
            //
            // dgvAuditLog
            //
            this.dgvAuditLog.AllowUserToAddRows = false;
            this.dgvAuditLog.AllowUserToDeleteRows = false;
            this.dgvAuditLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAuditLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuditLog.Location = new System.Drawing.Point(15, 60);
            this.dgvAuditLog.Name = "dgvAuditLog";
            this.dgvAuditLog.ReadOnly = true;
            this.dgvAuditLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAuditLog.Size = new System.Drawing.Size(1120, 390);
            this.dgvAuditLog.TabIndex = 0;
            //
            // tabFGA
            //
            this.tabFGA.Controls.Add(this.btnRefreshFGA);
            this.tabFGA.Controls.Add(this.dgvFGA);
            this.tabFGA.Location = new System.Drawing.Point(4, 25);
            this.tabFGA.Name = "tabFGA";
            this.tabFGA.Padding = new System.Windows.Forms.Padding(3);
            this.tabFGA.Size = new System.Drawing.Size(1152, 471);
            this.tabFGA.TabIndex = 1;
            this.tabFGA.Text = "Fine-Grained Audit";
            this.tabFGA.UseVisualStyleBackColor = true;
            //
            // btnRefreshFGA
            //
            this.btnRefreshFGA.Location = new System.Drawing.Point(950, 15);
            this.btnRefreshFGA.Name = "btnRefreshFGA";
            this.btnRefreshFGA.Size = new System.Drawing.Size(180, 35);
            this.btnRefreshFGA.TabIndex = 1;
            this.btnRefreshFGA.Text = "Làm mới";
            this.btnRefreshFGA.UseVisualStyleBackColor = true;
            this.btnRefreshFGA.Click += new System.EventHandler(this.btnRefreshFGA_Click);
            //
            // dgvFGA
            //
            this.dgvFGA.AllowUserToAddRows = false;
            this.dgvFGA.AllowUserToDeleteRows = false;
            this.dgvFGA.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvFGA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFGA.Location = new System.Drawing.Point(15, 60);
            this.dgvFGA.Name = "dgvFGA";
            this.dgvFGA.ReadOnly = true;
            this.dgvFGA.Size = new System.Drawing.Size(1120, 390);
            this.dgvFGA.TabIndex = 0;
            //
            // tabVPD
            //
            this.tabVPD.Controls.Add(this.groupBox2);
            this.tabVPD.Controls.Add(this.groupBox1);
            this.tabVPD.Location = new System.Drawing.Point(4, 25);
            this.tabVPD.Name = "tabVPD";
            this.tabVPD.Size = new System.Drawing.Size(1152, 471);
            this.tabVPD.TabIndex = 2;
            this.tabVPD.Text = "VPD Status";
            this.tabVPD.UseVisualStyleBackColor = true;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.dgvPolicies);
            this.groupBox2.Location = new System.Drawing.Point(15, 200);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1120, 250);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Active Policies";
            //
            // dgvPolicies
            //
            this.dgvPolicies.AllowUserToAddRows = false;
            this.dgvPolicies.AllowUserToDeleteRows = false;
            this.dgvPolicies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPolicies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPolicies.Location = new System.Drawing.Point(15, 30);
            this.dgvPolicies.Name = "dgvPolicies";
            this.dgvPolicies.ReadOnly = true;
            this.dgvPolicies.Size = new System.Drawing.Size(1090, 200);
            this.dgvPolicies.TabIndex = 0;
            //
            // groupBox1
            //
            this.groupBox1.Controls.Add(this.lblMANV);
            this.groupBox1.Controls.Add(this.lblMAKH);
            this.groupBox1.Controls.Add(this.lblUserType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(15, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1120, 160);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Current Context";
            //
            // lblMANV
            //
            this.lblMANV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblMANV.ForeColor = System.Drawing.Color.Blue;
            this.lblMANV.Location = new System.Drawing.Point(200, 110);
            this.lblMANV.Name = "lblMANV";
            this.lblMANV.Size = new System.Drawing.Size(900, 30);
            this.lblMANV.TabIndex = 5;
            this.lblMANV.Text = "N/A";
            //
            // lblMAKH
            //
            this.lblMAKH.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblMAKH.ForeColor = System.Drawing.Color.Blue;
            this.lblMAKH.Location = new System.Drawing.Point(200, 70);
            this.lblMAKH.Name = "lblMAKH";
            this.lblMAKH.Size = new System.Drawing.Size(900, 30);
            this.lblMAKH.TabIndex = 4;
            this.lblMAKH.Text = "N/A";
            //
            // lblUserType
            //
            this.lblUserType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblUserType.ForeColor = System.Drawing.Color.Green;
            this.lblUserType.Location = new System.Drawing.Point(200, 30);
            this.lblUserType.Name = "lblUserType";
            this.lblUserType.Size = new System.Drawing.Size(900, 30);
            this.lblUserType.TabIndex = 3;
            this.lblUserType.Text = "N/A";
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "MANV:";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "MAKH:";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "USER_TYPE:";
            //
            // tabFlashback
            //
            this.tabFlashback.Controls.Add(this.btnRestore);
            this.tabFlashback.Controls.Add(this.nudMinutes);
            this.tabFlashback.Controls.Add(this.label8);
            this.tabFlashback.Controls.Add(this.txtRecordID);
            this.tabFlashback.Controls.Add(this.label7);
            this.tabFlashback.Controls.Add(this.cboTable);
            this.tabFlashback.Controls.Add(this.label6);
            this.tabFlashback.Location = new System.Drawing.Point(4, 25);
            this.tabFlashback.Name = "tabFlashback";
            this.tabFlashback.Size = new System.Drawing.Size(1152, 471);
            this.tabFlashback.TabIndex = 3;
            this.tabFlashback.Text = "Flashback Recovery";
            this.tabFlashback.UseVisualStyleBackColor = true;
            //
            // btnRestore
            //
            this.btnRestore.BackColor = System.Drawing.Color.Green;
            this.btnRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnRestore.ForeColor = System.Drawing.Color.White;
            this.btnRestore.Location = new System.Drawing.Point(450, 250);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(250, 50);
            this.btnRestore.TabIndex = 6;
            this.btnRestore.Text = "Phục hồi dữ liệu";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            //
            // nudMinutes
            //
            this.nudMinutes.Location = new System.Drawing.Point(400, 180);
            this.nudMinutes.Maximum = new decimal(new int[] {
1440,
0,
0,
0});
            this.nudMinutes.Minimum = new decimal(new int[] {
1,
0,
0,
0});
            this.nudMinutes.Name = "nudMinutes";
            this.nudMinutes.Size = new System.Drawing.Size(300, 23);
            this.nudMinutes.TabIndex = 5;
            this.nudMinutes.Value = new decimal(new int[] {
60,
0,
0,
0});
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 182);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 17);
            this.label8.TabIndex = 4;
            this.label8.Text = "Phút trước đây:";
            //
            // txtRecordID
            //
            this.txtRecordID.Location = new System.Drawing.Point(400, 130);
            this.txtRecordID.Name = "txtRecordID";
            this.txtRecordID.Size = new System.Drawing.Size(300, 23);
            this.txtRecordID.TabIndex = 3;
            //
            // label7
            //
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(250, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "ID cần phục hồi:";
            //
            // cboTable
            //
            this.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(400, 80);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(300, 24);
            this.cboTable.TabIndex = 1;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Chọn bảng:";
            //
            // lblTitle
            //
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1184, 60);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "GIÁM SÁT & AUDIT (FORM 5)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnClose
            //
            this.btnClose.Location = new System.Drawing.Point(1072, 580);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // frmAuditMonitor
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 626);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmAuditMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Giám sát & Audit - FORM 5";
            this.Load += new System.EventHandler(this.frmAuditMonitor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabAuditLog.ResumeLayout(false);
            this.tabAuditLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditLog)).EndInit();
            this.tabFGA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFGA)).EndInit();
            this.tabVPD.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPolicies)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabFlashback.ResumeLayout(false);
            this.tabFlashback.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).EndInit();
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabAuditLog;
        private System.Windows.Forms.TabPage tabFGA;
        private System.Windows.Forms.TabPage tabVPD;
        private System.Windows.Forms.TabPage tabFlashback;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DataGridView dgvAuditLog;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Button btnRefreshAudit;
        private System.Windows.Forms.Button btnVerifySignature;
        private System.Windows.Forms.DataGridView dgvFGA;
        private System.Windows.Forms.Button btnRefreshFGA;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMANV;
        private System.Windows.Forms.Label lblMAKH;
        private System.Windows.Forms.Label lblUserType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvPolicies;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.TextBox txtRecordID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudMinutes;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnClose;
    }
}