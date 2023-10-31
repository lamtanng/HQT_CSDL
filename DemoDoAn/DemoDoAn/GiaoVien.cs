using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;

namespace DemoDoAn
{
    public class GiaoVien
    {
        private string gvid;
        private string hoten;
        private string cmnd;
        private string gioitinh;
        private DateTime ngaysinh;
        private string sdt;
        private string diachi;
        private string email;
        private string accid;
        private string username;
        private string password;
        //private Image avarta;

        public string EMAIL { get { return email; } set { email = value; } }
        public string GVID { get { return gvid; } set { gvid = value; } }
        public string ACCID { get { return accid; } set { accid = value; } }
        public string HOTEN { get { return hoten; } set { hoten = value; } }
        public string CMND { get { return cmnd; } set { cmnd = value; } }
        public string GIOITINH { get { return gioitinh; } set { gioitinh = value; } }
        public DateTime NGAYSINH { get { return ngaysinh; } set { ngaysinh = value; } }
        public string SDT { get { return sdt; } set { sdt = value; } }
        public string DIACHI { get { return diachi; } set { diachi = value; } }
        public string USERNAME { get { return username; } set { username = value; } }
        public string PASSWORD { get { return password; } set { password = value; } }
     //   public Image AVARTAR { get { return avarta; } }

        public GiaoVien()
        { }

        public GiaoVien(string gvid, string hoten, string cmnd, DateTime ngaysinh, string gioitinh, string sdt, string diachi,string email, string accid, string username, string password)
        {
            this.gvid = gvid;
            this.hoten = hoten;
            this.cmnd = cmnd;
            this.ngaysinh = ngaysinh;
            this.gioitinh = gioitinh;
            this.sdt = sdt;
            this.diachi = diachi;
            this.email = email;
            this.accid = accid;
            this.username = username;
            this.password = password;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
