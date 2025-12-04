using System;
using System.Data;
using System.Windows.Forms;

namespace Do_An_BM
{
    public partial class frmCustomerDashboard : Form
    {
        public frmCustomerDashboard()
        {
            InitializeComponent();
        }

        private void frmCustomerDashboard_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền Customer
            if (!SessionManager.IsCustomer())
            {
                MessageBox.Show("Bạn không có quyền truy cập!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Hiển thị thông tin user
            lblWelcome.Text = $"Xin chào, {SessionManager.CurrentUserName}!";
            lblUserInfo.Text = SessionManager.GetInfo();
            lblDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            // Load thống kê
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                int maKH = SessionManager.CurrentUserID;

                // 1. Số lượng sách trong giỏ hàng
                string sqlGioHang = @"
                    SELECT NVL(SUM(SoLuongSachCTGH), 0)
                    FROM ChiTietGH ctgh
                    JOIN GioHang gh ON ctgh.MaGH = gh.MaGH
                    JOIN KhachHang kh ON kh.MaGH = gh.MaGH
                    WHERE kh.MaKH = :makh
                ";

                var param1 = new Oracle.ManagedDataAccess.Client.OracleParameter("makh",
                    Oracle.ManagedDataAccess.Client.OracleDbType.Int32, maKH, System.Data.ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sqlGioHang, param1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int soLuong = Convert.ToInt32(dt.Rows[0][0]);
                    lblGioHang.Text = $"{soLuong} sản phẩm";
                }

                // 2. Tổng đơn hàng (do VPD, chỉ thấy đơn của mình)
                string sqlTongDon = "SELECT COUNT(*) FROM DonDatHang";
                dt = OracleHelper.ExecuteQuery(sqlTongDon);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblTongDon.Text = dt.Rows[0][0].ToString();
                }

                // 3. Đơn hàng chờ xác nhận
                string sqlDonCho = @"
                    SELECT COUNT(DISTINCT d.MaDon)
                    FROM DonDatHang d
                    JOIN ChiTietTrangThai ct ON d.MaDon = ct.MaDon
                    WHERE ct.MaTT = 1
                ";
                dt = OracleHelper.ExecuteQuery(sqlDonCho);
                if (dt != null && dt.Rows.Count > 0)
                {
                    lblDonHangCho.Text = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Menu Events

        private void menuTrangChu_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void menuDanhMuc_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Danh mục sách đang phát triển", "Thông báo");
            // new frmSachCatalog().ShowDialog();
            LoadStatistics();
        }

        private void menuGioHang_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Giỏ hàng đang phát triển", "Thông báo");
            LoadStatistics();
        }

        private void menuDonHang_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Đơn hàng đang phát triển", "Thông báo");
        }

        private void menuProfile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Thông tin cá nhân đang phát triển", "Thông báo");
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

        private void frmCustomerDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}