using DemoDoAn.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDoAn
{
    internal class NhomHocDao
    {
        DBConnection dbConn = new DBConnection();

        //lay ds lop
        public DataTable LayDanhSachNhom()
        {
            string sqlStr = string.Format("EXEC dbo.proc_LayDanhSachNhom");
            return dbConn.LayDanhSach(sqlStr);
        }

        //xep lop
        public void taoNhomMoi(NhomHoc nhom)
        {
            string sqlStr = string.Format("INSERT INTO NHOMHOC(MaNhomHoc, MaLopHoc, MaGiaoVien, MaPhongHoc, Ca, SoLuongHocVienToiThieu, SoLuongHocVienToiDa, NgayBatDau, NgayKetThuc, TrangThaiMoDangKy) VALUES " +
                                            "('{0}', '{1}', '{2}', '{3}', {4}, {5}, {6}, '{7}', '{8}', 1)",
                                            nhom.MaNhom, nhom.MaLop, nhom.MaGiaoVien, nhom.MaPhongHoc, nhom.Ca, nhom.SoLuongHocVienToiThieu, nhom.SoLuongHocVienToiDa, nhom.NgayBatDau, nhom.NgayKetThuc);
 
            dbConn.ThucThi(sqlStr);
        }

        //lay 1 nhom bang MaNhom:
        public DataTable layNhomMoiNhatTrongLopHoc(string maLop)
        {
            string sqlStr = string.Format("Select TOP 1 * From NHOMHOC WHERE MaLopHoc = '{0}' ORDER BY MaNhomHoc DESC", maLop);
            return dbConn.LayDanhSach(sqlStr);
        }

        //cap nhat thong tin lop hoc
        public void capNhatThongTinLop(NhomHoc lop)
        {
            //string sqlStr = string.Format("update LOPHOC Set SoBuoiTrongTuan = '{0}', GiangVien = '{1}', SoHocVien = '{2}', NgayBatDau ='{3}', NgayKetThuc = '{4}' where MaLop = '{5}'",
            //                                lop.SOBUOITRONGTUAN, lop.GIANGVIEN, lop.SOHOCVIEN, lop.NGAYBATDAU, lop.NGAYKETTHUC, lop.MALOP);
            string sqlStr = "";
             dbConn.ThucThi(sqlStr);
        }

        //lay gio hoc
        public DataTable gioHoc()
        {
            string sqlStr = string.Format("select * from CAHOC");
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay phong hoc
        public DataTable phongHoc()
        {
            string sqlStr = string.Format("Select * From PHONGHOC");
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay lich hoc
        public DataTable lichHocCacMon()
        {
            string sqlStr = string.Format("SELECT LICHHOC.MaLop, Thu, Ca, Phong, GiangVien as GvID FROM LICHHOC inner join LOPHOC on LICHHOC.MaLop = LOPHOC.MaLop");
            return dbConn.LayDanhSach(sqlStr);
        }

        //lay giang vien
        public DataTable giangVien()
        {
            string sqlStr = string.Format("Select * From GIAOVIEN");
            return dbConn.LayDanhSach(sqlStr);
        }

        //cap nhat trang thai đóng mở lớp
        public void capNhatTrangThai(string malop, int trangthai)
        {
            string sqlStr = string.Format("Update LOPHOC set TTMoLop = {0} Where MaLop = '{1}'", trangthai, malop);
            dbConn.ThucThi(sqlStr);
        }

        //xoa
        public void Xoa(string maNhom)
        {
            string sqlStr = string.Format("delete NHOMHOC where MaNhomHoc='{0}'", maNhom);
            dbConn.ThucThi(sqlStr);
        }

        //tim kiem
        public DataTable TimKiem(string duLieu)
        {
            string sqlStr = string.Format("SELECT MaLop, TenMon, SoHocVien, HocPhi, NgayBatDau, NgayKetThuc, TrangThai FROM LOPHOC\n" +
                                        "Where CONCAT(MaLop, TenMon, SoHocVien, HocPhi, NgayBatDau, NgayKetThuc, TrangThai, GiangVien) like N'%{0}%'", duLieu);
            return dbConn.LayDanhSach(sqlStr);        
        }

        //capnhat giang vien day 
        public void capNhatGiangVienChoNhom(string malop, string gvid)
        {
            string sqlStr = "";
            if (string.IsNullOrEmpty(gvid))
            {
                sqlStr = string.Format("Update LOPHOC set GiangVien = null Where MaLop = '{0}'", malop);
            }
            else
            {
                sqlStr = string.Format("Update LOPHOC set GiangVien = '{0}' Where MaLop = '{1}'", gvid, malop);
            }
            dbConn.ThucThi(sqlStr);
        }

    }
}

