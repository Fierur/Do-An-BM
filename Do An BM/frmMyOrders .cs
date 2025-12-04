using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmMyOrders : Form
    {
        public frmMyOrders()
        {
            InitializeComponent();
        }

        private void frmMyOrders_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsCustomer())
            {
                MessageBox.Show("Chỉ khách hàng mới xem được đơn hàng của mình!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            LoadMyOrders();
        }

        private void LoadMyOrders()
        {
            try
            {
                // VPD sẽ tự động filter theo MaKH từ context
                string sql = @"
                    SELECT d.MaDon, d.NgayDat, d.TongTien, d.PhiShip, 
                           (d.TongTien + d.PhiShip + (d.TongTien * d.ThueVAT / 100)) AS TongCong,
                           t.TenTT AS TrangThai
                    FROM DonDatHang d
                    LEFT JOIN (
                        SELECT MaDon, MaTT 
                        FROM ChiTietTrangThai 
                        WHERE NgayCapNhatTT = (
                            SELECT MAX(NgayCapNhatTT) 
                            FROM ChiTietTrangThai 
                            WHERE MaDon = d.MaDon
                        )
                    ) ctt ON d.MaDon = ctt.MaDon
                    LEFT JOIN TrangThai t ON ctt.MaTT = t.MaTT
                    ORDER BY d.NgayDat DESC";

                DataTable dt = OracleHelper.ExecuteQuery(sql);
                dgvDonHang.DataSource = dt;

                // Định dạng cột
                if (dgvDonHang.Columns.Contains("NgayDat"))
                {
                    dgvDonHang.Columns["NgayDat"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                }
                if (dgvDonHang.Columns.Contains("TongTien"))
                {
                    dgvDonHang.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                }
                if (dgvDonHang.Columns.Contains("PhiShip"))
                {
                    dgvDonHang.Columns["PhiShip"].DefaultCellStyle.Format = "N0";
                }
                if (dgvDonHang.Columns.Contains("TongCong"))
                {
                    dgvDonHang.Columns["TongCong"].DefaultCellStyle.Format = "N0";
                    dgvDonHang.Columns["TongCong"].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                    dgvDonHang.Columns["TongCong"].DefaultCellStyle.Font =
                        new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold);
                }

                // Đổi tên cột
                dgvDonHang.Columns["MaDon"].HeaderText = "Mã đơn";
                dgvDonHang.Columns["NgayDat"].HeaderText = "Ngày đặt";
                dgvDonHang.Columns["TongTien"].HeaderText = "Tổng tiền";
                dgvDonHang.Columns["PhiShip"].HeaderText = "Phí ship";
                dgvDonHang.Columns["TongCong"].HeaderText = "Tổng cộng";
                dgvDonHang.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải đơn hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadMyOrders();
        }

        private void btnXemChiTiet_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để xem chi tiết!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int maDon = Convert.ToInt32(dgvDonHang.SelectedRows[0].Cells["MaDon"].Value);
            frmDonHangDetail frm = new frmDonHangDetail(maDon);
            frm.ShowDialog();
        }

        private void btnHuyDon_Click(object sender, EventArgs e)
        {
            if (dgvDonHang.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để hủy!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int maDon = Convert.ToInt32(dgvDonHang.SelectedRows[0].Cells["MaDon"].Value);
            string trangThai = dgvDonHang.SelectedRows[0].Cells["TrangThai"].Value?.ToString() ?? "";

            // Chỉ được hủy nếu đơn chưa xác nhận
            if (trangThai != "Chờ xác nhận" && trangThai != "")
            {
                MessageBox.Show("Chỉ có thể hủy đơn hàng đang ở trạng thái 'Chờ xác nhận'!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = @"
                        INSERT INTO ChiTietTrangThai (MaDon, MaTT, GhiChuTT)
                        VALUES (:madon, 5, 'Khách hàng tự hủy đơn')";

                    var param = new OracleParameter("madon", OracleDbType.Int32, maDon, ParameterDirection.Input);
                    int result = OracleHelper.ExecuteNonQuery(sql, param);

                    if (result > 0)
                    {
                        MessageBox.Show("Đã hủy đơn hàng thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadMyOrders();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hủy đơn hàng: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}