using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class LopHoc
    {
        //txtmalop.Text, cbb_PhongHoc.Text,txt_SoLuongHV.Text, txt_SoBuoiTrongTuan.Text, cbb_TrangThai.Text, cbb_KhoaHoc.Text, cbb_LopHoc.Text, cbb_GiangVien.Text, txt_HocPhi.Text, txt_SoLuongHV.Text, datePTime_NgayBatDau.Text, datePTime_NgayKetThuc.Text, richTxt_NoiDung.Text);

        private string malop;
        private string tenlop;
        private string tengon;
        private string phong;
        private string khoahoc;
        private string giangvien;
        private string SoBuoiTrongTuan;
        private string hocphi;
        //private string noidung;
        private string trangthai;
        private int soHocVien;
        private DateTime ngayBatDau;
        private DateTime ngayKetThuc;
        private string thu;
        private string ca;
        private string gioBatDau;
        private string gioKetThuc;



        public string MALOP { get { return malop; } set { malop = value; } }
        public string GIANGVIEN { get { return giangvien; } set { giangvien = value; } }
        public string SOBUOITRONGTUAN { get { return SoBuoiTrongTuan; } set { SoBuoiTrongTuan = value; } }
        public string PHONG { get { return phong; } set { phong = value; } }
        public string KHOAHOC { get { return khoahoc; } set { khoahoc = value; } }
        //public string GIAOVIEn { get { return giaovien; } }
        public string HOCPHI { get { return hocphi; } set { hocphi = value; } }
        //public string NOIDUNG { get { return noidung; } }
        public int SOHOCVIEN { get { return soHocVien; } set { soHocVien = value; } }
        public string TRANGTHAI { get { return trangthai; } set { trangthai = value; } }
        public DateTime NGAYBATDAU { get { return ngayBatDau; } set { ngayBatDau = value; } }
        public DateTime NGAYKETTHUC { get { return ngayKetThuc; } set { ngayKetThuc = value; } }
        public string TENLOP { get { return tenlop; } set { tenlop = value; } }
        public string THU { get { return thu; } set { thu = value; } }
        public string CA { get { return ca; } set { ca = value; } }
        public string GIOBATDAU { get { return gioBatDau; } set { gioBatDau = value; } }
        public string GIOKETTHUC { get { return gioKetThuc; } set { gioKetThuc = value; } }
        public string TENGON { get { return tengon; } set { tengon = value; } }

        //CÁC HÀM CONSTRUCTION:
        //1.
        public LopHoc() { }
        public LopHoc(string malop, string tenlop, string trangthai, string khoahoc, int SoHocVien, DateTime ngayBatDau, DateTime ngayKetThuc, string hocphi, string giangvien, string SoBuoiTrongTuan, string tengon)
        {
            this.giangvien = giangvien;
            this.trangthai = trangthai;
            this.malop = malop;
            this.tenlop = tenlop;
            this.tengon = tengon;
            this.hocphi = hocphi;
            this.khoahoc = khoahoc;
            this.soHocVien = SoHocVien;
            this.ngayBatDau = ngayBatDau;
            this.ngayKetThuc = ngayKetThuc;
            this.SoBuoiTrongTuan = SoBuoiTrongTuan;
        }
        //2.Load thong tin lop hoc de DKI
        public LopHoc(string thu, string ca, string gioBatDau, string gioKetThuc)
        {
            this.thu = thu;
            this.ca = ca;
            this.gioBatDau = gioBatDau;
            this.gioKetThuc = gioKetThuc;
        }
        //3. Load thong tin lop hoc HV đã đăng kí
        public LopHoc(string malop, string tenlop, DateTime ngayBatDau, DateTime ngayKetThuc, string giangvien, string trangthai)
        {
            this.malop = malop;
            this.tenlop = tenlop;
            this.ngayBatDau = ngayBatDau;
            this.ngayKetThuc = ngayKetThuc;
            this.giangvien = giangvien;
            this.trangthai = trangthai;
        }
        //4.tao lop moi
        public LopHoc(string khoahoc, string tenlop, string hocphi)
        {
            this.hocphi = hocphi;
            this.khoahoc = khoahoc;
            this.tenlop = tenlop;
        }
    }
}
