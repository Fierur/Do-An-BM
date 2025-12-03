using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmQuanLyKhachHang : Form
    {
        public frmQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin
            if (!CheckAdminPermission())
            {
                MessageBox.Show("Chỉ Admin (BM_USER hoặc SYS) mới có quyền truy cập form này!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Hiển thị thông tin user đang đăng nhập
            lblUserInfo.Text = $"Đăng nhập với: {Database.User}";

            // Load dữ liệu ban đầu
            LoadDanhSachKhachHang();
            LoadAuditLog();

            // Set giá trị mặc định
            txtTrangThai.SelectedIndex = 0;
        }

        private bool CheckAdminPermission()
        {
            if (Database.Con == null || Database.Con.State != ConnectionState.Open)
            {
                MessageBox.Show("Chưa kết nối đến cơ sở dữ liệu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Chỉ cho phép BM_USER hoặc SYS
            if (!Database.User.Equals("BM_USER", StringComparison.OrdinalIgnoreCase) &&
                !Database.User.Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private void LoadDanhSachKhachHang()
        {
            try
            {
                // Truy vấn với giải mã Email và SĐT
                string sql = @"SELECT 
                                MaKH AS ""Mã KH"",
                                HoTenKH AS ""Họ tên"",
                                BM_USER.DECRYPT_AES(email) AS ""Email"",
                                BM_USER.DECRYPT_AES(SDT) AS ""SĐT"",
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

                // Ẩn selection ban đầu
                dgvKhachHang.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message +
                    "\n\nĐảm bảo các function DECRYPT_AES đã được tạo!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAuditLog(string filter = "")
        {
            try
            {
                string sql = @"SELECT 
                                TO_CHAR(LOG_TIME, 'DD/MM/YYYY HH24:MI:SS') AS ""Thời gian"",
                                USERNAME AS ""Người dùng"",
                                ACTION AS ""Hành động"",
                                IP AS ""Địa chỉ IP""
                              FROM AUDIT_LOG
                              WHERE 1=1 ";

                if (!string.IsNullOrEmpty(filter))
                {
                    sql += filter;
                }

                sql += " ORDER BY LOG_TIME DESC FETCH FIRST 50 ROWS ONLY";

                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvAuditLog.DataSource = dt;
                dgvAuditLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải Audit Log: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

                    // Hiển thị thông tin lên form chi tiết
                    txtMaKH.Text = row.Cells["Mã KH"].Value?.ToString() ?? "";
                    txtHoTen.Text = row.Cells["Họ tên"].Value?.ToString() ?? "";
                    txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                    txtSDT.Text = row.Cells["SĐT"].Value?.ToString() ?? "";
                    txtDiaChi.Text = row.Cells["Địa chỉ"].Value?.ToString() ?? "";

                    // Parse ngày đăng ký
                    string ngayDK = row.Cells["Ngày đăng ký"].Value?.ToString() ?? "";
                    if (DateTime.TryParseExact(ngayDK, "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime date))
                    {
                        dtpNgayDangKy.Value = date;
                    }

                    // Trạng thái
                    string trangThai = row.Cells["Trạng thái"].Value?.ToString() ?? "ACTIVE";
                    int index = txtTrangThai.FindString(trangThai);
                    if (index >= 0)
                        txtTrangThai.SelectedIndex = index;
                    else
                        txtTrangThai.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn khách hàng: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu
                if (string.IsNullOrWhiteSpace(txtMaKH.Text))
                {
                    MessageBox.Show("Vui lòng chọn khách hàng cần cập nhật!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput())
                    return;

                // Xác nhận cập nhật
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn cập nhật thông tin khách hàng này?\n\n" +
                    "Thay đổi sẽ được ghi vào Audit Log.",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                // Cập nhật vào database
                if (UpdateKhachHang())
                {
                    MessageBox.Show("Cập nhật thông tin thành công!\n\n" +
                        "Thay đổi đã được ghi vào Audit Log.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh dữ liệu
                    LoadDanhSachKhachHang();
                    LoadAuditLog();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Validate họ tên
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Họ tên không được để trống!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            // Validate email
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email không được để trống!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            // Validate SĐT
            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("SĐT không được để trống!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (!Regex.IsMatch(txtSDT.Text, @"^[0-9]{10,11}$"))
            {
                MessageBox.Show("SĐT phải có 10-11 chữ số!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
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

        private bool UpdateKhachHang()
        {
            try
            {
                // Câu lệnh UPDATE với mã hóa Email và SĐT
                // Trigger AUDIT_SENSITIVE sẽ tự động ghi log
                string sql = @"UPDATE KhachHang 
                              SET HoTenKH = :hoTen,
                                  email = BM_USER.ENCRYPT_AES(:email),
                                  SDT = BM_USER.ENCRYPT_AES(:sdt),
                                  DiaChiKH = :diaChi,
                                  TrangThai = :trangThai
                              WHERE MaKH = :maKH";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("hoTen", OracleDbType.NVarchar2).Value = txtHoTen.Text.Trim();
                cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                cmd.Parameters.Add("sdt", OracleDbType.Varchar2).Value = txtSDT.Text.Trim();
                cmd.Parameters.Add("diaChi", OracleDbType.NVarchar2).Value =
                    string.IsNullOrWhiteSpace(txtDiaChi.Text) ? (object)DBNull.Value : txtDiaChi.Text.Trim();
                cmd.Parameters.Add("trangThai", OracleDbType.Varchar2).Value = txtTrangThai.Text;
                cmd.Parameters.Add("maKH", OracleDbType.Int32).Value = int.Parse(txtMaKH.Text);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi UPDATE: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachKhachHang();
            ClearForm();
            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXemLichSu_Click(object sender, EventArgs e)
        {
            // Nếu đang chọn khách hàng, lọc theo khách hàng đó
            if (!string.IsNullOrEmpty(txtMaKH.Text))
            {
                string maKH = txtMaKH.Text;
                string filter = $" AND (ACTION LIKE '%KH {maKH}%' OR ACTION LIKE '%KHACHHANG%')";
                LoadAuditLog(filter);
                MessageBox.Show($"Đang hiển thị lịch sử liên quan đến Khách hàng {maKH}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Hiển thị tất cả log
                LoadAuditLog();
                MessageBox.Show("Đang hiển thị 50 log gần nhất",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            dtpNgayDangKy.Value = DateTime.Now;
            txtTrangThai.SelectedIndex = 0;
            dgvKhachHang.ClearSelection();
        }
    }
}