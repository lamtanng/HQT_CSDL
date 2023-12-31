﻿using DemoDoAn.FORM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;



namespace DemoDoAn
{

    public partial class Login : Form
    {
        LoginDAO logDAO = new LoginDAO();
        DataTable dtTaiKhoan = new DataTable();
        public static string userName = "";
        public static string password = "";

        enum roles
        {
            //role_Admin,
            role_HocVien, 
            role_GiaoVien
        }

        public Login()
        {
            InitializeComponent();
            
        }

        #region DOHOA
        bool isHidePass = false;
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (isHidePass == false)
            {
                txt_Password.PasswordChar = char.MinValue;
                pictureBox_HidePass.Image = new Bitmap(Application.StartupPath + "\\Resources\\eye-crossed.png");
                pictureBox_HidePass.SizeMode = PictureBoxSizeMode.CenterImage;
                isHidePass = true;
            }
            else
            {
                txt_Password.PasswordChar = '*';
                pictureBox_HidePass.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox_HidePass.Image = new Bitmap(Application.StartupPath + "\\Resources\\eye.png");
                isHidePass = false;
            }
        }

        bool isEmpty_Username = true;
        private void txt_Username_MouseClick(object sender, MouseEventArgs e)
        {

            if (isEmpty_Username == true)
            {
                txt_Username.Text = String.Empty;
                txt_Username.Font = new Font(txt_Username.Font, FontStyle.Regular);
                txt_Username.ForeColor = Color.DimGray;
                isEmpty_Username = false;
            }
        }

        //hien an pass
        private void hien_PasswordText()
        {
            txt_Password.Font = new Font(txt_Password.Font, FontStyle.Italic);
            txt_Password.ForeColor = Color.Silver;
        }
        private void an_PasswordText()
        {
            txt_Password.Font = new Font(txt_Password.Font, FontStyle.Regular);
            txt_Password.ForeColor = Color.DimGray;
        }

        bool isEmpty_Password = true;
        private void txt_Password_MouseClick(object sender, MouseEventArgs e)
        {
            if (isEmpty_Password == true)
            {
                txt_Password.PasswordChar = '*';
                txt_Password.Text = String.Empty;
                txt_Password.Font = new Font(txt_Password.Font, FontStyle.Regular);
                txt_Password.ForeColor = Color.DimGray;
                isEmpty_Password = false;
            }
        }

        //create acc
        private void lbl_CreateAccount_MouseMove(object sender, MouseEventArgs e)
        {
            Font fontMove = new Font("SFU Futura", 8);
            lbl_CreateAccount.ForeColor = Color.MediumAquamarine;
            lbl_CreateAccount.Font = fontMove;
            // lbl_CreateAccount.Font = new Font(lbl_CreateAccount.Font, FontStyle.Italic);
            lbl_CreateAccount.Font = new System.Drawing.Font("SFU Futura", 8.5F,
                ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        private void lbl_CreateAccount_MouseLeave(object sender, EventArgs e)
        {
            lbl_CreateAccount.ForeColor = Color.Green;
            lbl_CreateAccount.Font = new System.Drawing.Font("SFU Futura", 9F,
                ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))),
                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        #endregion

        //tai bang account
        private void taiTaiKhoan()
        {
            try
            {
                dtTaiKhoan = logDAO.Login(txt_Username.Text.Trim(), txt_Password.Text.Trim());
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //click login
        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                taiTaiKhoan();
                userName = txt_Username.Text.ToString();
                password = txt_Password.Text.ToString();
                //Enum.GetName(typeof(roles), roles.role_HocVien) -->string

                //check acc co ton tai khong
                int r = dtTaiKhoan.Rows.Count;
                if (r >= 0)
                {
                    String quyenNguoiDung = dtTaiKhoan.Rows[0]["Role"].ToString().Trim();
                    if (quyenNguoiDung == Enum.GetName(typeof(roles), roles.role_GiaoVien))
                    {
                        F_HOCVIEN gv = new F_HOCVIEN(2);
                        gv.Show();
                        this.Hide();
                    }
                    else if (quyenNguoiDung == Enum.GetName(typeof(roles), roles.role_HocVien))
                    {
                        F_HOCVIEN hv = new F_HOCVIEN(1);
                        hv.Show();
                        this.Hide();
                    }
                    else
                    {
                        F_Addmin admin = new F_Addmin();
                        admin.Show();
                        this.Hide();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng kiểm tra lại thông tin!");
            }
        }

        //check tai khoan có tồn tại chưa ->Trigger
        private int isEmptyAccount(string username, string pass)
        {
            for (int r = 0; r < dtTaiKhoan.Rows.Count; r++)
            {
                DataRow row = dtTaiKhoan.Rows[r];
                if (row["TenDangNhap"].ToString().Trim() == username.Trim() && row["MatKhau"].ToString().Trim() == pass.Trim())
                    return r;
            }
            return -1;
        }

        //dang ky tai khoan
        private void lbl_CreateAccount_Click(object sender, EventArgs e)
        {
            SignUp a = new SignUp();
            this.Hide();
            a.Show();
        }

        //hien pass da ghi nho
        private void txt_Username_TextChanged(object sender, EventArgs e)
        {
            userName = txt_Username.Text.ToString().Trim();
        }

        //check thong tin day du
        private bool checkLogin()
        {
            if (string.IsNullOrEmpty(txt_Password.Text.ToString()) || string.IsNullOrEmpty(txt_Username.Text.ToString()))
            {
                return false;
            }
            return true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_Password_TextChanged(object sender, EventArgs e)
        {
            password = txt_Password.Text.ToString().Trim();
        }
    }
}

