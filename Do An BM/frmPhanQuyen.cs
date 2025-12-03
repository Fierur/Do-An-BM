using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmPhanQuyen : Form
    {
        private int adminMaNV;

        public frmPhanQuyen(int maNV)
        {
            InitializeComponent();
            this.adminMaNV = maNV;
        }

        private void frmPhanQuyen_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin
            if (!CheckAdminPermission())
            {
                MessageBox.Show("Chỉ ADMIN mới có quyền truy cập form này!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            lblAdminInfo.Text = $"Admin: {Database.User} (MaNV: {adminMaNV})";

            LoadDanhSachNhanVien();
            LoadDanhSachVaiTro();
            LoadPhanQuyenHienTai();
        }

        private bool CheckAdminPermission()
        {
            try
            {
                if (Database.Con == null || Database.Con.State != ConnectionState.Open)
                {
                    MessageBox.Show("Chưa kết nối đến cơ sở dữ liệu!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Kiểm tra vai trò của user hiện tại
                string sql = "SELECT BM_USER.GET_ROLE(:maNV) FROM DUAL";
                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("maNV", OracleDbType.Int32).Value = adminMaNV;

                string role = cmd.ExecuteScalar()?.ToString();

                if (role != "ADMIN")
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kiểm tra quyền: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LoadDanhSachNhanVien()
        {
            try
            {
                string sql = @"SELECT 
                                nv.MaNV AS ""Mã NV"",
                                nv.HoTenNV AS ""Họ tên"",
                                BM_USER.DECRYPT_AES(nv.Email) AS ""Email"",
                                BM_USER.DECRYPT_AES(nv.SDTNV) AS ""SĐT"",
                                nv.GioiTinhNV AS ""Giới tính"",
                                TO_CHAR(nv.NgayTao, 'DD/MM/YYYY') AS ""Ngày tạo"",
                                NVL(vt.TenVT, 'Chưa phân quyền') AS ""Vai trò hiện tại""
                              FROM NhanVien nv
                              LEFT JOIN PhanQuyenNV pq ON nv.MaNV = pq.MaNV
                              LEFT JOIN VaiTro vt ON pq.MaVT = vt.MaVT
                              ORDER BY nv.MaNV";

                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvNhanVien.DataSource = dt;
                dgvNhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvNhanVien.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách nhân viên: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDanhSachVaiTro()
        {
            try
            {
                string sql = "SELECT MaVT, TenVT, MoTa FROM VaiTro ORDER BY MaVT";

                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboVaiTro.DataSource = dt;
                cboVaiTro.DisplayMember = "TenVT";
                cboVaiTro.ValueMember = "MaVT";
                cboVaiTro.SelectedIndex = -1;

                // Hiển thị thông tin vai trò
                dgvVaiTro.DataSource = dt.Copy();
                dgvVaiTro.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách vai trò: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPhanQuyenHienTai()
        {
            try
            {
                string sql = @"SELECT 
                                pq.MaNV AS ""Mã NV"",
                                nv.HoTenNV AS ""Họ tên"",
                                vt.TenVT AS ""Vai trò"",
                                vt.MoTa AS ""Mô tả"",
                                TO_CHAR(pq.NgayPhanQuyen, 'DD/MM/YYYY HH24:MI:SS') AS ""Ngày phân quyền"",
                                pq.NguoiPhanQuyen AS ""Người phân quyền""
                              FROM PhanQuyenNV pq
                              JOIN NhanVien nv ON pq.MaNV = nv.MaNV
                              JOIN VaiTro vt ON pq.MaVT = vt.MaVT
                              ORDER BY pq.NgayPhanQuyen DESC";

                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvPhanQuyen.DataSource = dt;
                dgvPhanQuyen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải lịch sử phân quyền: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

                    txtMaNV.Text = row.Cells["Mã NV"].Value?.ToString() ?? "";
                    txtHoTen.Text = row.Cells["Họ tên"].Value?.ToString() ?? "";
                    txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
                    txtSDT.Text = row.Cells["SĐT"].Value?.ToString() ?? "";

                    string vaiTroHienTai = row.Cells["Vai trò hiện tại"].Value?.ToString() ?? "";
                    lblVaiTroHienTai.Text = $"Vai trò hiện tại: {vaiTroHienTai}";

                    // Tự động chọn vai trò trong combobox nếu có
                    if (vaiTroHienTai != "Chưa phân quyền")
                    {
                        for (int i = 0; i < cboVaiTro.Items.Count; i++)
                        {
                            DataRowView drv = (DataRowView)cboVaiTro.Items[i];
                            if (drv["TenVT"].ToString() == vaiTroHienTai)
                            {
                                cboVaiTro.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi chọn nhân viên: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cboVaiTro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboVaiTro.SelectedIndex >= 0)
            {
                DataRowView drv = (DataRowView)cboVaiTro.SelectedItem;
                lblMoTaVaiTro.Text = "Mô tả: " + drv["MoTa"].ToString();
            }
        }

        private void btnCapQuyen_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần cấp quyền!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboVaiTro.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn vai trò!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int staffMaNV = int.Parse(txtMaNV.Text);
                int maVT = Convert.ToInt32(cboVaiTro.SelectedValue);
                string tenVT = ((DataRowView)cboVaiTro.SelectedItem)["TenVT"].ToString();

                // Xác nhận
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn cấp quyền '{tenVT}' cho nhân viên {txtHoTen.Text}?\n\n" +
                    $"Thao tác này sẽ XÓA vai trò cũ (nếu có) và gán vai trò mới.",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                // Gọi procedure ADMIN_GRANT_ROLE
                string sql = "BEGIN BM_USER.ADMIN_GRANT_ROLE(:adminMaNV, :staffMaNV, :maVT); END;";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("adminMaNV", OracleDbType.Int32).Value = adminMaNV;
                cmd.Parameters.Add("staffMaNV", OracleDbType.Int32).Value = staffMaNV;
                cmd.Parameters.Add("maVT", OracleDbType.Int32).Value = maVT;

                cmd.ExecuteNonQuery();

                MessageBox.Show($"Đã cấp quyền '{tenVT}' thành công cho {txtHoTen.Text}!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh dữ liệu
                LoadDanhSachNhanVien();
                LoadPhanQuyenHienTai();
                ClearForm();
            }
            catch (OracleException ex)
            {
                if (ex.Number == 20001)
                {
                    MessageBox.Show("Chỉ ADMIN mới được cấp quyền!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Lỗi Oracle: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cấp quyền: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThuHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtMaNV.Text))
                {
                    MessageBox.Show("Vui lòng chọn nhân viên cần thu hồi quyền!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int staffMaNV = int.Parse(txtMaNV.Text);

                // Xác nhận
                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn THU HỒI tất cả quyền của nhân viên {txtHoTen.Text}?\n\n" +
                    $"Nhân viên sẽ không thể đăng nhập vào hệ thống!",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                    return;

                // Xóa quyền
                string sql = "DELETE FROM PhanQuyenNV WHERE MaNV = :maNV";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("maNV", OracleDbType.Int32).Value = staffMaNV;

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show($"Đã thu hồi quyền của {txtHoTen.Text}!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadDanhSachNhanVien();
                    LoadPhanQuyenHienTai();
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("Nhân viên chưa có quyền nào!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thu hồi quyền: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachNhanVien();
            LoadDanhSachVaiTro();
            LoadPhanQuyenHienTai();
            ClearForm();

            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtMaNV.Clear();
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            cboVaiTro.SelectedIndex = -1;
            lblVaiTroHienTai.Text = "Vai trò hiện tại: -";
            lblMoTaVaiTro.Text = "Mô tả: -";
            dgvNhanVien.ClearSelection();
        }
    }
}