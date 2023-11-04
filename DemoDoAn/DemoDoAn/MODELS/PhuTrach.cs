using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class PhuTrach
    {
        private string maGiaoVien;
        private string maKhoaHoc;

        public PhuTrach()
        {
        }

        public PhuTrach(string maGiaoVien, string maKhoaHoc)
        {
            this.MaGiaoVien = maGiaoVien;
            this.MaKhoaHoc = maKhoaHoc;
        }

        public string MaGiaoVien { get => maGiaoVien; set => maGiaoVien = value; }
        public string MaKhoaHoc { get => maKhoaHoc; set => maKhoaHoc = value; }
    }
}
