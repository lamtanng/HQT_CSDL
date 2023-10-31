using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class KhoaHoc
    {
        private string makhoahoc;
        private string tenkhoahoc;
        private string trangthai;

        public string Makhoahoc { get { return makhoahoc; } set { makhoahoc = value; } }
        public string Tenkhoahoc { get { return tenkhoahoc; } set { tenkhoahoc = value; } }
        public string Trangthai { get { return trangthai; } set { trangthai = value; } }
        public KhoaHoc() { }
        public KhoaHoc(string makhoahoc, string tenkhoahoc, string trangthai)
        {
            this.makhoahoc = makhoahoc;
            this.tenkhoahoc = tenkhoahoc;
            this.trangthai = trangthai;
        }
    }
}
