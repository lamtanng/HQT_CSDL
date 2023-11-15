//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDoAn
{
    internal class LoginDAO
    {
        DBConnection dbConn = new DBConnection();

        //tai bang thong tin full
        public DataTable loadFull()
        {
            string sqlStr = string.Format("SELECT * FROM FULL_THONGTIN");
            return dbConn.LayDanhSach(sqlStr);
        }

        //tai bang acc
        public DataTable loadAccount()
        {
            string sqlStr = string.Format("Select * from TAIKHOAN");
            return dbConn.LayDanhSach(sqlStr);
        }

        //check acc
        public DataTable Login(string username, string password)
        {
            //string sqlStr = string.Format("Select * From dbo.func_kiemTraDangNhap('{0}', '{1}')", username, password);
            string sqlStr = string.Format("select R.Name as Role\r\n\tfrom sys.database_principals P \r\n\t\tleft outer join sys.database_role_members RM on P.principal_id=RM.member_principal_id \r\n\t\tleft outer join sys.database_principals R on R.principal_id=RM.role_principal_id\r\n\tWHERE P.name = '{0}'", username);
            return dbConn.LayDanhSach(sqlStr);
        }

        //check remember pass
        public DataTable updateRememberPass(string username, string accTable, int remember)
        {
            string sqlStr = string.Format("Update {0} Set Remember = {1} Where username = '{2}'", accTable, remember,username);
            return dbConn.LayDanhSach(sqlStr);
        }


    }
}
