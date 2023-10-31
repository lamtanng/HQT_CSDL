using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class ThongKeDao
    {
        DBConnection dbConn = new DBConnection();

        //lay bang ghi danh
        public DataTable layBangGhiDanh()
        {
            string sqlStr = string.Format("Select * From ACCOUNTS_STUDENT join HOCVIEN on ACCOUNTS_STUDENT.AccID_Stu = HOCVIEN.ACCID");
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay bang hoc phi
        public DataTable layBangHocPhi()
        {
            string sqlStr = string.Format("Select * From BANGHOCPHI join HOCVIEN on BANGHOCPHI.HVID = HOCVIEN.HVID");
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay bang hoc tap
        public DataTable layBangHocTap()
        {
            string sqlStr = string.Format("Select XepHang, BANGDIEM.HVID, HOTEN, BANGDIEM.MaLop, TenMon, DiemGiuaKy, DiemCuoiKy, DiemTB, BANGDIEM.TrangThai\r\nFrom BANGDIEM join HOCVIEN on BANGDIEM.HVID = HOCVIEN.HVID\r\n\t\t\tjoin LOPHOC on BANGDIEM.MaLop = LOPHOC.MaLop");
            return dbConn.LayDanhSach(sqlStr);
        }

    }
}
