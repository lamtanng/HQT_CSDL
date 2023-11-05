using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class NhomHoc
    {
        private string maNhom;
        private string maLop;
        private string maGiaoVien;
        private string maPhongHoc;
        private int ca;
        private int soLuongHocVienToiThieu;
        private int soLuongHocVienToiDa;
        private DateTime ngayBatDau;
        private DateTime ngayKetThuc;
        private bool trangThaiMoDangKy;

        public string MaNhom { get => maNhom; set => maNhom = value; }
        public string MaLop { get => maLop; set => maLop = value; }
        public string MaGiaoVien { get => maGiaoVien; set => maGiaoVien = value; }
        public string MaPhongHoc { get => maPhongHoc; set => maPhongHoc = value; }
        public int Ca { get => ca; set => ca = value; }
        public int SoLuongHocVienToiThieu { get => soLuongHocVienToiThieu; set => soLuongHocVienToiThieu = value; }
        public int SoLuongHocVienToiDa { get => soLuongHocVienToiDa; set => soLuongHocVienToiDa = value; }
        public DateTime NgayBatDau { get => ngayBatDau; set => ngayBatDau = value; }
        public DateTime NgayKetThuc { get => ngayKetThuc; set => ngayKetThuc = value; }
        public bool TrangThaiMoDangKy { get => trangThaiMoDangKy; set => trangThaiMoDangKy = value; }

        //private string khoahoc;
        //private string SoBuoiTrongTuan;
        //private string hocphi;
        //private int soLuongHocVien;
        //private string tengon;
        //private string noidung;
        //private string trangthai;
        //private string thu;
        //private string gioBatDau;
        //private string gioKetThuc;





        //CÁC HÀM CONSTRUCTION:
        //1.
        public NhomHoc() { }

        public NhomHoc(string maNhom, string maLop, string maGiaoVien, string maPhongHoc, int ca, int soLuongHocVienToiThieu, int soLuongHocVienToiDa, DateTime ngayBatDau, DateTime ngayKetThuc, bool trangThaiMoDangKy) : this(maNhom, maLop, maGiaoVien)
        {
            this.MaPhongHoc = maPhongHoc;
            this.Ca = ca;
            this.SoLuongHocVienToiThieu = soLuongHocVienToiThieu;
            this.SoLuongHocVienToiDa = soLuongHocVienToiDa;
            this.NgayBatDau = ngayBatDau;
            this.NgayKetThuc = ngayKetThuc;
            this.TrangThaiMoDangKy = trangThaiMoDangKy;
        }

        public NhomHoc(string maNhom, string tenlop, string trangthai, string khoahoc, int SoHocVien, DateTime ngayBatDau, DateTime ngayKetThuc, string hocphi, string giangvien, string SoBuoiTrongTuan, string tengon)
        {
            //this.maNhom = maNhom;
            //this.maLop = tenlop;
            //this.hocphi = hocphi;
            //this.khoahoc = khoahoc;
            //soHocVien = SoHocVien;
            //this.SoBuoiTrongTuan = SoBuoiTrongTuan;
            //this.giangvien = giangvien;
            //this.trangthai = trangthai;
            //this.tengon = tengon;
            //this.ngayBatDau = ngayBatDau;
            //this.ngayKetThuc = ngayKetThuc;
        }
        //2.Load thong tin lop hoc de DKI
        //public LopHoc(string thu, string ca, string gioBatDau, string gioKetThuc)
        //{
        //    this.thu = thu;
        //    this.ca = ca;
        //    this.gioBatDau = gioBatDau;
        //    this.gioKetThuc = gioKetThuc;
        //}
        //3. Load thong tin lop hoc HV đã đăng kí
        public NhomHoc(string maNhom, string tenlop, DateTime ngayBatDau, DateTime ngayKetThuc, string giangvien, string trangthai)
        {
            //this.maNhom = maNhom;
            //this.maLop = tenlop;
            //this.ngayBatDau = ngayBatDau;
            //this.ngayKetThuc = ngayKetThuc;
            //this.giangvien = giangvien;
            //this.trangthai = trangthai;
        }
        //4.tao lop moi
        public NhomHoc(string khoahoc, string tenlop, string hocphi)
        {
            //this.hocphi = hocphi;
            //this.khoahoc = khoahoc;
            //this.maLop = tenlop;
        }
    }
}
