using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class LopHoc
    {
        private string maLopHoc;
        private string maKhoaHoc;
        private string tenLopHoc;
        private int tongSoBuoiHoc;
        private int hocPhi;

        public LopHoc()
        {
        }

        public LopHoc(string maLopHoc, string maKhoaHoc, string tenLopHoc, int tongSoBuoiHoc, int hocPhi)
        {
            this.MaLopHoc = maLopHoc;
            this.MaKhoaHoc = maKhoaHoc;
            this.TenLopHoc = tenLopHoc;
            this.TongSoBuoiHoc = tongSoBuoiHoc;
            this.HocPhi = hocPhi;
        }

        public string MaLopHoc { get => maLopHoc; set => maLopHoc = value; }
        public string MaKhoaHoc { get => maKhoaHoc; set => maKhoaHoc = value; }
        public string TenLopHoc { get => tenLopHoc; set => tenLopHoc = value; }
        public int TongSoBuoiHoc { get => tongSoBuoiHoc; set => tongSoBuoiHoc = value; }
        public int HocPhi { get => hocPhi; set => hocPhi = value; }
    }
}
