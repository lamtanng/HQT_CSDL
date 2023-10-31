using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class AdminDao
    {
        DBConnection dbConn = new DBConnection();
        public DataTable LayDanhSachTaiKhoan()
        {
            string sqlStr = string.Format("SELECT *FROM ACCOUT ");
            return dbConn.LayDanhSach(sqlStr);
        }
       

    }
}
