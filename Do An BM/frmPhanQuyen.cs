using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Do_An_BM
{
    public partial class frmPhanQuyenOracle : Form
    {
        public frmPhanQuyenOracle()
        {
            InitializeComponent();
        }

        private void frmPhanQuyenOracle_Load(object sender, EventArgs e)
        {
            // Kiểm tra kết nối
            if (Database.Con == null || Database.Con.State != ConnectionState.Open)
            {
                MessageBox.Show("Chưa kết nối đến cơ sở dữ liệu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            lblUserInfo.Text = $"Đăng nhập với: {Database.User}";

            LoadAllUsers();
            LoadAllRoles();

            cboObjectType.Items.Clear();
            cboObjectType.Items.Add("PROCEDURE");
            cboObjectType.Items.Add("FUNCTION");
            // TRIGGER không GRANT EXECUTE được, nên chỉ cho xem (disable GRANT)
            cboObjectType.Items.Add("TRIGGER");
            cboObjectType.SelectedIndex = 0;
        }

        #region Load users/roles

        private void LoadAllUsers()
        {
            try
            {
                const string sql = "SELECT USERNAME FROM DBA_USERS WHERE ACCOUNT_STATUS = 'OPEN' ORDER BY USERNAME";
                using (var ad = new OracleDataAdapter(sql, Database.Con))
                {
                    var dt = new DataTable();
                    ad.Fill(dt);

                    cboOwner.DataSource = dt.Copy();
                    cboOwner.DisplayMember = "USERNAME";
                    cboOwner.SelectedIndex = -1;

                    cboTableOwner.DataSource = dt.Copy();
                    cboTableOwner.DisplayMember = "USERNAME";
                    cboTableOwner.SelectedIndex = -1;

                    cboTableOwner2.DataSource = dt.Copy();
                    cboTableOwner2.DisplayMember = "USERNAME";
                    cboTableOwner2.SelectedIndex = -1;

                    cboTargetUser.DataSource = dt.Copy();
                    cboTargetUser.DisplayMember = "USERNAME";
                    cboTargetUser.SelectedIndex = -1;

                    cboUserForTable.DataSource = dt.Copy();
                    cboUserForTable.DisplayMember = "USERNAME";
                    cboUserForTable.SelectedIndex = -1;

                    cboViewUser.DataSource = dt.Copy();
                    cboViewUser.DisplayMember = "USERNAME";
                    cboViewUser.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách users: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAllRoles()
        {
            try
            {
                const string sql = "SELECT ROLE FROM DBA_ROLES ORDER BY ROLE";
                using (var ad = new OracleDataAdapter(sql, Database.Con))
                {
                    var dt = new DataTable();
                    ad.Fill(dt);

                    cboTargetRole.DataSource = dt.Copy();
                    cboTargetRole.DisplayMember = "ROLE";
                    cboTargetRole.SelectedIndex = -1;

                    cboRoleForTable.DataSource = dt.Copy();
                    cboRoleForTable.DisplayMember = "ROLE";
                    cboRoleForTable.SelectedIndex = -1;

                    cboViewRole.DataSource = dt.Copy();
                    cboViewRole.DisplayMember = "ROLE";
                    cboViewRole.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách roles: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab 1: PROC/FUNC/TRIGGER

        private void btnLoadObjects_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboOwner.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner/User!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboObjectType.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn loại object!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text;
                string objectType = cboObjectType.Text;

                const string sql = @"SELECT OBJECT_NAME 
                                     FROM DBA_OBJECTS 
                                     WHERE OWNER = :owner 
                                       AND OBJECT_TYPE = :objectType
                                     ORDER BY OBJECT_NAME";

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner.ToUpper();
                    cmd.Parameters.Add("objectType", OracleDbType.Varchar2).Value = objectType.ToUpper();

                    using (var ad = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);

                        cboObject.DataSource = dt;
                        cboObject.DisplayMember = "OBJECT_NAME";
                        cboObject.SelectedIndex = -1;
                    }
                }

                MessageBox.Show($"Đã tải danh sách {objectType} của {owner}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải objects: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateObjectSelection()
        {
            if (cboOwner.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Owner!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboObject.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn Object!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsTriggerSelected()
        {
            return string.Equals(cboObjectType.Text, "TRIGGER", StringComparison.OrdinalIgnoreCase);
        }

        private void btnGrantExecToUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTriggerSelected())
                {
                    MessageBox.Show("Oracle không cho GRANT EXECUTE trên TRIGGER.\n" +
                                    "Chỉ áp dụng EXECUTE cho PROCEDURE/FUNCTION/PACKAGE.",
                        "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateObjectSelection()) return;

                if (cboTargetUser.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text.ToUpper();
                string objectName = cboObject.Text.ToUpper();
                string targetUser = cboTargetUser.Text.ToUpper();

                DialogResult dr = MessageBox.Show(
                    $"Bạn có chắc muốn GRANT EXECUTE trên {owner}.{objectName} cho USER {targetUser}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No) return;

                string sql = $"GRANT EXECUTE ON {owner}.{objectName} TO {targetUser}";
                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Đã GRANT EXECUTE thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Lỗi khi GRANT EXECUTE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT EXECUTE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrantExecToRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTriggerSelected())
                {
                    MessageBox.Show("Oracle không cho GRANT EXECUTE trên TRIGGER.\n" +
                                    "Chỉ áp dụng EXECUTE cho PROCEDURE/FUNCTION/PACKAGE.",
                        "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateObjectSelection()) return;

                if (cboTargetRole.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text.ToUpper();
                string objectName = cboObject.Text.ToUpper();
                string role = cboTargetRole.Text.ToUpper();

                DialogResult dr = MessageBox.Show(
                    $"Bạn có chắc muốn GRANT EXECUTE trên {owner}.{objectName} cho ROLE {role}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No) return;

                string sql = $"GRANT EXECUTE ON {owner}.{objectName} TO {role}";
                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Đã GRANT EXECUTE thành công!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Lỗi khi GRANT EXECUTE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT EXECUTE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab 2: Table privileges for ROLE

        private void btnLoadTables_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTableOwner.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner/User!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboTableOwner.Text.ToUpper();

                const string sql = @"SELECT TABLE_NAME 
                                     FROM DBA_TABLES 
                                     WHERE OWNER = :owner
                                     UNION
                                     SELECT VIEW_NAME AS TABLE_NAME
                                     FROM DBA_VIEWS
                                     WHERE OWNER = :owner
                                     ORDER BY TABLE_NAME";

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;

                    using (var ad = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);

                        cboTable.DataSource = dt;
                        cboTable.DisplayMember = "TABLE_NAME";
                        cboTable.SelectedIndex = -1;
                    }
                }

                MessageBox.Show($"Đã tải danh sách TABLE/VIEW của {owner}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải tables: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrantTableToRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTableOwner.SelectedIndex < 0 || cboTable.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner và Table!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboRoleForTable.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!cbSelect.Checked && !cbInsert.Checked && !cbUpdate.Checked && !cbDelete.Checked)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 quyền!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboTableOwner.Text.ToUpper();
                string table = cboTable.Text.ToUpper();
                string role = cboRoleForTable.Text.ToUpper();

                var privs = new System.Collections.Generic.List<string>();
                if (cbSelect.Checked) privs.Add("SELECT");
                if (cbInsert.Checked) privs.Add("INSERT");
                if (cbUpdate.Checked) privs.Add("UPDATE");
                if (cbDelete.Checked) privs.Add("DELETE");

                string privList = string.Join(", ", privs);

                DialogResult dr = MessageBox.Show(
                    $"GRANT các quyền sau?\n\nTable: {owner}.{table}\nRole: {role}\nQuyền: {privList}",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No) return;

                foreach (string p in privs)
                {
                    string sql = $"GRANT {p} ON {owner}.{table} TO {role}";
                    using (var cmd = new OracleCommand(sql, Database.Con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã GRANT thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region REVOKE functions

        private void btnRevokeExecFromUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTriggerSelected())
                {
                    MessageBox.Show("Oracle không cho REVOKE EXECUTE trên TRIGGER.\n" +
                                    "Chỉ áp dụng EXECUTE cho PROCEDURE/FUNCTION/PACKAGE.",
                        "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateObjectSelection()) return;

                if (cboTargetUser.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text.ToUpper();
                string objectName = cboObject.Text.ToUpper();
                string targetUser = cboTargetUser.Text.ToUpper();

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn REVOKE EXECUTE trên {owner}.{objectName} từ USER {targetUser}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                string sql = $"REVOKE EXECUTE ON {owner}.{objectName} FROM {targetUser}";
                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Đã REVOKE EXECUTE thành công!\n\n" +
                                $"Object: {owner}.{objectName}\n" +
                                $"User: {targetUser}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi REVOKE EXECUTE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeExecFromRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTriggerSelected())
                {
                    MessageBox.Show("Oracle không cho REVOKE EXECUTE trên TRIGGER.\n" +
                                    "Chỉ áp dụng EXECUTE cho PROCEDURE/FUNCTION/PACKAGE.",
                        "Không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateObjectSelection()) return;

                if (cboTargetRole.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text.ToUpper();
                string objectName = cboObject.Text.ToUpper();
                string targetRole = cboTargetRole.Text.ToUpper();

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn REVOKE EXECUTE trên {owner}.{objectName} từ ROLE {targetRole}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                string sql = $"REVOKE EXECUTE ON {owner}.{objectName} FROM {targetRole}";
                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Đã REVOKE EXECUTE thành công!\n\n" +
                                $"Object: {owner}.{objectName}\n" +
                                $"Role: {targetRole}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi REVOKE EXECUTE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeTableFromRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTableOwner.SelectedIndex < 0 || cboTable.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner và Table!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboRoleForTable.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!cbSelect.Checked && !cbInsert.Checked && !cbUpdate.Checked && !cbDelete.Checked)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 quyền cần REVOKE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboTableOwner.Text.ToUpper();
                string tableName = cboTable.Text.ToUpper();
                string role = cboRoleForTable.Text.ToUpper();

                var privileges = new System.Collections.Generic.List<string>();
                if (cbSelect.Checked) privileges.Add("SELECT");
                if (cbInsert.Checked) privileges.Add("INSERT");
                if (cbUpdate.Checked) privileges.Add("UPDATE");
                if (cbDelete.Checked) privileges.Add("DELETE");

                string privList = string.Join(", ", privileges);

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn REVOKE các quyền sau?\n\n" +
                    $"Table: {owner}.{tableName}\n" +
                    $"Role: {role}\n" +
                    $"Quyền: {privList}",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                foreach (string priv in privileges)
                {
                    string sql = $"REVOKE {priv} ON {owner}.{tableName} FROM {role}";
                    using (var cmd = new OracleCommand(sql, Database.Con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Đã REVOKE thành công!\n\n" +
                                $"Quyền: {privList}\n" +
                                $"Table: {owner}.{tableName}\n" +
                                $"Role: {role}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cbSelect.Checked = false;
                cbInsert.Checked = false;
                cbUpdate.Checked = false;
                cbDelete.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi REVOKE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeTableFromUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTableOwner2.SelectedIndex < 0 || cboTable2.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner và Table!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboUserForTable.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!cbSelect2.Checked && !cbInsert2.Checked && !cbUpdate2.Checked && !cbDelete2.Checked)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 quyền cần REVOKE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboTableOwner2.Text.ToUpper();
                string tableName = cboTable2.Text.ToUpper();
                string user = cboUserForTable.Text.ToUpper();

                var privileges = new System.Collections.Generic.List<string>();
                if (cbSelect2.Checked) privileges.Add("SELECT");
                if (cbInsert2.Checked) privileges.Add("INSERT");
                if (cbUpdate2.Checked) privileges.Add("UPDATE");
                if (cbDelete2.Checked) privileges.Add("DELETE");

                string privList = string.Join(", ", privileges);

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn REVOKE các quyền sau?\n\n" +
                    $"Table: {owner}.{tableName}\n" +
                    $"User: {user}\n" +
                    $"Quyền: {privList}",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                foreach (string priv in privileges)
                {
                    string sql = $"REVOKE {priv} ON {owner}.{tableName} FROM {user}";
                    using (var cmd = new OracleCommand(sql, Database.Con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Đã REVOKE thành công!\n\n" +
                                $"Quyền: {privList}\n" +
                                $"Table: {owner}.{tableName}\n" +
                                $"User: {user}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cbSelect2.Checked = false;
                cbInsert2.Checked = false;
                cbUpdate2.Checked = false;
                cbDelete2.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi REVOKE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeAllFromUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboViewUser.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = cboViewUser.Text.ToUpper();

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn THU HỒI TẤT CẢ quyền trên TABLE/VIEW đã cấp cho USER {username}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.No) return;

                const string query = @"SELECT OWNER, TABLE_NAME, PRIVILEGE 
                                       FROM DBA_TAB_PRIVS 
                                       WHERE GRANTEE = :grantee";

                using (var cmdSelect = new OracleCommand(query, Database.Con))
                {
                    cmdSelect.Parameters.Add("grantee", OracleDbType.Varchar2).Value = username;

                    using (var ad = new OracleDataAdapter(cmdSelect))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            string owner = row["OWNER"].ToString();
                            string table = row["TABLE_NAME"].ToString();
                            string priv = row["PRIVILEGE"].ToString();

                            string revokeSql = $"REVOKE {priv} ON {owner}.{table} FROM {username}";
                            using (var cmdRevoke = new OracleCommand(revokeSql, Database.Con))
                            {
                                cmdRevoke.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show($"Đã thu hồi {dt.Rows.Count} quyền của USER {username}!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                btnRefreshUserPriv_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thu hồi quyền USER: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeAllFromRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboViewRole.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string roleName = cboViewRole.Text.ToUpper();

                DialogResult confirm = MessageBox.Show(
                    $"Bạn có chắc muốn THU HỒI TẤT CẢ quyền trên TABLE/VIEW đã cấp cho ROLE {roleName}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.No) return;

                const string query = @"SELECT OWNER, TABLE_NAME, PRIVILEGE 
                                       FROM DBA_TAB_PRIVS 
                                       WHERE GRANTEE = :grantee";

                using (var cmdSelect = new OracleCommand(query, Database.Con))
                {
                    cmdSelect.Parameters.Add("grantee", OracleDbType.Varchar2).Value = roleName;

                    using (var ad = new OracleDataAdapter(cmdSelect))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            string owner = row["OWNER"].ToString();
                            string table = row["TABLE_NAME"].ToString();
                            string priv = row["PRIVILEGE"].ToString();

                            string revokeSql = $"REVOKE {priv} ON {owner}.{table} FROM {roleName}";
                            using (var cmdRevoke = new OracleCommand(revokeSql, Database.Con))
                            {
                                cmdRevoke.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show($"Đã thu hồi {dt.Rows.Count} quyền của ROLE {roleName}!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                btnRefreshRolePriv_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thu hồi quyền ROLE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab 3: Table privileges for USER

        private void btnLoadTables2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTableOwner2.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner/User!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboTableOwner2.Text.ToUpper();

                const string sql = @"SELECT TABLE_NAME 
                                     FROM DBA_TABLES 
                                     WHERE OWNER = :owner
                                     UNION
                                     SELECT VIEW_NAME AS TABLE_NAME
                                     FROM DBA_VIEWS
                                     WHERE OWNER = :owner
                                     ORDER BY TABLE_NAME";

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;

                    using (var ad = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);

                        cboTable2.DataSource = dt;
                        cboTable2.DisplayMember = "TABLE_NAME";
                        cboTable2.SelectedIndex = -1;
                    }
                }

                MessageBox.Show($"Đã tải danh sách TABLE/VIEW của {owner}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải tables: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrantTableToUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboTableOwner2.SelectedIndex < 0 || cboTable2.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn Owner và Table!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboUserForTable.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!cbSelect2.Checked && !cbInsert2.Checked && !cbUpdate2.Checked && !cbDelete2.Checked)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 quyền!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboTableOwner2.Text.ToUpper();
                string table = cboTable2.Text.ToUpper();
                string user = cboUserForTable.Text.ToUpper();

                var privs = new System.Collections.Generic.List<string>();
                if (cbSelect2.Checked) privs.Add("SELECT");
                if (cbInsert2.Checked) privs.Add("INSERT");
                if (cbUpdate2.Checked) privs.Add("UPDATE");
                if (cbDelete2.Checked) privs.Add("DELETE");

                string privList = string.Join(", ", privs);

                DialogResult dr = MessageBox.Show(
                    $"GRANT các quyền sau?\n\nTable: {owner}.{table}\nUser: {user}\nQuyền: {privList}",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No) return;

                foreach (string p in privs)
                {
                    string sql = $"GRANT {p} ON {owner}.{table} TO {user}";
                    using (var cmd = new OracleCommand(sql, Database.Con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Đã GRANT thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region View privileges

        private void btnRefreshUserPriv_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboViewUser.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string username = cboViewUser.Text.ToUpper();

                const string sql = @"SELECT 
                                        OWNER AS ""Owner"",
                                        TABLE_NAME AS ""Table/Object"",
                                        PRIVILEGE AS ""Quyền"",
                                        GRANTABLE AS ""Có thể Grant""
                                     FROM DBA_TAB_PRIVS
                                     WHERE GRANTEE = :grantee
                                     ORDER BY OWNER, TABLE_NAME, PRIVILEGE";

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.Parameters.Add("grantee", OracleDbType.Varchar2).Value = username;

                    using (var ad = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);
                        dgvUserPrivileges.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem quyền USER: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefreshRolePriv_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboViewRole.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string roleName = cboViewRole.Text.ToUpper();

                const string sql = @"SELECT 
                                        OWNER AS ""Owner"",
                                        TABLE_NAME AS ""Table/Object"",
                                        PRIVILEGE AS ""Quyền"",
                                        GRANTABLE AS ""Có thể Grant""
                                     FROM DBA_TAB_PRIVS
                                     WHERE GRANTEE = :grantee
                                     ORDER BY OWNER, TABLE_NAME, PRIVILEGE";

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    cmd.Parameters.Add("grantee", OracleDbType.Varchar2).Value = roleName;

                    using (var ad = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        ad.Fill(dt);
                        dgvRolePrivileges.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem quyền ROLE: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}


