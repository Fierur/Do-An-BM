namespace Do_An_BM
{
    partial class frmCheckout
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGhiChuDC = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSoNha = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboXaPhuong = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboQuanHuyen = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboThanhPho = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHoTenNN = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtGhiChuTT = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSoThe = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rbViDienTu = new System.Windows.Forms.RadioButton();
            this.rbChuyenKhoan = new System.Windows.Forms.RadioButton();
            this.rbTheTinDung = new System.Windows.Forms.RadioButton();
            this.rbTienMat = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblTongCong = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblVAT = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblPhiShip = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTamTinh = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnDatHang = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(900, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "THANH TOÁN ĐỚN HÀNG";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGhiChuDC);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSoNha);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboXaPhuong);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboQuanHuyen);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cboThanhPho);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtHoTenNN);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox1.Location = new System.Drawing.Point(20, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(860, 250);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Địa chỉ giao hàng";
            // 
            // txtGhiChuDC
            // 
            this.txtGhiChuDC.Location = new System.Drawing.Point(180, 200);
            this.txtGhiChuDC.Multiline = true;
            this.txtGhiChuDC.Name = "txtGhiChuDC";
            this.txtGhiChuDC.Size = new System.Drawing.Size(650, 35);
            this.txtGhiChuDC.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ghi chú:";
            // 
            // txtSoNha
            // 
            this.txtSoNha.Location = new System.Drawing.Point(180, 165);
            this.txtSoNha.Name = "txtSoNha";
            this.txtSoNha.Size = new System.Drawing.Size(650, 23);
            this.txtSoNha.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Số nhà, tên đường:";
            // 
            // cboXaPhuong
            // 
            this.cboXaPhuong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboXaPhuong.FormattingEnabled = true;
            this.cboXaPhuong.Location = new System.Drawing.Point(580, 130);
            this.cboXaPhuong.Name = "cboXaPhuong";
            this.cboXaPhuong.Size = new System.Drawing.Size(250, 24);
            this.cboXaPhuong.TabIndex = 7;
            this.cboXaPhuong.SelectedIndexChanged += new System.EventHandler(this.cboXaPhuong_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(450, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Xã/Phường:";
            // 
            // cboQuanHuyen
            // 
            this.cboQuanHuyen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQuanHuyen.FormattingEnabled = true;
            this.cboQuanHuyen.Location = new System.Drawing.Point(180, 130);
            this.cboQuanHuyen.Name = "cboQuanHuyen";
            this.cboQuanHuyen.Size = new System.Drawing.Size(250, 24);
            this.cboQuanHuyen.TabIndex = 5;
            this.cboQuanHuyen.SelectedIndexChanged += new System.EventHandler(this.cboQuanHuyen_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Quận/Huyện:";
            // 
            // cboThanhPho
            // 
            this.cboThanhPho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThanhPho.FormattingEnabled = true;
            this.cboThanhPho.Location = new System.Drawing.Point(180, 95);
            this.cboThanhPho.Name = "cboThanhPho";
            this.cboThanhPho.Size = new System.Drawing.Size(250, 24);
            this.cboThanhPho.TabIndex = 3;
            this.cboThanhPho.SelectedIndexChanged += new System.EventHandler(this.cboThanhPho_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tỉnh/Thành phố:";
            // 
            // txtHoTenNN
            // 
            this.txtHoTenNN.Location = new System.Drawing.Point(180, 60);
            this.txtHoTenNN.Name = "txtHoTenNN";
            this.txtHoTenNN.Size = new System.Drawing.Size(400, 23);
            this.txtHoTenNN.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Họ tên người nhận:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtGhiChuTT);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtSoThe);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.rbViDienTu);
            this.groupBox2.Controls.Add(this.rbChuyenKhoan);
            this.groupBox2.Controls.Add(this.rbTheTinDung);
            this.groupBox2.Controls.Add(this.rbTienMat);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox2.Location = new System.Drawing.Point(20, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(860, 180);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hình thức thanh toán";
            // 
            // txtGhiChuTT
            // 
            this.txtGhiChuTT.Location = new System.Drawing.Point(180, 135);
            this.txtGhiChuTT.Multiline = true;
            this.txtGhiChuTT.Name = "txtGhiChuTT";
            this.txtGhiChuTT.Size = new System.Drawing.Size(650, 35);
            this.txtGhiChuTT.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 138);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 17);
            this.label10.TabIndex = 7;
            this.label10.Text = "Ghi chú:";
            // 
            // txtSoThe
            // 
            this.txtSoThe.Enabled = false;
            this.txtSoThe.Location = new System.Drawing.Point(180, 100);
            this.txtSoThe.MaxLength = 19;
            this.txtSoThe.Name = "txtSoThe";
            this.txtSoThe.Size = new System.Drawing.Size(300, 23);
            this.txtSoThe.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 17);
            this.label9.TabIndex = 5;
            this.label9.Text = "Số thẻ tín dụng:";
            // 
            // rbViDienTu
            // 
            this.rbViDienTu.AutoSize = true;
            this.rbViDienTu.Location = new System.Drawing.Point(690, 60);
            this.rbViDienTu.Name = "rbViDienTu";
            this.rbViDienTu.Size = new System.Drawing.Size(88, 21);
            this.rbViDienTu.TabIndex = 4;
            this.rbViDienTu.Text = "Ví điện tử";
            this.rbViDienTu.UseVisualStyleBackColor = true;
            // 
            // rbChuyenKhoan
            // 
            this.rbChuyenKhoan.AutoSize = true;
            this.rbChuyenKhoan.Location = new System.Drawing.Point(510, 60);
            this.rbChuyenKhoan.Name = "rbChuyenKhoan";
            this.rbChuyenKhoan.Size = new System.Drawing.Size(124, 21);
            this.rbChuyenKhoan.TabIndex = 3;
            this.rbChuyenKhoan.Text = "Chuyển khoản";
            this.rbChuyenKhoan.UseVisualStyleBackColor = true;
            // 
            // rbTheTinDung
            // 
            this.rbTheTinDung.AutoSize = true;
            this.rbTheTinDung.Location = new System.Drawing.Point(330, 60);
            this.rbTheTinDung.Name = "rbTheTinDung";
            this.rbTheTinDung.Size = new System.Drawing.Size(111, 21);
            this.rbTheTinDung.TabIndex = 2;
            this.rbTheTinDung.Text = "Thẻ tín dụng";
            this.rbTheTinDung.UseVisualStyleBackColor = true;
            this.rbTheTinDung.CheckedChanged += new System.EventHandler(this.rbTheTinDung_CheckedChanged);
            // 
            // rbTienMat
            // 
            this.rbTienMat.AutoSize = true;
            this.rbTienMat.Checked = true;
            this.rbTienMat.Location = new System.Drawing.Point(180, 60);
            this.rbTienMat.Name = "rbTienMat";
            this.rbTienMat.Size = new System.Drawing.Size(80, 21);
            this.rbTienMat.TabIndex = 1;
            this.rbTienMat.TabStop = true;
            this.rbTienMat.Text = "Tiền mặt";
            this.rbTienMat.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Phương thức:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblTongCong);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.lblVAT);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lblPhiShip);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.lblTamTinh);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox3.Location = new System.Drawing.Point(20, 520);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(860, 180);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tổng kết";
            // 
            // lblTongCong
            // 
            this.lblTongCong.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTongCong.ForeColor = System.Drawing.Color.Red;
            this.lblTongCong.Location = new System.Drawing.Point(450, 135);
            this.lblTongCong.Name = "lblTongCong";
            this.lblTongCong.Size = new System.Drawing.Size(380, 30);
            this.lblTongCong.TabIndex = 7;
            this.lblTongCong.Text = "0 VNĐ";
            this.lblTongCong.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(30, 140);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(115, 20);
            this.label14.TabIndex = 6;
            this.label14.Text = "TỔNG CỘNG:";
            // 
            // lblVAT
            // 
            this.lblVAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblVAT.Location = new System.Drawing.Point(450, 100);
            this.lblVAT.Name = "lblVAT";
            this.lblVAT.Size = new System.Drawing.Size(380, 25);
            this.lblVAT.TabIndex = 5;
            this.lblVAT.Text = "0 VNĐ";
            this.lblVAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(30, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 17);
            this.label12.TabIndex = 4;
            this.label12.Text = "Thuế VAT (10%):";
            // 
            // lblPhiShip
            // 
            this.lblPhiShip.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblPhiShip.Location = new System.Drawing.Point(450, 70);
            this.lblPhiShip.Name = "lblPhiShip";
            this.lblPhiShip.Size = new System.Drawing.Size(380, 25);
            this.lblPhiShip.TabIndex = 3;
            this.lblPhiShip.Text = "0 VNĐ";
            this.lblPhiShip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 75);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 17);
            this.label11.TabIndex = 2;
            this.label11.Text = "Phí vận chuyển:";
            // 
            // lblTamTinh
            // 
            this.lblTamTinh.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblTamTinh.Location = new System.Drawing.Point(450, 40);
            this.lblTamTinh.Name = "lblTamTinh";
            this.lblTamTinh.Size = new System.Drawing.Size(380, 25);
            this.lblTamTinh.TabIndex = 1;
            this.lblTamTinh.Text = "0 VNĐ";
            this.lblTamTinh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 17);
            this.label8.TabIndex = 0;
            this.label8.Text = "Tạm tính:";
            // 
            // btnDatHang
            // 
            this.btnDatHang.BackColor = System.Drawing.Color.Green;
            this.btnDatHang.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnDatHang.ForeColor = System.Drawing.Color.White;
            this.btnDatHang.Location = new System.Drawing.Point(520, 710);
            this.btnDatHang.Name = "btnDatHang";
            this.btnDatHang.Size = new System.Drawing.Size(180, 50);
            this.btnDatHang.TabIndex = 4;
            this.btnDatHang.Text = "ĐẶT HÀNG";
            this.btnDatHang.UseVisualStyleBackColor = false;
            this.btnDatHang.Click += new System.EventHandler(this.btnDatHang_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(720, 710);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 50);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmCheckout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 772);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDatHang);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmCheckout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thanh toán đơn hàng";
            this.Load += new System.EventHandler(this.frmCheckout_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtHoTenNN;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboThanhPho;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboQuanHuyen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboXaPhuong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSoNha;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGhiChuDC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rbTienMat;
        private System.Windows.Forms.RadioButton rbTheTinDung;
        private System.Windows.Forms.RadioButton rbChuyenKhoan;
        private System.Windows.Forms.RadioButton rbViDienTu;
        private System.Windows.Forms.TextBox txtSoThe;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGhiChuTT;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblTamTinh;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblPhiShip;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblVAT;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblTongCong;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnDatHang;
        private System.Windows.Forms.Button btnCancel;
    }
}