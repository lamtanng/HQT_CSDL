using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class PhongHoc
    {
        //cbb_TrangThai cbb_ChonKhoaHoc txt_TenLopMoi txt_HocPhi
        private string phonghoc;
        private string trangthai;
        
        public string PHONGHOC { get { return phonghoc; } }
        public string TRANGTHAI { get { return trangthai; } }
        
        

        public PhongHoc(string phonghoc, string trangthai)
        {
            this.phonghoc = phonghoc;
            this.trangthai = trangthai;
           
        }
    }
}
