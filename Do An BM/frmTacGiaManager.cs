using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmTacGiaManager : Form
    {
        public frmTacGiaManager()
        {
            InitializeComponent();
        }

        private void frmTacGiaManager_Load(object sender, EventArgs e)
        {
            cboGioiTinh.Items.AddRange(new string[] { "Nam", "Nữ", "Khác" });
            cboGioiTinh.SelectedIndex = 0;
            LoadTacGia();
        }

        private void LoadTacGia()
        {
            try
            {
                string sql = @"SELECT MaTG AS ""Mã"", HoTenTG AS ""Họ tên"", 
                              ButDanh AS ""Bút danh"", 
                              TO_CHAR(NgaySinh, 'DD/MM/YYYY') AS ""Ngày sinh"", 
                              GioiTinh AS ""Giới tính"" 
                              FROM TacGia ORDER BY MaTG";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null)
                {
                    dgvTacGia.DataSource = dt;
                    dgvTacGia.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    lblCount.Text = $"Tổng số: {dt.Rows.Count} tác giả";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                // Lấy MaTG mới
                string sqlMax = "SELECT NVL(MAX(MaTG), 0) + 1 FROM TacGia";
                DataTable dtMax = OracleHelper.ExecuteQuery(sqlMax);
                int maTG = Convert.ToInt32(dtMax.Rows[0][0]);

                // Insert
                string sql = @"INSERT INTO TacGia (MaTG, HoTenTG, ButDanh, NgaySinh, GioiTinh) 
                              VALUES (:ma, :hoTen, :butDanh, :ngaySinh, :gioiTinh)";

                var param1 = new OracleParameter("ma", OracleDbType.Int32, maTG, ParameterDirection.Input);
                var param2 = new OracleParameter("hoTen", OracleDbType.NVarchar2, txtHoTen.Text, ParameterDirection.Input);

                var param3 = new OracleParameter("butDanh", OracleDbType.NVarchar2);
                param3.Value = string.IsNullOrWhiteSpace(txtButDanh.Text) ? DBNull.Value : (object)txtButDanh.Text;

                var param4 = new OracleParameter("ngaySinh", OracleDbType.Date);
                param4.Value = dtpNgaySinh.Checked ? (object)dtpNgaySinh.Value : DBNull.Value;

                var param5 = new OracleParameter("gioiTinh", OracleDbType.NVarchar2, cboGioiTinh.Text, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5);

                if (result > 0)
                {
                    MessageBox.Show("Thêm tác giả thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTacGia();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm tác giả: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTacGia.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tác giả cần sửa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput())
                    return;

                int maTG = Convert.ToInt32(dgvTacGia.SelectedRows[0].Cells[0].Value);

                // Update
                string sql = @"UPDATE TacGia 
                              SET HoTenTG = :hoTen, ButDanh = :butDanh, 
                                  NgaySinh = :ngaySinh, GioiTinh = :gioiTinh 
                              WHERE MaTG = :ma";

                var param1 = new OracleParameter("hoTen", OracleDbType.NVarchar2, txtHoTen.Text, ParameterDirection.Input);

                var param2 = new OracleParameter("butDanh", OracleDbType.NVarchar2);
                param2.Value = string.IsNullOrWhiteSpace(txtButDanh.Text) ? DBNull.Value : (object)txtButDanh.Text;

                var param3 = new OracleParameter("ngaySinh", OracleDbType.Date);
                param3.Value = dtpNgaySinh.Checked ? (object)dtpNgaySinh.Value : DBNull.Value;

                var param4 = new OracleParameter("gioiTinh", OracleDbType.NVarchar2, cboGioiTinh.Text, ParameterDirection.Input);
                var param5 = new OracleParameter("ma", OracleDbType.Int32, maTG, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5);

                if (result > 0)
                {
                    MessageBox.Show("Cập nhật tác giả thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTacGia();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật tác giả: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTacGia.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tác giả cần xóa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTG = Convert.ToInt32(dgvTacGia.SelectedRows[0].Cells[0].Value);
                string tenTG = dgvTacGia.SelectedRows[0].Cells[1].Value.ToString();

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn xóa tác giả '{tenTG}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No)
                    return;

                // Delete
                string sql = "DELETE FROM TacGia WHERE MaTG = :ma";
                var param = new OracleParameter("ma", OracleDbType.Int32, maTG, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param);

                if (result > 0)
                {
                    MessageBox.Show("Xóa tác giả thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTacGia();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    MessageBox.Show("Không thể xóa tác giả này vì có sách đang sử dụng!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Lỗi xóa tác giả: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadTacGia();
            ClearInputs();
            txtTimKiem.Clear();
        }

        private void dgvTacGia_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTacGia.SelectedRows.Count > 0)
            {
                txtHoTen.Text = dgvTacGia.SelectedRows[0].Cells[1].Value?.ToString() ?? "";
                txtButDanh.Text = dgvTacGia.SelectedRows[0].Cells[2].Value?.ToString() ?? "";

                string ngaySinhStr = dgvTacGia.SelectedRows[0].Cells[3].Value?.ToString();
                if (!string.IsNullOrEmpty(ngaySinhStr))
                {
                    DateTime ngaySinh;
                    if (DateTime.TryParse(ngaySinhStr, out ngaySinh))
                    {
                        dtpNgaySinh.Value = ngaySinh;
                        dtpNgaySinh.Checked = true;
                    }
                }
                else
                {
                    dtpNgaySinh.Checked = false;
                }

                string gioiTinh = dgvTacGia.SelectedRows[0].Cells[4].Value?.ToString() ?? "Nam";
                cboGioiTinh.SelectedItem = gioiTinh;
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string sql = @"SELECT MaTG AS ""Mã"", HoTenTG AS ""Họ tên"", 
                              ButDanh AS ""Bút danh"", 
                              TO_CHAR(NgaySinh, 'DD/MM/YYYY') AS ""Ngày sinh"", 
                              GioiTinh AS ""Giới tính"" 
                              FROM TacGia 
                              WHERE HoTenTG LIKE :keyword OR ButDanh LIKE :keyword
                              ORDER BY MaTG";

                var param = new OracleParameter("keyword", OracleDbType.NVarchar2,
                    $"%{keyword}%", ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null)
                {
                    dgvTacGia.DataSource = dt;
                    lblCount.Text = $"Tìm thấy: {dt.Rows.Count} tác giả";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên tác giả!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtHoTen.Clear();
            txtButDanh.Clear();
            dtpNgaySinh.Checked = false;
            cboGioiTinh.SelectedIndex = 0;
            txtHoTen.Focus();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}