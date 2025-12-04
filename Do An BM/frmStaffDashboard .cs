using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmStaffDashboard : Form
    {
        public frmStaffDashboard()
        {
            InitializeComponent();
        }

        private void frmStaffDashboard_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsStaff())
            {
                MessageBox.Show("Chỉ nhân viên mới được truy cập!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            lblUserInfo.Text = $"Nhân viên: {SessionManager.CurrentUserName}";
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                // Tổng đơn hàng (của tất cả khách hàng)
                string sqlTongDon = "SELECT COUNT(*) FROM DonDatHang";
                DataTable dtTongDon = OracleHelper.ExecuteQuery(sqlTongDon);
                if (dtTongDon.Rows.Count > 0)
                {
                    lblTongDon.Text = dtTongDon.Rows[0][0].ToString();
                }

                // Đơn chờ xác nhận (trạng thái 1)
                string sqlChoXN = @"
                    SELECT COUNT(*) 
                    FROM DonDatHang d
                    WHERE d.MaDon IN (
                        SELECT MaDon FROM ChiTietTrangThai 
                        WHERE MaTT = 1 AND NgayCapNhatTT = (
                            SELECT MAX(NgayCapNhatTT) 
                            FROM ChiTietTrangThai 
                            WHERE MaDon = d.MaDon
                        )
                    )";
                DataTable dtChoXN = OracleHelper.ExecuteQuery(sqlChoXN);
                if (dtChoXN.Rows.Count > 0)
                {
                    lblDonChoXN.Text = dtChoXN.Rows[0][0].ToString();
                }

                // Đơn đang giao (trạng thái 3)
                string sqlDangGiao = @"
                    SELECT COUNT(*) 
                    FROM DonDatHang d
                    WHERE d.MaDon IN (
                        SELECT MaDon FROM ChiTietTrangThai 
                        WHERE MaTT = 3 AND NgayCapNhatTT = (
                            SELECT MAX(NgayCapNhatTT) 
                            FROM ChiTietTrangThai 
                            WHERE MaDon = d.MaDon
                        )
                    )";
                DataTable dtDangGiao = OracleHelper.ExecuteQuery(sqlDangGiao);
                if (dtDangGiao.Rows.Count > 0)
                {
                    lblDonDangGiao.Text = dtDangGiao.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void menuQLKhachHang_Click(object sender, EventArgs e)
        {
            // Mở form quản lý khách hàng (chỉ xem/sửa, không xóa)
            frmQuanLyKhachHang frm = new frmQuanLyKhachHang();
            //frm.IsReadOnly = true; // Giả sử có property này
            frm.ShowDialog();
        }

        private void menuQLDonHang_Click(object sender, EventArgs e)
        {
            // Mở form quản lý đơn hàng (cập nhật trạng thái)
            frmDonHangManager frm = new frmDonHangManager();
            frm.ShowDialog();
        }

        private void menuQLSach_Click(object sender, EventArgs e)
        {
            // Mở form danh mục sách (chỉ xem)
            frmSachCatalog frm = new frmSachCatalog();
            frm.ShowDialog();
        }

        private void menuDangXuat_Click(object sender, EventArgs e)
        {
            SessionManager.Logout();
            this.Close();
            // Quay lại form Login
            Application.Restart();
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}