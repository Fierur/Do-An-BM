using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmSachDetail : Form
    {
        private int? _maSach = null;
        private bool _isEditMode = false;

        public frmSachDetail()
        {
            InitializeComponent();
            _isEditMode = false;
        }

        public frmSachDetail(int maSach)
        {
            InitializeComponent();
            _maSach = maSach;
            _isEditMode = true;
        }

        private void frmSachDetail_Load(object sender, EventArgs e)
        {
            LoadTheLoai();
            LoadNXB();
            LoadTacGia();

            if (_isEditMode && _maSach.HasValue)
            {
                LoadSachInfo(_maSach.Value);
                this.Text = "Sửa thông tin sách";
                lblTitle.Text = "SỬA THÔNG TIN SÁCH";
            }
            else
            {
                this.Text = "Thêm sách mới";
                lblTitle.Text = "THÊM SÁCH MỚI";
            }
        }

        private void LoadTheLoai()
        {
            try
            {
                string sql = "SELECT MaTLS, TenTLS FROM TheLoaiSach ORDER BY TenTLS";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null)
                {
                    cboTheLoai.DataSource = dt;
                    cboTheLoai.DisplayMember = "TenTLS";
                    cboTheLoai.ValueMember = "MaTLS";
                    cboTheLoai.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải thể loại: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNXB()
        {
            try
            {
                string sql = "SELECT MaNXB, TenNXB FROM NhaXuatBan ORDER BY TenNXB";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null)
                {
                    cboNXB.DataSource = dt;
                    cboNXB.DisplayMember = "TenNXB";
                    cboNXB.ValueMember = "MaNXB";
                    cboNXB.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải NXB: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTacGia()
        {
            try
            {
                string sql = "SELECT MaTG, HoTenTG FROM TacGia ORDER BY HoTenTG";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    clbTacGia.DataSource = dt;
                    clbTacGia.DisplayMember = "HoTenTG";
                    clbTacGia.ValueMember = "MaTG";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải tác giả: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSachInfo(int maSach)
        {
            try
            {
                // Load thông tin sách
                string sql = "SELECT * FROM Sach WHERE MaSach = :maSach";
                var param = new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtTenSach.Text = row["TenSach"]?.ToString() ?? "";
                    txtGia.Text = row["Gia"]?.ToString() ?? "";
                    txtNgonNgu.Text = row["AddNgonNgu"]?.ToString() ?? "";
                    txtSoTrang.Text = row["SoTrang"]?.ToString() ?? "";
                    txtMoTa.Text = row["Mota"]?.ToString() ?? "";
                    txtHinh.Text = row["Hinh"]?.ToString() ?? "";

                    // Set thể loại
                    if (row["MaTLS"] != DBNull.Value)
                        cboTheLoai.SelectedValue = Convert.ToInt32(row["MaTLS"]);

                    // Set NXB
                    if (row["MaNXB"] != DBNull.Value)
                        cboNXB.SelectedValue = Convert.ToInt32(row["MaNXB"]);
                }

                // Load tác giả của sách
                string sqlTG = "SELECT MaTG FROM Sach_TacGia WHERE MaSach = :maSach";
                var paramTG = new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input);
                DataTable dtTG = OracleHelper.ExecuteQuery(sqlTG, paramTG);

                if (dtTG != null)
                {
                    for (int i = 0; i < clbTacGia.Items.Count; i++)
                    {
                        DataRowView item = (DataRowView)clbTacGia.Items[i];
                        int maTG = Convert.ToInt32(item["MaTG"]);

                        foreach (DataRow rowTG in dtTG.Rows)
                        {
                            if (Convert.ToInt32(rowTG["MaTG"]) == maTG)
                            {
                                clbTacGia.SetItemChecked(i, true);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải thông tin sách: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                if (_isEditMode && _maSach.HasValue)
                {
                    UpdateSach(_maSach.Value);
                }
                else
                {
                    InsertSach();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu sách: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertSach()
        {
            // Lấy MaSach mới
            string sqlMax = "SELECT NVL(MAX(MaSach), 0) + 1 FROM Sach";
            DataTable dtMax = OracleHelper.ExecuteQuery(sqlMax);
            int maSach = Convert.ToInt32(dtMax.Rows[0][0]);

            // Insert Sach
            string sql = @"INSERT INTO Sach (MaSach, TenSach, Gia, AddNgonNgu, SoTrang, Mota, MaTLS, MaNXB, Hinh)
                          VALUES (:maSach, :tenSach, :gia, :ngonNgu, :soTrang, :moTa, :maTLS, :maNXB, :hinh)";

            var param1 = new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input);
            var param2 = new OracleParameter("tenSach", OracleDbType.NVarchar2, txtTenSach.Text, ParameterDirection.Input);
            var param3 = new OracleParameter("gia", OracleDbType.Decimal, decimal.Parse(txtGia.Text), ParameterDirection.Input);
            var param4 = new OracleParameter("ngonNgu", OracleDbType.NVarchar2, txtNgonNgu.Text, ParameterDirection.Input);
            var param5 = new OracleParameter("soTrang", OracleDbType.Int32, int.Parse(txtSoTrang.Text), ParameterDirection.Input);

            var param6 = new OracleParameter("moTa", OracleDbType.NVarchar2);
            param6.Value = string.IsNullOrWhiteSpace(txtMoTa.Text) ? DBNull.Value : (object)txtMoTa.Text;

            var param7 = new OracleParameter("maTLS", OracleDbType.Int32);
            param7.Value = cboTheLoai.SelectedValue != null ? cboTheLoai.SelectedValue : DBNull.Value;

            var param8 = new OracleParameter("maNXB", OracleDbType.Int32);
            param8.Value = cboNXB.SelectedValue != null ? cboNXB.SelectedValue : DBNull.Value;

            var param9 = new OracleParameter("hinh", OracleDbType.Varchar2);
            param9.Value = string.IsNullOrWhiteSpace(txtHinh.Text) ? DBNull.Value : (object)txtHinh.Text;

            int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7, param8, param9);

            if (result > 0)
            {
                // Insert tác giả
                InsertTacGia(maSach);
                MessageBox.Show("Thêm sách thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateSach(int maSach)
        {
            string sql = @"UPDATE Sach 
                          SET TenSach = :tenSach, Gia = :gia, AddNgonNgu = :ngonNgu, 
                              SoTrang = :soTrang, Mota = :moTa, MaTLS = :maTLS, 
                              MaNXB = :maNXB, Hinh = :hinh
                          WHERE MaSach = :maSach";

            var param1 = new OracleParameter("tenSach", OracleDbType.NVarchar2, txtTenSach.Text, ParameterDirection.Input);
            var param2 = new OracleParameter("gia", OracleDbType.Decimal, decimal.Parse(txtGia.Text), ParameterDirection.Input);
            var param3 = new OracleParameter("ngonNgu", OracleDbType.NVarchar2, txtNgonNgu.Text, ParameterDirection.Input);
            var param4 = new OracleParameter("soTrang", OracleDbType.Int32, int.Parse(txtSoTrang.Text), ParameterDirection.Input);

            var param5 = new OracleParameter("moTa", OracleDbType.NVarchar2);
            param5.Value = string.IsNullOrWhiteSpace(txtMoTa.Text) ? DBNull.Value : (object)txtMoTa.Text;

            var param6 = new OracleParameter("maTLS", OracleDbType.Int32);
            param6.Value = cboTheLoai.SelectedValue != null ? cboTheLoai.SelectedValue : DBNull.Value;

            var param7 = new OracleParameter("maNXB", OracleDbType.Int32);
            param7.Value = cboNXB.SelectedValue != null ? cboNXB.SelectedValue : DBNull.Value;

            var param8 = new OracleParameter("hinh", OracleDbType.Varchar2);
            param8.Value = string.IsNullOrWhiteSpace(txtHinh.Text) ? DBNull.Value : (object)txtHinh.Text;

            var param9 = new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input);

            int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7, param8, param9);

            if (result > 0)
            {
                // Xóa tác giả cũ
                string sqlDelete = "DELETE FROM Sach_TacGia WHERE MaSach = :maSach";
                var paramDelete = new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input);
                OracleHelper.ExecuteNonQuery(sqlDelete, paramDelete);

                // Insert tác giả mới
                InsertTacGia(maSach);
                MessageBox.Show("Cập nhật sách thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InsertTacGia(int maSach)
        {
            foreach (int index in clbTacGia.CheckedIndices)
            {
                DataRowView item = (DataRowView)clbTacGia.Items[index];
                int maTG = Convert.ToInt32(item["MaTG"]);

                string sql = "INSERT INTO Sach_TacGia (MaSach, MaTG) VALUES (:maSach, :maTG)";
                var param1 = new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input);
                var param2 = new OracleParameter("maTG", OracleDbType.Int32, maTG, ParameterDirection.Input);

                OracleHelper.ExecuteNonQuery(sql, param1, param2);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSach.Focus();
                return false;
            }

            decimal gia;
            if (!decimal.TryParse(txtGia.Text, out gia) || gia <= 0)
            {
                MessageBox.Show("Giá không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGia.Focus();
                return false;
            }

            int soTrang;
            if (!int.TryParse(txtSoTrang.Text, out soTrang) || soTrang <= 0)
            {
                MessageBox.Show("Số trang không hợp lệ!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTrang.Focus();
                return false;
            }

            if (clbTacGia.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 tác giả!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChonHinh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtHinh.Text = ofd.FileName;
                }
            }
        }
    }
}