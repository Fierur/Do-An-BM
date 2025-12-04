using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmQuanLyKhachHang : Form
    {
        private bool isEditing = false;
        public frmQuanLyKhachHang()
        {
            InitializeComponent();
        }

        private void frmQuanLyKhachHang_Load(object sender, EventArgs e)
        {
            LoadKhachHang();
        }

        private void LoadKhachHang()
        {
            try
            {
                string sql = @"SELECT MaKH, HoTenKH, DiaChiKH, NgayDangKy, TrangThai 
                              FROM BM_USER.KhachHang 
                              ORDER BY MaKH DESC";

                DataTable dt = OracleHelper.ExecuteQuery(sql);
                if (dt != null)
                {
                    dgvKhachHang.DataSource = dt;
                    dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
                    dgvKhachHang.Columns["HoTenKH"].HeaderText = "Họ tên";
                    dgvKhachHang.Columns["DiaChiKH"].HeaderText = "Địa chỉ";
                    dgvKhachHang.Columns["NgayDangKy"].HeaderText = "Ngày đăng ký";
                    dgvKhachHang.Columns["TrangThai"].HeaderText = "Trạng thái";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadKhachHang();
            txtSearch.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadKhachHang();
                return;
            }

            try
            {
                string sql = @"SELECT MaKH, HoTenKH, DiaChiKH, NgayDangKy, TrangThai 
                              FROM BM_USER.KhachHang 
                              WHERE LOWER(HoTenKH) LIKE LOWER(:keyword) 
                                 OR LOWER(DiaChiKH) LIKE LOWER(:keyword)
                                 OR MaKH = :makh
                              ORDER BY MaKH DESC";

                var param = new OracleParameter("keyword", OracleDbType.Varchar2, "%" + keyword + "%", ParameterDirection.Input);

                int maKH = 0;
                int.TryParse(keyword, out maKH);
                var param2 = new OracleParameter("makh", OracleDbType.Int32, maKH, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param, param2);
                if (dt != null)
                {
                    dgvKhachHang.DataSource = dt;
                    MessageBox.Show($"Tìm thấy {dt.Rows.Count} kết quả", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            groupBox1.Visible = true;
            isEditing = false;

            // Generate MaKH mới
            try
            {
                string sql = "SELECT NVL(MAX(MaKH), 0) + 1 AS NewMaKH FROM BM_USER.KhachHang";
                DataTable dt = OracleHelper.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtMaKH.Text = dt.Rows[0]["NewMaKH"].ToString();
                }
            }
            catch { }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần sửa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            groupBox1.Visible = true;
            isEditing = true;
            LoadDetailKhachHang();
        }

        private void LoadDetailKhachHang()
        {
            try
            {
                int maKH = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaKH"].Value);

                string sql = @"SELECT MaKH, HoTenKH, Email, SDT, DiaChiKH 
                              FROM BM_USER.KhachHang 
                              WHERE MaKH = :makh";

                var param = new OracleParameter("makh", OracleDbType.Int32, maKH, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMaKH.Text = row["MaKH"].ToString();
                    txtHoTen.Text = row["HoTenKH"].ToString();
                    txtDiaChi.Text = row["DiaChiKH"].ToString();

                    // Giải mã Email và SDT để hiển thị
                    byte[] encryptedEmail = row["Email"] as byte[];
                    byte[] encryptedSDT = row["SDT"] as byte[];

                    if (encryptedEmail != null)
                        txtEmail.Text = OracleHelper.DecryptAES(encryptedEmail);

                    if (encryptedSDT != null)
                        txtSDT.Text = OracleHelper.DecryptAES(encryptedSDT);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                // Mã hóa Email và SDT
                byte[] encryptedEmail = OracleHelper.EncryptAES(txtEmail.Text.Trim());
                byte[] encryptedSDT = OracleHelper.EncryptAES(txtSDT.Text.Trim());

                if (isEditing)
                {
                    // UPDATE
                    string sql = @"UPDATE BM_USER.KhachHang 
                                  SET HoTenKH = :hoten, 
                                      Email = :email, 
                                      SDT = :sdt, 
                                      DiaChiKH = :diachi 
                                  WHERE MaKH = :makh";

                    var param1 = new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text.Trim(), ParameterDirection.Input);
                    var param2 = new OracleParameter("email", OracleDbType.Raw, encryptedEmail, ParameterDirection.Input);
                    var param3 = new OracleParameter("sdt", OracleDbType.Raw, encryptedSDT, ParameterDirection.Input);
                    var param4 = new OracleParameter("diachi", OracleDbType.NVarchar2, txtDiaChi.Text.Trim(), ParameterDirection.Input);
                    var param5 = new OracleParameter("makh", OracleDbType.Int32, Convert.ToInt32(txtMaKH.Text), ParameterDirection.Input);

                    int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5);

                    if (result > 0)
                    {
                        MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        groupBox1.Visible = false;
                        LoadKhachHang();
                    }
                }
                else
                {
                    // INSERT - Cần tạo GioHang trước
                    int maGH = CreateGioHang();
                    if (maGH <= 0) return;

                    // Hash password mặc định
                    byte[] hashedPassword = OracleHelper.HashPassword("123456");

                    string sql = @"INSERT INTO BM_USER.KhachHang 
                                  (MaKH, HoTenKH, Email, SDT, DiaChiKH, MatKhau, MaGH, TrangThai) 
                                  VALUES (:makh, :hoten, :email, :sdt, :diachi, :matkhau, :magh, 'ACTIVE')";

                    var param1 = new OracleParameter("makh", OracleDbType.Int32, Convert.ToInt32(txtMaKH.Text), ParameterDirection.Input);
                    var param2 = new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text.Trim(), ParameterDirection.Input);
                    var param3 = new OracleParameter("email", OracleDbType.Raw, encryptedEmail, ParameterDirection.Input);
                    var param4 = new OracleParameter("sdt", OracleDbType.Raw, encryptedSDT, ParameterDirection.Input);
                    var param5 = new OracleParameter("diachi", OracleDbType.NVarchar2, txtDiaChi.Text.Trim(), ParameterDirection.Input);
                    var param6 = new OracleParameter("matkhau", OracleDbType.Raw, hashedPassword, ParameterDirection.Input);
                    var param7 = new OracleParameter("magh", OracleDbType.Int32, maGH, ParameterDirection.Input);

                    int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7);

                    if (result > 0)
                    {
                        MessageBox.Show("Thêm khách hàng thành công!\nMật khẩu mặc định: 123456",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        groupBox1.Visible = false;
                        LoadKhachHang();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int CreateGioHang()
        {
            try
            {
                string sql1 = "SELECT NVL(MAX(MaGH), 0) + 1 AS NewMaGH FROM BM_USER.GioHang";
                DataTable dt = OracleHelper.ExecuteQuery(sql1);
                int maGH = Convert.ToInt32(dt.Rows[0]["NewMaGH"]);

                string sql2 = "INSERT INTO BM_USER.GioHang (MaGH) VALUES (:magh)";
                var param = new OracleParameter("magh", OracleDbType.Int32, maGH, ParameterDirection.Input);
                OracleHelper.ExecuteNonQuery(sql2, param);

                return maGH;
            }
            catch
            {
                return 0;
            }
        }

        private bool ValidateForm()
        {
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

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập SĐT!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            ClearForm();
        }

        private void ClearForm()
        {
            txtMaKH.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maKH = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaKH"].Value);
            string hoTen = dgvKhachHang.SelectedRows[0].Cells["HoTenKH"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa khách hàng: {hoTen}?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM BM_USER.KhachHang WHERE MaKH = :makh";
                    var param = new OracleParameter("makh", OracleDbType.Int32, maKH, ParameterDirection.Input);

                    int rowsAffected = OracleHelper.ExecuteNonQuery(sql, param);
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKhachHang();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maKH = Convert.ToInt32(dgvKhachHang.SelectedRows[0].Cells["MaKH"].Value);

                string sql = @"SELECT Email, SDT FROM BM_USER.KhachHang WHERE MaKH = :makh";
                var param = new OracleParameter("makh", OracleDbType.Int32, maKH, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    byte[] encryptedEmail = dt.Rows[0]["Email"] as byte[];
                    byte[] encryptedSDT = dt.Rows[0]["SDT"] as byte[];

                    string decryptedEmail = OracleHelper.DecryptAES(encryptedEmail);
                    string decryptedSDT = OracleHelper.DecryptAES(encryptedSDT);

                    MessageBox.Show($"Thông tin đã giải mã:\n\n" +
                        $"Email: {decryptedEmail}\n" +
                        $"SĐT: {decryptedSDT}",
                        "Giải mã AES", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi giải mã: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Load detail when click
        }
    }
}