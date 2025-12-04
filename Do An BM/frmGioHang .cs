using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmGioHang : Form
    {
        private DataTable dsGioHang;
        private decimal tongTien = 0;

        public frmGioHang()
        {
            InitializeComponent();
        }

        private void frmGioHang_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn || !SessionManager.IsCustomer())
            {
                MessageBox.Show("Vui lòng đăng nhập với tài khoản khách hàng!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            LoadGioHang();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            dgvGioHang.Columns.Clear();

            // Thêm cột tự động từ DataTable
            dgvGioHang.DataSource = dsGioHang;

            // Thêm cột nút xóa
            DataGridViewButtonColumn btnXoa = new DataGridViewButtonColumn();
            btnXoa.Name = "Xoa";
            btnXoa.HeaderText = "Xóa";
            btnXoa.Text = "Xóa";
            btnXoa.UseColumnTextForButtonValue = true;
            dgvGioHang.Columns.Add(btnXoa);

            // Thêm cột nút sửa số lượng
            DataGridViewButtonColumn btnSuaSL = new DataGridViewButtonColumn();
            btnSuaSL.Name = "SuaSL";
            btnSuaSL.HeaderText = "Sửa SL";
            btnSuaSL.Text = "Sửa";
            btnSuaSL.UseColumnTextForButtonValue = true;
            dgvGioHang.Columns.Add(btnSuaSL);

            // Định dạng cột
            if (dgvGioHang.Columns.Contains("Gia"))
            {
                dgvGioHang.Columns["Gia"].DefaultCellStyle.Format = "N0";
            }

            if (dgvGioHang.Columns.Contains("ThanhTien"))
            {
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Font =
                    new System.Drawing.Font("Microsoft Sans Serif", 9, System.Drawing.FontStyle.Bold);
            }
        }

        private void LoadGioHang()
        {
            try
            {
                string sql = @"SELECT s.MaSach, s.TenSach, s.Gia, 
                                      c.SoLuongSachCTGH AS SoLuong,
                                      (s.Gia * c.SoLuongSachCTGH) AS ThanhTien
                               FROM ChiTietGH c
                               JOIN Sach s ON c.MaSach = s.MaSach
                               JOIN GioHang g ON c.MaGH = g.MaGH
                               JOIN KhachHang k ON g.MaGH = k.MaGH
                               WHERE k.MaKH = :maKH
                               ORDER BY s.TenSach";

                OracleParameter[] parameters = {
                    new OracleParameter("maKH", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
                };

                dsGioHang = OracleHelper.ExecuteQuery(sql, parameters);
                TinhTongTien();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải giỏ hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TinhTongTien()
        {
            tongTien = 0;
            if (dsGioHang != null)
            {
                foreach (DataRow row in dsGioHang.Rows)
                {
                    decimal thanhTien = Convert.ToDecimal(row["ThanhTien"]);
                    tongTien += thanhTien;
                }
            }
            lblTongTien.Text = $"Tổng tiền: {tongTien:N0} đ";
        }

        private void dgvGioHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Xử lý nút Xóa
            if (e.ColumnIndex == dgvGioHang.Columns["Xoa"].Index)
            {
                int maSach = Convert.ToInt32(dgvGioHang.Rows[e.RowIndex].Cells["MaSach"].Value);
                XoaKhoiGioHang(maSach);
            }
            // Xử lý nút Sửa SL
            else if (e.ColumnIndex == dgvGioHang.Columns["SuaSL"].Index)
            {
                int maSach = Convert.ToInt32(dgvGioHang.Rows[e.RowIndex].Cells["MaSach"].Value);
                int soLuongHienTai = Convert.ToInt32(dgvGioHang.Rows[e.RowIndex].Cells["SoLuong"].Value);

                using (frmSuaSoLuang frm = new frmSuaSoLuang(soLuongHienTai))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int soLuongMoi = frm.SoLuong;
                        CapNhatSoLuong(maSach, soLuongMoi);
                    }
                }
            }
        }

        private void XoaKhoiGioHang(int maSach)
        {
            try
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa sách này khỏi giỏ hàng?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                string sql = @"DELETE FROM ChiTietGH c
                               WHERE c.MaSach = :maSach 
                               AND c.MaGH = (SELECT MaGH FROM KhachHang WHERE MaKH = :maKH)";

                OracleParameter[] parameters = {
                    new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input),
                    new OracleParameter("maKH", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
                };

                int rowsAffected = OracleHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Đã xóa sách khỏi giỏ hàng!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGioHang();
                    SetupDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CapNhatSoLuong(int maSach, int soLuongMoi)
        {
            if (soLuongMoi <= 0)
            {
                XoaKhoiGioHang(maSach);
                return;
            }

            try
            {
                string sql = @"UPDATE ChiTietGH c
                               SET SoLuongSachCTGH = :soLuong
                               WHERE c.MaSach = :maSach 
                               AND c.MaGH = (SELECT MaGH FROM KhachHang WHERE MaKH = :maKH)";

                OracleParameter[] parameters = {
                    new OracleParameter("soLuong", OracleDbType.Int32, soLuongMoi, ParameterDirection.Input),
                    new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input),
                    new OracleParameter("maKH", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
                };

                int rowsAffected = OracleHelper.ExecuteNonQuery(sql, parameters);

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Đã cập nhật số lượng!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGioHang();
                    SetupDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật số lượng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (dsGioHang == null || dsGioHang.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Chuyển đến form thanh toán
            frmCheckout frm = new frmCheckout();
            frm.ShowDialog();

            // Sau khi thanh toán, reload giỏ hàng
            if (frm.DialogResult == DialogResult.OK)
            {
                LoadGioHang();
                SetupDataGridView();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGioHang();
            SetupDataGridView();
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    // Form phụ để sửa số lượng
    public class frmSuaSoLuang : Form
    {
        private NumericUpDown nudSoLuong;
        private Button btnOK;
        private Button btnCancel;
        public int SoLuong { get; private set; }

        public frmSuaSoLuang(int soLuongHienTai)
        {
            InitializeComponent();
            nudSoLuong.Value = soLuongHienTai;
        }

        private void InitializeComponent()
        {
            this.Text = "Sửa số lượng";
            this.Size = new System.Drawing.Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lbl = new Label();
            lbl.Text = "Số lượng:";
            lbl.Location = new System.Drawing.Point(20, 30);
            lbl.Size = new System.Drawing.Size(80, 25);

            nudSoLuong = new NumericUpDown();
            nudSoLuong.Minimum = 1;
            nudSoLuong.Maximum = 100;
            nudSoLuong.Location = new System.Drawing.Point(100, 30);
            nudSoLuong.Size = new System.Drawing.Size(100, 25);

            btnOK = new Button();
            btnOK.Text = "OK";
            btnOK.DialogResult = DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(50, 80);
            btnOK.Size = new System.Drawing.Size(80, 30);
            btnOK.Click += (s, e) => { SoLuong = (int)nudSoLuong.Value; this.Close(); };

            btnCancel = new Button();
            btnCancel.Text = "Hủy";
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(150, 80);
            btnCancel.Size = new System.Drawing.Size(80, 30);

            this.Controls.Add(lbl);
            this.Controls.Add(nudSoLuong);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }
    }
}