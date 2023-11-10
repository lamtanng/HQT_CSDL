using DemoDoAn.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDoAn.HOCVIEN
{
    public partial class F_DIEMDANH : Form
    {
        DanhSachNhomDao dsnDao = new DanhSachNhomDao();
        BangDiemDanhDao bddDao = new BangDiemDanhDao();
        string maNhom;

        enum nameCol_DSL_GV
        {
            STT,
            MaHocVien,
            TenHocVien,
            DiemDanh
        }

        public F_DIEMDANH()
        {
            InitializeComponent();
        }

        public F_DIEMDANH(string maNhom)
        {
            InitializeComponent();
            this.maNhom = maNhom;
            layDSHV();
        }

        //lay danh sach hv trong nhom
        private void layDSHV()
        {
            
            DataTable dtDSHV = new DataTable();
            dtDSHV.Rows.Clear();
            dtDSHV = dsnDao.layDSHocVienNhomHoc(maNhom);
            dataGrView_DSLop.DataSource = dtDSHV;
            //ẩn full các cột     
            for (int i = 0; i < dataGrView_DSLop.Columns.Count; i++)
            {
                DataGridViewColumn colDtg = dataGrView_DSLop.Columns[i];
                colDtg.Visible = false;
            }

            //hiện những cột cần, name của các cột được lưu trong Enum
            foreach (nameCol_DSL_GV day in Enum.GetValues(typeof(nameCol_DSL_GV)))
            {
                for (int i = 0; i < dataGrView_DSLop.Columns.Count; i++)
                {
                    DataGridViewColumn colDtg = dataGrView_DSLop.Columns[i];
                    if (colDtg.Name.ToString() == day.ToString())
                    {
                        colDtg.Visible = true;
                        break;
                    }
                }
            }
            dataGrView_DSLop.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        private void btn_BangDiem_Click(object sender, EventArgs e)
        {

        }

        private void dataGrView_DSLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pBox_Luu_Click(object sender, EventArgs e)
        {
            
            //xoa ngay hoc + diem danh
            bddDao.xoaDiemDanh(maNhom);
            //them ngay hoc
            bddDao.taoNgayHoc();
            //them bang diem danh
            diemDanhHocVien();

        }

        private void diemDanhHocVien()
        {    
            for (int i = dataGrView_DSLop.Rows.Count - 1; i >=0; i--)
            {
                DataGridViewRow row = dataGrView_DSLop.Rows[i];
                string maHV = row.Cells["MaHocVien"].Value.ToString().Trim();
                bool hienDien = Convert.ToBoolean(row.Cells["DiemDanh"].EditedFormattedValue);
                bddDao.diemDanhHocVien(maNhom, maHV, hienDien);

            }
            //foreach (DataGridViewRow row in dataGrView_DSLop.Rows)
            //{
            //    string maHV = row.Cells["MaHocVien"].Value.ToString().Trim();
            //    bool hienDien = Convert.ToBoolean(row.Cells["DiemDanh"].Value);
            //    bddDao.diemDanhHocVien(maNhom, maHV, hienDien);
            //}
        }
    }
}
