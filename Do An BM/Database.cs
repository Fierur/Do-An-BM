using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace Do_An_BM
{
    public class Database
    {
        public static OracleConnection Con { get; set; }
        public static string Host { get; set; }
        public static string Port { get; set; }
        public static string Sid { get; set; }
        public static string User { get; set; }
        public static string Pass { get; set; }

        public static void Set_Database(string host, string port, string sid, string user, string pass)
        {
            Host = host;
            Port = port;
            Sid = sid;
            User = user;
            Pass = pass;
        }

        public static void Set_Database(string user, string pass)
        {
            User = user;
            Pass = pass;
        }

        public static bool Connect()
        {
            try
            {
                string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SERVICE_NAME={Sid})));User Id={User};Password={Pass};";
                if (User.Trim().Equals("SYS", StringComparison.OrdinalIgnoreCase))
                {
                    connectionString += "DBA Privilege=SYSDBA;";
                }

                if (Con != null && Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                    Con.Dispose();
                }

                Con = new OracleConnection(connectionString);
                Con.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu: " + ex.Message);
                return false;
            }
        }


        public static bool ConncetSYS()
        {
            string sysUser = User;
            string sysPass = Pass;

            if (!sysUser.Trim().Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chức năng tạo người dùng chỉ cho phép kết nối bằng tài khoản SYS!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))(CONNECT_DATA=(SERVICE_NAME={Sid})));User Id={sysUser};Password={sysPass};DBA Privilege=SYSDBA;";

                if (Con != null && Con.State != ConnectionState.Closed)
                {
                    Con.Close();
                    Con.Dispose();
                }

                Con = new OracleConnection(connectionString);
                Con.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối SYS. Vui lòng kiểm tra mật khẩu SYS và thông số kết nối.\n" + ex.Message);
                return false;
            }
        }

        public static OracleConnection Get_Connect()
        {
            return Con;
        }

        public static void Close_Connect()
        {
            if (Con != null && Con.State != ConnectionState.Closed)
            {
                Con.Close();
                Con.Dispose();
            }
        }

        public Database() { }
        public Database(string host, string port, string sid, string user, string pass)
        {
            Host = host; Port = port; Sid = sid; User = user; Pass = pass;
        }
        public bool Ketnoi()
        {
            string conString = "Data Source=" + Host + ":" + Port + "/" + Sid + ";" +
                               "User Id=" + User + ";Password=" + Pass + ";";

            if (User.Trim().Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                conString += "DBA Privilege=SYSDBA;";
            }

            Con = new OracleConnection(conString);
            try
            {
                if (Con.State == ConnectionState.Closed)
                    Con.Open();
                MessageBox.Show("Kết nối thành công!", "Thông báo");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại! " + ex.Message, "Lỗi");
                return false;
            }
        }
    }
}
