using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = true;
            txtXacNhanMatKhau.UseSystemPasswordChar = true;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (!ValidateInput())
                    return;

                // Kiểm tra mật khẩu khớp
                if (txtMatKhau.Text != txtXacNhanMatKhau.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtXacNhanMatKhau.Focus();
                    return;
                }

                // Kiểm tra email đã tồn tại
                if (CheckEmailExists(txtEmail.Text))
                {
                    MessageBox.Show("Email đã được đăng ký!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }

                // Tạo giỏ hàng mới
                int maGH = CreateNewCart();
                if (maGH <= 0)
                {
                    MessageBox.Show("Lỗi tạo giỏ hàng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lấy MaKH mới
                int maKH = GetNextMaKH();

                // Mã hóa thông tin
                byte[] emailEncrypted = OracleHelper.EncryptAES(txtEmail.Text);
                byte[] sdtEncrypted = OracleHelper.EncryptAES(txtSDT.Text);
                byte[] passwordHashed = OracleHelper.HashPassword(txtMatKhau.Text);

                // Insert KhachHang
                string sql = @"INSERT INTO KhachHang 
                              (MaKH, HoTenKH, Email, SDT, DiaChiKH, MatKhau, MaGH, TrangThai)
                              VALUES 
                              (:maKH, :hoTen, :email, :sdt, :diaChi, :matKhau, :maGH, 'ACTIVE')";

                var param1 = new OracleParameter("maKH", OracleDbType.Int32, maKH, ParameterDirection.Input);
                var param2 = new OracleParameter("hoTen", OracleDbType.NVarchar2, txtHoTen.Text, ParameterDirection.Input);
                var param3 = new OracleParameter("email", OracleDbType.Raw, emailEncrypted, ParameterDirection.Input);
                var param4 = new OracleParameter("sdt", OracleDbType.Raw, sdtEncrypted, ParameterDirection.Input);
                var param5 = new OracleParameter("diaChi", OracleDbType.NVarchar2, txtDiaChi.Text, ParameterDirection.Input);
                var param6 = new OracleParameter("matKhau", OracleDbType.Raw, passwordHashed, ParameterDirection.Input);
                var param7 = new OracleParameter("maGH", OracleDbType.Int32, maGH, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7);

                if (result > 0)
                {
                    MessageBox.Show("Đăng ký thành công!\n\n" +
                        $"Email: {txtEmail.Text}\n" +
                        "Vui lòng đăng nhập để tiếp tục.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đăng ký thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text) || txtSDT.Text.Length < 10)
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text) || txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            return true;
        }

        private bool CheckEmailExists(string email)
        {
            byte[] emailEncrypted = OracleHelper.EncryptAES(email);
            string sql = "SELECT COUNT(*) FROM KhachHang WHERE Email = :email";
            var param = new OracleParameter("email", OracleDbType.Raw, emailEncrypted, ParameterDirection.Input);

            DataTable dt = OracleHelper.ExecuteQuery(sql, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]) > 0;
            }
            return false;
        }

        private int CreateNewCart()
        {
            // Lấy MaGH mới
            string sqlMax = "SELECT NVL(MAX(MaGH), 0) + 1 FROM GioHang";
            DataTable dt = OracleHelper.ExecuteQuery(sqlMax);
            int maGH = dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 1;

            // Insert GioHang
            string sql = "INSERT INTO GioHang (MaGH) VALUES (:maGH)";
            var param = new OracleParameter("maGH", OracleDbType.Int32, maGH, ParameterDirection.Input);

            int result = OracleHelper.ExecuteNonQuery(sql, param);
            return result > 0 ? maGH : -1;
        }

        private int GetNextMaKH()
        {
            string sql = "SELECT NVL(MAX(MaKH), 0) + 1 FROM KhachHang";
            DataTable dt = OracleHelper.ExecuteQuery(sql);
            return dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0][0]) : 1;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !cbHienMatKhau.Checked;
            txtXacNhanMatKhau.UseSystemPasswordChar = !cbHienMatKhau.Checked;
        }
    }
}