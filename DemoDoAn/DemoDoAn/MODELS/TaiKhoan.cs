using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.MODELS
{
    internal class TaiKhoan
    {
        private string tenDangNhap;
        private string matKhau;
        private string quyenNguoiDung;


        public TaiKhoan()
        {
        }

        public TaiKhoan(string tenDangNhap, string matKhau, string quyenNguoiDung)
        {
            this.TenDangNhap = tenDangNhap;
            this.MatKhau = matKhau;
            this.QuyenNguoiDung = quyenNguoiDung;
        }

        public string TenDangNhap { get => tenDangNhap; set => tenDangNhap = value; }
        public string MatKhau { get => matKhau; set => matKhau = value; }
        public string QuyenNguoiDung { get => quyenNguoiDung; set => quyenNguoiDung = value; }
    }
}
