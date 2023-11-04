using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class HocVao
    {
        private string maNhomHoc;
        private string thuTrongTuan;

        public HocVao()
        {
        }

        public HocVao(string maNhomHoc, string thuTrongTuan)
        {
            this.MaNhomHoc = maNhomHoc;
            this.ThuTrongTuan = thuTrongTuan;
        }

        public string MaNhomHoc { get => maNhomHoc; set => maNhomHoc = value; }
        public string ThuTrongTuan { get => thuTrongTuan; set => thuTrongTuan = value; }
    }
}
