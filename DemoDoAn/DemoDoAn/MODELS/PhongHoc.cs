using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class PhongHoc
    {
        private string maPhongHoc;
        private int soLuongChoNgoi;

        public PhongHoc()
        {
        }

        public PhongHoc(string maPhongHoc, int soLuongChoNgoi)
        {
            this.maPhongHoc = maPhongHoc;
            this.soLuongChoNgoi = soLuongChoNgoi;
        }

        public string MaPhongHoc { get => maPhongHoc; set => maPhongHoc = value; }
        public int SoLuongChoNgoi { get => soLuongChoNgoi; set => soLuongChoNgoi = value; }
    }
}
