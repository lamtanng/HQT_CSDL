using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DemoDoAn
{
    public class NhanVien
    {
        
        private string nvid;
        private string hoten;
        private string gioitinh;
        private string cmnd;
        private DateTime ngaysinh;
        private string sdt;
        private string diachi;
        private string email;
        private string accid;
        private string username;
        private string password;
        // private Image avarta;

        public string EMAIL { get { return email; } set { email = value; } }
        public string NVID { get { return nvid; } set { nvid = value; } }
        public string ACCID { get { return accid; } set { accid = value; } }
        public string HOTEN { get { return hoten; } set { hoten = value; } }
        public string CMND { get { return cmnd; } set { cmnd = value; } }
        public string GIOITINH { get { return gioitinh; } set { gioitinh = value; } }
        public DateTime NGAYSINH { get { return ngaysinh; } set { ngaysinh = value; } }
        public string SDT { get { return sdt; } set { sdt = value; } }
        public string DIACHI { get { return diachi; } set { diachi = value; } }
        public string USERNAME { get { return username; } set { username = value; } }
        public string PASSWORD { get { return password; } set { password = value; } }

        public NhanVien()
        { }
        //lbl_MaNV.Text, lbl_TenNV.Text, lblGioiTinh.Text, Convert.ToDateTime(lbl_NgaySinh.Text), lbl_DiaChi.Text, lblCCCD.Text, btn_SDT.Text, btn_Email.Text
        public NhanVien(string nvid,string hoten,string gioitinh,DateTime ngaysinh,string diachi,string cmnd,string sdt,string email, string accid, string username, string password)
        {
            this.nvid = nvid;
            this.hoten = hoten;
            this.gioitinh= gioitinh;
            this.ngaysinh = ngaysinh;
            this.diachi = diachi;
            this.cmnd = cmnd;
            this.sdt = sdt;
            this.email = email;
            this.accid = accid;
            this.username = username;
            this.password = password;
        }
    }
}
