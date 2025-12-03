using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_An_BM
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            CenterToScreen();
            txtPass.UseSystemPasswordChar = !cb_pass.Checked;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string host = txtHost.Text;
            string port = txtPort.Text;
            string sid = txtSid.Text;
            string user = txtUser.Text;
            string pass = txtPass.Text;

            if (Check_Textbox(host, port, sid, user, pass))
            {
                Database.Set_Database(host, port, sid, user, pass);
                if (Database.Connect())
                {
                    MessageBox.Show("Đăng nhập thành công");
                    //new .Show();
                    //this.Hide();
                }
            }
            else
            {
                Check_Status(user);
                return;
            }
        }

        bool Check_Textbox(string host, string port, string sid, string user, string pass)
        {
            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin đăng nhập (Host, Port, SID, User, Password).");
                return false;
            }
            return true;
        }
        void Check_Status(string user)
        {
            // Hàm này được gọi khi Check_Textbox() trả về false (theo code trong hình), 
            // hoặc có thể được sử dụng để hiển thị thông báo lỗi đăng nhập.

            // Nếu bạn muốn xử lý lỗi kết nối thất bại:
            // Thường sẽ kiểm tra trạng thái kết nối Database.conn.State. 
            // Nếu không, chỉ hiển thị thông báo chung.

            MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin kết nối và tài khoản.");
        }
       

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtHost.Text = "26.71.28.188";
            txtPort.Text = "1521";
            txtSid.Text = "orcl";
            txtUser.Text = "sys";
            txtPass.Text = "sys";
        }

        private void btnPhanQuyen_Click(object sender, EventArgs e)
        {
            //string host = txtHost.Text;
            //string port = txtPort.Text;
            //string sid = txtSid.Text;
            //string user = txtUser.Text;
            //string pass = txtPass.Text;
            //// Gán đầy đủ giá trị
            //Database.Set_Database(host, port, sid, user, pass);

            //// ConncetSYS() sẽ sử dụng User và Pass vừa gán
            //if (Database.ConncetSYS())
            //{
            //    //new .Show();
            //}

        }

        private void cb_pass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !cb_pass.Checked;
        }
    }
}
