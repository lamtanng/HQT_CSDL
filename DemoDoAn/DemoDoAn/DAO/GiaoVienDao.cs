using DemoDoAn.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class GiaoVienDao
    {
        DBConnection dbConn = new DBConnection();

        //lay ACCID
        public DataTable LayAccID(string userName)
        {
            string thuocTinh = "GvID";
            string sqlStr = string.Format("SELECT AccID_Tea FROM ACCOUNTS_TEACHER WHERE username = '{0}'", userName);
            return dbConn.LayDanhSach(sqlStr);
        }

        //gv xac nhan lop day 
        public void xacNhanDay(string malop, int xacnhan)
        {
            string sqlStr = string.Format("Update LOPHOC set XacNhan = {0} Where MaLop = '{1}'",xacnhan, malop);
            dbConn.ThucThi(sqlStr);
        }

        public DataTable LayThongTinGiaoVienVaLop(string gvID)
        {
            string sqlStr = string.Format("SELECT * FROM GIANGVIEN join LOPHOC on  GiangVien.GvID = LOPHOC.GiangVien join KHOAHOC on KHOAHOC.MaKH = LOPHOC.MaKH WHERE  GvID='{0}'", gvID);
            return dbConn.LayDanhSach(sqlStr);
        }
        public DataTable LayDanhSachGiaoVien()
        {
            //để sau này có đổi tên bảng dưới SQL thì chuyển cho nhanh
            // string bangTKGV = "ACCOUNTS_TEACHER";
            //string sqlStr = string.Format("SELECT *FROM GIANGVIEN left join {0} on GIANGVIEN.AccID = {1}.AccID_Tea", bangTKGV, bangTKGV);
            string sqlStr = string.Format("SELECT *FROM GIAOVIEN");
            return dbConn.LayDanhSach(sqlStr);
        }
        public void Them(GiaoVien gv)
        {
            string sqlStr = string.Format("");
            dbConn.ThucThi(sqlStr);
        }

        //them acc
        public void ThemAccout(string username, string password)
        {
            string sqlStr = string.Format("INSERT INTO ACCOUNTS_TEACHER(username, pass, NgayDK) VALUES ('{0}','{1}', '{2}')", username, password, DateTime.Now);
            dbConn.ThucThi(sqlStr);
        }

        //xoa tai khoan
        public void xoaTaiKhoan(string accID)
        {
            string sqlStr = string.Format("delete ACCOUNTS_TEACHER where AccID_Tea = '{0}'", accID);
            dbConn.ThucThi(sqlStr);
        }
        //xoa GV
        public void xoaThongTin(string GvID)
        {
            string sqlStr = string.Format("delete from GIANGVIEN where GvID ='{0}'", GvID);
            dbConn.ThucThi(sqlStr);
        }

        //cap nhat thong tin + cap nhat tai khoan Giang Vien
        public void CapNhat(GiaoVien gv)
        {
            //cap nhat thong tin ca nhan
            string sqlStr = string.Format("UPDATE GIANGVIEN SET HOTEN = N'{0}', GIOITINH = N'{1}', NGAYSINH = '{2}', DIACHI = N'{3}', SDT = '{4}', CMND = '{5}', EMAIL = '{6}' WHERE GvID = '{7}'",
                                        gv.HOTEN, gv.GIOITINH, gv.NGAYSINH, gv.DIACHI, gv.SDT, gv.EMAIL, gv.EMAIL, gv.GVID);
            //cap nhat thong tin acc
            string sqlStr1 = string.Format("Update ACCOUNTS_TEACHER Set pass = '{0}' Where username = '{1}'", gv.DIACHI, gv.USERNAME);
            dbConn.ThucThi(sqlStr);
            dbConn.ThucThi(sqlStr1);
        }

        //them bang luong
        public void ThemBangLuong(GiaoVien gv, int a)
        {
            string sqlStr = string.Format("INSERT INTO BANGLUONG (ID, HOTEN, CHUCVU, LuongDay, PhuCap, TienThuong, TienBaoHiem, THANG) VALUES ('{0}',N'{1}', N'Giáo viên' ,0, 0, 0, 0," + a + ");", gv.GVID, gv.HOTEN);
            dbConn.ThucThi(sqlStr);
        }
        public DataTable TaiThongTinGiaoVien(string username)
        {
            string sqlStr = string.Format("SELECT * FROM GIAOVIEN WHERE TenDangNhap = '{0}'", username);
            return dbConn.LayDanhSach(sqlStr);
        }
    }
}
