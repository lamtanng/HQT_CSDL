using DemoDoAn.ChildPage.HocTap;
using DemoDoAn.DAO;
using DemoDoAn.HOCVIEN.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace DemoDoAn.ChildPage
{
    public partial class UC_STUDENT_XEPLOP : UserControl
    {
        NhomHocDao LopHocDao = new NhomHocDao();
        BangDiemDAO bangDiemDao = new BangDiemDAO();
        HocSinhDao hvDao = new HocSinhDao();
        DangKiLopDao dklDao = new DangKiLopDao();
        PhieuThuDao ptDao = new PhieuThuDao();

        DanhSachNhomDao dsNhomDao = new DanhSachNhomDao();
        NhomHocDao nhomDao = new NhomHocDao();
        LopHocDao lopDao = new LopHocDao();
        KhoaHocDao khDao = new KhoaHocDao();
       
        DataTable dtDSHV_KhacNhom = new DataTable();
        DataTable dtDSHVNhom = new DataTable();
        DataTable dtDSNhomDaDangKy = new DataTable();

        string maNhom = "";

        enum nameCol_DSHV
        {
            STT_DSHV, HVID_DSHV, HOTEN_DSHV, Them
        }
        enum nameCol_DSL
        {
            STT_DSL, HVID_DSL, HOTEN_DSL, Xoa_DSL
        }

        //trang thai si so lop: hoat dong || da day
        string trangThai;

        public UC_STUDENT_XEPLOP()
        {
            InitializeComponent();
        }

        #region DoHoa
        //ẩn text Search
        bool isEmpty_Search_DSHV = true;
        bool isEmpty_Search_DSL = true;
        private void txt_SearchHVChuaCoLop_Click(object sender, EventArgs e)
        {
            an_SearchText(txt_SearchHVChuaCoLop, ref isEmpty_Search_DSHV);
        }
        private void txt_SearchDSLopHoc_Click(object sender, EventArgs e)
        {
            an_SearchText(txt_SearchDSLopHoc, ref isEmpty_Search_DSL);
        }
        private void an_SearchText(TextBox txt_Search, ref bool isEmpty_Search)
        {
            if (isEmpty_Search == true)
            {
                txt_Search.Text = String.Empty;
                txt_Search.Font = new Font(txt_Search.Font, FontStyle.Regular);
                txt_Search.ForeColor = Color.DimGray;
                isEmpty_Search = false;
            }
        }
        private void hien_SearchText(TextBox txt_Search, ref bool isEmpty_Search)
        {
            txt_Search.Text = "Search...";
            txt_Search.Font = new Font("SFU Futura", 10F, FontStyle.Italic);
            txt_Search.ForeColor = Color.Silver;
            isEmpty_Search = true;
        }
        #endregion

        private void UC_STUDENT_XEPLOP_Load(object sender, EventArgs e)
        {
            loadCbb_KhoaHoc();
        }

        //load form
        private void loadForm(DataGridView dtg, DataTable dt)
        {
            dtg.DataSource = dt;
        }

        //load ds hoc vien chua co lop
        private void taiDSHV_KhacNhom(DataGridView dtg)
        {
            dtDSHV_KhacNhom = dsNhomDao.layDSHocVien_KhacNhom(maNhom);

            loadForm(dtg, dtDSHV_KhacNhom);
            //ẩn full các cột     
            for (int i = 0; i < dtg.Columns.Count; i++)
            {
                DataGridViewColumn colDtg = dtg.Columns[i];
                colDtg.Visible = false;
            }
            //hiện những cột cần, name của các cột được lưu trong Enum
            foreach (nameCol_DSHV day in Enum.GetValues(typeof(nameCol_DSHV)))
            {
                for (int i = 0; i < dtg.Columns.Count; i++)
                {
                    DataGridViewColumn colDtg = dtg.Columns[i];
                    if (colDtg.Name.ToString() == day.ToString())
                    {
                        colDtg.Visible = true;
                        break;
                    }
                }
            }
           // dtg.Sort(dtg.Columns["HVID_DSHV"], ListSortDirection.Ascending);
        }

        //load ds lop
        private void taiDSHVNhom(DataGridView dtg )
        {
            dtDSHVNhom = dsNhomDao.layDSHocVienNhomHoc(maNhom);
            loadForm(dtg, dtDSHVNhom);
            for (int i = 0; i < dtg.Columns.Count; i++)
            {
                DataGridViewColumn colDtg = dtg.Columns[i];
                colDtg.Visible = false;
            }
            //hiện những cột cần, name của các cột được lưu trong Enum
            foreach (nameCol_DSL day in Enum.GetValues(typeof(nameCol_DSL)))
            {
                for (int i = 0; i < dtg.Columns.Count; i++)
                {
                    DataGridViewColumn colDtg = dtg.Columns[i];
                    if (colDtg.Name.ToString() == day.ToString())
                    {
                        colDtg.Visible = true;
                        break;
                    }
                }
            }
            //dtg.Sort(dtg.Columns["HVID_DSL"], ListSortDirection.Ascending);
        }

        //load combobox
        private void loadCombobox(ComboBox cbb, DataTable dt, string displayMember, string valueMember)
        {
            cbb.DataSource = dt;
            cbb.DisplayMember = displayMember;
            cbb.ValueMember = displayMember;
        }

        //load cbb khoa hoc
        private void loadCbb_KhoaHoc()
        {
            DataTable dtKhoaHoc = khDao.LayKhoaHoc();
            loadCombobox(cbb_KhoaHoc, dtKhoaHoc, "TenKhoaHoc", "MaKhoaHoc");
            string maKH = ((DataRowView)cbb_KhoaHoc.SelectedItem)["MaKhoaHoc"].ToString();

            //loadCbb_LopHoc(maKH);
        }
        //load cbb lop hoc
        private void loadCbb_LopHoc(string maKH)
        {
            DataTable dtLopHoc = lopDao.LayLopHoc_ThuocKhoaHoc(maKH);
            loadCombobox(cbb_LopHoc, dtLopHoc, "TenLopHoc", "MaLopHoc");
            string maLH = ((DataRowView)cbb_LopHoc.SelectedItem)["MaLopHoc"].ToString();
            //loadCbb_NhomHoc(maLH);
            //dtKhoaHoc.Rows.Clear();
            //dtKhoaHoc = LopHocDao.LayDanhSachNhom();
            ////duyet lui chứ mỗi lần xóa bị lỗi
            //int rows = dtKhoaHoc.Rows.Count;
            //for (int r = rows - 1; r >= 0; r--)
            //{
            //    DataRow row = dtKhoaHoc.Rows[r];
            //    //loại bỏ các dòng có khóa học và lớp học đã ngưng hoạt động
            //    if (Convert.ToInt32(row["TrangThaiKH"]) == 0 || Convert.ToInt32(row["TTMoLop"]) == 0)
            //        dtKhoaHoc.Rows.Remove(row);
            //}
            //DataTable distinctMaKH = dtKhoaHoc.DefaultView.ToTable(true, new string[] { "MaKH", "TenKH" });
            //loadCombobox(cbb_LopHoc, distinctMaKH, "TenKH", "MaKH");
        }

        //load cbb nhom hoc
        private void loadCbb_NhomHoc(string maLH)
        {
            DataTable dtNhomHoc = nhomDao.LayDanhSachNhom_ThuocLopHoc(maLH);
            loadCombobox(cbb_NhomHoc, dtNhomHoc, "MaNhomHoc", "MaNhomHoc");
            ////tạo DT mới có số cột = số cột cũ qua .Clone()
            //DataTable dtLopHoc = dtKhoaHoc.Clone();

            //for (int r = 0; r < dtKhoaHoc.Rows.Count; r++)
            //{
            //    //tìm những dòng có MãKH đã được chọn
            //    if (dtKhoaHoc.Rows[r]["MaKH"].ToString() == maKH)
            //    {
            //        //tạo dataRow lưu hàng đó lại
            //        DataRow newRow = dtLopHoc.NewRow();
            //        newRow.ItemArray = dtKhoaHoc.Rows[r].ItemArray; // sao chép dữ liệu từ dòng r của dt vào newRow
            //        dtLopHoc.Rows.Add(newRow);
            //    }
            //}
            ////load những lớp thuộc MãKH đó lên thôi
            //loadCombobox(cbb_NhomHoc, dtLopHoc, "TenMon", "MaLop");
        }

        //xu li chon khoa hoc
        private void cbb_KhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedIndex >= 0)
            {
                string maKH = ((DataRowView)cbb.SelectedItem)["MaKhoaHoc"].ToString();
                loadCbb_LopHoc(maKH);
            }
        }
        //xu li chon lop hoc 
        private void cbb_LopHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedIndex >= 0)
            {
                string maLH = ((DataRowView)cbb.SelectedItem)["MaLopHoc"].ToString();
                loadCbb_NhomHoc(maLH);
            }
        }
        //xu li chon nhom hoc
        private void cbb_NhomHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedIndex >= 0)
            {
                maNhom = ((DataRowView)cbb_NhomHoc.SelectedItem)["MaNhomHoc"].ToString();
                taiDSHVNhom(dataGrView_DSLop);
                taiDSHV_KhacNhom(dataGrView_DSHocVien);
                hienThongTinNhom();
            }
        }

        //hien thong tin nhom
        private void hienThongTinNhom()
        {
            //lấy thông tin lớp
            DataTable dtThongTinNhom = new DataTable();
            dtThongTinNhom = nhomDao.LayThongTinMotNhom(maNhom);

            DataRow row = dtThongTinNhom.Rows[0];
            lbl_TT_TenKhoaHoc.Text = ((DataRowView)cbb_KhoaHoc.SelectedItem)["TenKhoaHoc"].ToString();
            lbl_TT_TenLopHoc.Text = row["TenLopHoc"].ToString();
            string ngayBD = ((DateTime)row["NgayBatDau"]).ToString("dd/MM/yyyy");
            lbl_TT_NgayBatDau.Text = ngayBD;
            string ngayKT = ((DateTime)row["NgayKetThuc"]).ToString("dd/MM/yyyy");
            lbl_TT_NgayKetThuc.Text = ngayKT;
            lbl_TT_TongSoHV.Text = row["SoLuongHocVienToiDa"].ToString();
            lbl_TT_HocVienHienTai.Text = row["TongHocVien"].ToString();
            lbl_TT_TenGiangVien.Text = row["TenGiaoVien"].ToString();
            lbl_TT_SoTienHocPhi.Text = row["HocPhi"].ToString();

           //trangThai = row["TrangThai"].ToString().Trim();
        }

        //them hoc vien vao lop
        private void dataGrView_DSHocVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            try
            {
                row = dataGrView_DSHocVien.Rows[e.RowIndex];
                if (dataGrView_DSHocVien.Columns[e.ColumnIndex].Name == "Them")
                {
                    string hvID = row.Cells["HVID_DSHV"].Value.ToString();
                    dsNhomDao.themHocVienVaoNhom(maNhom, hvID);
                     
                }
                taiDSHVNhom(dataGrView_DSLop);
                taiDSHV_KhacNhom(dataGrView_DSHocVien);
                hienThongTinNhom();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        //check trung lich
        private bool kiemTraTrungLich(string maLop, ref string loptrunglich, string hvid)
        {
            taiDSL_DaDangKy(hvid);

            //lay thong tin lich hoc cua nhóm
            DataTable dtTTCT = new DataTable("TTChiTiet");
            dtTTCT = dklDao.LayThongTinLop(maLop);
            for (int i = 0; i < dtTTCT.Rows.Count; i++)
            {
                DataRow rowDK = dtTTCT.Rows[i];

                //duyệt từng nhóm đã đki
                for (int j = 0; j < dtDSNhomDaDangKy.Rows.Count; j++)
                {
                    //lay thong tin lich hoc của từng nhóm đã đăng ký
                    DataTable dtLichHocNhom = new DataTable("LichHocNhom");
                    dtLichHocNhom = dklDao.LayThongTinLop(dtDSNhomDaDangKy.Rows[j]["MaLop"].ToString());
                    for (int r = 0; r < dtLichHocNhom.Rows.Count; r++)
                    {
                        //xét trùng thứ
                        DataRow rowKQ = dtLichHocNhom.Rows[r];
                        if (Convert.ToInt32(rowKQ["Thu"].ToString()) == Convert.ToInt32(rowDK["Thu"].ToString()))
                        {   //xét trùng ca học
                            if (Convert.ToInt32(rowKQ["Ca"].ToString()) == Convert.ToInt32(rowDK["Ca"].ToString()))
                            {
                                //MessageBox.Show(rowKQ["MaLop"].ToString().Trim());
                                loptrunglich = dtDSNhomDaDangKy.Rows[j]["TenMon"].ToString();
                                return false;//trung lich
                            }
                        }
                    }

                }

            }
            return true;

        }
        //tai DSLdaDangKy
        private void taiDSL_DaDangKy(string hvid)
        {
            dtDSNhomDaDangKy.Rows.Clear();
            dtDSNhomDaDangKy = dklDao.LayDanhSachLopDaDangKi(hvid);
        }

        //xoa hoc vien khoi lop
        private void dataGrView_DSLop_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dtg = sender as DataGridView;
            try
            {
                DataGridViewRow row = new DataGridViewRow();
                row = dtg.Rows[e.RowIndex];
                if (dtg.Columns[e.ColumnIndex].Name == "Xoa_DSL")
                {
                    if (MessageBox.Show("Bạn muốn xóa học viên?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string hvID = row.Cells["HVID_DSL"].Value.ToString();
                        dsNhomDao.xoaHocVienKhoiNhom(maNhom, hvID);

                        hienThongTinNhom();//cập nhật lại trạng thái 
                        taiDSHVNhom(dataGrView_DSLop);
                        taiDSHV_KhacNhom(dataGrView_DSHocVien);
                        hienThongTinNhom();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        
        //tim kiem + reset DSHV
        private void txt_SearchHVChuaCoLop_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                thucHienTimKiem(dataGrView_DSHocVien, txt_SearchHVChuaCoLop.Text.ToString(), typeof(nameCol_DSHV));
            }
            else
                if (e.KeyCode == Keys.Escape)
            {
                thucHienReset(dataGrView_DSHocVien, txt_SearchHVChuaCoLop, ref isEmpty_Search_DSHV);
            }
        }
        //tim kiem + reset DSL
        private void txt_SearchDSLopHoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                thucHienTimKiem(dataGrView_DSLop, txt_SearchDSLopHoc.Text.ToString(), typeof(nameCol_DSL));
            }
            else
               if (e.KeyCode == Keys.Escape)
            {
                thucHienReset(dataGrView_DSLop, txt_SearchDSLopHoc, ref isEmpty_Search_DSHV);
            }
        }
        //thuc hien tim kiem
        private void thucHienTimKiem(DataGridView dtg, string duLieu, Type enumType)
        {
            locDuLieuTimKiem(dtg, duLieu, enumType);
        }
        //lọc dữ liệu để tìm kiếm trong DataGridView
        private void locDuLieuTimKiem(DataGridView dtg, string searchText, Type enumType)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                // Nếu không có dữ liệu nhập vào, hiển thị tất cả các dòng
                dtg.Rows.Cast<DataGridViewRow>().ToList().ForEach(r => r.Visible = true);
            }
            else
            {
                for (int r = 0; r < dtg.Rows.Count; r++)
                {
                    DataGridViewRow row = dtg.Rows[r];

                    //4 dòng fix lỗi : 'Row associated with the currency manager's position cannot be made invisible.'
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dtg.DataSource];
                    currencyManager1.SuspendBinding();
                    row.Visible = false;
                    currencyManager1.ResumeBinding();

                    //tim kiem dữ liệu có chứa trong các ô của row không
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        //hiện những cột cần, name của các cột được lưu trong Enum
                        foreach (var day in Enum.GetValues(enumType))
                        {
                            //chỉ tìm trên các ô thuộc cột có trong enum:
                            if (dtg.Columns[cell.ColumnIndex].Name == day.ToString())
                            {
                                if (cell.Value != null && cell.Value.ToString().Contains(searchText))
                                {
                                    row.Visible = true;
                                    break;
                                }
                            }
                        }
                    }
                }


            }
        }
        //thuc hien reset
        private void thucHienReset(DataGridView dtg, TextBox txt_Search,ref bool isEmptyText)
        {
            // hiển thị tất cả các dòng
            dtg.Rows.Cast<DataGridViewRow>().ToList().ForEach(r => r.Visible = true);
            hien_SearchText(txt_Search, ref isEmptyText);
        }

        //danh STT
        private void dataGrView_DSLop_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow row = dataGrView_DSLop.Rows[e.RowIndex];
            row.Cells[0].Value = (e.RowIndex + 1).ToString();
        }
        private void dataGrView_DSHocVien_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow row = dataGrView_DSHocVien.Rows[e.RowIndex];
            row.Cells[0].Value = (e.RowIndex + 1).ToString();
        }

    }
}
