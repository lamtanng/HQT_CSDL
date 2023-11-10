using DemoDoAn.DAO;
using DemoDoAn.HOCVIEN.Class;
using DemoDoAn.MODELS;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace DemoDoAn.HOCVIEN
{
    public partial class F_HAMTHU : Form
    {
        LoginDAO logDao = new LoginDAO();
        HocSinhDao hsDao = new HocSinhDao();
        HamThuDAO thuDao = new HamThuDAO();
        GiaoVienDao giaoVienDao= new GiaoVienDao();
        LopHocDao lopHocDao= new LopHocDao();
        NhomHocDao nhomHocDao = new NhomHocDao();

        DataTable dt_GiaoVien = new DataTable();
        DataTable dt_NhomHoc = new DataTable();

        //DataTable dtFULL_INFO = new DataTable();
        string IDGui ;
        string IDNhan;

        public F_HAMTHU()
        {
            InitializeComponent();
            //dtFULL_INFO = logDao.loadFull();
            dt_GiaoVien = giaoVienDao.TaiThongTinGiaoVien(Login.userName.ToString().Trim());
            layIDNguoiGui();
        }

        #region XuLiDoHoc
        bool isEmpty_Search = true;
        bool isEmpty_Title = true;
        bool isEmpty_Content = true;
        private void an_SearchText(TextBox txt_Search, ref bool isEmpty_Search)
        {
            if (isEmpty_Search == true)
            {
                txt_Search.Text = String.Empty;
                //txt_Search.Font = new Font(txt_Search.Font, FontStyle.Regular);
                //txt_Search.ForeColor = Color.DimGray;
                isEmpty_Search = false;
            }
        }
        private void hien_SearchText(TextBox txt_Search, ref bool isEmpty_Search)
        {
            txt_Search.Visible =true;
            txt_Search.Font = new Font("SFU Futura", 10F, FontStyle.Italic);
            txt_Search.ForeColor = Color.Silver;
            isEmpty_Search = true;
        }
        private void txt_IDNhan_Click(object sender, EventArgs e)
        {
            //an_SearchText(txt_IDNhan, ref isEmpty_Search);
        }
        private void lbl_ContentTxt_Click(object sender, EventArgs e)
        {
            an_SearchText(txt_NoiDung, ref isEmpty_Content);
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            an_SearchText(txt_TieuDe, ref isEmpty_Title);
        }
        private void picBox_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        //lay accID nguoi gui
        private void layIDNguoiGui()
        {
           if(dt_GiaoVien.Rows.Count > 0)
            {
                    IDGui = dt_GiaoVien.Rows[0]["MaGiaoVien"].ToString().Trim();
            }
            
        }

        private void F_HAMTHU_Load(object sender, EventArgs e)
        {
            //string AccID = hsDao.Lay_MSSV(Login.userName);
            //dt_HV = hsDao.LoadThongTin(accID);
            //lbl_HoTen.Text = dtFULL_INFO.Rows[0]["HOTEN"].ToString();
            if (dt_GiaoVien.Rows.Count > 0)
            {
                    lbl_HoTen.Text = dt_GiaoVien.Rows[0]["HoTen"].ToString().Trim();
            }
            loadCbbNhomHoc();
        }

        //gui thu
        private void pBox_Gui_Click(object sender, EventArgs e)
        {
            //string accID = hsDao.LayAccID(Login.userName);
            //if(checkNguoiNhan(IDNhan))
            {
                ThongBao thongBao = new ThongBao(null, IDGui, txt_TieuDe.Text.ToString(), txt_NoiDung.Text.ToString());
                thuDao.GuiThongBao(thongBao);
                DataTable dt_thongBao = thuDao.LayMaThongBaoMoiNhat();
                if (dt_thongBao.Rows.Count > 0)
                {
                    thongBao.MaThongBao = dt_thongBao.Rows[0]["MaThongBao"].ToString().Trim();
                }
                TruyenTin truyenTin = new TruyenTin(thongBao.MaThongBao, ((DataRowView)gCbb_NhomHoc.SelectedItem)["MaNhomHoc"].ToString());
                thuDao.TruyenTin(truyenTin);
                this.Close();
            }
            //else
            //{
            //    MessageBox.Show("Không thể xác nhận người nhận!");
            //}
            
        }

        private void loadCbbNhomHoc()
        {
            dt_NhomHoc.Rows.Clear();
            dt_NhomHoc = nhomHocDao.LayDanhSachNhom();
            //duyet lui chứ mỗi lần xóa bị lỗi
            int rows = dt_NhomHoc.Rows.Count;
            for (int r = rows - 1; r >= 0; r--)
            {
                DataRow row = dt_NhomHoc.Rows[r];
                if (Convert.ToString(row["MaGiaoVien"]) != IDGui)
                    dt_NhomHoc.Rows.Remove(row);
            }
            loadCombobox(gCbb_NhomHoc, dt_NhomHoc, "MaNhomHoc", "MaNhomHoc");
        }

        //load combobox
        private void loadCombobox(Guna2ComboBox cbb, DataTable dt, string displayMember, string valueMember)
        {
            cbb.DataSource = dt;
            cbb.DisplayMember = displayMember;
            cbb.ValueMember = valueMember;
        }

        //load thong tin ng nhan thu
        private void txt_IDNhan_TextChanged(object sender, EventArgs e)
        {
            //IDNhan = txt_IDNhan.Text.ToString();
            //for (int i = 0; i < dtFULL_INFO.Rows.Count; i++)
            //{
            //    DataRow row = dtFULL_INFO.Rows[i];
            //    if (row["ID"].ToString().Trim() == IDNhan)
            //    {
            //        lbl_HoTenNhan.Text = "(" + row["HOTEN"].ToString() + ")";
            //        lbl_ChucVu.Text = row["ChucVu"].ToString().Trim();
            //        return;
            //    }
            //    else
            //    {
            //        lbl_HoTenNhan.Text = "Recipient";
            //        lbl_ChucVu.Text = "Duty";
            //    }
            //}
        }

        //check 
        private bool checkNguoiNhan(string ID)
        {
            //for(int i = 0; i < dtFULL_INFO.Rows.Count; i++)
            //{
            //    DataRow row  = dtFULL_INFO.Rows[i];
            //    if (row["ID"].ToString().Trim() == ID)
            //    {
            //        return true;
            //    }    
            //}
            return false;
        }

        
    }
}
