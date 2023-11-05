using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class DanhSachNhom
    {
        string maNhomHoc;
        string maHocVien;
        int diemLyThuyet;
        int diemThucHanh;
        bool trangThaiThanhToan;
        bool trangThaiCapChungChi;

        public DanhSachNhom()
        {
        }

        public DanhSachNhom(string maNhomHoc, string maHocVien, int diemLyThuyet, int diemThucHanh, bool trangThaiThanhToan, bool trangThaiCapChungChi)
        {
            this.MaNhomHoc = maNhomHoc;
            this.MaHocVien = maHocVien;
            this.DiemLyThuyet = diemLyThuyet;
            this.DiemThucHanh = diemThucHanh;
            this.TrangThaiThanhToan = trangThaiThanhToan;
            this.TrangThaiCapChungChi = trangThaiCapChungChi;
        }

        public string MaNhomHoc { get => maNhomHoc; set => maNhomHoc = value; }
        public string MaHocVien { get => maHocVien; set => maHocVien = value; }
        public int DiemLyThuyet { get => diemLyThuyet; set => diemLyThuyet = value; }
        public int DiemThucHanh { get => diemThucHanh; set => diemThucHanh = value; }
        public bool TrangThaiThanhToan { get => trangThaiThanhToan; set => trangThaiThanhToan = value; }
        public bool TrangThaiCapChungChi { get => trangThaiCapChungChi; set => trangThaiCapChungChi = value; }
    }
}
