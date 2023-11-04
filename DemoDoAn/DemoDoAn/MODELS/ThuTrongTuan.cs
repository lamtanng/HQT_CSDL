using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class ThuTrongTuan
    {
        private string thu;

        public ThuTrongTuan()
        {
        }

        public ThuTrongTuan(string thu)
        {
            this.Thu = thu;
        }

        public string Thu { get => thu; set => thu = value; }
    }
}
