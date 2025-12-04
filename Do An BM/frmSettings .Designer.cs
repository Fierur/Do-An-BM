namespace Do_An_BM
{
    partial class frmSettings
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRegenerateRSA = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnClearAuditLog = new System.Windows.Forms.Button();
            this.nudDays = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnBackupDB = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkEnableVPD = new System.Windows.Forms.CheckBox();
            this.chkEnableFGA = new System.Windows.Forms.CheckBox();
            this.chkEnableAudit = new System.Windows.Forms.CheckBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDays)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SteelBlue;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 60);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "CÀI ĐẶT HỆ THỐNG";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 60);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(900, 400);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(892, 374);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Bảo mật & Sao lưu";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRegenerateRSA);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(850, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RSA Keys";
            // 
            // btnRegenerateRSA
            // 
            this.btnRegenerateRSA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnRegenerateRSA.Location = new System.Drawing.Point(20, 50);
            this.btnRegenerateRSA.Name = "btnRegenerateRSA";
            this.btnRegenerateRSA.Size = new System.Drawing.Size(200, 35);
            this.btnRegenerateRSA.TabIndex = 1;
            this.btnRegenerateRSA.Text = "Regenerate RSA Keys";
            this.btnRegenerateRSA.UseVisualStyleBackColor = true;
            this.btnRegenerateRSA.Click += new System.EventHandler(this.btnRegenerateRSA_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label2.Location = new System.Drawing.Point(20, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(500, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tạo lại cặp khóa RSA mới. Lưu ý: Cần mã hóa lại toàn bộ dữ liệu đã mã hóa RSA!";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClearAuditLog);
            this.groupBox2.Controls.Add(this.nudDays);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(20, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(850, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Audit Log";
            // 
            // btnClearAuditLog
            // 
            this.btnClearAuditLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClearAuditLog.Location = new System.Drawing.Point(20, 50);
            this.btnClearAuditLog.Name = "btnClearAuditLog";
            this.btnClearAuditLog.Size = new System.Drawing.Size(200, 35);
            this.btnClearAuditLog.TabIndex = 2;
            this.btnClearAuditLog.Text = "Clear Audit Log";
            this.btnClearAuditLog.UseVisualStyleBackColor = true;
            this.btnClearAuditLog.Click += new System.EventHandler(this.btnClearAuditLog_Click);
            // 
            // nudDays
            // 
            this.nudDays.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.nudDays.Location = new System.Drawing.Point(300, 55);
            this.nudDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.nudDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDays.Name = "nudDays";
            this.nudDays.Size = new System.Drawing.Size(100, 23);
            this.nudDays.TabIndex = 1;
            this.nudDays.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(20, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(400, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Xóa các bản ghi Audit Log cũ hơn (ngày):";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnBackupDB);
            this.groupBox3.Location = new System.Drawing.Point(20, 250);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(850, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Database";
            // 
            // btnBackupDB
            // 
            this.btnBackupDB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBackupDB.Location = new System.Drawing.Point(20, 40);
            this.btnBackupDB.Name = "btnBackupDB";
            this.btnBackupDB.Size = new System.Drawing.Size(200, 35);
            this.btnBackupDB.TabIndex = 0;
            this.btnBackupDB.Text = "Backup Database";
            this.btnBackupDB.UseVisualStyleBackColor = true;
            this.btnBackupDB.Click += new System.EventHandler(this.btnBackupDB_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(892, 374);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Cấu hình hệ thống";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSaveSettings);
            this.groupBox4.Controls.Add(this.chkEnableAudit);
            this.groupBox4.Controls.Add(this.chkEnableFGA);
            this.groupBox4.Controls.Add(this.chkEnableVPD);
            this.groupBox4.Location = new System.Drawing.Point(20, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(850, 150);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Tính năng bảo mật";
            // 
            // chkEnableVPD
            // 
            this.chkEnableVPD.AutoSize = true;
            this.chkEnableVPD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.chkEnableVPD.Location = new System.Drawing.Point(20, 30);
            this.chkEnableVPD.Name = "chkEnableVPD";
            this.chkEnableVPD.Size = new System.Drawing.Size(212, 21);
            this.chkEnableVPD.TabIndex = 0;
            this.chkEnableVPD.Text = "Kích hoạt VPD (Row Security)";
            this.chkEnableVPD.UseVisualStyleBackColor = true;
            // 
            // chkEnableFGA
            // 
            this.chkEnableFGA.AutoSize = true;
            this.chkEnableFGA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.chkEnableFGA.Location = new System.Drawing.Point(20, 60);
            this.chkEnableFGA.Name = "chkEnableFGA";
            this.chkEnableFGA.Size = new System.Drawing.Size(244, 21);
            this.chkEnableFGA.TabIndex = 1;
            this.chkEnableFGA.Text = "Kích hoạt FGA (Fine-Grained Audit)";
            this.chkEnableFGA.UseVisualStyleBackColor = true;
            // 
            // chkEnableAudit
            // 
            this.chkEnableAudit.AutoSize = true;
            this.chkEnableAudit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.chkEnableAudit.Location = new System.Drawing.Point(20, 90);
            this.chkEnableAudit.Name = "chkEnableAudit";
            this.chkEnableAudit.Size = new System.Drawing.Size(147, 21);
            this.chkEnableAudit.TabIndex = 2;
            this.chkEnableAudit.Text = "Kích hoạt Audit Log";
            this.chkEnableAudit.UseVisualStyleBackColor = true;
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSaveSettings.Location = new System.Drawing.Point(300, 90);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(150, 35);
            this.btnSaveSettings.TabIndex = 3;
            this.btnSaveSettings.Text = "Lưu cấu hình";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(750, 470);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(130, 35);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 520);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cài đặt hệ thống";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDays)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnBackupDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearAuditLog;
        private System.Windows.Forms.NumericUpDown nudDays;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRegenerateRSA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.CheckBox chkEnableAudit;
        private System.Windows.Forms.CheckBox chkEnableFGA;
        private System.Windows.Forms.CheckBox chkEnableVPD;
        private System.Windows.Forms.Button btnClose;
    }
}