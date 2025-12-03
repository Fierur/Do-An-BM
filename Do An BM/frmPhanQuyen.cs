using System;
using System.Data;
using System.Linq;
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
            // Kiểm tra quyền Admin
            if (!CheckAdminPermission())
            {
                MessageBox.Show("Chỉ SYS hoặc BM_USER mới có quyền truy cập form này!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            lblUserInfo.Text = $"Đăng nhập với: {Database.User}";

            // Load danh sách users và roles
            LoadAllUsers();
            LoadAllRoles();

            // Setup object types
            cboObjectType.Items.Clear();
            cboObjectType.Items.Add("PROCEDURE");
            cboObjectType.Items.Add("FUNCTION");
            cboObjectType.Items.Add("TRIGGER");
            cboObjectType.SelectedIndex = 0;
        }

        private bool CheckAdminPermission()
        {
            if (Database.Con == null || Database.Con.State != ConnectionState.Open)
            {
                MessageBox.Show("Chưa kết nối đến cơ sở dữ liệu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Chỉ cho phép SYS hoặc BM_USER
            if (!Database.User.Equals("SYS", StringComparison.OrdinalIgnoreCase) &&
                !Database.User.Equals("BM_USER", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        #region Load Users and Roles

        private void LoadAllUsers()
        {
            try
            {
                string sql = "SELECT USERNAME FROM DBA_USERS WHERE ACCOUNT_STATUS = 'OPEN' ORDER BY USERNAME";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Load vào các combobox khác nhau
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
                string sql = "SELECT ROLE FROM DBA_ROLES ORDER BY ROLE";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, Database.Con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

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

                string sql = $@"SELECT OBJECT_NAME 
                               FROM DBA_OBJECTS 
                               WHERE OWNER = :owner 
                               AND OBJECT_TYPE = :objectType
                               ORDER BY OBJECT_NAME";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;
                cmd.Parameters.Add("objectType", OracleDbType.Varchar2).Value = objectType;

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboObject.DataSource = dt;
                cboObject.DisplayMember = "OBJECT_NAME";
                cboObject.SelectedIndex = -1;

                MessageBox.Show($"Đã tải {dt.Rows.Count} {objectType}(s) của {owner}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải objects: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGrantExecToUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateObjectSelection()) return;

                if (cboTargetUser.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn USER!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text;
                string objectName = cboObject.Text;
                string targetUser = cboTargetUser.Text;

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn GRANT EXECUTE trên {owner}.{objectName} cho USER {targetUser}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                string sql = $"GRANT EXECUTE ON {owner}.{objectName} TO {targetUser}";
                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Đã GRANT EXECUTE thành công!\n\n" +
                    $"Object: {owner}.{objectName}\n" +
                    $"User: {targetUser}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (!ValidateObjectSelection()) return;

                if (cboTargetRole.SelectedIndex < 0)
                {
                    MessageBox.Show("Vui lòng chọn ROLE!", "Cảnh báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string owner = cboOwner.Text;
                string objectName = cboObject.Text;
                string targetRole = cboTargetRole.Text;

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn GRANT EXECUTE trên {owner}.{objectName} cho ROLE {targetRole}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                string sql = $"GRANT EXECUTE ON {owner}.{objectName} TO {targetRole}";
                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Đã GRANT EXECUTE thành công!\n\n" +
                    $"Object: {owner}.{objectName}\n" +
                    $"Role: {targetRole}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT EXECUTE: " + ex.Message,
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

        #endregion

        #region Tab 2: Table Privileges for ROLE

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

                string owner = cboTableOwner.Text;

                string sql = @"SELECT TABLE_NAME 
                              FROM DBA_TABLES 
                              WHERE OWNER = :owner
                              UNION
                              SELECT VIEW_NAME AS TABLE_NAME
                              FROM DBA_VIEWS
                              WHERE OWNER = :owner
                              ORDER BY TABLE_NAME";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboTable.DataSource = dt;
                cboTable.DisplayMember = "TABLE_NAME";
                cboTable.SelectedIndex = -1;

                MessageBox.Show($"Đã tải {dt.Rows.Count} table(s)/view(s) của {owner}",
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

                string owner = cboTableOwner.Text;
                string tableName = cboTable.Text;
                string role = cboRoleForTable.Text;

                var privileges = new System.Collections.Generic.List<string>();
                if (cbSelect.Checked) privileges.Add("SELECT");
                if (cbInsert.Checked) privileges.Add("INSERT");
                if (cbUpdate.Checked) privileges.Add("UPDATE");
                if (cbDelete.Checked) privileges.Add("DELETE");

                string privList = string.Join(", ", privileges);

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn GRANT các quyền sau?\n\n" +
                    $"Table: {owner}.{tableName}\n" +
                    $"Role: {role}\n" +
                    $"Quyền: {privList}",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                foreach (string priv in privileges)
                {
                    string sql = $"GRANT {priv} ON {owner}.{tableName} TO {role}";
                    OracleCommand cmd = new OracleCommand(sql, Database.Con);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Đã GRANT thành công!\n\n" +
                    $"Quyền: {privList}\n" +
                    $"Table: {owner}.{tableName}\n" +
                    $"Role: {role}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear checkboxes
                cbSelect.Checked = false;
                cbInsert.Checked = false;
                cbUpdate.Checked = false;
                cbDelete.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab 3: Table Privileges for USER

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

                string owner = cboTableOwner2.Text;

                string sql = @"SELECT TABLE_NAME 
                              FROM DBA_TABLES 
                              WHERE OWNER = :owner
                              UNION
                              SELECT VIEW_NAME AS TABLE_NAME
                              FROM DBA_VIEWS
                              WHERE OWNER = :owner
                              ORDER BY TABLE_NAME";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("owner", OracleDbType.Varchar2).Value = owner;

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cboTable2.DataSource = dt;
                cboTable2.DisplayMember = "TABLE_NAME";
                cboTable2.SelectedIndex = -1;

                MessageBox.Show($"Đã tải {dt.Rows.Count} table(s)/view(s) của {owner}",
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

                string owner = cboTableOwner2.Text;
                string tableName = cboTable2.Text;
                string user = cboUserForTable.Text;

                var privileges = new System.Collections.Generic.List<string>();
                if (cbSelect2.Checked) privileges.Add("SELECT");
                if (cbInsert2.Checked) privileges.Add("INSERT");
                if (cbUpdate2.Checked) privileges.Add("UPDATE");
                if (cbDelete2.Checked) privileges.Add("DELETE");

                string privList = string.Join(", ", privileges);

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc muốn GRANT các quyền sau?\n\n" +
                    $"Table: {owner}.{tableName}\n" +
                    $"User: {user}\n" +
                    $"Quyền: {privList}",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return;

                foreach (string priv in privileges)
                {
                    string sql = $"GRANT {priv} ON {owner}.{tableName} TO {user}";
                    OracleCommand cmd = new OracleCommand(sql, Database.Con);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Đã GRANT thành công!\n\n" +
                    $"Quyền: {privList}\n" +
                    $"Table: {owner}.{tableName}\n" +
                    $"User: {user}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear checkboxes
                cbSelect2.Checked = false;
                cbInsert2.Checked = false;
                cbUpdate2.Checked = false;
                cbDelete2.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi GRANT: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Tab 4: View Privileges

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

                string username = cboViewUser.Text;

                string sql = @"SELECT 
                                OWNER AS ""Owner"",
                                TABLE_NAME AS ""Table/Object"",
                                PRIVILEGE AS ""Quyền"",
                                GRANTABLE AS ""Có thể Grant""
                              FROM DBA_TAB_PRIVS
                              WHERE GRANTEE = :username
                              ORDER BY OWNER, TABLE_NAME, PRIVILEGE";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("username", OracleDbType.Varchar2).Value = username;

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvUserPrivileges.DataSource = dt;
                dgvUserPrivileges.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MessageBox.Show($"Tìm thấy {dt.Rows.Count} quyền của user {username}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem quyền: " + ex.Message,
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

                string roleName = cboViewRole.Text;

                string sql = @"SELECT 
                                OWNER AS ""Owner"",
                                TABLE_NAME AS ""Table/Object"",
                                PRIVILEGE AS ""Quyền"",
                                GRANTABLE AS ""Có thể Grant""
                              FROM DBA_TAB_PRIVS
                              WHERE GRANTEE = :roleName
                              ORDER BY OWNER, TABLE_NAME, PRIVILEGE";

                OracleCommand cmd = new OracleCommand(sql, Database.Con);
                cmd.Parameters.Add("roleName", OracleDbType.Varchar2).Value = roleName;

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvRolePrivileges.DataSource = dt;
                dgvRolePrivileges.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                MessageBox.Show($"Tìm thấy {dt.Rows.Count} quyền của role {roleName}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xem quyền: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}