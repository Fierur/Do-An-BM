using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmSachCatalog : Form
    {
        private DataTable dsSach;
        private DataTable dsTheLoai;

        public frmSachCatalog()
        {
            InitializeComponent();
        }

        private void frmSachCatalog_Load(object sender, EventArgs e)
        {
            LoadTheLoai();
            LoadSach();
        }

        private void LoadTheLoai()
        {
            try
            {
                string sql = "SELECT MaTLS, TenTLS FROM TheLoaiSach ORDER BY TenTLS";
                dsTheLoai = OracleHelper.ExecuteQuery(sql);

                cboTheLoai.DataSource = dsTheLoai;
                cboTheLoai.DisplayMember = "TENTLS";
                cboTheLoai.ValueMember = "MATLS";
                cboTheLoai.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thể loại: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSach(string search = "", int? maTLS = null)
        {
            try
            {
                string sql = @"SELECT s.MaSach, s.TenSach, s.Gia, s.Mota, 
                                      t.TenTLS, n.TenNXB, s.Hinh
                               FROM Sach s
                               LEFT JOIN TheLoaiSach t ON s.MaTLS = t.MaTLS
                               LEFT JOIN NhaXuatBan n ON s.MaNXB = n.MaNXB
                               WHERE 1=1";

                if (!string.IsNullOrEmpty(search))
                {
                    sql += " AND UPPER(s.TenSach) LIKE UPPER(:search)";
                }

                if (maTLS.HasValue)
                {
                    sql += " AND s.MaTLS = :maTLS";
                }

                sql += " ORDER BY s.TenSach";

                OracleParameter[] parameters = null;
                if (!string.IsNullOrEmpty(search) || maTLS.HasValue)
                {
                    var paramList = new System.Collections.Generic.List<OracleParameter>();
                    if (!string.IsNullOrEmpty(search))
                    {
                        paramList.Add(new OracleParameter("search", OracleDbType.Varchar2, $"%{search}%", ParameterDirection.Input));
                    }
                    if (maTLS.HasValue)
                    {
                        paramList.Add(new OracleParameter("maTLS", OracleDbType.Int32, maTLS.Value, ParameterDirection.Input));
                    }
                    parameters = paramList.ToArray();
                }

                dsSach = OracleHelper.ExecuteQuery(sql, parameters);
                DisplaySach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sách: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplaySach()
        {
            flowLayoutPanel1.Controls.Clear();
            if (dsSach == null || dsSach.Rows.Count == 0)
            {
                Label lbl = new Label();
                lbl.Text = "Không tìm thấy sách nào.";
                lbl.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Italic);
                lbl.AutoSize = true;
                flowLayoutPanel1.Controls.Add(lbl);
                lblTongSach.Text = "Tổng sách: 0 cuốn";
                return;
            }

            foreach (DataRow row in dsSach.Rows)
            {
                Panel card = CreateBookCard(row);
                flowLayoutPanel1.Controls.Add(card);
            }

            lblTongSach.Text = $"Tổng sách: {dsSach.Rows.Count} cuốn";
        }

        private Panel CreateBookCard(DataRow book)
        {
            Panel panel = new Panel();
            panel.Size = new Size(250, 400);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Margin = new Padding(10);
            panel.BackColor = Color.White;

            // Hình ảnh
            PictureBox pic = new PictureBox();
            pic.Size = new Size(230, 180);
            pic.Location = new Point(10, 10);
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            pic.BackColor = Color.LightGray;

            string hinhPath = book["Hinh"] != DBNull.Value ? book["Hinh"].ToString() : "";
            if (!string.IsNullOrEmpty(hinhPath))
            {
                try
                {
                    pic.Image = Image.FromFile(hinhPath);
                }
                catch
                {
                    pic.Image = Properties.Resources.book_default; // Cần thêm resource
                }
            }

            // Thông tin sách
            Label lblTen = new Label();
            lblTen.Text = book["TenSach"].ToString();
            lblTen.Font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);
            lblTen.Location = new Point(10, 200);
            lblTen.Size = new Size(230, 40);

            Label lblGia = new Label();
            decimal gia = Convert.ToDecimal(book["Gia"]);
            lblGia.Text = $"Giá: {gia:N0} VNĐ";
            lblGia.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
            lblGia.ForeColor = Color.Red;
            lblGia.Location = new Point(10, 240);
            lblGia.Size = new Size(230, 20);

            Label lblTheLoai = new Label();
            lblTheLoai.Text = $"Thể loại: {book["TenTLS"]}";
            lblTheLoai.Font = new Font("Microsoft Sans Serif", 9);
            lblTheLoai.Location = new Point(10, 265);
            lblTheLoai.Size = new Size(230, 20);

            Label lblNXB = new Label();
            lblNXB.Text = $"NXB: {book["TenNXB"]}";
            lblNXB.Font = new Font("Microsoft Sans Serif", 9);
            lblNXB.Location = new Point(10, 285);
            lblNXB.Size = new Size(230, 20);

            // Button thêm giỏ hàng
            Button btnThem = new Button();
            btnThem.Text = "Thêm vào giỏ hàng";
            btnThem.Font = new Font("Microsoft Sans Serif", 9);
            btnThem.Location = new Point(10, 320);
            btnThem.Size = new Size(230, 35);
            btnThem.Tag = book["MaSach"]; // Lưu MaSach
            btnThem.Click += BtnThemGioHang_Click;

            panel.Controls.Add(pic);
            panel.Controls.Add(lblTen);
            panel.Controls.Add(lblGia);
            panel.Controls.Add(lblTheLoai);
            panel.Controls.Add(lblNXB);
            panel.Controls.Add(btnThem);

            return panel;
        }

        private void BtnThemGioHang_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn || !SessionManager.IsCustomer())
            {
                MessageBox.Show("Vui lòng đăng nhập với tài khoản khách hàng để thêm sách vào giỏ!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Button btn = sender as Button;
            if (btn != null)
            {
                int maSach = Convert.ToInt32(btn.Tag);
                ThemVaoGioHang(maSach);
            }
        }

        private void ThemVaoGioHang(int maSach)
        {
            try
            {
                // Kiểm tra sách đã có trong giỏ chưa
                string checkSql = @"SELECT COUNT(*) FROM ChiTietGH c
                                   JOIN GioHang g ON c.MaGH = g.MaGH
                                   WHERE c.MaSach = :maSach 
                                   AND g.MaGH = (SELECT MaGH FROM KhachHang WHERE MaKH = :maKH)";

                OracleParameter[] checkParams = {
                    new OracleParameter("maSach", OracleDbType.Int32, maSach, ParameterDirection.Input),
                    new OracleParameter("maKH", OracleDbType.Int32, SessionManager.CurrentUserID, ParameterDirection.Input)
                };

                DataTable dtCheck = OracleHelper.ExecuteQuery(checkSql, checkParams);
                int count = Convert.ToInt32(dtCheck.Rows[0][0]);

                if (count > 0)
                {
                    // Cập nhật số lượng
                    string updateSql = @"UPDATE ChiTietGH c
                                        SET SoLuongSachCTGH = SoLuongSachCTGH + 1
                                        WHERE c.MaSach = :maSach 
                                        AND c.MaGH = (SELECT MaGH FROM KhachHang WHERE MaKH = :maKH)";
                    int result = OracleHelper.ExecuteNonQuery(updateSql, checkParams);
                    if (result > 0)
                        MessageBox.Show("Đã cập nhật số lượng sách trong giỏ hàng!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Thêm mới
                    string insertSql = @"INSERT INTO ChiTietGH (MaGH, MaSach, SoLuongSachCTGH)
                                        SELECT MaGH, :maSach, 1 
                                        FROM KhachHang 
                                        WHERE MaKH = :maKH";
                    int result = OracleHelper.ExecuteNonQuery(insertSql, checkParams);
                    if (result > 0)
                        MessageBox.Show("Đã thêm sách vào giỏ hàng!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm vào giỏ hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string search = txtTimKiem.Text.Trim();
            int? maTLS = null;
            if (cboTheLoai.SelectedIndex >= 0)
                maTLS = Convert.ToInt32(cboTheLoai.SelectedValue);

            LoadSach(search, maTLS);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            cboTheLoai.SelectedIndex = -1;
            LoadSach();
        }

        private void cboTheLoai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTheLoai.SelectedIndex >= 0)
            {
                string search = txtTimKiem.Text.Trim();
                int maTLS = Convert.ToInt32(cboTheLoai.SelectedValue);
                LoadSach(search, maTLS);
            }
        }

        private void btnXemGioHang_Click(object sender, EventArgs e)
        {
            if (!SessionManager.IsLoggedIn || !SessionManager.IsCustomer())
            {
                MessageBox.Show("Vui lòng đăng nhập với tài khoản khách hàng!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmGioHang frm = new frmGioHang();
            frm.ShowDialog();
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}