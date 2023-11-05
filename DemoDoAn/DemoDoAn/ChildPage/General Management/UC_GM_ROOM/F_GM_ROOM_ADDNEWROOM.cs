using DemoDoAn.DAO;
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
using System.Windows.Forms;

namespace DemoDoAn.ChildPage.General_Management.UC_GM_ROOM
{
    public partial class F_GM_ROOM_ADDNEWROOM : Form
    {
        LopHocDao lopHocDao = new LopHocDao();
        KhoaHocDao khoaHocDao = new KhoaHocDao();
        DataTable dtKhoaHoc = new DataTable("KhoaHoc");
        //PhongHocDao phongHocDao=new PhongHocDao();
        public F_GM_ROOM_ADDNEWROOM()
        {
            InitializeComponent();
        }

        private void btn_Huy_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region XuLiDoHoa
        bool isEmpty_Search = true;
        #region hàm dùng chung 
        private void SelectBtn(FlowLayoutPanel fLPnl, Button btnFLPnl, Button btn, bool select)
        {
            fLPnl.Height = fLPnl.MinimumSize.Height;
            select_TrangThai = false;
            btnFLPnl.Text = btn.Text;
        }
        // show/hide list lựa chọn 
        bool select_TrangThai = false;
        private void ShowListChoss(FlowLayoutPanel fLPnl, Timer t, ref bool select)
        {
            if (select == false)
            {
                fLPnl.Height += 15;
                if (fLPnl.Height >= fLPnl.MaximumSize.Height)
                {
                    t.Stop();
                    select = true;
                }
            }
            else
            {
                fLPnl.Height -= 15;
                if (fLPnl.Height <= fLPnl.MinimumSize.Height)
                {
                    t.Stop();
                    select = false;
                }
            }
        }

        #endregion

        private void btn_TrangThai_Click(object sender, EventArgs e)
        {
            timer_LoadTrangThai.Start();
        }
        private void timer_LoadTrangThai_Tick(object sender, EventArgs e)
        {
            ShowListChoss(fLPnl_TrangThai, timer_LoadTrangThai, ref select_TrangThai);
        }
            
        private void btn_TT_DaDay_Click(object sender, EventArgs e)
        {
            SelectBtn(fLPnl_TrangThai, btn_TrangThai, btn_TT_DaDay, select_TrangThai);
        }

        private void btn_TT_HoatDong_Click(object sender, EventArgs e)
        {
            SelectBtn(fLPnl_TrangThai, btn_TrangThai, btn_TT_HoatDong, select_TrangThai);
        }
        #endregion
        /
        //load khoa hoc len cbb
        private void loadCbbKhoaHoc()
        {
            dtKhoaHoc.Rows.Clear();
            dtKhoaHoc = khoaHocDao.LayKhoaHoc();
            //duyet lui chứ mỗi lần xóa bị lỗi
            //int rows = dtKhoaHoc.Rows.Count;
            //for (int r = rows - 1; r >= 0; r--)
            //{
            //    DataRow row = dtKhoaHoc.Rows[r];
            //    if (Convert.ToInt32(row["TrangThaiKH"]) == 0)
            //        dtKhoaHoc.Rows.Remove(row);
            //}
            loadCombobox(gCbb_KhoaHoc, dtKhoaHoc, "TenKhoaHoc", "MaKhoaHoc");
        }

        //load combobox
        private void loadCombobox(Guna2ComboBox cbb, DataTable dt, string displayMember, string valueMember)
        {
            cbb.DataSource = dt;
            cbb.DisplayMember = displayMember;
            cbb.ValueMember = valueMember;
        }

        private void btn_HoanThanh_Click(object sender, EventArgs e)
        {
            LopHoc lopHoc = new LopHoc(txt_MaLopHoc.Text.ToString(), ((DataRowView)gCbb_KhoaHoc.SelectedItem)["MaKhoaHoc"].ToString(), txt_TenLopHoc.Text.ToString(), Convert.ToInt32(txt_TongSoBuoiHoc.Text.ToString()), Convert.ToInt32(txt_HocPhi.Text.ToString()));
            lopHocDao.ThemLopHoc(lopHoc);
        }

        private void F_GM_ROOM_ADDNEWROOM_Load(object sender, EventArgs e)
        {
            loadCbbKhoaHoc();
        }
    }
}
