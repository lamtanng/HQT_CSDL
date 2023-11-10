using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.DAO
{
    internal class DanhSachNhomDao
    {
        DBConnection dbConn = new DBConnection();

        //tai danh sach hoc vien trong nhom:
        public DataTable layDSHocVienNhomHoc(string maNhom)
        {
            string sqlStr = string.Format("SELECT * FROM func_layDSHocVienNhomHoc('{0}')", maNhom);
            return dbConn.LayDanhSach(sqlStr);
        }

        //tai danh sach hoc vien không thuộc nhom:
        public DataTable layDSHocVien_KhacNhom(string maNhom)
        {
            string sqlStr = string.Format("Select * From dbo.func_layDSHocVien_KhacNhom('{0}')", maNhom);
            return dbConn.LayDanhSach(sqlStr);
        }

        //them hoc vien vao lop
        public void themHocVienVaoNhom(string maNhom, string hvID)
        {
            string sqlStr = string.Format("INSERT INTO DANHSACHNHOM(MaNhomHoc, MaHocVien) VALUES ('{0}', '{1}')", maNhom, hvID);
            dbConn.ThucThi(sqlStr);
        }

        //xoa hoc vien khoi lop
        public void xoaHocVienKhoiNhom(string maLop, string hvID)
        {
            string sqlStr = string.Format("DELETE DANHSACHNHOM Where MaNhomHoc = '{0}' and MaHocVien = '{1}'", maLop, hvID);
            dbConn.ThucThi(sqlStr);
        }
    }

}
