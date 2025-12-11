using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmProfile : Form
    {
        public frmProfile()
        {
            InitializeComponent();
        }

        private void frmProfile_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsCustomer())
            {
                MessageBox.Show("Chỉ khách hàng mới có thông tin cá nhân!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            //Thêm event cho checkbox
            cbHienMatKhau.CheckedChanged += (s, ev) => {
                txtMatKhauCu.UseSystemPasswordChar = !cbHienMatKhau.Checked;
                txtMatKhauMoi.UseSystemPasswordChar = !cbHienMatKhau.Checked;
                txtXacNhanMK.UseSystemPasswordChar = !cbHienMatKhau.Checked;
            };

            // Set mặc định ẩn mật khẩu
            txtMatKhauCu.UseSystemPasswordChar = true;
            txtMatKhauMoi.UseSystemPasswordChar = true;
            txtXacNhanMK.UseSystemPasswordChar = true;

            LoadProfile();
            LoadDiaChi();
        }

        private void LoadProfile()
        {
            try
            {
                string sql = @"
                    SELECT k.MaKH, k.HoTenKH, 
                           DECRYPT_AES(k.Email) AS Email,
                           DECRYPT_AES(k.SDT) AS SDT,
                           k.DiaChiKH
                    FROM KhachHang k
                    WHERE k.MaKH = :makh";

                var param = new OracleParameter("makh", OracleDbType.Int32,
                    SessionManager.CurrentUserID, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtMaKH.Text = row["MaKH"].ToString();
                    txtHoTen.Text = row["HoTenKH"].ToString();
                    txtEmail.Text = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
                    txtSDT.Text = row["SDT"] != DBNull.Value ? row["SDT"].ToString() : "";
                    txtDiaChi.Text = row["DiaChiKH"] != DBNull.Value ? row["DiaChiKH"].ToString() : "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDiaChi()
        {
            try
            {
                string sql = @"
                    SELECT d.MaDCGH, d.HoTenNN, d.SoNha, d.GhiChu,
                           x.TenXP, q.TenQH, t.TenTP
                    FROM DiaChiGiaoHang d
                    JOIN XaPhuong x ON d.MaXP = x.MaXP
                    JOIN QuanHuyen q ON x.MaQH = q.MaQH
                    JOIN ThanhPho t ON q.MaTP = t.MaTP
                    WHERE d.MaKH = :makh
                    ORDER BY d.MaDCGH";

                var param = new OracleParameter("makh", OracleDbType.Int32,
                    SessionManager.CurrentUserID, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                dgvDiaChi.DataSource = dt;

                // Đổi tên cột
                if (dgvDiaChi.Columns.Contains("MaDCGH"))
                    dgvDiaChi.Columns["MaDCGH"].HeaderText = "Mã ĐC";
                if (dgvDiaChi.Columns.Contains("HoTenNN"))
                    dgvDiaChi.Columns["HoTenNN"].HeaderText = "Người nhận";
                if (dgvDiaChi.Columns.Contains("SoNha"))
                    dgvDiaChi.Columns["SoNha"].HeaderText = "Số nhà";
                if (dgvDiaChi.Columns.Contains("GhiChu"))
                    dgvDiaChi.Columns["GhiChu"].HeaderText = "Ghi chú";
                if (dgvDiaChi.Columns.Contains("TenXP"))
                    dgvDiaChi.Columns["TenXP"].HeaderText = "Xã/Phường";
                if (dgvDiaChi.Columns.Contains("TenQH"))
                    dgvDiaChi.Columns["TenQH"].HeaderText = "Quận/Huyện";
                if (dgvDiaChi.Columns.Contains("TenTP"))
                    dgvDiaChi.Columns["TenTP"].HeaderText = "Tỉnh/Thành";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải địa chỉ: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(txtHoTen.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Cảnh báo");
                    return;
                }

                // Mã hóa email và SDT
                byte[] encryptedEmail = null;
                if (!string.IsNullOrEmpty(txtEmail.Text))
                    encryptedEmail = OracleHelper.EncryptAES(txtEmail.Text);

                byte[] encryptedSDT = null;
                if (!string.IsNullOrEmpty(txtSDT.Text))
                    encryptedSDT = OracleHelper.EncryptAES(txtSDT.Text);

                string sql = @"
                    UPDATE KhachHang 
                    SET HoTenKH = :hoten,
                        Email = :email,
                        SDT = :sdt,
                        DiaChiKH = :diachi
                    WHERE MaKH = :makh";

                var parameters = new[] {
                    new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text, ParameterDirection.Input),
                    new OracleParameter("email", OracleDbType.Raw,
                        encryptedEmail ?? (object)DBNull.Value, ParameterDirection.Input),
                    new OracleParameter("sdt", OracleDbType.Raw,
                        encryptedSDT ?? (object)DBNull.Value, ParameterDirection.Input),
                    new OracleParameter("diachi", OracleDbType.NVarchar2,
                        txtDiaChi.Text, ParameterDirection.Input),
                    new OracleParameter("makh", OracleDbType.Int32,
                        SessionManager.CurrentUserID, ParameterDirection.Input)
                };

                int result = OracleHelper.ExecuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SessionManager.CurrentUserName = txtHoTen.Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(txtMatKhauCu.Text) ||
                    string.IsNullOrEmpty(txtMatKhauMoi.Text) ||
                    string.IsNullOrEmpty(txtXacNhanMK.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo");
                    return;
                }

                if (txtMatKhauMoi.Text != txtXacNhanMK.Text)
                {
                    MessageBox.Show("Mật khẩu mới và xác nhận không khớp!", "Cảnh báo");
                    return;
                }

                // Kiểm tra mật khẩu cũ
                string sqlCheck = "SELECT MatKhau FROM KhachHang WHERE MaKH = :makh";
                var paramCheck = new OracleParameter("makh", OracleDbType.Int32,
                    SessionManager.CurrentUserID, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sqlCheck, paramCheck);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng!", "Lỗi");
                    return;
                }

                byte[] storedHash = (byte[])dt.Rows[0]["MatKhau"];
                byte[] inputHash = OracleHelper.HashPassword(txtMatKhauCu.Text);

                // So sánh hash
                if (!CompareByteArrays(storedHash, inputHash))
                {
                    MessageBox.Show("Mật khẩu cũ không đúng!", "Cảnh báo");
                    return;
                }

                // Update mật khẩu mới
                byte[] newHash = OracleHelper.HashPassword(txtMatKhauMoi.Text);
                string sqlUpdate = "UPDATE KhachHang SET MatKhau = :matkhau WHERE MaKH = :makh";
                var parameters = new[] {
                    new OracleParameter("matkhau", OracleDbType.Raw, newHash, ParameterDirection.Input),
                    new OracleParameter("makh", OracleDbType.Int32,
                        SessionManager.CurrentUserID, ParameterDirection.Input)
                };

                int result = OracleHelper.ExecuteNonQuery(sqlUpdate, parameters);
                if (result > 0)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields
                    txtMatKhauCu.Text = "";
                    txtMatKhauMoi.Text = "";
                    txtXacNhanMK.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đổi mật khẩu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CompareByteArrays(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        private void btnThemDC_Click(object sender, EventArgs e)
        {
            // Mở form thêm địa chỉ mới
            frmDiaChiDetail frm = new frmDiaChiDetail(0, true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDiaChi();
            }
        }

        private void btnSuaDC_Click(object sender, EventArgs e)
        {
            if (dgvDiaChi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một địa chỉ để sửa!", "Thông báo");
                return;
            }

            int maDCGH = Convert.ToInt32(dgvDiaChi.SelectedRows[0].Cells["MaDCGH"].Value);
            frmDiaChiDetail frm = new frmDiaChiDetail(maDCGH, false);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadDiaChi();
            }
        }

        private void btnXoaDC_Click(object sender, EventArgs e)
        {
            if (dgvDiaChi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một địa chỉ để xóa!", "Thông báo");
                return;
            }

            int maDCGH = Convert.ToInt32(dgvDiaChi.SelectedRows[0].Cells["MaDCGH"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa địa chỉ này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM DiaChiGiaoHang WHERE MaDCGH = :madcgh";
                    var param = new OracleParameter("madcgh", OracleDbType.Int32, maDCGH, ParameterDirection.Input);

                    int result = OracleHelper.ExecuteNonQuery(sql, param);
                    if (result > 0)
                    {
                        MessageBox.Show("Đã xóa địa chỉ thành công!", "Thông báo");
                        LoadDiaChi();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa địa chỉ: " + ex.Message, "Lỗi");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Form phụ để thêm/sửa địa chỉ
    public class frmDiaChiDetail : Form
    {
        private int maDCGH;
        private bool isNew;
        private ComboBox cboThanhPho, cboQuanHuyen, cboXaPhuong;
        private TextBox txtHoTen, txtSoNha, txtGhiChu;
        private Button btnSave, btnCancel;

        public frmDiaChiDetail(int maDCGH, bool isNew)
        {
            this.maDCGH = maDCGH;
            this.isNew = isNew;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = isNew ? "Thêm địa chỉ mới" : "Sửa địa chỉ";
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Tạo controls
            Label lblHoTen = new Label { Text = "Họ tên người nhận:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(150, 25) };
            txtHoTen = new TextBox { Location = new System.Drawing.Point(180, 20), Size = new System.Drawing.Size(250, 25) };

            Label lblSoNha = new Label { Text = "Số nhà/Đường:", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(150, 25) };
            txtSoNha = new TextBox { Location = new System.Drawing.Point(180, 60), Size = new System.Drawing.Size(250, 25) };

            Label lblThanhPho = new Label { Text = "Tỉnh/Thành:", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(150, 25) };
            cboThanhPho = new ComboBox { Location = new System.Drawing.Point(180, 100), Size = new System.Drawing.Size(250, 25) };
            cboThanhPho.SelectedIndexChanged += (s, e) => LoadQuanHuyen();

            Label lblQuanHuyen = new Label { Text = "Quận/Huyện:", Location = new System.Drawing.Point(20, 140), Size = new System.Drawing.Size(150, 25) };
            cboQuanHuyen = new ComboBox { Location = new System.Drawing.Point(180, 140), Size = new System.Drawing.Size(250, 25) };
            cboQuanHuyen.SelectedIndexChanged += (s, e) => LoadXaPhuong();

            Label lblXaPhuong = new Label { Text = "Xã/Phường:", Location = new System.Drawing.Point(20, 180), Size = new System.Drawing.Size(150, 25) };
            cboXaPhuong = new ComboBox { Location = new System.Drawing.Point(180, 180), Size = new System.Drawing.Size(250, 25) };

            Label lblGhiChu = new Label { Text = "Ghi chú:", Location = new System.Drawing.Point(20, 220), Size = new System.Drawing.Size(150, 25) };
            txtGhiChu = new TextBox { Location = new System.Drawing.Point(180, 220), Size = new System.Drawing.Size(250, 60), Multiline = true };

            btnSave = new Button { Text = "Lưu", Location = new System.Drawing.Point(150, 300), Size = new System.Drawing.Size(100, 30) };
            btnSave.Click += (s, e) => Save();

            btnCancel = new Button { Text = "Hủy", Location = new System.Drawing.Point(270, 300), Size = new System.Drawing.Size(100, 30) };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            // Thêm controls vào form
            this.Controls.AddRange(new Control[] {
                lblHoTen, txtHoTen,
                lblSoNha, txtSoNha,
                lblThanhPho, cboThanhPho,
                lblQuanHuyen, cboQuanHuyen,
                lblXaPhuong, cboXaPhuong,
                lblGhiChu, txtGhiChu,
                btnSave, btnCancel
            });

            LoadThanhPho();
        }

        private void LoadThanhPho()
        {
            try
            {
                string sql = "SELECT MaTP, TenTP FROM ThanhPho ORDER BY TenTP";
                DataTable dt = OracleHelper.ExecuteQuery(sql);
                cboThanhPho.DataSource = dt;
                cboThanhPho.DisplayMember = "TenTP";
                cboThanhPho.ValueMember = "MaTP";
                cboThanhPho.SelectedIndex = -1;
            }
            catch { }
        }

        private void LoadQuanHuyen()
        {
            if (cboThanhPho.SelectedValue == null) return;
            try
            {
                int maTP = Convert.ToInt32(cboThanhPho.SelectedValue);
                string sql = "SELECT MaQH, TenQH FROM QuanHuyen WHERE MaTP = :matp ORDER BY TenQH";
                var param = new OracleParameter("matp", OracleDbType.Int32, maTP, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                cboQuanHuyen.DataSource = dt;
                cboQuanHuyen.DisplayMember = "TenQH";
                cboQuanHuyen.ValueMember = "MaQH";
                cboQuanHuyen.SelectedIndex = -1;
                cboXaPhuong.DataSource = null;
            }
            catch { }
        }

        private void LoadXaPhuong()
        {
            if (cboQuanHuyen.SelectedValue == null) return;
            try
            {
                int maQH = Convert.ToInt32(cboQuanHuyen.SelectedValue);
                string sql = "SELECT MaXP, TenXP FROM XaPhuong WHERE MaQH = :maqh ORDER BY TenXP";
                var param = new OracleParameter("maqh", OracleDbType.Int32, maQH, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                cboXaPhuong.DataSource = dt;
                cboXaPhuong.DisplayMember = "TenXP";
                cboXaPhuong.ValueMember = "MaXP";
                cboXaPhuong.SelectedIndex = -1;
            }
            catch { }
        }

        private void LoadData()
        {
            if (!isNew && maDCGH > 0)
            {
                try
                {
                    string sql = @"
                        SELECT d.HoTenNN, d.SoNha, d.GhiChu, x.MaXP, x.MaQH, q.MaTP
                        FROM DiaChiGiaoHang d
                        JOIN XaPhuong x ON d.MaXP = x.MaXP
                        JOIN QuanHuyen q ON x.MaQH = q.MaQH
                        WHERE d.MaDCGH = :madcgh";

                    var param = new OracleParameter("madcgh", OracleDbType.Int32, maDCGH, ParameterDirection.Input);
                    DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        txtHoTen.Text = row["HoTenNN"].ToString();
                        txtSoNha.Text = row["SoNha"].ToString();
                        txtGhiChu.Text = row["GhiChu"].ToString();

                        // Select in comboboxes
                        int maTP = Convert.ToInt32(row["MaTP"]);
                        int maQH = Convert.ToInt32(row["MaQH"]);
                        int maXP = Convert.ToInt32(row["MaXP"]);

                        // Cần load dữ liệu trước khi select
                        // (Tạm thời để trống, có thể cần cải thiện)
                    }
                }
                catch { }
            }
        }

        private void Save()
        {
            try
            {
                if (cboXaPhuong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ địa chỉ!", "Cảnh báo");
                    return;
                }

                int maXP = Convert.ToInt32(cboXaPhuong.SelectedValue);

                if (isNew)
                {
                    // Tạo mã mới
                    string sqlMax = "SELECT NVL(MAX(MaDCGH), 0) + 1 FROM DiaChiGiaoHang";
                    DataTable dt = OracleHelper.ExecuteQuery(sqlMax);
                    maDCGH = Convert.ToInt32(dt.Rows[0][0]);

                    string sql = @"
                        INSERT INTO DiaChiGiaoHang (MaDCGH, HoTenNN, SoNha, GhiChu, MaXP, MaKH)
                        VALUES (:madcgh, :hoten, :sonha, :ghichu, :maxp, :makh)";

                    var parameters = new[] {
                        new OracleParameter("madcgh", OracleDbType.Int32, maDCGH, ParameterDirection.Input),
                        new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text, ParameterDirection.Input),
                        new OracleParameter("sonha", OracleDbType.NVarchar2, txtSoNha.Text, ParameterDirection.Input),
                        new OracleParameter("ghichu", OracleDbType.NVarchar2, txtGhiChu.Text, ParameterDirection.Input),
                        new OracleParameter("maxp", OracleDbType.Int32, maXP, ParameterDirection.Input),
                        new OracleParameter("makh", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
                    };

                    OracleHelper.ExecuteNonQuery(sql, parameters);
                }
                else
                {
                    string sql = @"
                        UPDATE DiaChiGiaoHang 
                        SET HoTenNN = :hoten, SoNha = :sonha, GhiChu = :ghichu, MaXP = :maxp
                        WHERE MaDCGH = :madcgh";

                    var parameters = new[] {
                        new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTen.Text, ParameterDirection.Input),
                        new OracleParameter("sonha", OracleDbType.NVarchar2, txtSoNha.Text, ParameterDirection.Input),
                        new OracleParameter("ghichu", OracleDbType.NVarchar2, txtGhiChu.Text, ParameterDirection.Input),
                        new OracleParameter("maxp", OracleDbType.Int32, maXP, ParameterDirection.Input),
                        new OracleParameter("madcgh", OracleDbType.Int32, maDCGH, ParameterDirection.Input)
                    };

                    OracleHelper.ExecuteNonQuery(sql, parameters);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu địa chỉ: " + ex.Message, "Lỗi");
            }
        }
    }
}