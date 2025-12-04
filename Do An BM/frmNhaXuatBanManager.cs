using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmNhaXuatBanManager : Form
    {
        public frmNhaXuatBanManager()
        {
            InitializeComponent();
        }

        private void frmNhaXuatBanManager_Load(object sender, EventArgs e)
        {
            LoadNXB();
        }

        private void LoadNXB()
        {
            try
            {
                string sql = @"SELECT MaNXB AS ""Mã"", TenNXB AS ""Tên NXB"", 
                              DiaChiNXB AS ""Địa chỉ"", Email, SDT AS ""SĐT"" 
                              FROM NhaXuatBan ORDER BY MaNXB";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null)
                {
                    dgvNXB.DataSource = dt;
                    dgvNXB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    lblCount.Text = $"Tổng số: {dt.Rows.Count} nhà xuất bản";
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

                // Lấy MaNXB mới
                string sqlMax = "SELECT NVL(MAX(MaNXB), 0) + 1 FROM NhaXuatBan";
                DataTable dtMax = OracleHelper.ExecuteQuery(sqlMax);
                int maNXB = Convert.ToInt32(dtMax.Rows[0][0]);

                // Insert
                string sql = @"INSERT INTO NhaXuatBan (MaNXB, TenNXB, DiaChiNXB, Email, SDT) 
                              VALUES (:ma, :ten, :diaChi, :email, :sdt)";

                var param1 = new OracleParameter("ma", OracleDbType.Int32, maNXB, ParameterDirection.Input);
                var param2 = new OracleParameter("ten", OracleDbType.NVarchar2, txtTenNXB.Text, ParameterDirection.Input);
                var param3 = new OracleParameter("diaChi", OracleDbType.NVarchar2, txtDiaChi.Text, ParameterDirection.Input);
                var param4 = new OracleParameter("email", OracleDbType.NVarchar2, txtEmail.Text, ParameterDirection.Input);
                var param5 = new OracleParameter("sdt", OracleDbType.Varchar2, txtSDT.Text, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5);

                if (result > 0)
                {
                    MessageBox.Show("Thêm nhà xuất bản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNXB();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm NXB: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNXB.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn NXB cần sửa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInput())
                    return;

                int maNXB = Convert.ToInt32(dgvNXB.SelectedRows[0].Cells[0].Value);

                // Update
                string sql = @"UPDATE NhaXuatBan 
                              SET TenNXB = :ten, DiaChiNXB = :diaChi, 
                                  Email = :email, SDT = :sdt 
                              WHERE MaNXB = :ma";

                var param1 = new OracleParameter("ten", OracleDbType.NVarchar2, txtTenNXB.Text, ParameterDirection.Input);
                var param2 = new OracleParameter("diaChi", OracleDbType.NVarchar2, txtDiaChi.Text, ParameterDirection.Input);
                var param3 = new OracleParameter("email", OracleDbType.NVarchar2, txtEmail.Text, ParameterDirection.Input);
                var param4 = new OracleParameter("sdt", OracleDbType.Varchar2, txtSDT.Text, ParameterDirection.Input);
                var param5 = new OracleParameter("ma", OracleDbType.Int32, maNXB, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5);

                if (result > 0)
                {
                    MessageBox.Show("Cập nhật nhà xuất bản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNXB();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật NXB: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvNXB.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn NXB cần xóa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maNXB = Convert.ToInt32(dgvNXB.SelectedRows[0].Cells[0].Value);
                string tenNXB = dgvNXB.SelectedRows[0].Cells[1].Value.ToString();

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn xóa NXB '{tenNXB}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No)
                    return;

                // Delete
                string sql = "DELETE FROM NhaXuatBan WHERE MaNXB = :ma";
                var param = new OracleParameter("ma", OracleDbType.Int32, maNXB, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param);

                if (result > 0)
                {
                    MessageBox.Show("Xóa nhà xuất bản thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadNXB();
                    ClearInputs();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    MessageBox.Show("Không thể xóa NXB này vì có sách đang sử dụng!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Lỗi xóa NXB: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadNXB();
            ClearInputs();
            txtTimKiem.Clear();
        }

        private void dgvNXB_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvNXB.SelectedRows.Count > 0)
            {
                txtTenNXB.Text = dgvNXB.SelectedRows[0].Cells[1].Value?.ToString() ?? "";
                txtDiaChi.Text = dgvNXB.SelectedRows[0].Cells[2].Value?.ToString() ?? "";
                txtEmail.Text = dgvNXB.SelectedRows[0].Cells[3].Value?.ToString() ?? "";
                txtSDT.Text = dgvNXB.SelectedRows[0].Cells[4].Value?.ToString() ?? "";
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string sql = @"SELECT MaNXB AS ""Mã"", TenNXB AS ""Tên NXB"", 
                              DiaChiNXB AS ""Địa chỉ"", Email, SDT AS ""SĐT"" 
                              FROM NhaXuatBan 
                              WHERE TenNXB LIKE :keyword 
                              ORDER BY MaNXB";

                var param = new OracleParameter("keyword", OracleDbType.NVarchar2,
                    $"%{keyword}%", ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null)
                {
                    dgvNXB.DataSource = dt;
                    lblCount.Text = $"Tìm thấy: {dt.Rows.Count} NXB";
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
            if (string.IsNullOrWhiteSpace(txtTenNXB.Text))
            {
                MessageBox.Show("Vui lòng nhập tên NXB!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNXB.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private void ClearInputs()
        {
            txtTenNXB.Clear();
            txtDiaChi.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            txtTenNXB.Focus();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}