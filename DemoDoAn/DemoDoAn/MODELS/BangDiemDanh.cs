using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class BangDiemDanh
    {
        DateTime ngayHoc; 
        string maNhomHoc;
        string maHocVien;
        bool hienDien;

        public BangDiemDanh()
        {
        }

        public BangDiemDanh(DateTime ngayHoc, string maNhomHoc, string maHocVien, bool hienDien)
        {
            this.NgayHoc = ngayHoc;
            this.MaNhomHoc = maNhomHoc;
            this.MaHocVien = maHocVien;
            this.HienDien = hienDien;
        }

        public DateTime NgayHoc { get => ngayHoc; set => ngayHoc = value; }
        public string MaNhomHoc { get => maNhomHoc; set => maNhomHoc = value; }
        public string MaHocVien { get => maHocVien; set => maHocVien = value; }
        public bool HienDien { get => hienDien; set => hienDien = value; }
    }
}
