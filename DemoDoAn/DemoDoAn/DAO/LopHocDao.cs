using DemoDoAn.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.DAO
{
    internal class LopHocDao
    {
        DBConnection dbConn = new DBConnection();

        //lay all danh sach lop
        public DataTable LayDanhSachLop()
        {
            string sqlStr = string.Format("SELECT * FROM LOPHOC");
            return dbConn.LayDanhSach(sqlStr);
        }
        public void ThemLopHoc(LopHoc lopHoc)
        {
            string sqlStr = string.Format("Exec dbo.proc_ThemLopHoc @MaLopHoc = '{0}', @MaKhoaHoc = '{1}', @TenLopHoc = N'{2}', @TongSoBuoiHoc = {3}, @HocPhi = {4}",
                                            lopHoc.MaLopHoc, lopHoc.MaKhoaHoc, lopHoc.TenLopHoc, lopHoc.TongSoBuoiHoc, lopHoc.HocPhi);
            dbConn.ThucThi(sqlStr);
        }
        public void XoaLopHoc(LopHoc lopHoc)
        {
            string sqlStr = string.Format("Exec dbo.proc_XoaLopHoc @MaLopHoc = '{0}'", lopHoc.MaLopHoc);
            dbConn.ThucThi(sqlStr);
        }

        //lay so buoi hoc:
        public DataTable LaySoBuoiHoc(String maLop)
        {
            string sqlStr = string.Format("Select dbo.uf_TimTongSoBuoiHoc('{0}') TongSoBuoiHoc",maLop );
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay DS LopHoc của Khóa học
        public DataTable LayLopHoc_ThuocKhoaHoc(string maKhoaHoc)
        {
            string sqlStr = string.Format("select * from LOPHOC Where MaKhoaHoc = '{0}'", maKhoaHoc);
            return dbConn.LayDanhSach(sqlStr);
        }
    }
}
