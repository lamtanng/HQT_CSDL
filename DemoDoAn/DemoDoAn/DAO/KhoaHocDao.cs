using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class KhoaHocDao
    {
        DBConnection dbConn = new DBConnection();
        public DataTable LayKhoaHoc()
        {
            string sqlStr = string.Format("SELECT *FROM KHOAHOC");
            return dbConn.LayDanhSach(sqlStr);
        }
        public void Them(KhoaHoc kh)
        {
            string sqlStr = string.Format("INSERT INTO KHOAHOC (MaKhoaHoc, TenKhoaHoc) VALUES ('{0}', '{1}');", kh.Makhoahoc, kh.Tenkhoahoc);
            dbConn.ThucThi(sqlStr);
        }
        public void Xoa(KhoaHoc kh)
        {
            string sqlStr = string.Format("DELETE FROM KHOAHOC WHERE MaKhoaHoc = '{0}'", kh.Makhoahoc);
            dbConn.ThucThi(sqlStr);
        }
        public void CapNhat(KhoaHoc kh)
        {
            string sqlStr = string.Format("");
            dbConn.ThucThi(sqlStr);
        }
        public DataTable timKiem(string duLieu)
        {
            string sqlStr = string.Format("SELECT * FROM KHOAHOC WHERE CONCAT(MaKH, TenKH, TrangThai) like N'%{0}%'", duLieu);
            return dbConn.LayDanhSach(sqlStr);
        }
    }
}
