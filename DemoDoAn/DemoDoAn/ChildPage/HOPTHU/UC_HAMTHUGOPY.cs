using DemoDoAn.ChildPage.HAMTHU;
using DemoDoAn.HOCVIEN.Class;
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
    public partial class UC_HAMTHUGOPY : UserControl
    {
        HocSinhDao hsDao = new HocSinhDao();
        HamThuDAO thuDao = new HamThuDAO();
        LoginDAO logDao = new LoginDAO(); 
        GiaoVienDao gvDao = new GiaoVienDao(); 

        DataTable dtDSThu = new DataTable("DSThu");
        DataTable dtFULL_INFO = new DataTable();
        DataTable dt_DSThongBao = new DataTable();
        DataTable dt_HocVien = new DataTable();
        DataTable dt_GiaoVien = new DataTable();

        string mathu;
        bool danhdau;

        string maThongBao;
        string hoTenNguoiGui;
        string maNhomHoc;
        string tieuDe;
        string noiDung;
        DateTime ngayGui;
        TimeSpan gioGui;

        int chucVu; // 0 la admin;  1 la hocvien;  2 la giao vien

        #region DOHOA
        public void checkDanhDau(bool danhdau)
        {
            if (danhdau == true)
            {
                pBox_DanhDau.Image = new Bitmap(Application.StartupPath + "\\Resources\\star_Gold.png");
                pBox_DanhDau.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pBox_DanhDau.Image = new Bitmap(Application.StartupPath + "\\Resources\\star_White.png");
                pBox_DanhDau.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        private void pBox_DanhDau_Click(object sender, EventArgs e)
        {
            if (danhdau == false)
            {
                pBox_DanhDau.Image = new Bitmap(Application.StartupPath + "\\Resources\\star_Gold.png");
                pBox_DanhDau.SizeMode = PictureBoxSizeMode.Zoom;
                danhdau = true;
                thuDao.DanhDau(danhdau, mathu);
            }
            else
            {
                pBox_DanhDau.Image = new Bitmap(Application.StartupPath + "\\Resources\\star_White.png");
                pBox_DanhDau.SizeMode = PictureBoxSizeMode.Zoom;
                danhdau = false;
                thuDao.DanhDau(danhdau, mathu);
            }
           // LoadThuDen();
        }
        #endregion
        public UC_HAMTHUGOPY(int chucVu)
        {
            InitializeComponent();
            this.chucVu = chucVu;
        }

        private void UC_HAMTHUGOPY_Load(object sender, EventArgs e)
        {
            //taiFULL_INFO();
            gBtn_Gui_Visiblitiy(chucVu);
            LoadThuDen(chucVu);
        }

        //tai thong tin nguoi gui
        private void taiFULL_INFO()
        {
            dtFULL_INFO = logDao.loadFull();
        }

        //load danh sách thư đến
        private void LoadThuDen(int chucVu)
        {
            fLPnl_ThuDen.Controls.Clear();
            string ID = "";
            //lay ID nguoi nhan
            //string hvID = "";
            //for (int r= 0; r < dtFULL_INFO.Rows.Count; r++)
            //{
            //    DataRow row = dtFULL_INFO.Rows[r];
            //    if (row["USERNAME"].ToString().Trim() == Login.userName.ToString())
            //    {
            //        hvID = row["ID"].ToString();
            //        break;
            //    }
            //}
            if (chucVu == 1)
            {
                if (dt_HocVien.Rows.Count > 0)
                {
                    ID = dt_HocVien.Rows[0]["Ma"].ToString().Trim();
                }
                dt_DSThongBao = thuDao.LayThongBaoMoiNhat(ID);
            }
            if (chucVu == 2)
            {
                if (dt_GiaoVien.Rows.Count > 0)
                {
                    ID = dt_GiaoVien.Rows[0]["MaGiaoVien"].ToString().Trim();
                }
                dt_DSThongBao = thuDao.LayThongBaoDuocGui(ID);
            }
            if (dt_DSThongBao.Rows.Count > 0)
            {
                Load_UC_HAMTHUGOPY_CHILD(dt_DSThongBao);
            }    
            //string username = Login.userName.ToString();
            //string hoTen = dtFULL_INFO.AsEnumerable().Where(r => r.Field<string>("USERNAME") == username).Select(r => r.Field<string>("ID")).FirstOrDefault().ToString();
            //tai danh sach thu den
            //dtDSThu = thuDao.LayThuDen(hvID);
            //Load_UC_HAMTHUGOPY_CHILD(dtDSThu);
            //UC_HAMTHUGOPY_CHILD[] ucThuChild = new UC_HAMTHUGOPY_CHILD[dtDSThu.Rows.Count];

            //for (int row = 0; row < dtDSThu.Rows.Count; row++)
            //{
                
            //    //Hoten
            //    string hoten = "";
            //    for (int r = 0; r < dtFULL_INFO.Rows.Count; r++)
            //    {
            //        DataRow dtrow = dtFULL_INFO.Rows[r];
            //        if (dtDSThu.Rows[row]["IDGui"].ToString().Trim() == dtrow["ID"].ToString().Trim())
            //        {
            //            hoten = dtrow["HOTEN"].ToString().Trim();
            //            break;
            //        }
            //    }

            //    //Ngay
            //    DateTime Ngay = (DateTime)dtDSThu.Rows[row]["Ngay"];

            //    //Gio
            //    TimeSpan Gio = (TimeSpan)dtDSThu.Rows[row]["Gio"];
            //    DateTime gio = Ngay.Add(Gio);
            //    ucThuChild[row] = new UC_HAMTHUGOPY_CHILD(dtDSThu.Rows[row]["MaThu"].ToString(), hoten, 
            //                                              dtDSThu.Rows[row]["TieuDe"].ToString(), dtDSThu.Rows[row]["Noidung"].ToString(), 
            //                                              Ngay, gio, Convert.ToBoolean(dtDSThu.Rows[row]["DanhDau"]));
            //    fLPnl_ThuDen.Controls.Add(ucThuChild[row]);
            //}

            //for(int r = 0; r < dtDSThu.Rows.Count; r++)
            //{
            //    ucThuChild[r].ReadClicked += UC_HAMTHUGOPY_ReadClicked;
            //    ucThuChild[r].DeleteClicked += UC_HAMTHUGOPY_DeleteClicked;
            //}
        }

        //xoa thu
        private void UC_HAMTHUGOPY_DeleteClicked(object sender, EventArgs e)
        {
            UC_HAMTHUGOPY_CHILD ucThu = sender as UC_HAMTHUGOPY_CHILD;
            string mathu = ucThu.MATHU.ToString();
            thuDao.xoaThu(mathu);
            //LoadThuDen();
        }

        //hien chi tiet thu
        private void UC_HAMTHUGOPY_ReadClicked(object sender, EventArgs e)
        {
            UC_HAMTHUGOPY_CHILD ucThu = sender as UC_HAMTHUGOPY_CHILD;

            mathu = ucThu.MATHU.ToString();
            //danhdau = ucThu.DANHDAU;
            lbl_HoTen.Text = ucThu.TEN.ToString();
            lbl_TieuDe_CT.Text = ucThu.TIEUDE.ToString();
            lbl_NoiDung_CT.Text = ucThu.NOIDUNG.ToString();
            lbl_NgayThang.Text = ucThu.NGAY.ToString("dd/MM/yyyy");
            lbl_Gio.Text = ucThu.GIO.ToString("hh':'mm':'ss");
            //checkDanhDau(danhdau);
        }

        private void pBox_Gui_Click(object sender, EventArgs e)
        {
            F_HAMTHU hopThu = new F_HAMTHU();
            hopThu.ShowDialog();
            UC_HAMTHUGOPY_Load(sender, e);
        }

        private void gBtn_Gui_Visiblitiy (int chucVu)
        {
            if (chucVu == 1)
            {
                dt_HocVien = hsDao.Lay_MSSV(Login.userName.ToString().Trim());
                gBtn_Gui.Visible = false;
                if(dt_HocVien.Rows.Count > 0)
                {
                    dt_DSThongBao = thuDao.LayThongBaoMoiNhat(dt_HocVien.Rows[0]["Ma"].ToString().Trim());
                }
                if (dt_DSThongBao.Rows.Count > 0)
                {
                    hoTenNguoiGui = dt_DSThongBao.Rows[0]["HoTen"].ToString().Trim();
                    maNhomHoc = dt_DSThongBao.Rows[0]["MaNhomHoc"].ToString().Trim();
                    tieuDe = dt_DSThongBao.Rows[0]["TieuDe"].ToString().Trim();
                    noiDung = dt_DSThongBao.Rows[0]["NoiDung"].ToString().Trim();
                    ngayGui = (DateTime)dt_DSThongBao.Rows[0]["NgayGui"];
                    gioGui = (TimeSpan)dt_DSThongBao.Rows[0]["GioGui"];
                    LoadChiTietThongBao(hoTenNguoiGui, maNhomHoc, tieuDe, noiDung, ngayGui, gioGui);
                }
            }
            if (chucVu == 2)
            {
                dt_GiaoVien = gvDao.TaiThongTinGiaoVien(Login.userName.ToString().Trim());
                if (dt_GiaoVien.Rows.Count > 0)
                {
                    dt_DSThongBao = thuDao.LayThongBaoDuocGui(dt_GiaoVien.Rows[0]["MaGiaoVien"].ToString().Trim());
                }
                if (dt_DSThongBao.Rows.Count > 0)
                {
                    maThongBao = dt_DSThongBao.Rows[0]["MaThongBao"].ToString().Trim();
                    hoTenNguoiGui = dt_DSThongBao.Rows[0]["HoTen"].ToString().Trim();
                    maNhomHoc = dt_DSThongBao.Rows[0]["MaNhomHoc"].ToString().Trim();
                    tieuDe = dt_DSThongBao.Rows[0]["TieuDe"].ToString().Trim();
                    noiDung = dt_DSThongBao.Rows[0]["NoiDung"].ToString().Trim();
                    ngayGui = (DateTime)dt_DSThongBao.Rows[0]["NgayGui"];
                    gioGui = (TimeSpan)dt_DSThongBao.Rows[0]["GioGui"];
                    LoadChiTietThongBao(hoTenNguoiGui, maNhomHoc, tieuDe, noiDung, ngayGui, gioGui);
                }
            }
        }

        private void LoadChiTietThongBao (string hoTen, string maNhomHoc, string tieuDe, string noiDung, DateTime ngayGui, TimeSpan gioGui)
        {
            lbl_HoTen.Text = hoTen.ToString();
            lbl_MaNhomHoc.Text = maNhomHoc.ToString();
            lbl_TieuDe_CT.Text = tieuDe.ToString();
            lbl_NoiDung_CT.Text = noiDung.ToString();
            lbl_NgayThang.Text = ngayGui.ToString("dd/MM/yyyy");
            lbl_Gio.Text = gioGui.ToString("hh':'mm':'ss");
        }

        private void Load_UC_HAMTHUGOPY_CHILD (DataTable dataTable)
        {
            UC_HAMTHUGOPY_CHILD[] ucThuChild = new UC_HAMTHUGOPY_CHILD[dataTable.Rows.Count];

            for (int row = 0; row < dataTable.Rows.Count; row++)
            {

                //Hoten
                string hoten = dataTable.Rows[row]["HoTen"].ToString().Trim();
                //for (int r = 0; r < dtFULL_INFO.Rows.Count; r++)
                //{
                //    DataRow dtrow = dtFULL_INFO.Rows[r];
                //    if (dtDSThu.Rows[row]["IDGui"].ToString().Trim() == dtrow["ID"].ToString().Trim())
                //    {
                //        hoten = dtrow["HOTEN"].ToString().Trim();
                //        break;
                //    }
                //}

                //Ngay
                DateTime Ngay = (DateTime)dataTable.Rows[row]["NgayGui"];

                //Gio
                TimeSpan Gio = (TimeSpan)dataTable.Rows[row]["GioGui"];
                DateTime gio = Ngay.Add(Gio);
                ucThuChild[row] = new UC_HAMTHUGOPY_CHILD(dataTable.Rows[row]["MaThongBao"].ToString().Trim(), hoten,
                                                          dataTable.Rows[row]["TieuDe"].ToString().Trim(), dataTable.Rows[row]["NoiDung"].ToString().Trim(),
                                                          Ngay, gio);
                fLPnl_ThuDen.Controls.Add(ucThuChild[row]);
            }

            for (int r = 0; r < dataTable.Rows.Count; r++)
            {
                ucThuChild[r].ReadClicked += UC_HAMTHUGOPY_ReadClicked;
                //ucThuChild[r].DeleteClicked += UC_HAMTHUGOPY_DeleteClicked;
            }
        }

#if false
        public void LoadNotice(string agent)
        {
            pnNotice.Controls.Clear();
            if (agent != user)
            {
                this.dt = notice.SelectNoticeByLop(agent);
            }
            else LoadBy(accountType);
            UcNoticeItem[] ucNoticeItems = new UcNoticeItem[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ucNoticeItems[i] = new UcNoticeItem(user, dt.Rows[i]["maNguoiGui"].ToString(),
                                                    dt.Rows[i]["mathongbao"].ToString(), dt.Rows[i]["ngaygui"].ToString(),
                                                    dt.Rows[i]["hoten"].ToString(), dt.Rows[i]["malophoc"].ToString(),
                                                    dt.Rows[i]["makhoahoc"].ToString(),
                                                    dt.Rows[i]["tieude"].ToString(), dt.Rows[i]["noidung"].ToString());
                ucNoticeItems[i].Dock = DockStyle.Top;
                pnNotice.Controls.Add(ucNoticeItems[i]);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ucNoticeItems[i].btnReadClicked += myUserControl_btnReadClicked;
                ucNoticeItems[i].btnDeleteClicked += myUserControl_btnDeleteClicked;
            }


        }
        private void myUserControl_btnReadClicked(object sender, EventArgs e)
        {
            UcNoticeItem myUserControl = sender as UcNoticeItem;
            txtTieuDe.Text = myUserControl.TextTieuDe;
            txtNoiDung.Text = myUserControl.TextNoiDung;
            lbName.Text = myUserControl.NameGiaoVien;
            cbbMaLop.Text = myUserControl.IDLop;
            lbNgayGui.Text = myUserControl.NgayGui;
        }
#endif
    }
}
