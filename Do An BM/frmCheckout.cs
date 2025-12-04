using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmCheckout : Form
    {
        private decimal tongTienGioHang = 0;
        private decimal phiShip = 0;
        private decimal thueVAT = 0;
        private decimal tongCong = 0;

        public frmCheckout()
        {
            InitializeComponent();
        }

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền Customer
            if (!SessionManager.IsCustomer())
            {
                MessageBox.Show("Chỉ khách hàng mới được thanh toán!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            LoadThanhPho();
            LoadDiaChiDaCo(); // Thêm: Load địa chỉ đã lưu trước đó
            LoadGioHang();    // Thêm: Tự load giỏ hàng
            CalculateTotal();
        }
        private void LoadDiaChiDaCo()
        {
            try
            {
                string sql = @"
            SELECT TOP 1 d.MaXP, d.HoTenNN, d.SoNha, d.GhiChu,
                   x.TenXP, q.TenQH, t.TenTP
            FROM DiaChiGiaoHang d
            JOIN XaPhuong x ON d.MaXP = x.MaXP
            JOIN QuanHuyen q ON x.MaQH = q.MaQH
            JOIN ThanhPho t ON q.MaTP = t.MaTP
            WHERE d.MaKH = :makh
            ORDER BY d.MaDCGH DESC
        ";

                var param = new OracleParameter("makh", OracleDbType.Int32,
                    SessionManager.CurrentUserID, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtHoTenNN.Text = row["HoTenNN"].ToString();
                    txtSoNha.Text = row["SoNha"].ToString();
                    txtGhiChuDC.Text = row["GhiChu"].ToString();

                    // Chọn thành phố, quận, xã tương ứng
                    // (Cần thêm logic để chọn đúng trong combobox)
                }
            }
            catch
            {
                // Không có địa chỉ cũ thì để trống
            }
        }
        private void LoadGioHang()
        {
            try
            {
                int maKH = SessionManager.CurrentUserID;

                string sql = @"
            SELECT SUM(s.Gia * ctgh.SoLuongSachCTGH)
            FROM ChiTietGH ctgh
            JOIN Sach s ON ctgh.MaSach = s.MaSach
            JOIN GioHang gh ON ctgh.MaGH = gh.MaGH
            JOIN KhachHang kh ON kh.MaGH = gh.MaGH
            WHERE kh.MaKH = :makh
        ";

                var param = new OracleParameter("makh", OracleDbType.Int32,
                    maKH, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    tongTienGioHang = Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                {
                    tongTienGioHang = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load giỏ hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Load Địa chỉ (3-level cascading)

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Thành phố: " + ex.Message);
            }
        }

        private void cboThanhPho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboThanhPho.SelectedValue != null)
            {
                try
                {
                    int maTP = Convert.ToInt32(cboThanhPho.SelectedValue);
                    string sql = "SELECT MaQH, TenQH FROM QuanHuyen WHERE MaTP = :matp ORDER BY TenQH";

                    var param = new OracleParameter("matp", OracleDbType.Int32,
                        maTP, ParameterDirection.Input);

                    DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                    cboQuanHuyen.DataSource = dt;
                    cboQuanHuyen.DisplayMember = "TenQH";
                    cboQuanHuyen.ValueMember = "MaQH";
                    cboQuanHuyen.SelectedIndex = -1;

                    cboXaPhuong.DataSource = null;
                }
                catch { }
            }
        }

        private void cboQuanHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboQuanHuyen.SelectedValue != null)
            {
                try
                {
                    int maQH = Convert.ToInt32(cboQuanHuyen.SelectedValue);
                    string sql = "SELECT MaXP, TenXP, ChiPhiGHXP FROM XaPhuong WHERE MaQH = :maqh ORDER BY TenXP";

                    var param = new OracleParameter("maqh", OracleDbType.Int32,
                        maQH, ParameterDirection.Input);

                    DataTable dt = OracleHelper.ExecuteQuery(sql, param);

                    cboXaPhuong.DataSource = dt;
                    cboXaPhuong.DisplayMember = "TenXP";
                    cboXaPhuong.ValueMember = "MaXP";
                    cboXaPhuong.SelectedIndex = -1;
                }
                catch { }
            }
        }

        private void cboXaPhuong_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tính lại phí ship khi chọn Xã/Phường
            CalculateTotal();
        }

        #endregion

        #region Tính toán

        private void CalculateTotal()
        {
            try
            {
                //// 1. Lấy tổng tiền giỏ hàng (demo - thực tế lấy từ database)
                //int maKH = SessionManager.CurrentUserID;

                //string sql = @"
                //    SELECT NVL(SUM(s.Gia * ctgh.SoLuongSachCTGH), 0)
                //    FROM ChiTietGH ctgh
                //    JOIN Sach s ON ctgh.MaSach = s.MaSach
                //    JOIN GioHang gh ON ctgh.MaGH = gh.MaGH
                //    JOIN KhachHang kh ON kh.MaGH = gh.MaGH
                //    WHERE kh.MaKH = :makh
                //";

                //var param = new OracleParameter("makh", OracleDbType.Int32,
                //    maKH, ParameterDirection.Input);

                //DataTable dt = OracleHelper.ExecuteQuery(sql, param);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    tongTienGioHang = Convert.ToDecimal(dt.Rows[0][0]);
                //}

                //// 2. Phí ship (lấy từ Xã/Phường)
                //phiShip = 0;
                //if (cboXaPhuong.SelectedIndex >= 0)
                //{
                //    DataRowView row = (DataRowView)cboXaPhuong.SelectedItem;
                //    phiShip = Convert.ToDecimal(row["ChiPhiGHXP"]);
                //}

                //// 3. Thuế VAT (10%)
                //thueVAT = tongTienGioHang * 0.1m;

                //// 4. Tổng cộng
                //tongCong = tongTienGioHang + phiShip + thueVAT;

                //// Hiển thị
                //lblTamTinh.Text = string.Format("{0:N0} VNĐ", tongTienGioHang);
                //lblPhiShip.Text = string.Format("{0:N0} VNĐ", phiShip);
                //lblVAT.Text = string.Format("{0:N0} VNĐ", thueVAT);
                //lblTongCong.Text = string.Format("{0:N0} VNĐ", tongCong);


                // 1. Tổng tiền giỏ hàng (đã load trong LoadGioHang)
                // 2. Phí ship (lấy từ Xã/Phường)
                phiShip = 0;
                if (cboXaPhuong.SelectedIndex >= 0)
                {
                    DataRowView row = (DataRowView)cboXaPhuong.SelectedItem;
                    phiShip = Convert.ToDecimal(row["ChiPhiGHXP"]);
                }

                // 3. Thuế VAT (10%)
                thueVAT = tongTienGioHang * 0.1m;

                // 4. Tổng cộng
                tongCong = tongTienGioHang + phiShip + thueVAT;

                // Hiển thị
                lblTamTinh.Text = string.Format("{0:N0} VNĐ", tongTienGioHang);
                lblPhiShip.Text = string.Format("{0:N0} VNĐ", phiShip);
                lblVAT.Text = string.Format("{0:N0} VNĐ", thueVAT);
                lblTongCong.Text = string.Format("{0:N0} VNĐ", tongCong);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tính toán: " + ex.Message);
            }
        }

        #endregion

        private void rbTheTinDung_CheckedChanged(object sender, EventArgs e)
        {
            // Enable/Disable textbox số thẻ
            txtSoThe.Enabled = rbTheTinDung.Checked;
        }

        private void btnDatHang_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate
                if (string.IsNullOrEmpty(txtHoTenNN.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên người nhận!", "Cảnh báo");
                    return;
                }

                if (cboXaPhuong.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ địa chỉ giao hàng!", "Cảnh báo");
                    return;
                }

                if (tongTienGioHang <= 0)
                {
                    MessageBox.Show("Giỏ hàng trống!", "Cảnh báo");
                    return;
                }

                // Lấy hình thức thanh toán
                int maHTTT = 2; // Mặc định: Tiền mặt
                if (rbTheTinDung.Checked) maHTTT = 1;
                else if (rbChuyenKhoan.Checked) maHTTT = 3;
                else if (rbViDienTu.Checked) maHTTT = 4;

                // Mã hóa thông tin nhạy cảm
                byte[] encryptedCard = null;
                if (rbTheTinDung.Checked && !string.IsNullOrEmpty(txtSoThe.Text))
                {
                    encryptedCard = OracleHelper.EncryptRSA(txtSoThe.Text);
                }

                byte[] encryptedNote = null;
                if (!string.IsNullOrEmpty(txtGhiChuTT.Text))
                {
                    encryptedNote = OracleHelper.EncryptAES(txtGhiChuTT.Text);
                }

                // 1. Tạo DiaChiGiaoHang
                int maXP = Convert.ToInt32(cboXaPhuong.SelectedValue);
                int maDCGH = CreateDiaChiGiaoHang(maXP);

                // 2. Tạo DonDatHang
                int maDon = CreateDonDatHang(maDCGH, maHTTT, encryptedCard, encryptedNote);

                // 3. Copy từ GioHang sang ChiTietDonDH
                CopyFromCartToOrder(maDon);

                // 4. Tạo trạng thái đơn hàng
                CreateOrderStatus(maDon);

                // 5. Xóa giỏ hàng
                ClearCart();

                MessageBox.Show(
                    "✓ ĐẶT HÀNG THÀNH CÔNG!\n\n" +
                    $"Mã đơn: {maDon}\n" +
                    $"Tổng tiền: {tongCong:N0} VNĐ\n\n" +
                    "Thông tin thanh toán đã được mã hóa an toàn (AES-256 & RSA-2048).",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đặt hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Helper Methods

        private int CreateDiaChiGiaoHang(int maXP)
        {
            // Lấy MaDCGH tiếp theo
            string sqlMax = "SELECT NVL(MAX(MaDCGH), 0) + 1 FROM DiaChiGiaoHang";
            DataTable dt = OracleHelper.ExecuteQuery(sqlMax);
            int maDCGH = Convert.ToInt32(dt.Rows[0][0]);

            string sql = @"
                INSERT INTO DiaChiGiaoHang (MaDCGH, HoTenNN, SoNha, GhiChu, MaXP, MaKH)
                VALUES (:madcgh, :hoten, :sonha, :ghichu, :maxp, :makh)
            ";

            var parameters = new[] {
                new OracleParameter("madcgh", OracleDbType.Int32, maDCGH, ParameterDirection.Input),
                new OracleParameter("hoten", OracleDbType.NVarchar2, txtHoTenNN.Text, ParameterDirection.Input),
                new OracleParameter("sonha", OracleDbType.NVarchar2, txtSoNha.Text, ParameterDirection.Input),
                new OracleParameter("ghichu", OracleDbType.NVarchar2, txtGhiChuDC.Text, ParameterDirection.Input),
                new OracleParameter("maxp", OracleDbType.Int32, maXP, ParameterDirection.Input),
                new OracleParameter("makh", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
            };

            OracleHelper.ExecuteNonQuery(sql, parameters);
            return maDCGH;
        }

        private int CreateDonDatHang(int maDCGH, int maHTTT, byte[] encryptedCard, byte[] encryptedNote)
        {
            // Lấy MaDon tiếp theo
            string sqlMax = "SELECT NVL(MAX(MaDon), 0) + 1 FROM DonDatHang";
            DataTable dt = OracleHelper.ExecuteQuery(sqlMax);
            int maDon = Convert.ToInt32(dt.Rows[0][0]);

            string sql = @"
                INSERT INTO DonDatHang 
                (MaDon, TongTien, PhiShip, ThueVAT, SoTheTinDung_RSA, GhiChuThanhToan_AES, MaKH, MaHTTT, MaDCGH)
                VALUES (:madon, :tongtien, :phiship, :thuevat, :sothe, :ghichu, :makh, :mahttt, :madcgh)
            ";

            var parameters = new[] {
                new OracleParameter("madon", OracleDbType.Int32, maDon, ParameterDirection.Input),
                new OracleParameter("tongtien", OracleDbType.Decimal, tongCong, ParameterDirection.Input),
                new OracleParameter("phiship", OracleDbType.Decimal, phiShip, ParameterDirection.Input),
                new OracleParameter("thuevat", OracleDbType.Decimal, 10, ParameterDirection.Input),
                new OracleParameter("sothe", OracleDbType.Raw,
                    encryptedCard ?? (object)DBNull.Value, ParameterDirection.Input),
                new OracleParameter("ghichu", OracleDbType.Raw,
                    encryptedNote ?? (object)DBNull.Value, ParameterDirection.Input),
                new OracleParameter("makh", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input),
                new OracleParameter("mahttt", OracleDbType.Int32, maHTTT, ParameterDirection.Input),
                new OracleParameter("madcgh", OracleDbType.Int32, maDCGH, ParameterDirection.Input)
            };

            OracleHelper.ExecuteNonQuery(sql, parameters);
            return maDon;
        }

        private void CopyFromCartToOrder(int maDon)
        {
            string sql = @"
                INSERT INTO ChiTietDonDH (MaDon, MaSach, SoLuongCTDDH, DonGiaCTDDH)
                SELECT :madon, ctgh.MaSach, ctgh.SoLuongSachCTGH, s.Gia
                FROM ChiTietGH ctgh
                JOIN Sach s ON ctgh.MaSach = s.MaSach
                JOIN GioHang gh ON ctgh.MaGH = gh.MaGH
                JOIN KhachHang kh ON kh.MaGH = gh.MaGH
                WHERE kh.MaKH = :makh
            ";

            var parameters = new[] {
                new OracleParameter("madon", OracleDbType.Int32, maDon, ParameterDirection.Input),
                new OracleParameter("makh", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
            };

            OracleHelper.ExecuteNonQuery(sql, parameters);
        }

        private void CreateOrderStatus(int maDon)
        {
            string sql = @"
                INSERT INTO ChiTietTrangThai (MaDon, MaTT, GhiChuTT)
                VALUES (:madon, 1, :ghichu)
            ";

            var parameters = new[] {
                new OracleParameter("madon", OracleDbType.Int32, maDon, ParameterDirection.Input),
                new OracleParameter("ghichu", OracleDbType.NVarchar2,
                    "Đơn hàng mới, chờ xác nhận", ParameterDirection.Input)
            };

            OracleHelper.ExecuteNonQuery(sql, parameters);
        }

        private void ClearCart()
        {
            string sql = @"
                DELETE FROM ChiTietGH
                WHERE MaGH IN (
                    SELECT MaGH FROM KhachHang WHERE MaKH = :makh
                )
            ";

            var param = new OracleParameter("makh", OracleDbType.Int32,
                SessionManager.CurrentUserID, ParameterDirection.Input);

            OracleHelper.ExecuteNonQuery(sql, param);
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}