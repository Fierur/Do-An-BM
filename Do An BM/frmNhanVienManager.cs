using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmNhanVienManager : Form
    {
        public frmNhanVienManager()
        {
            InitializeComponent();
        }

        private void frmNhanVienManager_Load(object sender, EventArgs e)
        {
            LoadNhanVien();

            // Kiểm tra quyền
            if (!SessionManager.IsAdmin())
            {
                btnDecryptSalary.Enabled = false;
                btnGrantRole.Enabled = false;
                btnAdd.Enabled = false;
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void LoadNhanVien()
        {
            try
            {
                string sql = @"SELECT MaNV, HoTenNV, DiaChiNV, GioiTinhNV, NgayTao 
                              FROM BM_USER.NhanVien 
                              ORDER BY MaNV DESC";

                DataTable dt = OracleHelper.ExecuteQuery(sql);
                if (dt != null)
                {
                    dgvNhanVien.DataSource = dt;
                    dgvNhanVien.Columns["MaNV"].HeaderText = "Mã NV";
                    dgvNhanVien.Columns["HoTenNV"].HeaderText = "Họ tên";
                    dgvNhanVien.Columns["DiaChiNV"].HeaderText = "Địa chỉ";
                    dgvNhanVien.Columns["GioiTinhNV"].HeaderText = "Giới tính";
                    dgvNhanVien.Columns["NgayTao"].HeaderText = "Ngày tạo";
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
            LoadNhanVien();
            txtSearch.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadNhanVien();
                return;
            }

            try
            {
                string sql = @"SELECT MaNV, HoTenNV, DiaChiNV, GioiTinhNV, NgayTao 
                              FROM BM_USER.NhanVien 
                              WHERE LOWER(HoTenNV) LIKE LOWER(:keyword) 
                                 OR LOWER(DiaChiNV) LIKE LOWER(:keyword)
                                 OR MaNV = :manv
                              ORDER BY MaNV DESC";

                var paramList = new System.Collections.Generic.List<OracleParameter>();
                paramList.Add(new OracleParameter("keyword", OracleDbType.Varchar2,
                    "%" + keyword + "%", ParameterDirection.Input));

                int maNV;
                if (int.TryParse(keyword, out maNV))
                {
                    sql += " OR MaNV = :manv";
                    paramList.Add(new OracleParameter("manv", OracleDbType.Int32,
                        maNV, ParameterDirection.Input));
                }

                sql += " ORDER BY MaNV DESC";

                DataTable dt = OracleHelper.ExecuteQuery(sql, paramList.ToArray());
                if (dt != null)
                {
                    dgvNhanVien.DataSource = dt;
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
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ ADMIN mới được thêm nhân viên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmNhanVienDetail frm = new frmNhanVienDetail(0); // 0 = Add mode
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadNhanVien();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ ADMIN mới được sửa nhân viên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần sửa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);
            frmNhanVienDetail frm = new frmNhanVienDetail(maNV); // Edit mode
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadNhanVien();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ ADMIN mới được xóa nhân viên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên cần xóa!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maNV = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);
            string hoTen = dgvNhanVien.SelectedRows[0].Cells["HoTenNV"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc muốn xóa nhân viên: {hoTen}?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Xóa phân quyền trước
                    string sql1 = "DELETE FROM BM_USER.PhanQuyenNV WHERE MaNV = :manv";
                    var param1 = new OracleParameter("manv", OracleDbType.Int32, maNV, ParameterDirection.Input);
                    OracleHelper.ExecuteNonQuery(sql1, param1);

                    // Xóa nhân viên
                    string sql2 = "DELETE FROM BM_USER.NhanVien WHERE MaNV = :manv";
                    var param2 = new OracleParameter("manv", OracleDbType.Int32, maNV, ParameterDirection.Input);
                    int rowsAffected = OracleHelper.ExecuteNonQuery(sql2, param2);

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadNhanVien();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDecryptSalary_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ ADMIN mới được xem Lương/CMND!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maNV = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);

                string sql = @"SELECT HoTenNV, Luong_RSA, CMND_RSA FROM BM_USER.NhanVien WHERE MaNV = :manv";
                var param = new OracleParameter("manv", OracleDbType.Int32, maNV, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string hoTen = dt.Rows[0]["HoTenNV"].ToString();
                    byte[] encryptedLuong = dt.Rows[0]["Luong_RSA"] as byte[];
                    byte[] encryptedCMND = dt.Rows[0]["CMND_RSA"] as byte[];

                    string luong = "N/A";
                    string cmnd = "N/A";

                    if (encryptedLuong != null && encryptedLuong.Length > 0)
                        luong = OracleHelper.DecryptRSA(encryptedLuong);

                    if (encryptedCMND != null && encryptedCMND.Length > 0)
                        cmnd = OracleHelper.DecryptRSA(encryptedCMND);

                    MessageBox.Show($"Thông tin nhạy cảm (Mã hóa RSA-2048):\n\n" +
                        $"Nhân viên: {hoTen}\n" +
                        $"Lương: {luong} VNĐ\n" +
                        $"CMND: {cmnd}",
                        "Giải mã RSA (Admin Only)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi giải mã: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrantRole_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ ADMIN mới được cấp quyền!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvNhanVien.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int staffMaNV = Convert.ToInt32(dgvNhanVien.SelectedRows[0].Cells["MaNV"].Value);
            string hoTen = dgvNhanVien.SelectedRows[0].Cells["HoTenNV"].Value.ToString();

            // Load roles
            string sqlRoles = "SELECT MaVT, TenVT FROM BM_USER.VaiTro ORDER BY MaVT";
            DataTable dtRoles = OracleHelper.ExecuteQuery(sqlRoles);

            if (dtRoles == null || dtRoles.Rows.Count == 0)
            {
                MessageBox.Show("Không có vai trò nào trong hệ thống!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Simple dialog to select role
            Form frmSelectRole = new Form();
            frmSelectRole.Text = $"Cấp quyền cho: {hoTen}";
            frmSelectRole.Size = new System.Drawing.Size(400, 200);
            frmSelectRole.StartPosition = FormStartPosition.CenterParent;

            Label lbl = new Label();
            lbl.Text = "Chọn vai trò:";
            lbl.Location = new System.Drawing.Point(20, 20);
            lbl.AutoSize = true;
            frmSelectRole.Controls.Add(lbl);

            ComboBox cboRole = new ComboBox();
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.DataSource = dtRoles;
            cboRole.DisplayMember = "TenVT";
            cboRole.ValueMember = "MaVT";
            cboRole.Location = new System.Drawing.Point(20, 50);
            cboRole.Size = new System.Drawing.Size(340, 25);
            frmSelectRole.Controls.Add(cboRole);

            Button btnOK = new Button();
            btnOK.Text = "Cấp quyền";
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(200, 110);
            btnOK.Size = new System.Drawing.Size(80, 30);
            frmSelectRole.Controls.Add(btnOK);

            Button btnCancel = new Button();
            btnCancel.Text = "Hủy";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(290, 110);
            btnCancel.Size = new System.Drawing.Size(70, 30);
            frmSelectRole.Controls.Add(btnCancel);

            if (frmSelectRole.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int maVT = Convert.ToInt32(cboRole.SelectedValue);
                    bool result = OracleHelper.AdminGrantRole(SessionManager.CurrentUserID, staffMaNV, maVT);

                    if (result)
                    {
                        MessageBox.Show($"Đã cấp quyền thành công!\n\nNhân viên: {hoTen}\nVai trò: {cboRole.Text}",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cấp quyền: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}