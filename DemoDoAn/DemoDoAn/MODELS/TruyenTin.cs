using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class TruyenTin
    {
        private string maThongBao;
        private string maNhomHoc;

        public TruyenTin()
        {
        }

        public TruyenTin(string maThongBao, string maNhomHoc)
        {
            this.MaThongBao = maThongBao;
            this.MaNhomHoc = maNhomHoc;
        }

        public string MaThongBao { get => maThongBao; set => maThongBao = value; }
        public string MaNhomHoc { get => maNhomHoc; set => maNhomHoc = value; }
    }
}
