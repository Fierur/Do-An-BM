using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmRegisterKhachHang : Form
    {
        public frmRegisterKhachHang()
        {
            InitializeComponent();
        }

        private void frmRegisterKhachHang_Load(object sender, EventArgs e)
        {
            // Kiểm tra kết nối database
            if (Database.Con == null || Database.Con.State != ConnectionState.Open)
            {
                MessageBox.Show("Chưa kết nối đến cơ sở dữ liệu. Vui lòng đăng nhập trước!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu đầu vào
                if (!ValidateInput())
                    return;

                // Kiểm tra mật khẩu khớp
                if (txtMatKhau.Text != txtXacNhanMK.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtXacNhanMK.Focus();
                    return;
                }

                // Lấy mã khách hàng mới
                int maKH = GetNewMaKH();
                int maGH = GetNewMaGH();

                // Tạo giỏ hàng trước
                if (!CreateGioHang(maGH))
                {
                    MessageBox.Show("Lỗi tạo giỏ hàng!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Đăng ký khách hàng
                if (RegisterKhachHang(maKH, maGH))
                {
                    MessageBox.Show($"Đăng ký thành công!\nMã khách hàng: {maKH}\n\n" +
                        "Email và SĐT đã được mã hóa AES.\nMật khẩu đã được hash SHA-512.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Kiểm tra trường bắt buộc
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validate email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            // Validate SĐT (10-11 số)
            if (!Regex.IsMatch(txtSDT.Text, @"^[0-9]{10,11}$"))
            {
                MessageBox.Show("Số điện thoại phải có 10-11 chữ số!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            if (txtMatKhau.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMatKhau.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email && email.Contains("@") && email.Contains(".");
            }
            catch
            {
                return false;
            }
        }

        private int GetNewMaKH()
        {
            try
            {
                string sql = "SELECT NVL(MAX(MaKH), 0) + 1 FROM KhachHang";
                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return 1;
            }
        }

        private int GetNewMaGH()
        {
            try
            {
                string sql = "SELECT NVL(MAX(MaGH), 0) + 1 FROM GioHang";
                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return 1;
            }
        }

        private bool CreateGioHang(int maGH)
        {
            try
            {
                string sql = "INSERT INTO GioHang(MaGH) VALUES(:maGH)";
                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("maGH", OracleDbType.Int32).Value = maGH;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo giỏ hàng: " + ex.Message);
                return false;
            }
        }

        private bool RegisterKhachHang(int maKH, int maGH)
        {
            try
            {
                // Gọi function HASH_PASS, ENCRYPT_AES từ Oracle
                string sql = @"INSERT INTO KhachHang(MaKH, HoTenKH, email, SDT, DiaChiKH, MatKhau, MaGH) 
                              VALUES(:maKH, :hoTen, BM_USER.ENCRYPT_AES(:email), BM_USER.ENCRYPT_AES(:sdt), 
                                     :diaChi, BM_USER.HASH_PASS(:matKhau), :maGH)";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("maKH", OracleDbType.Int32).Value = maKH;
                cmd.Parameters.Add("hoTen", OracleDbType.NVarchar2).Value = txtHoTen.Text.Trim();
                cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                cmd.Parameters.Add("sdt", OracleDbType.Varchar2).Value = txtSDT.Text.Trim();
                cmd.Parameters.Add("diaChi", OracleDbType.NVarchar2).Value =
                    string.IsNullOrWhiteSpace(txtDiaChi.Text) ? (object)DBNull.Value : txtDiaChi.Text.Trim();
                cmd.Parameters.Add("matKhau", OracleDbType.Varchar2).Value = txtMatKhau.Text;
                cmd.Parameters.Add("maGH", OracleDbType.Int32).Value = maGH;

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng ký: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnXemDuLieu_Click(object sender, EventArgs e)
        {
            try
            {
                // Chỉ admin mới xem được dữ liệu giải mã
                if (!Database.User.Equals("BM_USER", StringComparison.OrdinalIgnoreCase) &&
                    !Database.User.Equals("SYS", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Chỉ Admin (BM_USER hoặc SYS) mới có quyền xem dữ liệu giải mã!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Truy vấn với giải mã
                string sql = @"SELECT 
                                MaKH AS ""Mã KH"",
                                HoTenKH AS ""Họ tên"",
                                BM_USER.DECRYPT_AES(email) AS ""Email (Giải mã)"",
                                BM_USER.DECRYPT_AES(SDT) AS ""SĐT (Giải mã)"",
                                DiaChiKH AS ""Địa chỉ"",
                                TO_CHAR(NgayDangKy, 'DD/MM/YYYY') AS ""Ngày đăng ký"",
                                TrangThai AS ""Trạng thái""
                              FROM KhachHang
                              ORDER BY MaKH DESC";

                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvKhachHang.DataSource = dt;
                dgvKhachHang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MessageBox.Show($"Đã tải {dt.Rows.Count} khách hàng.\nEmail và SĐT đã được giải mã!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem dữ liệu: " + ex.Message +
                    "\n\nĐảm bảo bạn đã kết nối với user BM_USER và các function mã hóa đã được tạo!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !cbHienMatKhau.Checked;
            txtXacNhanMK.UseSystemPasswordChar = !cbHienMatKhau.Checked;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            txtMatKhau.Clear();
            txtXacNhanMK.Clear();
            cbHienMatKhau.Checked = false;
            txtHoTen.Focus();
        }
    }
}