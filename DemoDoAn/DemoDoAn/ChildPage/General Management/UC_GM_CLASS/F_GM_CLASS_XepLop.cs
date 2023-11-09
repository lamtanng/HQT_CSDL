using DemoDoAn.Custom_Control;
using DemoDoAn.DAO;
using DemoDoAn.MODELS;
//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoDoAn.ChildPage.General_Management.UC_GM_CLASS
{
    public partial class F_GM_CLASS_XepLop : Form
    {
        public const int SOBUOIHOCTRONGTUAN = 3;
        int soBuoiHoc = SOBUOIHOCTRONGTUAN;

        PhongHocDao phongHocDao = new PhongHocDao();
        NhomHocDao nhomHocDao = new NhomHocDao();
        LichHocDao LichHocDao = new LichHocDao();
        HocVaoDao hocVaoDao = new HocVaoDao();
        LopHocDao lopHocDao = new LopHocDao();

        DataTable dtKhoaHoc = new DataTable();
        DataTable dtLichHoc = new DataTable();
        string[] arrCacBuoiHoc = new string[3];
        string malop, tenlop, maKH, tenKH;


        public F_GM_CLASS_XepLop()
        {
            InitializeComponent();
        }

        public F_GM_CLASS_XepLop(string malop, string tenlop, string maKH, string tenKH)
        {
            InitializeComponent();
            this.malop = malop;
            this.tenlop = tenlop;
            this.maKH = maKH;
            this.tenKH = tenKH;
            //hienThongTin();
        }

        #region XULIDOHOa
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

        #endregion//hàm load các thanh trạng thái(fake combobox)
        private void closeForm()
        {
            this.Close();
        }
        private void timer_LoadTrangThai_Tick(object sender, EventArgs e)
        {
            ShowListChoss(fLPnl_TrangThai, timer_LoadTrangThai, ref select_TrangThai);
        }
        private void btn_Huy_Click(object sender, EventArgs e)
        {
            closeForm();
        }
        private void btn_TrangThai_Click(object sender, EventArgs e)
        {
            timer_LoadTrangThai.Start();
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

        private void F_GM_CLASS_XepLop_Load(object sender, EventArgs e)
        {
            dtKhoaHoc = nhomHocDao.LayDanhSachNhom();
            loadCbb_LopHoc();
            loadCbb_GioHoc();
        }

        //cap nhat
        private void btn_HoanThanh_Click(object sender, EventArgs e)
        {
            
            try
            {   //kiểm tra xem đã đủ thông tin hay chưa
                if (chonDuSoNgayHoc() && !String.IsNullOrEmpty(txt_SLHVToiThieu.Text))
                {
                    string maLop = ((DataRowView)cbb_LopHoc.SelectedItem)["MaLopHoc"].ToString();
                    string trangThai = btn_TrangThai.Text.ToString();
                    int soHVToiThieu = Convert.ToInt32(txt_SLHVToiThieu.Text.ToString());
                    int soHVToiDa = Convert.ToInt32(txt_SLHVToiDa.Text.ToString());
                    DateTime ngayBD = datePTime_NgayBatDau.Value;
                    DateTime ngayKT = Convert.ToDateTime(lbl_NgayKetThuc.Text.Trim(), CultureInfo.CreateSpecificCulture("en-US"));
                    string maGV = ((DataRowView)cbb_GiaoVien.SelectedItem)["MaGiaoVien"].ToString();
                    string soBuoi = "0";
                    string ca = ((DataRowView)cbb_GioHoc.SelectedItem)["Ca"].ToString();
                    string maPhong = ((DataRowView)cbb_PhongHoc.SelectedItem)["MaPhongHoc"].ToString();

                    //them l
                    NhomHoc nhom = new NhomHoc("", maLop, maGV, maPhong, Convert.ToInt32(ca), soHVToiThieu, soHVToiDa ,ngayBD, ngayKT, true);
                    nhomHocDao.taoNhomMoi(nhom);

                    DataTable dtNhomHocMoiThem = nhomHocDao.layNhomMoiNhatTrongLopHoc(nhom.MaLop);
                    if(dtNhomHocMoiThem.Rows.Count == 1)
                    {
                        HocVao hocVao = new HocVao();
                        hocVao.MaNhomHoc = dtNhomHocMoiThem.Rows[0]["MaNhomHoc"].ToString().Trim();
                        for(int i = 0; i< 3; i++)
                        {
                            hocVao.ThuTrongTuan = arrCacBuoiHoc[i];
                            hocVaoDao.themBuoiHoc(hocVao);
                        }

                    }


                    //xoa lich hoc cu
                    //LichHocDao.xoaLichHoc(malop);
                    //update thong tin lop hoc
                    //nhomHocDao.capNhatThongTinLop(lop);
                    //tao lich hoc moi
                    
                    this.Close();

                }
                else//chưa nhập đủ thông tin
                {
                    MessageBox.Show("Thông tin chưa chính xác!");
                }
            }
            catch//lỗi 
            {
                MessageBox.Show("Không hoàn thành!");
            }

        }

        //load combobox
        private void loadCombobox(ComboBox cbb, DataTable dt, string displayMember, string valueMember)
        {
            cbb.ValueMember = valueMember;
            cbb.DisplayMember = displayMember;
            cbb.DataSource = dt;
        }

        //load cbb lop hoc: Tạm thời bỏ vào class NhomHocDAO
        private void loadCbb_LopHoc()
        {
            DataTable dtLopHoc = new DataTable();
            dtLopHoc = lopHocDao.LayDanhSachLop();
            loadCombobox(cbb_LopHoc, dtLopHoc, "TenLopHoc", "MaLopHoc");
        }

        //load cbb gio hoc
        private void loadCbb_GioHoc()
        {
            DataTable dtGioHoc = new DataTable();
            dtGioHoc = nhomHocDao.gioHoc();
            dtGioHoc.Columns.Add("GioHoc");

            for (int r = 0; r < dtGioHoc.Rows.Count; r++)
            {
                string gioHoc = "Ca " + dtGioHoc.Rows[r]["Ca"].ToString() + ": " + dtGioHoc.Rows[r]["GioBatDau"].ToString() + " - " + dtGioHoc.Rows[r]["GioKetThuc"].ToString();
                dtGioHoc.Rows[r]["GioHoc"] = gioHoc;
            }
            loadCombobox(cbb_GioHoc, dtGioHoc, "GioHoc", "Ca");
        }

        //load cbb phong hoc
        private void loadCbb_PhongHoc(int ca)
        {
            DataTable dtPhongHoc = new DataTable();
            dtPhongHoc = LichHocDao.taiPhongTrongLich(arrCacBuoiHoc[0], arrCacBuoiHoc[1], arrCacBuoiHoc[2], ca);
            loadCombobox(cbb_PhongHoc, dtPhongHoc, "MaPhongHoc", "MaPhongHoc");

        }

        //check lịch trống
        //private void checkLichTrong(DataTable dtCheck, string thuocTinhCheck)
        //{
        //    //kiểm tra những "Thứ" nào được chọn
        //    foreach (CheckBox cbox in pnl_QLNgayHoc.Controls.OfType<CheckBox>())
        //    {
        //        if (cbox.Checked == true)
        //        {
        //            //chuẩn hóa 'Thứ 2' -> '2'
        //            string thu = chuanHoa(cbox.Text.ToString());
        //            //tìm tra Table lịch học all môn
        //            for (int r = 0; r < dtLichHoc.Rows.Count; r++)
        //            {   //tìm những "Thứ" và "Ca" đã được chọn đã có lịch dạy của môn khác hay chưa
        //                if (dtLichHoc.Rows[r]["Thu"].ToString().Trim() == thu && dtLichHoc.Rows[r]["Ca"].ToString().Trim() == ((DataRowView)cbb_GioHoc.SelectedItem)["Ca"].ToString().Trim())
        //                {
        //                    //nếu có thì phòng/GV đó đã có lịch
        //                    for (int i = dtCheck.Rows.Count- 1; i >=0 ; i--)
        //                    {
        //                        if (dtCheck.Rows[i][thuocTinhCheck].ToString().Trim() == dtLichHoc.Rows[r][thuocTinhCheck].ToString().Trim())
        //                        {
        //                            MessageBox.Show(dtCheck.Rows.Count.ToString());
        //                            //xóa phòng/GV đó khỏi DataTable
        //                            dtCheck.Rows[i].Delete();
        //                            dtCheck.AcceptChanges();

        //                            MessageBox.Show(dtCheck.Rows.Count.ToString());
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}

        //load cbb giao vien
        private void loadCbb_GiaoVien(int ca)
        {
            DataTable dtGV = new DataTable();
            dtGV = LichHocDao.taiGiaoVienTrongLich(arrCacBuoiHoc[0], arrCacBuoiHoc[1], arrCacBuoiHoc[2], ca);
           loadCombobox(cbb_GiaoVien, dtGV, "HoTen", "MaGiaoVien");
        }

        //load lịch học all môn
        private void loadLichHoc()
        {
            dtLichHoc = nhomHocDao.lichHocCacMon();
        }

        //chuẩn hóa 'Thứ 2' về '2':
        private string chuanHoa(string s)
        {
            // Xóa tất cả các dấu cách trong chuỗi
            s = s.Replace(" ", "");

            // Loại bỏ tất cả các kí tự không phải số
            s = Regex.Replace(s, "[^0-9]", "");

            return s;
        }

        //ktra chọn đủ số buổi học chưa
        private void kiemTraNgayHoc(CheckBox cb)
        {
            if (cb.Checked == true)
            {
                --soBuoiHoc;
            }
            else
            {
                ++soBuoiHoc;
            }
      
            //nếu đã chọn đủ số lượng buổi học thì không được chọn thêm buổi nào nữa
            if (soBuoiHoc == 0)
            {
                foreach (CheckBox cbox in pnl_QLNgayHoc.Controls.OfType<CheckBox>())
                {
                    if (cbox.Checked == false)
                        cbox.Enabled = false;

                }
                btn_Loc.Enabled = true; //bật nút kiểm tra
            }
            else//ngược lại nếu chưa đủ thì tất cả checkbox đều được chọn
            {
                foreach (CheckBox cbox in pnl_QLNgayHoc.Controls.OfType<CheckBox>())
                {
                    cbox.Enabled = true;
                }
                btn_Loc.Enabled = false;//tắt nút ktra
                cbb_GiaoVien.DataSource = null;
                cbb_PhongHoc.DataSource= null;
            }
        }
        private void cbox_Thu2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            try
            {
                kiemTraNgayHoc(cb);
                 
                
            }
            catch
            {
                //MessageBox.Show("Bạn chưa chọn số buổi học!");
                cb.Checked = false;
            }
            return;

        }
        private void cbox_Thu3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            kiemTraNgayHoc(cb);
             
            
        }
        private void cbox_Thu4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            kiemTraNgayHoc(cb);
             
            
        }
        private void cbox_Thu5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            kiemTraNgayHoc(cb);
             
            
        }
        private void cbox_Thu6_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            kiemTraNgayHoc(cb);
             
            
        }
        private void cbox_Thu7_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            kiemTraNgayHoc(cb);
             
            
        }

        private void txt_SoBuoiHoc_TextChanged(object sender, EventArgs e)
        {
            lbl_NgayKetThuc.Text = tinhNgayKetThuc();
        }

        private void datePTime_NgayBatDau_ValueChanged(object sender, EventArgs e)
        {
            lbl_NgayKetThuc.Text = tinhNgayKetThuc();
        }

        //Tính ngày kết thúc:
        private string tinhNgayKetThuc()
        {
          
            DataTable dtNgayKetThuc = LichHocDao.tinhNgayKetThuc(datePTime_NgayBatDau.Value, 
                                                                ((DataRowView)cbb_LopHoc.SelectedItem)["MaLopHoc"].ToString());
            DateTime ngayKetThuc = (DateTime)dtNgayKetThuc.Rows[0]["NgayKetThuc"];
            return ngayKetThuc.ToShortDateString(); // MM/dd/yyyy
        }

        //Lọc Phòng học + Giáo viên
        private void btn_Loc_Click(object sender, EventArgs e)
        {
            int i = 2;
            int ca = Convert.ToInt32(((DataRowView)cbb_GioHoc.SelectedItem)["Ca"].ToString());
            foreach (CheckBox cbox in pnl_QLNgayHoc.Controls.OfType<CheckBox>())
            {
                if (cbox.Checked == true)
                {
                    //chuẩn hóa 'Thứ 2' -> '2'
                    string thu = chuanHoa(cbox.Text.ToString());
                    arrCacBuoiHoc[i--] = thu;
                }
            }

            loadCbb_PhongHoc(ca);
            loadCbb_GiaoVien(ca);
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbb_LopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            string maLop = ((DataRowView)cbb_LopHoc.SelectedItem)["MaLopHoc"].ToString();
            DataTable dt = lopHocDao.LaySoBuoiHoc(maLop);
            if (dt.Rows.Count == 1)
            {
                txt_SoBuoiHoc.Text = dt.Rows[0]["TongSoBuoiHoc"].ToString().Trim();
            }
        }

        private void cbb_PhongHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbb_PhongHoc.DataSource != null)
            {
                try
                {
                    string maPhong = ((DataRowView)cbb_PhongHoc.SelectedItem)["MaPhongHoc"].ToString();
                    DataTable dt = phongHocDao.LaySoHocVienToiDa(maPhong);
                    if (dt.Rows.Count == 1)
                    {
                        txt_SLHVToiDa.Text = dt.Rows[0]["SoHocVienToiDa"].ToString().Trim();
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                txt_SLHVToiDa.Text = string.Empty;
            }
            
            
        }


        #region checkLoi
        //ktra đã chọn sô buổi học chưa
        private void chonSoBuoiHoc()
        {
            
        }

        private bool kiemTraRong(TextBox textbox)
        {
            int soLuongHV;
            bool isInt = int.TryParse(textbox.Text.ToString(), out soLuongHV);
            //ktra txtboxHocVien có rỗng hay có phải số nguyên không
            if (String.IsNullOrEmpty(textbox.Text.ToString()) || !isInt)
            {
                return false;
            }
            return true;//thỏa
        }
        //ktra tích đủ số thứ học 
        private bool chonDuSoNgayHoc()
        {
            return btn_Loc.Enabled == true;
        }
        #endregion

#if false //load cbb lophoc, khoahoc
        //load cbb lop hoc
        private void loadCbb_LopHoc(string maKH)
        {
            //tạo DT mới có số cột = số cột cũ qua .Clone()
            DataTable dtLopHoc = dtKhoaHoc.Clone();

            for (int r = 0; r < dtKhoaHoc.Rows.Count; r++)
            {
                //tìm những dòng có MãKH đã được chọn
                if (dtKhoaHoc.Rows[r]["MaKH"].ToString() == maKH)
                {
                    //tạo dataRow lưu hàng đó lại
                    DataRow newRow = dtLopHoc.NewRow();
                    newRow.ItemArray = dtKhoaHoc.Rows[r].ItemArray; // sao chép dữ liệu từ dòng r của dt vào newRow
                    dtLopHoc.Rows.Add(newRow);
                }
            }
            //load những lớp thuộc MãKH đó lên thôi
            loadCombobox(cbb_ChonLopHoc, dtLopHoc, "TenMon", "MaLop");
        }

        //load cbb khoa hoc
        private void loadCbb_KhoaHoc()
        {
            DataTable distinctMaKH = dtKhoaHoc.DefaultView.ToTable(true, new string[] { "MaKH", "TenKH" });
            loadCombobox(cbb_ChonKhoaHoc, distinctMaKH, "TenKH", "MaKH");
        }
#endif

    }
}
