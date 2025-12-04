using System;

namespace Do_An_BM
{
    /// <summary>
    /// Quản lý session của user đang đăng nhập
    /// </summary>
    public static class SessionManager
    {
        public static int CurrentUserID { get; set; }
        public static string CurrentUserName { get; set; }
        public static string Role { get; set; }
        public static bool IsLoggedIn { get; set; }
        public static string Email { get; set; }

        /// <summary>
        /// Đăng nhập và set context
        /// </summary>
        public static void Login(int userId, string userName, string role, string email = "")
        {
            CurrentUserID = userId;
            CurrentUserName = userName;
            Role = role.ToUpper();
            Email = email;
            IsLoggedIn = true;

            // Set VPD context
            if (Role == "CUSTOMER")
            {
                OracleHelper.SetUserContext(maKH: userId, maNV: null);
            }
            else if (Role == "ADMIN" || Role == "STAFF")
            {
                OracleHelper.SetUserContext(maKH: null, maNV: userId);
            }
        }

        /// <summary>
        /// Đăng xuất và clear context
        /// </summary>
        public static void Logout()
        {
            OracleHelper.LogoutUser();

            CurrentUserID = 0;
            CurrentUserName = string.Empty;
            Role = string.Empty;
            Email = string.Empty;
            IsLoggedIn = false;
        }

        /// <summary>
        /// Kiểm tra quyền Admin
        /// </summary>
        public static bool IsAdmin()
        {
            return Role == "ADMIN";
        }

        /// <summary>
        /// Kiểm tra quyền Staff
        /// </summary>
        public static bool IsStaff()
        {
            return Role == "STAFF";
        }

        /// <summary>
        /// Kiểm tra quyền Customer
        /// </summary>
        public static bool IsCustomer()
        {
            return Role == "CUSTOMER";
        }

        /// <summary>
        /// Lấy thông tin session dạng string
        /// </summary>
        public static string GetInfo()
        {
            if (!IsLoggedIn)
                return "Chưa đăng nhập";

            return $"{CurrentUserName} ({Role}) - ID: {CurrentUserID}";
        }
    }
}