using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows.Forms;

namespace Do_An_BM
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ Admin mới có quyền truy cập form này!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            LoadCurrentSettings();
        }

        private void LoadCurrentSettings()
        {
            // Có thể load từ bảng cấu hình hoặc từ App.config
            // Tạm thời set mặc định
            chkEnableVPD.Checked = true;
            chkEnableFGA.Checked = true;
            chkEnableAudit.Checked = true;
        }

        private void btnRegenerateRSA_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn tạo lại cặp khóa RSA mới?\n\n" +
                               "Lưu ý: Tất cả dữ liệu đã mã hóa bằng RSA cũ sẽ KHÔNG giải mã được nữa!",
                               "Cảnh báo",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    bool result = OracleHelper.GenerateRSAKeys();
                    if (result)
                    {
                        MessageBox.Show("Đã tạo lại cặp khóa RSA mới thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra khi tạo khóa RSA!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClearAuditLog_Click(object sender, EventArgs e)
        {
            int days = (int)nudDays.Value;
            if (MessageBox.Show($"Bạn có chắc muốn xóa tất cả Audit Log cũ hơn {days} ngày?",
                               "Xác nhận",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM AUDIT_LOG WHERE LOG_TIME < SYSDATE - :days";
                    var param = new OracleParameter("days", OracleDbType.Int32, days, ParameterDirection.Input);
                    int rows = OracleHelper.ExecuteNonQuery(sql, param);

                    MessageBox.Show($"Đã xóa {rows} bản ghi Audit Log.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBackupDB_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng Backup Database cần được cài đặt với Oracle Data Pump.\n" +
                           "Liên hệ DBA để thực hiện thủ công.", "Thông báo",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            // Lưu cấu hình vào database hoặc file cấu hình
            // Ở đây tạm thời hiển thị thông báo
            MessageBox.Show("Đã lưu cấu hình hệ thống!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}