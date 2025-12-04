using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmAuditMonitor : Form
    {
        public frmAuditMonitor()
        {
            InitializeComponent();
        }

        private void frmAuditMonitor_Load(object sender, EventArgs e)
        {
            // Kiểm tra quyền Admin
            if (!SessionManager.IsAdmin())
            {
                MessageBox.Show("Chỉ Admin mới có quyền truy cập!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Khởi tạo date pickers
            dtpStart.Value = DateTime.Now.AddDays(-7);
            dtpEnd.Value = DateTime.Now;

            // Load dữ liệu mặc định
            LoadAuditLog();
            LoadFGA();
            LoadVPDStatus();
            LoadFlashbackTables();
        }

        #region Tab 1: Audit Log

        private void LoadAuditLog()
        {
            try
            {
                string sql = @"
                    SELECT 
                        LOG_ID,
                        TO_CHAR(LOG_TIME, 'YYYY-MM-DD HH24:MI:SS') AS Thoi_Gian,
                        USERNAME AS Nguoi_Dung,
                        ACTION AS Hanh_Dong,
                        TABLE_NAME AS Bang,
                        RECORD_ID AS ID_Record,
                        SUBSTR(IP_ADDRESS, 1, 20) AS IP_Hash
                    FROM AUDIT_LOG
                    WHERE TRUNC(LOG_TIME) BETWEEN :start_date AND :end_date
                    ORDER BY LOG_TIME DESC
                ";

                var param1 = new OracleParameter("start_date", OracleDbType.Date, dtpStart.Value.Date, ParameterDirection.Input);
                var param2 = new OracleParameter("end_date", OracleDbType.Date, dtpEnd.Value.Date, ParameterDirection.Input);

                DataTable dt = OracleHelper.ExecuteQuery(sql, param1, param2);
                dgvAuditLog.DataSource = dt;
                dgvAuditLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load Audit Log: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshAudit_Click(object sender, EventArgs e)
        {
            LoadAuditLog();
        }

        private void btnVerifySignature_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAuditLog.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn một log để verify!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int logId = Convert.ToInt32(dgvAuditLog.SelectedRows[0].Cells["LOG_ID"].Value);

                // Lấy signature đã lưu
                string sqlStored = "SELECT LOG_SIGNATURE FROM AUDIT_LOG WHERE LOG_ID = :log_id";
                var param1 = new OracleParameter("log_id", OracleDbType.Int32, logId, ParameterDirection.Input);
                DataTable dtStored = OracleHelper.ExecuteQuery(sqlStored, param1);

                if (dtStored == null || dtStored.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy log!", "Lỗi");
                    return;
                }

                byte[] storedSignature = (byte[])dtStored.Rows[0]["LOG_SIGNATURE"];

                // Tính signature mới
                byte[] calculatedSignature = OracleHelper.GenerateLogSignature(logId);

                // So sánh
                bool isValid = CompareByteArrays(storedSignature, calculatedSignature);

                if (isValid)
                {
                    MessageBox.Show(
                        "✓ SIGNATURE VALID\n\n" +
                        "Log chưa bị giả mạo (tamper-proof verified).",
                        "Kết quả Verify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "✗ SIGNATURE INVALID\n\n" +
                        "⚠️ Log có thể đã bị chỉnh sửa!",
                        "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi verify signature: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CompareByteArrays(byte[] arr1, byte[] arr2)
        {
            if (arr1 == null || arr2 == null) return false;
            if (arr1.Length != arr2.Length) return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i]) return false;
            }
            return true;
        }

        #endregion

        #region Tab 2: FGA

        private void LoadFGA()
        {
            try
            {
                string sql = @"
                    SELECT 
                        LOG_ID,
                        TO_CHAR(LOG_TIME, 'YYYY-MM-DD HH24:MI:SS') AS Thoi_Gian,
                        USERNAME AS Nguoi_Dung,
                        ACTION AS Hanh_Dong,
                        TABLE_NAME AS Bang,
                        SUBSTR(IP_ADDRESS, 1, 20) AS IP_Hash
                    FROM AUDIT_LOG
                    WHERE ACTION = 'FGA_ACCESS_SALARY'
                    ORDER BY LOG_TIME DESC
                    FETCH FIRST 100 ROWS ONLY
                ";

                DataTable dt = OracleHelper.ExecuteQuery(sql);
                dgvFGA.DataSource = dt;
                dgvFGA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load FGA: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshFGA_Click(object sender, EventArgs e)
        {
            LoadFGA();
        }

        #endregion

        #region Tab 3: VPD Status

        private void LoadVPDStatus()
        {
            try
            {
                // Lấy context hiện tại
                string sqlContext = @"
                SELECT
                    SYS_CONTEXT('USER_CTX', 'USER_TYPE') AS USER_TYPE,
                    SYS_CONTEXT('USER_CTX', 'MAKH') AS MAKH,
                    SYS_CONTEXT('USER_CTX', 'MANV') AS MANV
                FROM DUAL
                    ";
                DataTable dtContext = OracleHelper.ExecuteQuery(sqlContext);
                if (dtContext != null && dtContext.Rows.Count > 0)
                {
                    lblUserType.Text = dtContext.Rows[0]["USER_TYPE"]?.ToString() ?? "N/A";
                    lblMAKH.Text = dtContext.Rows[0]["MAKH"]?.ToString() ?? "N/A";
                    lblMANV.Text = dtContext.Rows[0]["MANV"]?.ToString() ?? "N/A";
                }

                // Load active policies
                string sqlPolicies = @"
                SELECT 
                    object_schema AS Schema,
                    object_name AS Table_Name,
                    policy_name AS Policy_Name,
                    function AS Function_Name,
                    CASE WHEN enable = 'YES' THEN '✓ Enabled' ELSE '✗ Disabled' END AS Status
                FROM ALL_POLICIES
                WHERE object_owner = 'BM_USER'
                ORDER BY object_name, policy_name
            ";

                DataTable dtPolicies = OracleHelper.ExecuteQuery(sqlPolicies);
                dgvPolicies.DataSource = dtPolicies;
                dgvPolicies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load VPD Status: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab 4: Flashback Recovery

        private void LoadFlashbackTables()
        {
            cboTable.Items.Clear();
            cboTable.Items.Add("DonDatHang");
            cboTable.Items.Add("KhachHang");
            cboTable.SelectedIndex = 0;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtRecordID.Text))
                {
                    MessageBox.Show("Vui lòng nhập ID cần phục hồi!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tableName = cboTable.Text;
                int recordId = Convert.ToInt32(txtRecordID.Text);
                int minutesAgo = (int)nudMinutes.Value;

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn phục hồi dữ liệu?\n\n" +
                    $"Bảng: {tableName}\n" +
                    $"ID: {recordId}\n" +
                    $"Thời điểm: {minutesAgo} phút trước",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.No) return;

                bool success = false;

                if (tableName == "DonDatHang")
                {
                    success = OracleHelper.RestoreDeletedOrder(
                        SessionManager.CurrentUserID,
                        recordId,
                        minutesAgo
                    );
                }
                else if (tableName == "KhachHang")
                {
                    // Gọi procedure RESTORE_KHACHHANG
                    var param1 = new OracleParameter("p_admin_manv", OracleDbType.Int32,
                        SessionManager.CurrentUserID, ParameterDirection.Input);
                    var param2 = new OracleParameter("p_makh", OracleDbType.Int32,
                        recordId, ParameterDirection.Input);
                    var param3 = new OracleParameter("p_minutes_ago", OracleDbType.Int32,
                        minutesAgo, ParameterDirection.Input);

                    success = OracleHelper.ExecuteProcedure("BM_USER.RESTORE_KHACHHANG",
                        param1, param2, param3);
                }

                if (success)
                {
                    MessageBox.Show("Phục hồi dữ liệu thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không thể phục hồi dữ liệu. Có thể:\n" +
                        "- Dữ liệu vẫn tồn tại\n" +
                        "- Không tìm thấy dữ liệu trong khoảng thời gian đã chọn\n" +
                        "- Thiếu quyền FLASHBACK",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phục hồi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}