using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class DanhSachLopDao
    {
        DBConnection dbConn = new DBConnection();

        //them hoc vien vao lop
        public void themHocVienVaoLop(string maLop, string hvID)
        {
            string sqlStr = string.Format("INSERT INTO DANHSACHLOP (MaLop, HVID, TrangThai) VALUES('{0}', '{1}', N'{2}') ", maLop, hvID, "Chưa hoàn thành");
            dbConn.ThucThi(sqlStr);
        }

        //xoa hoc vien khoi lop
        public void xoaHocVien(string maLop, string hvID)
        {
            string sqlStr = string.Format("DELETE DANHSACHLOP Where MaLop = '{0}' and HVID = '{1}'", maLop, hvID);
            dbConn.ThucThi(sqlStr);
        }
    }

}
