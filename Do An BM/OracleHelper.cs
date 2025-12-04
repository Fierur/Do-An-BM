using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Windows.Forms;

namespace Do_An_BM
{
    /// <summary>
    /// Helper class để gọi các function và procedure của Oracle
    /// </summary>
    public static class OracleHelper
    {
        #region Execute Function (Return value)

        /// <summary>
        /// Gọi Function trả về VARCHAR2
        /// </summary>
        public static string ExecuteFunction(string functionName, params OracleParameter[] parameters)
        {
            try
            {
                if (Database.Con == null || Database.Con.State != ConnectionState.Open)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = Database.Con;
                    cmd.CommandText = functionName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Return parameter
                    var returnParam = new OracleParameter("RETURN_VALUE", OracleDbType.Varchar2, 4000);
                    returnParam.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnParam);

                    // Input parameters
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();

                    if (returnParam.Value != null && returnParam.Value != DBNull.Value)
                    {
                        return returnParam.Value.ToString();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gọi function {functionName}: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Gọi Function trả về RAW (byte array)
        /// </summary>
        public static byte[] ExecuteFunctionRAW(string functionName, params OracleParameter[] parameters)
        {
            try
            {
                if (Database.Con == null || Database.Con.State != ConnectionState.Open)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = Database.Con;
                    cmd.CommandText = functionName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Return parameter (RAW)
                    var returnParam = new OracleParameter("RETURN_VALUE", OracleDbType.Raw, 2000);
                    returnParam.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnParam);

                    // Input parameters
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();

                    if (returnParam.Value != null && returnParam.Value != DBNull.Value)
                    {
                        if (returnParam.Value is OracleBinary)
                        {
                            return ((OracleBinary)returnParam.Value).Value;
                        }
                        else if (returnParam.Value is byte[])
                        {
                            return (byte[])returnParam.Value;
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gọi function {functionName}: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        #endregion

        #region Execute Procedure

        /// <summary>
        /// Gọi Procedure không có output
        /// </summary>
        public static bool ExecuteProcedure(string procedureName, params OracleParameter[] parameters)
        {
            try
            {
                if (Database.Con == null || Database.Con.State != ConnectionState.Open)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                using (var cmd = new OracleCommand())
                {
                    cmd.Connection = Database.Con;
                    cmd.CommandText = procedureName;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gọi procedure {procedureName}: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Execute Query

        /// <summary>
        /// Thực thi câu SELECT, trả về DataTable
        /// </summary>
        public static DataTable ExecuteQuery(string sql, params OracleParameter[] parameters)
        {
            try
            {
                if (Database.Con == null || Database.Con.State != ConnectionState.Open)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    var adapter = new OracleDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thực thi query: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Thực thi INSERT/UPDATE/DELETE
        /// </summary>
        public static int ExecuteNonQuery(string sql, params OracleParameter[] parameters)
        {
            try
            {
                if (Database.Con == null || Database.Con.State != ConnectionState.Open)
                {
                    MessageBox.Show("Chưa kết nối database!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }

                using (var cmd = new OracleCommand(sql, Database.Con))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thực thi command: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        #endregion

        #region Security Functions - Mapping với SQL

        /// <summary>
        /// Hash mật khẩu bằng SHA-512
        /// </summary>
        public static byte[] HashPassword(string password)
        {
            var param = new OracleParameter("p_pass", OracleDbType.Varchar2, password, ParameterDirection.Input);
            return ExecuteFunctionRAW("BM_USER.HASH_PASS", param);
        }

        /// <summary>
        /// Mã hóa AES-256
        /// </summary>
        public static byte[] EncryptAES(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return null;

            var param = new OracleParameter("p_text", OracleDbType.Varchar2, plaintext, ParameterDirection.Input);
            return ExecuteFunctionRAW("BM_USER.ENCRYPT_AES", param);
        }

        /// <summary>
        /// Giải mã AES-256
        /// </summary>
        public static string DecryptAES(byte[] encrypted)
        {
            if (encrypted == null || encrypted.Length == 0) return null;

            var param = new OracleParameter("p_raw", OracleDbType.Raw, encrypted, ParameterDirection.Input);
            return ExecuteFunction("BM_USER.DECRYPT_AES", param);
        }

        /// <summary>
        /// Mã hóa RSA-2048
        /// </summary>
        public static byte[] EncryptRSA(string plaintext)
        {
            if (string.IsNullOrEmpty(plaintext)) return null;

            var param = new OracleParameter("p_text", OracleDbType.Varchar2, plaintext, ParameterDirection.Input);
            return ExecuteFunctionRAW("BM_USER.ENCRYPT_RSA", param);
        }

        /// <summary>
        /// Giải mã RSA-2048
        /// </summary>
        public static string DecryptRSA(byte[] encrypted)
        {
            if (encrypted == null || encrypted.Length == 0) return null;

            var param = new OracleParameter("p_raw", OracleDbType.Raw, encrypted, ParameterDirection.Input);
            return ExecuteFunction("BM_USER.DECRYPT_RSA", param);
        }

        /// <summary>
        /// Đăng nhập Nhân viên
        /// </summary>
        public static string LoginNV(int maNV, string password)
        {
            var param1 = new OracleParameter("p_manv", OracleDbType.Int32, maNV, ParameterDirection.Input);
            var param2 = new OracleParameter("p_password", OracleDbType.Varchar2, password, ParameterDirection.Input);

            return ExecuteFunction("BM_USER.LOGIN_NV", param1, param2);
        }

        /// <summary>
        /// Đăng nhập Khách hàng
        /// </summary>
        public static string LoginKH(string email, string password)
        {
            var param1 = new OracleParameter("p_email", OracleDbType.Varchar2, email, ParameterDirection.Input);
            var param2 = new OracleParameter("p_password", OracleDbType.Varchar2, password, ParameterDirection.Input);

            return ExecuteFunction("BM_USER.LOGIN_KH", param1, param2);
        }

        /// <summary>
        /// Lấy vai trò nhân viên
        /// </summary>
        public static string GetRole(int maNV)
        {
            var param = new OracleParameter("p_manv", OracleDbType.Int32, maNV, ParameterDirection.Input);
            return ExecuteFunction("BM_USER.GET_ROLE", param);
        }

        /// <summary>
        /// Set context cho VPD
        /// </summary>
        public static bool SetUserContext(int? maKH = null, int? maNV = null)
        {
            var param1 = new OracleParameter("p_makh", OracleDbType.Int32);
            param1.Value = maKH.HasValue ? (object)maKH.Value : DBNull.Value;
            param1.Direction = ParameterDirection.Input;

            var param2 = new OracleParameter("p_manv", OracleDbType.Int32);
            param2.Value = maNV.HasValue ? (object)maNV.Value : DBNull.Value;
            param2.Direction = ParameterDirection.Input;

            return ExecuteProcedure("BM_USER.SET_USER_CONTEXT", param1, param2);
        }

        /// <summary>
        /// Logout - Clear context
        /// </summary>
        public static bool LogoutUser()
        {
            return ExecuteProcedure("BM_USER.LOGOUT_USER");
        }

        /// <summary>
        /// Admin cấp quyền
        /// </summary>
        public static bool AdminGrantRole(int adminMaNV, int staffMaNV, int maVT)
        {
            var param1 = new OracleParameter("p_admin_manv", OracleDbType.Int32, adminMaNV, ParameterDirection.Input);
            var param2 = new OracleParameter("p_staff_manv", OracleDbType.Int32, staffMaNV, ParameterDirection.Input);
            var param3 = new OracleParameter("p_mavt", OracleDbType.Int32, maVT, ParameterDirection.Input);

            return ExecuteProcedure("BM_USER.ADMIN_GRANT_ROLE", param1, param2, param3);
        }

        /// <summary>
        /// Generate Log Signature
        /// </summary>
        public static byte[] GenerateLogSignature(int logId)
        {
            var param = new OracleParameter("p_log_id", OracleDbType.Int32, logId, ParameterDirection.Input);
            return ExecuteFunctionRAW("BM_USER.GENERATE_LOG_SIGNATURE", param);
        }

        /// <summary>
        /// Restore Deleted Order (Flashback)
        /// </summary>
        public static bool RestoreDeletedOrder(int adminMaNV, int maDon, int minutesAgo)
        {
            var param1 = new OracleParameter("p_admin_manv", OracleDbType.Int32, adminMaNV, ParameterDirection.Input);
            var param2 = new OracleParameter("p_madon", OracleDbType.Int32, maDon, ParameterDirection.Input);
            var param3 = new OracleParameter("p_minutes_ago", OracleDbType.Int32, minutesAgo, ParameterDirection.Input);

            return ExecuteProcedure("BM_USER.RESTORE_DELETED_ORDER", param1, param2, param3);
        }

        /// <summary>
        /// Generate RSA Keys
        /// </summary>
        public static bool GenerateRSAKeys()
        {
            return ExecuteProcedure("BM_USER.GENERATE_RSA_KEYS");
        }

        #endregion
    }
}