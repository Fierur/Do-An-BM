using System;
using System.Data;
using System.Windows.Forms;

namespace Do_An_BM
{
    public partial class frmAdminDashboard : Form
    {
        public frmAdminDashboard()
        {
            InitializeComponent();
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Hiển thị thông tin user
            lblUserInfo.Text = SessionManager.GetInfo();
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            // Load dashboard statistics
            LoadDashboardStats();

            timer1.Interval = 1000; // 1 giây
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void LoadDashboardStats()
        {
            try
            {
                // 1. Tổng doanh thu hôm nay
                var dt = OracleHelper.ExecuteQuery(@"
                    SELECT NVL(SUM(TongTien), 0) 
                    FROM DonDatHang 
                    WHERE TRUNC(NgayDat) = TRUNC(SYSDATE)
                ");
                if (dt != null && dt.Rows.Count > 0)
                {
                    decimal doanhThu = Convert.ToDecimal(dt.Rows[0][0]);
                    lblDoanhThu.Text = string.Format("{0:N0} VNĐ", doanhThu);
                }

                // 2. Số đơn hàng mới
                dt = OracleHelper.ExecuteQuery(@"
                    SELECT COUNT(*) 
                    FROM DonDatHang 
                    WHERE TRUNC(NgayDat) = TRUNC(SYSDATE)
                ");
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblDonHangMoi.Text = dt.Rows[0][0].ToString();
                }

                // 3. Số khách hàng mới
                dt = OracleHelper.ExecuteQuery(@"
                    SELECT COUNT(*) 
                    FROM KhachHang 
                    WHERE TRUNC(NgayDangKy) = TRUNC(SYSDATE)
                ");
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblKhachHangMoi.Text = dt.Rows[0][0].ToString();
                }

                // 4. Cảnh báo bảo mật (FGA alerts)
                dt = OracleHelper.ExecuteQuery(@"
                    SELECT COUNT(*) 
                    FROM AUDIT_LOG 
                    WHERE ACTION = 'FGA_ACCESS_SALARY' 
                    AND TRUNC(LOG_TIME) = TRUNC(SYSDATE)
                ");

                string canhBao = "✅ Hệ thống an toàn.\n\n";

                if (dt != null && dt.Rows.Count > 0)
                {
                    int fgaCount = Convert.ToInt32(dt.Rows[0][0]);
                    if (fgaCount > 0)
                    {
                        canhBao += $"⚠️ Có {fgaCount} lần truy cập cột nhạy cảm (Lương, CMND) hôm nay!\n";
                    }
                }

                // Kiểm tra đơn hàng bất thường
                dt = OracleHelper.ExecuteQuery(@"
                    SELECT COUNT(*) 
                    FROM DonDatHang 
                    WHERE TongTien > 50000000 
                    AND TRUNC(NgayDat) = TRUNC(SYSDATE)
                ");
                if (dt != null && dt.Rows.Count > 0)
                {
                    int highValueOrders = Convert.ToInt32(dt.Rows[0][0]);
                    if (highValueOrders > 0)
                    {
                        canhBao += $"💰 Có {highValueOrders} đơn hàng giá trị cao (>50 triệu) hôm nay.\n";
                    }
                }

                lblCanhBao.Text = canhBao;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dashboard: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        #region Menu Events

        private void menuNhanVien_Click(object sender, EventArgs e)
        {
            new frmNhanVienManager().ShowDialog();
            LoadDashboardStats(); // Refresh
        }

        private void menuKhachHang_Click(object sender, EventArgs e)
        {
            new frmQuanLyKhachHang().ShowDialog();
            LoadDashboardStats();
        }

        private void menuSach_Click(object sender, EventArgs e)
        {
            new frmSachManager().ShowDialog();
        }

        private void menuDonHang_Click(object sender, EventArgs e)
        {
            new frmDonHangManager().ShowDialog();
            LoadDashboardStats();
        }

        private void menuAudit_Click(object sender, EventArgs e)
        {
            new frmAuditMonitor().ShowDialog();
        }

        private void menuBaoCao_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Báo cáo đang phát triển", "Thông báo");
        }

        private void menuPhanQuyen_Click(object sender, EventArgs e)
        {
            new frmPhanQuyenOracle().ShowDialog();
        }

        private void menuSettings_Click(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SessionManager.Logout();
                this.Close();
            }
        }

        #endregion

        private void frmAdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}