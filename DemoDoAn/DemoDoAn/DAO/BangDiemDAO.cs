using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.ChildPage.HocTap
{
    internal class BangDiemDAO
    {
        DBConnection dbConn = new DBConnection();

        //lay bang diem
        public DataTable taiBangDiem(string maLop)
        {
            string sqlStr = string.Format("SELECT BANGDIEM.MaLop, HOCVIEN.HVID, HOCVIEN.HOTEN, DiemGiuaKy, DiemCuoiKy, DiemTB, XepHang\r\nFROM BANGDIEM inner join HOCVIEN on BANGDIEM.HVID = HOCVIEN.HVID\r\nWHERE MaLop = '{0}'", maLop);
            return dbConn.LayDanhSach(sqlStr);
        }
        public DataTable lauBangDiem(string maNhom)
        {
            string sqlStr = string.Format("EXEC dbo.proc_LapBangDiem '{0}'", maNhom);
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay thong tin lop
        public DataTable layThongTinLop(string maLop)
        {
            //lấy cả những lớp chưa có giảng viên
            string sqlStr = string.Format("SELECT KHOAHOC.MaKH, KHOAHOC.TenKH, LOPHOC.MaLop, LOPHOC.TenMon, NgayBatDau, NgayKetThuc, SoHocVien, GIANGVIEN.GvID, GIANGVIEN.HOTEN, HocPhi, TrangThai,TTMoLop, XacNhan " +
                                        "FROM LOPHOC inner join KHOAHOC on LOPHOC.MaKH = KHOAHOC.MaKH " +
                                                    "left join GIANGVIEN on GIANGVIEN.GvID = LOPHOC.GiangVien " +
                                        "WHERE LOPHOC.MaLop = '{0}'", maLop);
            return dbConn.LayDanhSach(sqlStr);
        }

        //cap nhat diem
        public void capNhatDiem(string maNhom, string maHocVien, double diemLyThuyet, double diemThucHanh)
        {
            diemThucHanh = String.IsNullOrEmpty(diemThucHanh.ToString()) ? 0.0 : diemThucHanh;
            diemLyThuyet = String.IsNullOrEmpty(diemLyThuyet.ToString()) ? 0.0 : diemLyThuyet;
            string sqlStr = string.Format("UpDate DANHSACHNHOM Set DiemLyThuyet = {0}, DiemThucHanh = {1} WHERE MaNhomHoc = '{2}' and MaHocVien = '{3}'", diemLyThuyet, diemThucHanh, maNhom, maHocVien);
            dbConn.ThucThi(sqlStr);
        }
    }
}
