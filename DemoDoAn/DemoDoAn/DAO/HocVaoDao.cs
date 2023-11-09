using DemoDoAn.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn.DAO
{
    internal class HocVaoDao
    {
        DBConnection dbConn = new DBConnection();

        //them
        public void themBuoiHoc(HocVao model)
        {
            string sqlStr = string.Format("INSERT INTO HOCVAO(MaNhomHoc, ThuTrongTuan) VALUES ('{0}', '{1}')", model.MaNhomHoc, model.ThuTrongTuan);
            dbConn.ThucThi(sqlStr);
        }
    }
}
