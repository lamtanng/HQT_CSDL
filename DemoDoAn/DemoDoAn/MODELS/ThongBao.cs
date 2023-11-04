using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class ThongBao
    {
        private string maThongBao;
        private string maGiaoVien;
        private string tieuDe;
        private string noiDung;


        public ThongBao()
        {
        }

        public ThongBao(string maThongBao, string maGiaoVien, string tieuDe, string noiDung)
        {
            this.MaThongBao = maThongBao;
            this.MaGiaoVien = maGiaoVien;
            this.TieuDe = tieuDe;
            this.NoiDung = noiDung;
        }

        public string MaThongBao { get => maThongBao; set => maThongBao = value; }
        public string MaGiaoVien { get => maGiaoVien; set => maGiaoVien = value; }
        public string TieuDe { get => tieuDe; set => tieuDe = value; }
        public string NoiDung { get => noiDung; set => noiDung = value; }
    }
}
