using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoDoAn
{
    internal class BangLuong
    {
        private string id;
        public string hoten;
        private double luong;
        private double phucap;
        private double tienthuong;
        private double tienbaohiem;
        private double thangluong;

        public double THANGLUONG
        {
            get { return thangluong; }
            set
            { thangluong = value; }
        }
        public string ID
        {
            get { return id; }
            set
            { id = value; }
        }
        public string HOTEN
        {
            get { return hoten; }
            set
            { hoten = value; }
        }

        public double LUONG { get { return luong; } set { luong = value; } }

        public double PHUCAP
        {
            get { return phucap; }
            set { phucap = value; }
        }
        public double TIENTHUONG
        {
            get { return tienthuong; }
            set { tienthuong = value; }
        }
        public double TIENBAOHIEM { get { return tienbaohiem; } set { tienbaohiem = value; } }


        public BangLuong()
        { }

        public BangLuong(string id, string hoten, double luong, double phucap, double tienthuong, double tienbaohiem, double thangluong)
        {
            this.thangluong = thangluong;
            this.id = id;
            this.hoten = hoten;
            this.luong = luong;
            this.phucap = phucap;
            this.tienthuong = tienthuong;
            this.tienbaohiem = tienbaohiem;


        }
    }
}
