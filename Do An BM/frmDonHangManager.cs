using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmDonHangManager : Form
    {
        public frmDonHangManager()
        {
            InitializeComponent();
        }

        private void frmDonHangManager_Load(object sender, EventArgs e)
        {
            LoadTrangThai();
            LoadDonHang();

            // Phân quyền hiển thị theo loại user
            if (SessionManager.IsCustomer())
            {
                // Khách hàng: chỉ xem đơn của chính mình (VPD đã lọc ở DB)
                lblTitle.Text = "ĐỌN HÀNG CỦA BẠN";

                btnDecryptPayment.Visible = false;
                btnUpdateStatus.Visible = false;

                // Ẩn thanh tìm kiếm / lọc nâng cao để giao diện đơn giản hơn
                txtSearch.Visible = false;
                lblSearch.Visible = false;
                btnSearch.Visible = false;

                groupBox1.Visible = false; // nhóm lọc trạng thái + khoảng ngày

                lblWarning.Text = "Thông tin thanh toán của bạn được mã hóa an toàn (AES-256 & RSA-2048).";
            }
            else
            {
                // Nhân viên / Admin: hiển thị đầy đủ, nhưng chỉ Admin được giải mã thanh toán
                if (!SessionManager.IsAdmin())
                {
                    btnDecryptPayment.Enabled = false;
                }
            }

            // Set ngày mặc định
            dtpStart.Value = DateTime.Now.AddMonths(-1);
            dtpEnd.Value = DateTime.Now;
        }

        private void LoadTrangThai()
        {
            try
            {
                string sql = "SELECT MaTT, TenTT FROM BM_USER.TrangThai ORDER BY MaTT";
                DataTable dt = OracleHelper.ExecuteQuery(sql);

                if (dt != null)
                {
                    DataRow allRow = dt.NewRow();
                    allRow["MaTT"] = 0;
                    allRow["TenTT"] = "-- Tất cả --";
                    dt.Rows.InsertAt(allRow, 0);

                    cboTrangThai.DataSource = dt;
                    cboTrangThai.DisplayMember = "TenTT";
                    cboTrangThai.ValueMember = "MaTT";
                    cboTrangThai.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải trạng thái: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDonHang()
        {
            try
            {
                // ✅ FIX: Thêm TenHTTT vào SELECT
                string sql = @"SELECT 
                                  d.MaDon, 
                                  d.NgayDat, 
                                  k.HoTenKH, 
                                  d.TongTien, 
                                  d.PhiShip, 
                                  d.ThueVAT,
                                  h.TenHTTT,  -- ✅ THÊM dòng này
                                  (SELECT TenTT 
                                   FROM BM_USER.TrangThai t 
                                   JOIN BM_USER.ChiTietTrangThai ct ON t.MaTT = ct.MaTT 
                                   WHERE ct.MaDon = d.MaDon 
                                   ORDER BY ct.NgayCapNhatTT DESC 
                                   FETCH FIRST 1 ROW ONLY) AS TrangThaiHienTai
                              FROM BM_USER.DonDatHang d
                              JOIN BM_USER.KhachHang k ON d.MaKH = k.MaKH
                              LEFT JOIN BM_USER.HinhThucThanhToan h ON d.MaHTTT = h.MaHTTT  -- ✅ THÊM JOIN
                              {0}
                              ORDER BY d.NgayDat DESC";

                DataTable dt;

                if (SessionManager.IsCustomer())
                {
                    // Khách hàng: chỉ xem đơn của chính mình
                    string whereClause = "WHERE d.MaKH = :makh";
                    string finalSql = string.Format(sql, whereClause);

                    var param = new OracleParameter("makh", OracleDbType.Int32,
                        SessionManager.CurrentUserID, ParameterDirection.Input);

                    dt = OracleHelper.ExecuteQuery(finalSql, param);
                }
                else
                {
                    // Admin / Staff: xem tất cả đơn hàng
                    string finalSql = string.Format(sql, string.Empty);
                    dt = OracleHelper.ExecuteQuery(finalSql);
                }

                if (dt != null)
                {
                    dgvDonHang.DataSource = dt;

                    // ✅ Đổi tên cột hiển thị
                    dgvDonHang.Columns["MaDon"].HeaderText = "Mã đơn";
                    dgvDonHang.Columns["NgayDat"].HeaderText = "Ngày đặt";
                    dgvDonHang.Columns["HoTenKH"].HeaderText = "Khách hàng";
                    dgvDonHang.Columns["TongTien"].HeaderText = "Tổng tiền";
                    dgvDonHang.Columns["PhiShip"].HeaderText = "Phí ship";
                    dgvDonHang.Columns["ThueVAT"].HeaderText = "VAT (%)";
                    dgvDonHang.Columns["TenHTTT"].HeaderText = "Hình thức TT";
                    dgvDonHang.Columns["TrangThaiHienTai"].HeaderText = "Trạng thái";

                    // ✅ Format tiền
                    dgvDonHang.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    dgvDonHang.Columns["PhiShip"].DefaultCellStyle.Format = "N0";
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
            LoadDonHang();
            txtSearch.Clear();
            cboTrangThai.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadDonHang();
                return;
            }

            try
            {
                // ✅ FIX: Thêm TenHTTT vào SELECT
                string sql = @"SELECT 
                                  d.MaDon, 
                                  d.NgayDat, 
                                  k.HoTenKH, 
                                  d.TongTien, 
                                  d.PhiShip, 
                                  d.ThueVAT,
                                  h.TenHTTT,  -- ✅ THÊM
                                  (SELECT TenTT 
                                   FROM BM_USER.TrangThai t 
                                   JOIN BM_USER.ChiTietTrangThai ct ON t.MaTT = ct.MaTT 
                                   WHERE ct.MaDon = d.MaDon 
                                   ORDER BY ct.NgayCapNhatTT DESC 
                                   FETCH FIRST 1 ROW ONLY) AS TrangThaiHienTai
                              FROM BM_USER.DonDatHang d
                              JOIN BM_USER.KhachHang k ON d.MaKH = k.MaKH
                              LEFT JOIN BM_USER.HinhThucThanhToan h ON d.MaHTTT = h.MaHTTT  -- ✅ THÊM
                              WHERE {0}
                              ORDER BY d.NgayDat DESC";

                int maDon = 0;
                int.TryParse(keyword, out maDon);

                DataTable dt;

                if (SessionManager.IsCustomer())
                {
                    // Khách hàng: chỉ tìm trong đơn của chính mình
                    string cond = @"d.MaKH = :makh AND 
                                    (LOWER(k.HoTenKH) LIKE LOWER(:keyword) OR d.MaDon = :madon)";
                    string finalSql = string.Format(sql, cond);

                    var pMakh = new OracleParameter("makh", OracleDbType.Int32,
                        SessionManager.CurrentUserID, ParameterDirection.Input);
                    var pKeyword = new OracleParameter("keyword", OracleDbType.Varchar2,
                        "%" + keyword + "%", ParameterDirection.Input);
                    var pMaDon = new OracleParameter("madon", OracleDbType.Int32,
                        maDon, ParameterDirection.Input);

                    dt = OracleHelper.ExecuteQuery(finalSql, pMakh, pKeyword, pMaDon);
                }
                else
                {
                    // Admin / Staff: tìm trên toàn bộ đơn
                    string cond = @"LOWER(k.HoTenKH) LIKE LOWER(:keyword) OR d.MaDon = :madon";
                    string finalSql = string.Format(sql, cond);

                    var pKeyword = new OracleParameter("keyword", OracleDbType.Varchar2,
                        "%" + keyword + "%", ParameterDirection.Input);
                    var pMaDon = new OracleParameter("madon", OracleDbType.Int32,
                        maDon, ParameterDirection.Input);

                    dt = OracleHelper.ExecuteQuery(finalSql, pKeyword, pMaDon);
                }

                if (dt != null)
                {
                    dgvDonHang.DataSource = dt;
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

        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTrangThai.SelectedItem == null) return;

            int maTT = 0;
            var selectedItem = cboTrangThai.SelectedItem;

            if (selectedItem is DataRowView rowView)
            {
                maTT = Convert.ToInt32(rowView["MaTT"]);
            }
            else if (selectedItem is DataRow row)
            {
                maTT = Convert.ToInt32(row["MaTT"]);
            }
            else if (cboTrangThai.SelectedValue != null)
            {
                int.TryParse(cboTrangThai.SelectedValue.ToString(), out maTT);
            }

            if (maTT == 0)
            {
                LoadDonHang();
                return;
            }

            try
            {
                // ✅ FIX: Thêm TenHTTT
                string sql = @"SELECT 
                                   d.MaDon, 
                                   d.NgayDat, 
                                   k.HoTenKH, 
                                   d.TongTien, 
                                   d.PhiShip, 
                                   d.ThueVAT,
                                   h.TenHTTT,  -- ✅ THÊM
                                   t.TenTT AS TrangThaiHienTai
                               FROM BM_USER.DonDatHang d
                               JOIN BM_USER.KhachHang k ON d.MaKH = k.MaKH
                               LEFT JOIN BM_USER.HinhThucThanhToan h ON d.MaHTTT = h.MaHTTT  -- ✅ THÊM
                               JOIN BM_USER.ChiTietTrangThai ct ON d.MaDon = ct.MaDon
                               JOIN BM_USER.TrangThai t ON ct.MaTT = t.MaTT
                               WHERE ct.MaTT = :matt
                               {0}
                               AND ct.NgayCapNhatTT = (
                                   SELECT MAX(NgayCapNhatTT) 
                                   FROM BM_USER.ChiTietTrangThai 
                                   WHERE MaDon = d.MaDon
                               )
                               ORDER BY d.NgayDat DESC";

                DataTable dt;
                if (SessionManager.IsCustomer())
                {
                    string extra = "AND d.MaKH = :makh";
                    string finalSql = string.Format(sql, extra);

                    var pMatt = new OracleParameter("matt", OracleDbType.Int32, maTT, ParameterDirection.Input);
                    var pMakh = new OracleParameter("makh", OracleDbType.Int32,
                        SessionManager.CurrentUserID, ParameterDirection.Input);

                    dt = OracleHelper.ExecuteQuery(finalSql, pMatt, pMakh);
                }
                else
                {
                    string finalSql = string.Format(sql, string.Empty);
                    var pMatt = new OracleParameter("matt", OracleDbType.Int32, maTT, ParameterDirection.Input);
                    dt = OracleHelper.ExecuteQuery(finalSql, pMatt);
                }

                if (dt != null)
                {
                    dgvDonHang.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lọc: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maDon = Convert.ToInt32(dgvDonHang.SelectedRows[0].Cells["MaDon"].Value);
                frmDonHangDetail frm = new frmDonHangDetail(maDon);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mở chi tiết: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecryptPayment_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ ADMIN mới được xem thông tin thanh toán!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDonHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int maDon = Convert.ToInt32(dgvDonHang.SelectedRows[0].Cells["MaDon"].Value);

                string sql = @"SELECT SoTheTinDung_RSA, GhiChuThanhToan_AES 
                              FROM BM_USER.DonDatHang 
                              WHERE MaDon = :madon";

                var param = new OracleParameter("madon", OracleDbType.Int32, maDon, ParameterDirection.Input);
                DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    byte[] encryptedCard = dt.Rows[0]["SoTheTinDung_RSA"] as byte[];
                    byte[] encryptedNote = dt.Rows[0]["GhiChuThanhToan_AES"] as byte[];

                    string cardNumber = "N/A";
                    string note = "N/A";

                    if (encryptedCard != null && encryptedCard.Length > 0)
                    {
                        cardNumber = OracleHelper.DecryptRSA(encryptedCard);
                        // Mask card number: **** **** **** 3456
                        if (cardNumber.Length >= 4)
                        {
                            cardNumber = "**** **** **** " + cardNumber.Substring(cardNumber.Length - 4);
                        }
                    }

                    if (encryptedNote != null && encryptedNote.Length > 0)
                        note = OracleHelper.DecryptAES(encryptedNote);

                    MessageBox.Show($"Thông tin thanh toán (Mã hóa Hybrid):\n\n" +
                        $"Số thẻ (RSA-2048): {cardNumber}\n" +
                        $"Ghi chú (AES-256): {note}",
                        "Giải mã thanh toán (Admin Only)", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi giải mã: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn đơn hàng!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDon = Convert.ToInt32(dgvDonHang.SelectedRows[0].Cells["MaDon"].Value);

            // Load trạng thái
            string sqlTT = "SELECT MaTT, TenTT FROM BM_USER.TrangThai ORDER BY MaTT";
            DataTable dtTT = OracleHelper.ExecuteQuery(sqlTT);

            if (dtTT == null || dtTT.Rows.Count == 0)
            {
                MessageBox.Show("Không có trạng thái nào!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Simple dialog to select status
            Form frmSelectStatus = new Form();
            frmSelectStatus.Text = $"Cập nhật trạng thái đơn hàng #{maDon}";
            frmSelectStatus.Size = new System.Drawing.Size(450, 250);
            frmSelectStatus.StartPosition = FormStartPosition.CenterParent;

            Label lbl = new Label();
            lbl.Text = "Chọn trạng thái mới:";
            lbl.Location = new System.Drawing.Point(20, 20);
            lbl.AutoSize = true;
            frmSelectStatus.Controls.Add(lbl);

            ComboBox cboStatus = new ComboBox();
            cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStatus.DataSource = dtTT;
            cboStatus.DisplayMember = "TenTT";
            cboStatus.ValueMember = "MaTT";
            cboStatus.Location = new System.Drawing.Point(20, 50);
            cboStatus.Size = new System.Drawing.Size(390, 25);
            frmSelectStatus.Controls.Add(cboStatus);

            Label lbl2 = new Label();
            lbl2.Text = "Ghi chú:";
            lbl2.Location = new System.Drawing.Point(20, 90);
            lbl2.AutoSize = true;
            frmSelectStatus.Controls.Add(lbl2);

            TextBox txtGhiChu = new TextBox();
            txtGhiChu.Location = new System.Drawing.Point(20, 115);
            txtGhiChu.Size = new System.Drawing.Size(390, 20);
            txtGhiChu.Multiline = true;
            txtGhiChu.Height = 50;
            frmSelectStatus.Controls.Add(txtGhiChu);

            Button btnOK = new Button();
            btnOK.Text = "Cập nhật";
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(250, 180);
            btnOK.Size = new System.Drawing.Size(80, 30);
            frmSelectStatus.Controls.Add(btnOK);

            Button btnCancel = new Button();
            btnCancel.Text = "Hủy";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(340, 180);
            btnCancel.Size = new System.Drawing.Size(70, 30);
            frmSelectStatus.Controls.Add(btnCancel);

            if (frmSelectStatus.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int maTT = Convert.ToInt32(cboStatus.SelectedValue);
                    string ghiChu = txtGhiChu.Text.Trim();

                    string sql = @"INSERT INTO BM_USER.ChiTietTrangThai (MaDon, MaTT, GhiChuTT) 
                                  VALUES (:madon, :matt, :ghichu)";

                    var param1 = new OracleParameter("madon", OracleDbType.Int32, maDon, ParameterDirection.Input);
                    var param2 = new OracleParameter("matt", OracleDbType.Int32, maTT, ParameterDirection.Input);
                    var param3 = new OracleParameter("ghichu", OracleDbType.NVarchar2, ghiChu, ParameterDirection.Input);

                    int result = OracleHelper.ExecuteNonQuery(sql, param1, param2, param3);

                    if (result > 0)
                    {
                        MessageBox.Show($"Đã cập nhật trạng thái thành công!\n\nTrạng thái: {cboStatus.Text}",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDonHang();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}