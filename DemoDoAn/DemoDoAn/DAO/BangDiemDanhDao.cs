using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.DAO
{
    internal class BangDiemDanhDao
    {
        DBConnection dbConn = new DBConnection();

        //xoa diem danh, xoa ngay hoc
        public void xoaDiemDanh(string maNhom)
        {
            string sqlStr = String.Format("exec dbo.proc_XoaDiemDanh '{0}';", maNhom);
            dbConn.ThucThi(sqlStr);
        }

        //tao ngay hoc
        public void taoNgayHoc()
        {
            string sqlStr = String.Format("exec dbo.proc_ThemNgayHoc;");
            dbConn.ThucThi(sqlStr);
        }

        public void diemDanhHocVien(string maNhom, string maHV, bool hienDien)
        {
            string sqlStr = String.Format("exec dbo.proc_DiemDanhHocVien '{0}', '{1}', {2}", maNhom, maHV, hienDien);
            dbConn.ThucThi(sqlStr);
        }
    }
}
