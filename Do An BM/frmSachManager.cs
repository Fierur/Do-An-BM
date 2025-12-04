using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmSachManager : Form
    {
        public frmSachManager()
        {
            InitializeComponent();
        }

        private void frmSachManager_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền (Admin hoặc Staff có thể xem)
            if (!SessionManager.IsAdmin() && !SessionManager.IsStaff())
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Staff chỉ có quyền xem (read-only)
            if (SessionManager.IsStaff())
            {
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }

            LoadTheLoai();
            LoadSach();
        }

        private void LoadTheLoai()
        {
            try
            {
                string sql = "SELECT MaTLS, TenTLS FROM TheLoaiSach ORDER BY TenTLS";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                cboTheLoai.DataSource = dt;
                cboTheLoai.DisplayMember = "TenTLS";
                cboTheLoai.ValueMember = "MaTLS";

                // Thêm option "Tất cả"
                DataRow row = dt.NewRow();
                row["MaTLS"] = 0;
                row["TenTLS"] = "-- Tất cả --";
                dt.Rows.InsertAt(row, 0);

                cboTheLoai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thể loại: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSach(int? maTheLoai = null, string search = null)
        {
            try
            {
                string sql = @"
                    SELECT 
                        s.MaSach,
                        s.TenSach,
                        s.Gia,
                        s.SoTrang,
                        s.AddNgonNgu AS NgonNgu,
                        tls.TenTLS AS TheLoai,
                        nxb.TenNXB AS NhaXuatBan,
                        TO_CHAR(s.NgayNhap, 'DD/MM/YYYY') AS NgayNhap
                    FROM Sach s
                    LEFT JOIN TheLoaiSach tls ON s.MaTLS = tls.MaTLS
                    LEFT JOIN NhaXuatBan nxb ON s.MaNXB = nxb.MaNXB
                    WHERE 1=1
                ";

                var parameters = new System.Collections.Generic.List<OracleParameter>();

                // Filter theo thể loại
                if (maTheLoai.HasValue && maTheLoai.Value > 0)
                {
                    sql += " AND s.MaTLS = :matls";
                    parameters.Add(new OracleParameter("matls", OracleDbType.Int32,
                        maTheLoai.Value, ParameterDirection.Input));
                }

                // Filter theo tên sách
                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND UPPER(s.TenSach) LIKE UPPER(:search)";
                    parameters.Add(new OracleParameter("search", OracleDbType.Varchar2,
                        "%" + search + "%", ParameterDirection.Input));
                }

                sql += " ORDER BY s.MaSach DESC";

                DataTable dt = OracleHelper.ExecuteQuery(sql, parameters.ToArray());
                dgvSach.DataSource = dt;
                dgvSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Format giá
                if (dgvSach.Columns["Gia"] != null)
                {
                    dgvSach.Columns["Gia"].DefaultCellStyle.Format = "N0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboTheLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTheLoai.SelectedValue != null)
            {
                int maTheLoai = Convert.ToInt32(cboTheLoai.SelectedValue);
                LoadSach(maTheLoai > 0 ? maTheLoai : (int?)null);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int? maTheLoai = null;
            if (cboTheLoai.SelectedValue != null)
            {
                int temp = Convert.ToInt32(cboTheLoai.SelectedValue);
                if (temp > 0) maTheLoai = temp;
            }

            LoadSach(maTheLoai, txtSearch.Text.Trim());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cboTheLoai.SelectedIndex = 0;
            LoadSach();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm sách đang phát triển.\n" +
                "Cần tạo form frmSachDetail để nhập thông tin sách mới.", "Thông báo");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSach.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Chức năng sửa sách đang phát triển.\n" +
                "Cần tạo form frmSachDetail để sửa thông tin sách.", "Thông báo");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSach.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn sách cần xóa!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int maSach = Convert.ToInt32(dgvSach.SelectedRows[0].Cells["MaSach"].Value);
                string tenSach = dgvSach.SelectedRows[0].Cells["TenSach"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa sách:\n\n{tenSach}?",
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                // Xóa sách
                string sql = "DELETE FROM Sach WHERE MaSach = :masach";
                var param = new OracleParameter("masach", OracleDbType.Int32,
                    maSach, ParameterDirection.Input);

                int rowsAffected = OracleHelper.ExecuteNonQuery(sql, param);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Xóa sách thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSach();
                }
                else
                {
                    MessageBox.Show("Không thể xóa sách!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sách: " + ex.Message + "\n\n" +
                    "Có thể sách đã được sử dụng trong đơn hàng.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}