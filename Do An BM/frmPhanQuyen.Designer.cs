namespace Do_An_BM
{
    partial class frmPhanQuyenOracle
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabProcFunc = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoadObjects = new System.Windows.Forms.Button();
            this.cboObjectType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboOwner = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboObject = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGrantExecToUser = new System.Windows.Forms.Button();
            this.cboTargetUser = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGrantExecToRole = new System.Windows.Forms.Button();
            this.cboTargetRole = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabTablePrivRole = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnLoadTables = new System.Windows.Forms.Button();
            this.cboTableOwner = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnGrantTableToRole = new System.Windows.Forms.Button();
            this.cbDelete = new System.Windows.Forms.CheckBox();
            this.cbUpdate = new System.Windows.Forms.CheckBox();
            this.cbInsert = new System.Windows.Forms.CheckBox();
            this.cbSelect = new System.Windows.Forms.CheckBox();
            this.cboRoleForTable = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabTablePrivUser = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnLoadTables2 = new System.Windows.Forms.Button();
            this.cboTableOwner2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboTable2 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnGrantTableToUser = new System.Windows.Forms.Button();
            this.cbDelete2 = new System.Windows.Forms.CheckBox();
            this.cbUpdate2 = new System.Windows.Forms.CheckBox();
            this.cbInsert2 = new System.Windows.Forms.CheckBox();
            this.cbSelect2 = new System.Windows.Forms.CheckBox();
            this.cboUserForTable = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tabViewPriv = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnRefreshUserPriv = new System.Windows.Forms.Button();
            this.dgvUserPrivileges = new System.Windows.Forms.DataGridView();
            this.cboViewUser = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnRefreshRolePriv = new System.Windows.Forms.Button();
            this.dgvRolePrivileges = new System.Windows.Forms.DataGridView();
            this.cboViewRole = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabProcFunc.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabTablePrivRole.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabTablePrivUser.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabViewPriv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserPrivileges)).BeginInit();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRolePrivileges)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1200, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ PHÂN QUYỀN ORACLE (ADMIN)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserInfo.ForeColor = System.Drawing.Color.Blue;
            this.lblUserInfo.Location = new System.Drawing.Point(12, 50);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(193, 17);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Đăng nhập với: BM_USER";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabProcFunc);
            this.tabControl1.Controls.Add(this.tabTablePrivRole);
            this.tabControl1.Controls.Add(this.tabTablePrivUser);
            this.tabControl1.Controls.Add(this.tabViewPriv);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.tabControl1.Location = new System.Drawing.Point(12, 75);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1176, 520);
            this.tabControl1.TabIndex = 2;
            // 
            // tabProcFunc
            // 
            this.tabProcFunc.Controls.Add(this.groupBox3);
            this.tabProcFunc.Controls.Add(this.groupBox2);
            this.tabProcFunc.Controls.Add(this.groupBox1);
            this.tabProcFunc.Location = new System.Drawing.Point(4, 25);
            this.tabProcFunc.Name = "tabProcFunc";
            this.tabProcFunc.Padding = new System.Windows.Forms.Padding(3);
            this.tabProcFunc.Size = new System.Drawing.Size(1168, 491);
            this.tabProcFunc.TabIndex = 0;
            this.tabProcFunc.Text = "Phân quyền PROC/FUNC/TRIGGER";
            this.tabProcFunc.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadObjects);
            this.groupBox1.Controls.Add(this.cboObjectType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboOwner);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboObject);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(15, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1135, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Chọn Object (PROC/FUNC/TRIGGER)";
            // 
            // btnLoadObjects
            // 
            this.btnLoadObjects.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnLoadObjects.ForeColor = System.Drawing.Color.White;
            this.btnLoadObjects.Location = new System.Drawing.Point(950, 35);
            this.btnLoadObjects.Name = "btnLoadObjects";
            this.btnLoadObjects.Size = new System.Drawing.Size(150, 35);
            this.btnLoadObjects.TabIndex = 6;
            this.btnLoadObjects.Text = "Tải danh sách";
            this.btnLoadObjects.UseVisualStyleBackColor = false;
            this.btnLoadObjects.Click += new System.EventHandler(this.btnLoadObjects_Click);
            // 
            // cboObjectType
            // 
            this.cboObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObjectType.FormattingEnabled = true;
            this.cboObjectType.Location = new System.Drawing.Point(150, 95);
            this.cboObjectType.Name = "cboObjectType";
            this.cboObjectType.Size = new System.Drawing.Size(250, 24);
            this.cboObjectType.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Loại Object:";
            // 
            // cboOwner
            // 
            this.cboOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOwner.FormattingEnabled = true;
            this.cboOwner.Location = new System.Drawing.Point(150, 40);
            this.cboOwner.Name = "cboOwner";
            this.cboOwner.Size = new System.Drawing.Size(250, 24);
            this.cboOwner.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Owner/User:";
            // 
            // cboObject
            // 
            this.cboObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObject.FormattingEnabled = true;
            this.cboObject.Location = new System.Drawing.Point(565, 40);
            this.cboObject.Name = "cboObject";
            this.cboObject.Size = new System.Drawing.Size(350, 24);
            this.cboObject.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(440, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Proc/Func/Trigger:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGrantExecToUser);
            this.groupBox2.Controls.Add(this.cboTargetUser);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(15, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(550, 120);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Phân quyền EXECUTE cho USER";
            // 
            // btnGrantExecToUser
            // 
            this.btnGrantExecToUser.BackColor = System.Drawing.Color.Green;
            this.btnGrantExecToUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnGrantExecToUser.ForeColor = System.Drawing.Color.White;
            this.btnGrantExecToUser.Location = new System.Drawing.Point(350, 60);
            this.btnGrantExecToUser.Name = "btnGrantExecToUser";
            this.btnGrantExecToUser.Size = new System.Drawing.Size(180, 40);
            this.btnGrantExecToUser.TabIndex = 2;
            this.btnGrantExecToUser.Text = "GRANT EXECUTE";
            this.btnGrantExecToUser.UseVisualStyleBackColor = false;
            this.btnGrantExecToUser.Click += new System.EventHandler(this.btnGrantExecToUser_Click);
            // 
            // cboTargetUser
            // 
            this.cboTargetUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTargetUser.FormattingEnabled = true;
            this.cboTargetUser.Location = new System.Drawing.Point(150, 40);
            this.cboTargetUser.Name = "cboTargetUser";
            this.cboTargetUser.Size = new System.Drawing.Size(250, 24);
            this.cboTargetUser.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Chọn USER:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnGrantExecToRole);
            this.groupBox3.Controls.Add(this.cboTargetRole);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(600, 180);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(550, 120);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "3. Phân quyền EXECUTE cho ROLE";
            // 
            // btnGrantExecToRole
            // 
            this.btnGrantExecToRole.BackColor = System.Drawing.Color.Green;
            this.btnGrantExecToRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnGrantExecToRole.ForeColor = System.Drawing.Color.White;
            this.btnGrantExecToRole.Location = new System.Drawing.Point(350, 60);
            this.btnGrantExecToRole.Name = "btnGrantExecToRole";
            this.btnGrantExecToRole.Size = new System.Drawing.Size(180, 40);
            this.btnGrantExecToRole.TabIndex = 2;
            this.btnGrantExecToRole.Text = "GRANT EXECUTE";
            this.btnGrantExecToRole.UseVisualStyleBackColor = false;
            this.btnGrantExecToRole.Click += new System.EventHandler(this.btnGrantExecToRole_Click);
            // 
            // cboTargetRole
            // 
            this.cboTargetRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTargetRole.FormattingEnabled = true;
            this.cboTargetRole.Location = new System.Drawing.Point(150, 40);
            this.cboTargetRole.Name = "cboTargetRole";
            this.cboTargetRole.Size = new System.Drawing.Size(250, 24);
            this.cboTargetRole.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Chọn ROLE:";
            // 
            // tabTablePrivRole
            // 
            this.tabTablePrivRole.Controls.Add(this.groupBox5);
            this.tabTablePrivRole.Controls.Add(this.groupBox4);
            this.tabTablePrivRole.Location = new System.Drawing.Point(4, 25);
            this.tabTablePrivRole.Name = "tabTablePrivRole";
            this.tabTablePrivRole.Padding = new System.Windows.Forms.Padding(3);
            this.tabTablePrivRole.Size = new System.Drawing.Size(1168, 491);
            this.tabTablePrivRole.TabIndex = 1;
            this.tabTablePrivRole.Text = "Phân quyền TABLE cho ROLE";
            this.tabTablePrivRole.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnLoadTables);
            this.groupBox4.Controls.Add(this.cboTableOwner);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.cboTable);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(15, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1135, 120);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "1. Chọn TABLE/VIEW";
            // 
            // btnLoadTables
            // 
            this.btnLoadTables.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnLoadTables.ForeColor = System.Drawing.Color.White;
            this.btnLoadTables.Location = new System.Drawing.Point(950, 35);
            this.btnLoadTables.Name = "btnLoadTables";
            this.btnLoadTables.Size = new System.Drawing.Size(150, 35);
            this.btnLoadTables.TabIndex = 4;
            this.btnLoadTables.Text = "Tải danh sách";
            this.btnLoadTables.UseVisualStyleBackColor = false;
            this.btnLoadTables.Click += new System.EventHandler(this.btnLoadTables_Click);
            // 
            // cboTableOwner
            // 
            this.cboTableOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTableOwner.FormattingEnabled = true;
            this.cboTableOwner.Location = new System.Drawing.Point(150, 40);
            this.cboTableOwner.Name = "cboTableOwner";
            this.cboTableOwner.Size = new System.Drawing.Size(250, 24);
            this.cboTableOwner.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 17);
            this.label6.TabIndex = 2;
            this.label6.Text = "Owner/User:";
            // 
            // cboTable
            // 
            this.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(565, 40);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(350, 24);
            this.cboTable.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(440, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Tên TABLE/VIEW:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnGrantTableToRole);
            this.groupBox5.Controls.Add(this.cbDelete);
            this.groupBox5.Controls.Add(this.cbUpdate);
            this.groupBox5.Controls.Add(this.cbInsert);
            this.groupBox5.Controls.Add(this.cbSelect);
            this.groupBox5.Controls.Add(this.cboRoleForTable);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Location = new System.Drawing.Point(15, 150);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1135, 180);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "2. Chọn ROLE và quyền cần GRANT";
            // 
            // btnGrantTableToRole
            // 
            this.btnGrantTableToRole.BackColor = System.Drawing.Color.Green;
            this.btnGrantTableToRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnGrantTableToRole.ForeColor = System.Drawing.Color.White;
            this.btnGrantTableToRole.Location = new System.Drawing.Point(565, 100);
            this.btnGrantTableToRole.Name = "btnGrantTableToRole";
            this.btnGrantTableToRole.Size = new System.Drawing.Size(200, 50);
            this.btnGrantTableToRole.TabIndex = 6;
            this.btnGrantTableToRole.Text = "GRANT cho ROLE";
            this.btnGrantTableToRole.UseVisualStyleBackColor = false;
            this.btnGrantTableToRole.Click += new System.EventHandler(this.btnGrantTableToRole_Click);
            // 
            // cbDelete
            // 
            this.cbDelete.AutoSize = true;
            this.cbDelete.Location = new System.Drawing.Point(565, 65);
            this.cbDelete.Name = "cbDelete";
            this.cbDelete.Size = new System.Drawing.Size(85, 21);
            this.cbDelete.TabIndex = 5;
            this.cbDelete.Text = "DELETE";
            this.cbDelete.UseVisualStyleBackColor = true;
            // 
            // cbUpdate
            // 
            this.cbUpdate.AutoSize = true;
            this.cbUpdate.Location = new System.Drawing.Point(440, 65);
            this.cbUpdate.Name = "cbUpdate";
            this.cbUpdate.Size = new System.Drawing.Size(87, 21);
            this.cbUpdate.TabIndex = 4;
            this.cbUpdate.Text = "UPDATE";
            this.cbUpdate.UseVisualStyleBackColor = true;
            // 
            // cbInsert
            // 
            this.cbInsert.AutoSize = true;
            this.cbInsert.Location = new System.Drawing.Point(320, 65);
            this.cbInsert.Name = "cbInsert";
            this.cbInsert.Size = new System.Drawing.Size(80, 21);
            this.cbInsert.TabIndex = 3;
            this.cbInsert.Text = "INSERT";
            this.cbInsert.UseVisualStyleBackColor = true;
            // 
            // cbSelect
            // 
            this.cbSelect.AutoSize = true;
            this.cbSelect.Location = new System.Drawing.Point(200, 65);
            this.cbSelect.Name = "cbSelect";
            this.cbSelect.Size = new System.Drawing.Size(84, 21);
            this.cbSelect.TabIndex = 2;
            this.cbSelect.Text = "SELECT";
            this.cbSelect.UseVisualStyleBackColor = true;
            // 
            // cboRoleForTable
            // 
            this.cboRoleForTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRoleForTable.FormattingEnabled = true;
            this.cboRoleForTable.Location = new System.Drawing.Point(150, 30);
            this.cboRoleForTable.Name = "cboRoleForTable";
            this.cboRoleForTable.Size = new System.Drawing.Size(250, 24);
            this.cboRoleForTable.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Chọn ROLE:";
            // 
            // tabTablePrivUser
            // 
            this.tabTablePrivUser.Controls.Add(this.groupBox7);
            this.tabTablePrivUser.Controls.Add(this.groupBox6);
            this.tabTablePrivUser.Location = new System.Drawing.Point(4, 25);
            this.tabTablePrivUser.Name = "tabTablePrivUser";
            this.tabTablePrivUser.Size = new System.Drawing.Size(1168, 491);
            this.tabTablePrivUser.TabIndex = 2;
            this.tabTablePrivUser.Text = "Phân quyền TABLE cho USER";
            this.tabTablePrivUser.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnLoadTables2);
            this.groupBox6.Controls.Add(this.cboTableOwner2);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.cboTable2);
            this.groupBox6.Controls.Add(this.label10);
            this.groupBox6.Location = new System.Drawing.Point(15, 15);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1135, 120);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "1. Chọn TABLE/VIEW";
            // 
            // btnLoadTables2
            // 
            this.btnLoadTables2.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnLoadTables2.ForeColor = System.Drawing.Color.White;
            this.btnLoadTables2.Location = new System.Drawing.Point(950, 35);
            this.btnLoadTables2.Name = "btnLoadTables2";
            this.btnLoadTables2.Size = new System.Drawing.Size(150, 35);
            this.btnLoadTables2.TabIndex = 4;
            this.btnLoadTables2.Text = "Tải danh sách";
            this.btnLoadTables2.UseVisualStyleBackColor = false;
            this.btnLoadTables2.Click += new System.EventHandler(this.btnLoadTables2_Click);
            // 
            // cboTableOwner2
            // 
            this.cboTableOwner2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTableOwner2.FormattingEnabled = true;
            this.cboTableOwner2.Location = new System.Drawing.Point(150, 40);
            this.cboTableOwner2.Name = "cboTableOwner2";
            this.cboTableOwner2.Size = new System.Drawing.Size(250, 24);
            this.cboTableOwner2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 17);
            this.label9.TabIndex = 2;
            this.label9.Text = "Owner/User:";
            // 
            // cboTable2
            // 
            this.cboTable2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTable2.FormattingEnabled = true;
            this.cboTable2.Location = new System.Drawing.Point(565, 40);
            this.cboTable2.Name = "cboTable2";
            this.cboTable2.Size = new System.Drawing.Size(350, 24);
            this.cboTable2.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(440, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "Tên TABLE/VIEW:";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnGrantTableToUser);
            this.groupBox7.Controls.Add(this.cbDelete2);
            this.groupBox7.Controls.Add(this.cbUpdate2);
            this.groupBox7.Controls.Add(this.cbInsert2);
            this.groupBox7.Controls.Add(this.cbSelect2);
            this.groupBox7.Controls.Add(this.cboUserForTable);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Location = new System.Drawing.Point(15, 150);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(1135, 180);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "2. Chọn USER và quyền cần GRANT";
            // 
            // btnGrantTableToUser
            // 
            this.btnGrantTableToUser.BackColor = System.Drawing.Color.Green;
            this.btnGrantTableToUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnGrantTableToUser.ForeColor = System.Drawing.Color.White;
            this.btnGrantTableToUser.Location = new System.Drawing.Point(565, 100);
            this.btnGrantTableToUser.Name = "btnGrantTableToUser";
            this.btnGrantTableToUser.Size = new System.Drawing.Size(200, 50);
            this.btnGrantTableToUser.TabIndex = 6;
            this.btnGrantTableToUser.Text = "GRANT cho USER";
            this.btnGrantTableToUser.UseVisualStyleBackColor = false;
            this.btnGrantTableToUser.Click += new System.EventHandler(this.btnGrantTableToUser_Click);
            // 
            // cbDelete2
            // 
            this.cbDelete2.AutoSize = true;
            this.cbDelete2.Location = new System.Drawing.Point(565, 65);
            this.cbDelete2.Name = "cbDelete2";
            this.cbDelete2.Size = new System.Drawing.Size(85, 21);
            this.cbDelete2.TabIndex = 5;
            this.cbDelete2.Text = "DELETE";
            this.cbDelete2.UseVisualStyleBackColor = true;
            // 
            // cbUpdate2
            // 
            this.cbUpdate2.AutoSize = true;
            this.cbUpdate2.Location = new System.Drawing.Point(440, 65);
            this.cbUpdate2.Name = "cbUpdate2";
            this.cbUpdate2.Size = new System.Drawing.Size(87, 21);
            this.cbUpdate2.TabIndex = 4;
            this.cbUpdate2.Text = "UPDATE";
            this.cbUpdate2.UseVisualStyleBackColor = true;
            // 
            // cbInsert2
            // 
            this.cbInsert2.AutoSize = true;
            this.cbInsert2.Location = new System.Drawing.Point(320, 65);
            this.cbInsert2.Name = "cbInsert2";
            this.cbInsert2.Size = new System.Drawing.Size(80, 21);
            this.cbInsert2.TabIndex = 3;
            this.cbInsert2.Text = "INSERT";
            this.cbInsert2.UseVisualStyleBackColor = true;
            // 
            // cbSelect2
            // 
            this.cbSelect2.AutoSize = true;
            this.cbSelect2.Location = new System.Drawing.Point(200, 65);
            this.cbSelect2.Name = "cbSelect2";
            this.cbSelect2.Size = new System.Drawing.Size(84, 21);
            this.cbSelect2.TabIndex = 2;
            this.cbSelect2.Text = "SELECT";
            this.cbSelect2.UseVisualStyleBackColor = true;
            // 
            // cboUserForTable
            // 
            this.cboUserForTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUserForTable.FormattingEnabled = true;
            this.cboUserForTable.Location = new System.Drawing.Point(150, 30);
            this.cboUserForTable.Name = "cboUserForTable";
            this.cboUserForTable.Size = new System.Drawing.Size(250, 24);
            this.cboUserForTable.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 17);
            this.label11.TabIndex = 0;
            this.label11.Text = "Chọn USER:";
            // 
            // tabViewPriv
            // 
            this.tabViewPriv.Controls.Add(this.splitContainer1);
            this.tabViewPriv.Location = new System.Drawing.Point(4, 25);
            this.tabViewPriv.Name = "tabViewPriv";
            this.tabViewPriv.Size = new System.Drawing.Size(1168, 491);
            this.tabViewPriv.TabIndex = 3;
            this.tabViewPriv.Text = "Xem quyền đã GRANT";
            this.tabViewPriv.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox8);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox9);
            this.splitContainer1.Size = new System.Drawing.Size(1168, 491);
            this.splitContainer1.SplitterDistance = 580;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.btnRefreshUserPriv);
            this.groupBox8.Controls.Add(this.dgvUserPrivileges);
            this.groupBox8.Controls.Add(this.cboViewUser);
            this.groupBox8.Controls.Add(this.label12);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(580, 491);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Quyền của USER";
            // 
            // btnRefreshUserPriv
            // 
            this.btnRefreshUserPriv.Location = new System.Drawing.Point(420, 30);
            this.btnRefreshUserPriv.Name = "btnRefreshUserPriv";
            this.btnRefreshUserPriv.Size = new System.Drawing.Size(130, 30);
            this.btnRefreshUserPriv.TabIndex = 3;
            this.btnRefreshUserPriv.Text = "Xem quyền";
            this.btnRefreshUserPriv.UseVisualStyleBackColor = true;
            this.btnRefreshUserPriv.Click += new System.EventHandler(this.btnRefreshUserPriv_Click);
            // 
            // dgvUserPrivileges
            // 
            this.dgvUserPrivileges.AllowUserToAddRows = false;
            this.dgvUserPrivileges.AllowUserToDeleteRows = false;
            this.dgvUserPrivileges.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUserPrivileges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserPrivileges.Location = new System.Drawing.Point(15, 70);
            this.dgvUserPrivileges.Name = "dgvUserPrivileges";
            this.dgvUserPrivileges.ReadOnly = true;
            this.dgvUserPrivileges.Size = new System.Drawing.Size(550, 405);
            this.dgvUserPrivileges.TabIndex = 2;
            // 
            // cboViewUser
            // 
            this.cboViewUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboViewUser.FormattingEnabled = true;
            this.cboViewUser.Location = new System.Drawing.Point(150, 33);
            this.cboViewUser.Name = "cboViewUser";
            this.cboViewUser.Size = new System.Drawing.Size(250, 24);
            this.cboViewUser.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "Chọn USER:";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.btnRefreshRolePriv);
            this.groupBox9.Controls.Add(this.dgvRolePrivileges);
            this.groupBox9.Controls.Add(this.cboViewRole);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox9.Location = new System.Drawing.Point(0, 0);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(584, 491);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Quyền của ROLE";
            // 
            // btnRefreshRolePriv
            // 
            this.btnRefreshRolePriv.Location = new System.Drawing.Point(420, 30);
            this.btnRefreshRolePriv.Name = "btnRefreshRolePriv";
            this.btnRefreshRolePriv.Size = new System.Drawing.Size(130, 30);
            this.btnRefreshRolePriv.TabIndex = 3;
            this.btnRefreshRolePriv.Text = "Xem quyền";
            this.btnRefreshRolePriv.UseVisualStyleBackColor = true;
            this.btnRefreshRolePriv.Click += new System.EventHandler(this.btnRefreshRolePriv_Click);
            // 
            // dgvRolePrivileges
            // 
            this.dgvRolePrivileges.AllowUserToAddRows = false;
            this.dgvRolePrivileges.AllowUserToDeleteRows = false;
            this.dgvRolePrivileges.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRolePrivileges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRolePrivileges.Location = new System.Drawing.Point(15, 70);
            this.dgvRolePrivileges.Name = "dgvRolePrivileges";
            this.dgvRolePrivileges.ReadOnly = true;
            this.dgvRolePrivileges.Size = new System.Drawing.Size(555, 405);
            this.dgvRolePrivileges.TabIndex = 2;
            // 
            // cboViewRole
            // 
            this.cboViewRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboViewRole.FormattingEnabled = true;
            this.cboViewRole.Location = new System.Drawing.Point(150, 33);
            this.cboViewRole.Name = "cboViewRole";
            this.cboViewRole.Size = new System.Drawing.Size(250, 24);
            this.cboViewRole.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 36);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 17);
            this.label13.TabIndex = 0;
            this.label13.Text = "Chọn ROLE:";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(1080, 605);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmPhanQuyenOracle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 650);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblUserInfo);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmPhanQuyenOracle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý phân quyền Oracle - Admin";
            this.Load += new System.EventHandler(this.frmPhanQuyenOracle_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabProcFunc.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabTablePrivRole.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabTablePrivUser.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabViewPriv.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserPrivileges)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRolePrivileges)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabProcFunc;
        private System.Windows.Forms.TabPage tabTablePrivRole;
        private System.Windows.Forms.TabPage tabTablePrivUser;
        private System.Windows.Forms.TabPage tabViewPriv;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboObject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboOwner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboObjectType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadObjects;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboTargetUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGrantExecToUser;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnGrantExecToRole;
        private System.Windows.Forms.ComboBox cboTargetRole;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnLoadTables;
        private System.Windows.Forms.ComboBox cboTableOwner;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cbDelete;
        private System.Windows.Forms.CheckBox cbUpdate;
        private System.Windows.Forms.CheckBox cbInsert;
        private System.Windows.Forms.CheckBox cbSelect;
        private System.Windows.Forms.ComboBox cboRoleForTable;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnGrantTableToRole;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnLoadTables2;
        private System.Windows.Forms.ComboBox cboTableOwner2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboTable2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnGrantTableToUser;
        private System.Windows.Forms.CheckBox cbDelete2;
        private System.Windows.Forms.CheckBox cbUpdate2;
        private System.Windows.Forms.CheckBox cbInsert2;
        private System.Windows.Forms.CheckBox cbSelect2;
        private System.Windows.Forms.ComboBox cboUserForTable;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.DataGridView dgvUserPrivileges;
        private System.Windows.Forms.ComboBox cboViewUser;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.DataGridView dgvRolePrivileges;
        private System.Windows.Forms.ComboBox cboViewRole;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnRefreshUserPriv;
        private System.Windows.Forms.Button btnRefreshRolePriv;
        private System.Windows.Forms.Button btnClose;
    }
}