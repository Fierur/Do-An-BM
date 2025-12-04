using System.Windows.Forms;

namespace Do_An_BM
{
    partial class frmUserLogin
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblLoai = new System.Windows.Forms.Label();
            this.rbKhachHang = new System.Windows.Forms.RadioButton();
            this.rbNhanVien = new System.Windows.Forms.RadioButton();
            this.lblTaiKhoan = new System.Windows.Forms.Label();
            this.txtTaiKhoan = new System.Windows.Forms.TextBox();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.cbHienMatKhau = new System.Windows.Forms.CheckBox();
            this.btnDangNhap = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.linkDangKy = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(80, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ĐĂNG NHẬP HỆ THỐNG";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLoai
            // 
            this.lblLoai.Location = new System.Drawing.Point(50, 70);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(120, 25);
            this.lblLoai.TabIndex = 1;
            this.lblLoai.Text = "Loại tài khoản:";
            // 
            // rbKhachHang
            // 
            this.rbKhachHang.Checked = true;
            this.rbKhachHang.Location = new System.Drawing.Point(170, 70);
            this.rbKhachHang.Name = "rbKhachHang";
            this.rbKhachHang.Size = new System.Drawing.Size(100, 25);
            this.rbKhachHang.TabIndex = 2;
            this.rbKhachHang.TabStop = true;
            this.rbKhachHang.Text = "Khách hàng";
            // 
            // rbNhanVien
            // 
            this.rbNhanVien.Location = new System.Drawing.Point(280, 70);
            this.rbNhanVien.Name = "rbNhanVien";
            this.rbNhanVien.Size = new System.Drawing.Size(100, 25);
            this.rbNhanVien.TabIndex = 3;
            this.rbNhanVien.Text = "Nhân viên";
            // 
            // lblTaiKhoan
            // 
            this.lblTaiKhoan.Location = new System.Drawing.Point(50, 110);
            this.lblTaiKhoan.Name = "lblTaiKhoan";
            this.lblTaiKhoan.Size = new System.Drawing.Size(120, 25);
            this.lblTaiKhoan.TabIndex = 4;
            this.lblTaiKhoan.Text = "Email:";
            // 
            // txtTaiKhoan
            // 
            this.txtTaiKhoan.Location = new System.Drawing.Point(170, 110);
            this.txtTaiKhoan.Name = "txtTaiKhoan";
            this.txtTaiKhoan.Size = new System.Drawing.Size(220, 20);
            this.txtTaiKhoan.TabIndex = 5;
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.Location = new System.Drawing.Point(50, 150);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new System.Drawing.Size(120, 25);
            this.lblMatKhau.TabIndex = 6;
            this.lblMatKhau.Text = "Mật khẩu:";
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.Location = new System.Drawing.Point(170, 150);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.Size = new System.Drawing.Size(220, 20);
            this.txtMatKhau.TabIndex = 7;
            this.txtMatKhau.UseSystemPasswordChar = true;
            // 
            // cbHienMatKhau
            // 
            this.cbHienMatKhau.Location = new System.Drawing.Point(170, 180);
            this.cbHienMatKhau.Name = "cbHienMatKhau";
            this.cbHienMatKhau.Size = new System.Drawing.Size(120, 25);
            this.cbHienMatKhau.TabIndex = 8;
            this.cbHienMatKhau.Text = "Hiện mật khẩu";
            // 
            // btnDangNhap
            // 
            this.btnDangNhap.Location = new System.Drawing.Point(170, 220);
            this.btnDangNhap.Name = "btnDangNhap";
            this.btnDangNhap.Size = new System.Drawing.Size(100, 35);
            this.btnDangNhap.TabIndex = 9;
            this.btnDangNhap.Text = "Đăng nhập";
            this.btnDangNhap.Click += new System.EventHandler(this.BtnDangNhap_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(290, 220);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(100, 35);
            this.btnHuy.TabIndex = 10;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // linkDangKy
            // 
            this.linkDangKy.Location = new System.Drawing.Point(170, 265);
            this.linkDangKy.Name = "linkDangKy";
            this.linkDangKy.Size = new System.Drawing.Size(220, 20);
            this.linkDangKy.TabIndex = 11;
            this.linkDangKy.TabStop = true;
            this.linkDangKy.Text = "Chưa có tài khoản? Đăng ký ngay";
            // 
            // frmUserLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 311);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblLoai);
            this.Controls.Add(this.rbKhachHang);
            this.Controls.Add(this.rbNhanVien);
            this.Controls.Add(this.lblTaiKhoan);
            this.Controls.Add(this.txtTaiKhoan);
            this.Controls.Add(this.lblMatKhau);
            this.Controls.Add(this.txtMatKhau);
            this.Controls.Add(this.cbHienMatKhau);
            this.Controls.Add(this.btnDangNhap);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.linkDangKy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmUserLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập hệ thống";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton rbKhachHang, rbNhanVien;
        private CheckBox cbHienMatKhau;
        private Button btnDangNhap;
        private LinkLabel linkDangKy;
        private Label lblTitle;
        private Label lblLoai;
        private Label lblMatKhau;
        private Button btnHuy;
        public Label lblTaiKhoan;
        public TextBox txtTaiKhoan;
        public TextBox txtMatKhau;
    }
}