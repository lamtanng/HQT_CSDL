using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DemoDoAn
{
    public class HocSinh
    {
        private string hsid;
        private string accid;
        private string hoten;
        private string cmnd;
        private string gioitinh;
        private DateTime ngaysinh;
        private string sdt;
        private string email;
        private string diachi;
        private int tien;
        private string username;
        private string password;
        //private Image avarta;

        public int TIEN { get { return tien; } set { tien = value; } }
        public string HSID { get { return hsid; } set { hsid = value; } }
        public string ACCID { get { return accid; } set { accid = value; } }
        public string HOTEN { get { return hoten; } set { hoten = value; } }
        public string CMND { get { return cmnd; } set { cmnd = value; } }
        public string GIOITINH { get { return gioitinh; } set { gioitinh = value; } }
        public DateTime NGAYSINH { get { return ngaysinh; } set { ngaysinh = value; } }
        public string SDT { get { return sdt; } set { sdt = value; } }
        public string EMAIL { get { return email; } set { email = value; } }
        public string DIACHI { get { return diachi; } set { diachi = value; } }
        public string USERNAME { get { return username; } set { username = value; } }
        public string PASSWORD { get { return password; } set { password = value; } }
        // public Image AVARTAR { get { return avarta; } }

        public HocSinh()
        { }
        public HocSinh(string hsid, string hoten, string cmnd, string gioitinh, DateTime ngaysinh, string sdt, string email, string diachi, int tien)
        {
            this.hsid = hsid;
            this.hoten = hoten;
            this.cmnd = cmnd;
            this.gioitinh = gioitinh;
            this.ngaysinh = ngaysinh;
            this.sdt = sdt;
            this.email = email;
            this.diachi = diachi;
            this.tien = tien;
           // this.avarta = avarta;
        }
        public HocSinh(string accid, string username, string password)
        {
            this.accid = accid;
            this.username = username;
            this.password = password;
        }
    }
}
