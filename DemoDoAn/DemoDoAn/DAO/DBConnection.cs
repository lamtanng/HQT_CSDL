using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DemoDoAn.FORM;

namespace DemoDoAn
{
    internal class DBConnection
    {
        //SqlConnection conn = new SqlConnection(Properties.Settings.Default.cnnStr);


        //lấy danh sách của các bảng có sẵn
        public DataTable LayDanhSach(string sqlStr)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-JGRTVMGL\MYCOM;Initial Catalog=TRUNGTAMHOCLAPTRINH;User Id=" + Login.userName + ";Password=" + Login.password + ";");

            DataTable dtds = new DataTable();
            try


            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sqlStr, conn);
                adapter.Fill(dtds);
            }
            catch (SqlException exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
            return dtds;
        }

        //
        public void ThucThi(string sqlStr)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-JGRTVMGL\MYCOM;Initial Catalog=TRUNGTAMHOCLAPTRINH;User Id=" + Login.userName + ";Password=" + Login.password + ";");

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlStr, conn);

                if (cmd.ExecuteNonQuery() > 0)
                { }
                    //MessageBox.Show("thanh cong");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
