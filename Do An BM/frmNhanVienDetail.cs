using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmNhanVienDetail : Form
    {
        private int _maNV;
        private bool _isEditMode;

        public frmNhanVienDetail(int maNV)
        {
            InitializeComponent();
            _maNV = maNV;
            _isEditMode = (maNV > 0);
        }

        private void frmNhanVienDetail_Load(object sender, EventArgs e)
        {
            cboGioiTinh.SelectedIndex = 0;

            if (_isEditMode)
            {
                lblTitle.Text = "SỬA THÔNG TIN NHÂN VIÊN";
                LoadNhanVienDetail();
                lblMatKhau.Text = "Mật khẩu mới (để trống nếu không đổi):";
            }
            else
            {
                lblTitle.Text = "THÊM NHÂN VIÊN MỚI";
                GenerateNewMaNV();
            }
        }

        private void GenerateNewMaNV()
        {
            try
            {
                string sql = "SELECT NVL(MAX(MaNV), 0) + 1 AS NewMaNV FROM BM_USER.NhanVien";
                DataTable dt = OracleHelper.ExecuteQuery(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtMaNV.Text = dt.Rows[0]["NewMaNV"].ToString();
                }
            }
            catch { }
        }

        private void LoadNhanVienDetail()
        {
            try
            {
                string sql = @"SELECT MaNV, HoTenNV, SDTNV, Email, DiaChiNV, GioiTinhNV, Luong_RSA, CMND_RSA 
                              FROM BM_USER.NhanVien 
                              WHERE MaNV = :manv";

                var param = new OracleParameter("manv", OracleDbType.Int32, _maNV, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMaNV.Text = row["MaNV"].ToString();
                    txtHoTen.Text = row["HoTenNV"].ToString();
                    txtDiaChi.Text = row["DiaChiNV"].ToString();
                    cboGioiTinh.Text = row["GioiTinhNV"].ToString();

                    // Giải mã AES
                    byte[] encryptedSDT = row["SDTNV"] as byte[];
                    byte[] encryptedEmail = row["Email"] as byte[];

                    if (encryptedSDT != null)
                        txtSDT.Text = OracleHelper.DecryptAES(encryptedSDT);

                    if (encryptedEmail != null)
                        txtEmail.Text = OracleHelper.DecryptAES(encryptedEmail);

                    // Giải mã RSA (chỉ Admin)
                    if (SessionManager.IsAdmin())
                    {
                        byte[] encryptedLuong = row["Luong_RSA"] as byte[];
                        byte[] encryptedCMND = row["CMND_RSA"] as byte[];

                        if (encryptedLuong != null)
                            txtLuong.Text = OracleHelper.DecryptRSA(encryptedLuong);

                        if (encryptedCMND != null)
                            txtCMND.Text = OracleHelper.DecryptRSA(encryptedCMND);
                    }
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
                // Mã hóa AES
                byte[] encryptedSDT = OracleHelper.EncryptAES(txtSDT.Text.Trim());
                byte[] encryptedEmail = OracleHelper.EncryptAES(txtEmail.Text.Trim());

                // Mã hóa RSA
                byte[] encryptedLuong = null;
                byte[] encryptedCMND = null;

                if (!string.IsNullOrWhiteSpace(txtLuong.Text))
                    encryptedLuong = OracleHelper.EncryptRSA(txtLuong.Text.Trim());

                if (!string.IsNullOrWhiteSpace(txtCMND.Text))
                    encryptedCMND = OracleHelper.EncryptRSA(txtCMND.Text.Trim());

                if (_isEditMode)
                {
                    // UPDATE
                    string sql = @"UPDATE BM_USER.NhanVien 
                                  SET HoTenNV = :hoten,
                                      SDTNV = :sdt,
                                      Email = :email,
                                      DiaChiNV = :diachi,
                                      GioiTinhNV = :gioitinh,
                                      Luong_RSA = :luong,
                                      CMND_RSA = :cmnd";

                    // Nếu có mật khẩu mới
                    if (!string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        sql += ", MatKhauNV = :matkhau";
                    }

                    sql += " WHERE MaNV = :manv";

                    var param1 = new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text.Trim(), ParameterDirection.Input);
                    var param2 = new OracleParameter("sdt", OracleDbType.Raw, encryptedSDT, ParameterDirection.Input);
                    var param3 = new OracleParameter("email", OracleDbType.Raw, encryptedEmail, ParameterDirection.Input);
                    var param4 = new OracleParameter("diachi", OracleDbType.NVarchar2, txtDiaChi.Text.Trim(), ParameterDirection.Input);
                    var param5 = new OracleParameter("gioitinh", OracleDbType.NVarchar2, cboGioiTinh.Text, ParameterDirection.Input);
                    var param6 = new OracleParameter("luong", OracleDbType.Raw, encryptedLuong ?? (object)DBNull.Value, ParameterDirection.Input);
                    var param7 = new OracleParameter("cmnd", OracleDbType.Raw, encryptedCMND ?? (object)DBNull.Value, ParameterDirection.Input);
                    var param8 = new OracleParameter("manv", OracleDbType.Int32, _maNV, ParameterDirection.Input);

                    if (!string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        byte[] hashedPassword = OracleHelper.HashPassword(txtMatKhau.Text.Trim());
                        var param9 = new OracleParameter("matkhau", OracleDbType.Raw, hashedPassword, ParameterDirection.Input);
                        int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7, param9, param8);
                    }
                    else
                    {
                        int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7, param8);
                    }

                    MessageBox.Show("Cập nhật nhân viên thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    // INSERT
                    if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                    {
                        MessageBox.Show("Vui lòng nhập mật khẩu!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    byte[] hashedPassword = OracleHelper.HashPassword(txtMatKhau.Text.Trim());

                    string sql = @"INSERT INTO BM_USER.NhanVien 
                                  (MaNV, HoTenNV, SDTNV, Email, DiaChiNV, GioiTinhNV, MatKhauNV, Luong_RSA, CMND_RSA) 
                                  VALUES (:manv, :hoten, :sdt, :email, :diachi, :gioitinh, :matkhau, :luong, :cmnd)";

                    var param1 = new OracleParameter("manv", OracleDbType.Int32, Convert.ToInt32(txtMaNV.Text), ParameterDirection.Input);
                    var param2 = new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text.Trim(), ParameterDirection.Input);
                    var param3 = new OracleParameter("sdt", OracleDbType.Raw, encryptedSDT, ParameterDirection.Input);
                    var param4 = new OracleParameter("email", OracleDbType.Raw, encryptedEmail, ParameterDirection.Input);
                    var param5 = new OracleParameter("diachi", OracleDbType.NVarchar2, txtDiaChi.Text.Trim(), ParameterDirection.Input);
                    var param6 = new OracleParameter("gioitinh", OracleDbType.NVarchar2, cboGioiTinh.Text, ParameterDirection.Input);
                    var param7 = new OracleParameter("matkhau", OracleDbType.Raw, hashedPassword, ParameterDirection.Input);
                    var param8 = new OracleParameter("luong", OracleDbType.Raw, encryptedLuong ?? (object)DBNull.Value, ParameterDirection.Input);
                    var param9 = new OracleParameter("cmnd", OracleDbType.Raw, encryptedCMND ?? (object)DBNull.Value, ParameterDirection.Input);

                    int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3, param4, param5, param6, param7, param8, param9);

                    if (result > 0)
                    {
                        MessageBox.Show("Thêm nhân viên thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập SĐT!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập Email!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (cboGioiTinh.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}