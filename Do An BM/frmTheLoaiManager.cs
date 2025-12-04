using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmTheLoaiManager : Form
    {
        public frmTheLoaiManager()
        {
            InitializeComponent();
        }

        private void frmTheLoaiManager_Load(object sender, EventArgs e)
        {
            LoadTheLoai();
        }

        private void LoadTheLoai()
        {
            try
            {
                string sql = "SELECT MaTLS AS \"Mã\", TenTLS AS \"Tên thể loại\" FROM TheLoaiSach ORDER BY MaTLS";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null)
                {
                    dgvTheLoai.DataSource = dt;
                    dgvTheLoai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    lblCount.Text = $"Tổng số: {dt.Rows.Count} thể loại";
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
                if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thể loại!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenTheLoai.Focus();
                    return;
                }

                // Lấy MaTLS mới
                string sqlMax = "SELECT NVL(MAX(MaTLS), 0) + 1 FROM TheLoaiSach";
                DataTable dtMax = OracleHelper.ExecuteQuery(sqlMax);
                int maTLS = Convert.ToInt32(dtMax.Rows[0][0]);

                // Insert
                string sql = "INSERT INTO TheLoaiSach (MaTLS, TenTLS) VALUES (:ma, :ten)";
                var param1 = new OracleParameter("ma", OracleDbType.Int32, maTLS, ParameterDirection.Input);
                var param2 = new OracleParameter("ten", OracleDbType.NVarchar2, txtTenTheLoai.Text, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2);

                if (result > 0)
                {
                    MessageBox.Show("Thêm thể loại thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTheLoai();
                    txtTenTheLoai.Clear();
                    txtTenTheLoai.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm thể loại: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTheLoai.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn thể loại cần sửa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenTheLoai.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thể loại!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenTheLoai.Focus();
                    return;
                }

                int maTLS = Convert.ToInt32(dgvTheLoai.SelectedRows[0].Cells[0].Value);

                // Update
                string sql = "UPDATE TheLoaiSach SET TenTLS = :ten WHERE MaTLS = :ma";
                var param1 = new OracleParameter("ten", OracleDbType.NVarchar2, txtTenTheLoai.Text, ParameterDirection.Input);
                var param2 = new OracleParameter("ma", OracleDbType.Int32, maTLS, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param1, param2);

                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thể loại thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTheLoai();
                    txtTenTheLoai.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật thể loại: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTheLoai.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn thể loại cần xóa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maTLS = Convert.ToInt32(dgvTheLoai.SelectedRows[0].Cells[0].Value);
                string tenTLS = dgvTheLoai.SelectedRows[0].Cells[1].Value.ToString();

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn xóa thể loại '{tenTLS}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No)
                    return;

                // Delete
                string sql = "DELETE FROM TheLoaiSach WHERE MaTLS = :ma";
                var param = new OracleParameter("ma", OracleDbType.Int32, maTLS, ParameterDirection.Input);

                int result = OracleHelper.ExecuteNonQuery(sql, param);

                if (result > 0)
                {
                    MessageBox.Show("Xóa thể loại thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadTheLoai();
                    txtTenTheLoai.Clear();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    MessageBox.Show("Không thể xóa thể loại này vì có sách đang sử dụng!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Lỗi xóa thể loại: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadTheLoai();
            txtTenTheLoai.Clear();
            txtTimKiem.Clear();
        }

        private void dgvTheLoai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTheLoai.SelectedRows.Count > 0)
            {
                txtTenTheLoai.Text = dgvTheLoai.SelectedRows[0].Cells[1].Value.ToString();
            }
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                string sql = @"SELECT MaTLS AS ""Mã"", TenTLS AS ""Tên thể loại"" 
                              FROM TheLoaiSach 
                              WHERE TenTLS LIKE :keyword 
                              ORDER BY MaTLS";

                var param = new OracleParameter("keyword", OracleDbType.NVarchar2,
                    $"%{keyword}%", ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null)
                {
                    dgvTheLoai.DataSource = dt;
                    lblCount.Text = $"Tìm thấy: {dt.Rows.Count} thể loại";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}