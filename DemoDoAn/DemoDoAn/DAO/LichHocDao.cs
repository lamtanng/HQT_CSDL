using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class LichHocDao
    {
        DBConnection dbConn = new DBConnection();

        //
        public DataTable tinhNgayKetThuc(DateTime ngayBatDau, int soBuoiHoc
            )
        {
    
            string sqlStr = string.Format("SELECT dbo.uf_TinhNgayKetThuc('{0}',{1}) AS NgayKetThuc;", ngayBatDau, soBuoiHoc);
            return dbConn.LayDanhSach(sqlStr);
        }

        public DataTable taiGiaoVienTrongLich(string thu1, string thu2, string thu3, int ca)
        {
            string sqlStr = string.Format("exec dbo.proc_GV_LichTrong @thu1 = '{0}', @thu2 = '{1}', @thu3 = '{2}', @ca = {3}",
                                            thu1, thu2, thu3, ca);
            return dbConn.LayDanhSach(sqlStr);
        }

        public DataTable taiPhongTrongLich(string thu1, string thu2, string thu3, int ca)
        {
            string sqlStr = string.Format("exec dbo.proc_PH_LichTrong @thu1 = '{0}', @thu2 = '{1}', @thu3 = '{2}', @ca = {3}",
                                            thu1, thu2, thu3, ca);
            return dbConn.LayDanhSach(sqlStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //tai lich hoc
        public DataTable taiLichHoc()
        {
            string sqlStr = string.Format("Select * From LOPHOC left join LICHHOC on LICHHOC.MaLop = LOPHOC.MaLop " +
                                        "join KHOAHOC on KHOAHOC.MaKH = LOPHOC.MaKH Where TTMoLop = 1");
            return dbConn.LayDanhSach(sqlStr);
        }

        //xoa lich hoc
        public void xoaLichHoc(string maLop)
        {
            string sqlStr = string.Format("Delete LICHHOC Where MaLop = '{0}'", maLop);
            dbConn.ThucThi(sqlStr);
        }

        //xep lich hoc
        public void xepLichHoc(string maLop, string thu, string ca, string phong)
        {
            string sqlStr = string.Format("INSERT INTO LICHHOC(MaLop, Thu, Ca, Phong) VALUES ('{0}', '{1}', '{2}', '{3}')", maLop, thu, ca, phong);
            dbConn.ThucThi(sqlStr);
        }
    }
}
