using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmDonHangDetail : Form
    {
        private int maDon;
        private DataTable dtChiTiet;
        private byte[] encryptedCard;
        private byte[] encryptedNote;
        private string hinhThucTT;

        public frmDonHangDetail(int maDon)
        {
            InitializeComponent();
            this.maDon = maDon;
        }

        private void frmDonHangDetail_Load(object sender, EventArgs e)
        {
            LoadDonHangDetail();
            LoadChiTietDonHang();
            SetupPermission();
        }

        private void SetupPermission()
        {
            // Chỉ Admin mới có nút giải mã
            if (!SessionManager.IsAdmin())
            {
                btnGiaiMa.Visible = false;
            }
        }

        private void LoadDonHangDetail()
        {
            try
            {
                string sql = @"SELECT 
                d.MaDon, 
                d.NgayDat, 
                d.TongTien, 
                d.PhiShip, 
                d.ThueVAT,
                k.HoTenKH, 
                h.TenHTTT,
                d.SoTheTinDung_RSA, 
                d.GhiChuThanhToan_AES,
                -- Lấy trạng thái mới nhất
                (SELECT t.TenTT 
                 FROM ChiTietTrangThai ctt 
                 JOIN TrangThai t ON ctt.MaTT = t.MaTT 
                 WHERE ctt.MaDon = d.MaDon 
                 ORDER BY ctt.NgayCapNhatTT DESC 
                 FETCH FIRST 1 ROW ONLY) AS TenTT
           FROM DonDatHang d
           JOIN KhachHang k ON d.MaKH = k.MaKH
           JOIN HinhThucThanhToan h ON d.MaHTTT = h.MaHTTT
           WHERE d.MaDon = :maDon";

                OracleParameter[] parameters = {
                    new OracleParameter("maDon", OracleDbType.Int32, maDon, ParameterDirection.Input)
                };

                DataTable dt = OracleHelper.ExecuteQuery(sql, parameters);
                // Kiểm tra nghiêm ngặt
                if (dt == null)
                {
                    MessageBox.Show("Lỗi khi thực hiện truy vấn!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy đơn hàng #{maDon}", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                DataRow row = dt.Rows[0];

                // Hiển thị thông tin cơ bản
                lblMaDon.Text = $"Mã đơn: #{maDon}";
                lblNgayDat.Text = $"Ngày đặt: {Convert.ToDateTime(row["NgayDat"]):dd/MM/yyyy HH:mm}";
                lblKhachHang.Text = $"Khách hàng: {row["HoTenKH"]}";
                lblTongTien.Text = $"Tổng tiền: {Convert.ToDecimal(row["TongTien"]):N0} đ";
                lblPhiShip.Text = $"Phí ship: {Convert.ToDecimal(row["PhiShip"]):N0} đ";
                lblThueVAT.Text = $"Thuế VAT: {Convert.ToDecimal(row["ThueVAT"])}%";

                decimal tongTien = Convert.ToDecimal(row["TongTien"]);
                decimal phiShip = Convert.ToDecimal(row["PhiShip"]);
                decimal thueVAT = Convert.ToDecimal(row["ThueVAT"]);
                decimal tongCong = tongTien + phiShip + (tongTien * thueVAT / 100);

                lblTongCong.Text = $"Tổng cộng: {tongCong:N0} đ";
                lblTrangThai.Text = $"Trạng thái: {row["TenTT"]}";

                // Thông tin thanh toán
                label11.Text = $"Hình thức TT: {row["TenHTTT"]}";
                hinhThucTT = row["TenHTTT"].ToString();

                // Lưu dữ liệu mã hóa
                if (row["SoTheTinDung_RSA"] != DBNull.Value)
                    encryptedCard = (byte[])row["SoTheTinDung_RSA"];

                if (row["GhiChuThanhToan_AES"] != DBNull.Value)
                    encryptedNote = (byte[])row["GhiChuThanhToan_AES"];

                // Hiển thị số thẻ đã mask
                if (encryptedCard != null)
                {
                    lblSoThe.Text = "Số thẻ: **********";
                }
                else
                {
                    lblSoThe.Text = "Số thẻ: (Không có)";
                }

                // Hiển thị ghi chú
                if (encryptedNote != null)
                {
                    txtGhiChu.Text = "[Thông tin đã mã hóa - Nhấn 'Giải mã' để xem]";
                }
                else
                {
                    txtGhiChu.Text = "(Không có ghi chú)";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết đơn hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadChiTietDonHang()
        {
            try
            {
                string sql = @"SELECT s.MaSach, s.TenSach, c.SoLuongCTDDH, 
                                      c.DonGiaCTDDH, 
                                      (c.SoLuongCTDDH * c.DonGiaCTDDH) AS ThanhTien
                               FROM ChiTietDonDH c
                               JOIN Sach s ON c.MaSach = s.MaSach
                               WHERE c.MaDon = :maDon
                               ORDER BY s.TenSach";

                OracleParameter[] parameters = {
                    new OracleParameter("maDon", OracleDbType.Int32, maDon, ParameterDirection.Input)
                };

                dtChiTiet = OracleHelper.ExecuteQuery(sql, parameters);
                dgvChiTiet.DataSource = dtChiTiet;

                // Định dạng cột
                if (dgvChiTiet.Columns.Contains("DonGiaCTDDH"))
                {
                    dgvChiTiet.Columns["DonGiaCTDDH"].DefaultCellStyle.Format = "N0";
                }

                if (dgvChiTiet.Columns.Contains("ThanhTien"))
                {
                    dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                    dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    dgvChiTiet.Columns["ThanhTien"].DefaultCellStyle.Font =
                        new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold);
                }

                // Đổi tên cột
                dgvChiTiet.Columns["MaSach"].HeaderText = "Mã sách";
                dgvChiTiet.Columns["TenSach"].HeaderText = "Tên sách";
                dgvChiTiet.Columns["SoLuongCTDDH"].HeaderText = "Số lượng";
                dgvChiTiet.Columns["DonGiaCTDDH"].HeaderText = "Đơn giá";
                dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGiaiMa_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ Admin mới có quyền giải mã thông tin nhạy cảm!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Giải mã số thẻ
                if (encryptedCard != null)
                {
                    string soThe = OracleHelper.DecryptRSA(encryptedCard);
                    if (!string.IsNullOrEmpty(soThe) && soThe != "DECRYPTION_ERROR")
                    {
                        // Mask số thẻ: hiển thị 4 số cuối
                        if (soThe.Length >= 4)
                        {
                            string masked = new string('*', soThe.Length - 4) + soThe.Substring(soThe.Length - 4);
                            lblSoThe.Text = $"Số thẻ: {masked}";
                        }
                        else
                        {
                            lblSoThe.Text = $"Số thẻ: {soThe}";
                        }
                    }
                    else
                    {
                        lblSoThe.Text = "Số thẻ: [Giải mã lỗi]";
                    }
                }

                // Giải mã ghi chú
                if (encryptedNote != null)
                {
                    string ghiChu = OracleHelper.DecryptAES(encryptedNote);
                    if (!string.IsNullOrEmpty(ghiChu))
                    {
                        txtGhiChu.Text = ghiChu;
                    }
                    else
                    {
                        txtGhiChu.Text = "[Không có ghi chú]";
                    }
                }

                MessageBox.Show("Đã giải mã thông tin thanh toán!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi giải mã: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo nội dung hóa đơn
                string hoaDon = $"HÓA ĐƠN BÁN SÁCH\n";
                hoaDon += $"==================\n";
                hoaDon += $"Mã đơn: #{maDon}\n";
                hoaDon += $"Ngày đặt: {lblNgayDat.Text.Replace("Ngày đặt: ", "")}\n";
                hoaDon += $"Khách hàng: {lblKhachHang.Text.Replace("Khách hàng: ", "")}\n\n";
                hoaDon += $"CHI TIẾT SẢN PHẨM:\n";
                hoaDon += $"{"Sách",-40} {"SL",5} {"Đơn giá",15} {"Thành tiền",15}\n";
                hoaDon += new string('-', 75) + "\n";

                if (dtChiTiet != null)
                {
                    foreach (DataRow row in dtChiTiet.Rows)
                    {
                        string tenSach = row["TenSach"].ToString();
                        if (tenSach.Length > 35) tenSach = tenSach.Substring(0, 35) + "...";

                        hoaDon += $"{tenSach,-40} " +
                                 $"{row["SoLuongCTDDH"],5} " +
                                 $"{Convert.ToDecimal(row["DonGiaCTDDH"]):N0,15} " +
                                 $"{Convert.ToDecimal(row["ThanhTien"]):N0,15}\n";
                    }
                }

                hoaDon += $"\n{"Tổng tiền:",-45} {lblTongTien.Text.Replace("Tổng tiền: ", "")}\n";
                hoaDon += $"{"Phí ship:",-45} {lblPhiShip.Text.Replace("Phí ship: ", "")}\n";
                hoaDon += $"{"Thuế VAT:",-45} {lblThueVAT.Text.Replace("Thuế VAT: ", "")}\n";
                hoaDon += $"\n{"TỔNG CỘNG:",-45} {lblTongCong.Text.Replace("Tổng cộng: ", "")}\n";

                // Hiển thị hóa đơn trong MessageBox hoặc form riêng
                using (frmXemHoaDon frm = new frmXemHoaDon(hoaDon))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Form phụ để xem hóa đơn
    public class frmXemHoaDon : Form
    {
        private TextBox txtHoaDon;
        private Button btnClose;

        public frmXemHoaDon(string noiDung)
        {
            InitializeComponent();
            txtHoaDon.Text = noiDung;
        }

        private void InitializeComponent()
        {
            this.Text = "Hóa Đơn";
            this.Size = new System.Drawing.Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            txtHoaDon = new TextBox();
            txtHoaDon.Multiline = true;
            txtHoaDon.ReadOnly = true;
            txtHoaDon.Font = new System.Drawing.Font("Courier New", 10);
            txtHoaDon.Dock = DockStyle.Fill;
            txtHoaDon.ScrollBars = ScrollBars.Both;

            btnClose = new Button();
            btnClose.Text = "Đóng";
            btnClose.Dock = DockStyle.Bottom;
            btnClose.Height = 40;
            btnClose.Click += (s, e) => this.Close();

            this.Controls.Add(txtHoaDon);
            this.Controls.Add(btnClose);
        }
    }
}