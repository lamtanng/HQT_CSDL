using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class CaHoc
    {
        int ca;
        string gioBatDau;
        string gioKetThuc;

        public CaHoc()
        {
        }

        public CaHoc(int ca, string gioBatDau, string gioKetThuc)
        {
            this.Ca = ca;
            this.GioBatDau = gioBatDau;
            this.GioKetThuc = gioKetThuc;
        }

        public int Ca { get => ca; set => ca = value; }
        public string GioBatDau { get => gioBatDau; set => gioBatDau = value; }
        public string GioKetThuc { get => gioKetThuc; set => gioKetThuc = value; }
    }
}
