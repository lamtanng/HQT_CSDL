using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class NgayHoc
    {
        DateTime ngayHoc;

        public NgayHoc()
        {
        }

        public NgayHoc(DateTime ngayHoc)
        {
            this.NgayHoc = ngayHoc;
        }

        public DateTime NgayHoc { get => ngayHoc; set => ngayHoc = value; }
    }
}
