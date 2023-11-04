using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class NgayHoc
    {
        DateTime ngay;

        public NgayHoc()
        {
        }

        public NgayHoc(DateTime ngay)
        {
            this.Ngay = ngay;
        }

        public DateTime Ngay { get => ngay; set => ngay = value; }
    }
}
