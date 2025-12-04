using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_An_BM
{
    public partial class frmUserLogin : Form
    {
        public frmUserLogin()
        {
            InitializeComponent();

            cbHienMatKhau.CheckedChanged += (s, e) => {
                txtMatKhau.UseSystemPasswordChar = !cbHienMatKhau.Checked;
            };

            linkDangKy.LinkClicked += (s, e) => {
                new frmRegister().ShowDialog();
            };
            rbKhachHang.Checked = true;
        }

        private void RbLoaiTaiKhoan_CheckedChanged(object sender, EventArgs e)
        {
            if (rbKhachHang.Checked)
            {
                lblTaiKhoan.Text = "Email:";
                linkDangKy.Visible = true;
                txtTaiKhoan.Text = "khach@gmail.com"; // Demo
                txtMatKhau.Text = "Khach@123";
            }
            else
            {
                lblTaiKhoan.Text = "Mã NV:";
                linkDangKy.Visible = false;
                txtTaiKhoan.Text = "999"; // Demo: Admin
                txtMatKhau.Text = "Admin@123";
            }
        }

        private void BtnDangNhap_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaiKhoan.Text) ||
                string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string result;

                if (rbKhachHang.Checked)
                {
                    // Login Khách hàng
                    result = OracleHelper.LoginKH(txtTaiKhoan.Text, txtMatKhau.Text);

                    if (result.StartsWith("SUCCESS"))
                    {
                        // Format: SUCCESS|MaKH|HoTen
                        string[] parts = result.Split('|');
                        int maKH = Convert.ToInt32(parts[1]);
                        string hoTen = parts[2];

                        SessionManager.Login(maKH, hoTen, "CUSTOMER", txtTaiKhoan.Text);

                        MessageBox.Show($"Chào mừng {hoTen}!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();
                        new frmCustomerDashboard().ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        ShowLoginError(result);
                    }
                }
                else
                {
                    // Login Nhân viên
                    int maNV = Convert.ToInt32(txtTaiKhoan.Text);
                    result = OracleHelper.LoginNV(maNV, txtMatKhau.Text);

                    if (result.Contains("|"))
                    {
                        // Format: ROLE|MaNV|HoTen
                        string[] parts = result.Split('|');
                        string role = parts[0];
                        int maNVResult = Convert.ToInt32(parts[1]);
                        string hoTen = parts[2];

                        SessionManager.Login(maNVResult, hoTen, role);

                        MessageBox.Show($"Chào mừng {hoTen} ({role})!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Hide();

                        if (role == "ADMIN")
                        {
                            new frmAdminDashboard().ShowDialog();
                        }
                        else if (role == "STAFF")
                        {
                            new frmStaffDashboard().ShowDialog();
                        }

                        this.Close();
                    }
                    else
                    {
                        ShowLoginError(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowLoginError(string errorCode)
        {
            string message;

            switch (errorCode)
            {
                case "INVALID_PASSWORD":
                    message = "Mật khẩu không đúng!";
                    break;

                case "USER_NOT_FOUND":
                    message = "Tài khoản không tồn tại!";
                    break;

                case "NO_ROLE":
                    message = "Tài khoản chưa được cấp quyền!";
                    break;

                default:
                    message = "Đăng nhập thất bại: " + errorCode;
                    break;
            }

            MessageBox.Show(message, "Lỗi đăng nhập",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}