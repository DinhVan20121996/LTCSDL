using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Bai4_B3
{
    public partial class Form1 : Form
    {
        SqlConnection sc = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string cnstr = "Server = . ; Database = QLBanHang; Integrated Security = true ";
            sc = new SqlConnection(cnstr);
            dsGV1.DataSource = LoadData();
        }
        private void Connect()
        {  
            try
            {
                if (sc != null && sc.State != ConnectionState.Open)
                    sc.Open();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Kết nối đã mở hoặc chưa có CSDL\n" + ex.Message);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Mật khẩu CSDL sai\n" + ex.Message);
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show("Có 2 CSDL giống nhau\n" + ex.Message);
            }
        }
        private void DisConnect()
        {
            if (sc != null && sc.State != ConnectionState.Closed)
            {
                sc.Close();
            }
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            string Query = "INSERT FROM LoaiSP WHERE MaLoaiSP =" + txtmaloai.Text; 
        }
        private void btxoa_Click(object sender, EventArgs e)
        {
            string Query = "DELETE FROM LoaiSP WHERE MaLoaiSP =" + txtmaloai.Text;
            Connect();
            SqlCommand cmd = new SqlCommand(Query, sc);
            int NumberOfRows = 0;
            NumberOfRows = cmd.ExecuteNonQuery();
            dsGV1.DataSource = LoadData();
            MessageBox.Show("Đã xóa dòng : " + NumberOfRows.ToString());
            DisConnect();
            
        }
        private List<object> LoadData()
        {
            Connect();
            string sql = "SELECT * FROM LoaiSP";
            List<object> list = new List<object>();
            try
            {
                SqlCommand cmd = new SqlCommand(sql, sc);
                SqlDataReader Reader = cmd.ExecuteReader();
                int id; 
                string name;
                while (Reader.Read())
                {
                    id = Reader.GetInt32(0);
                    name = Reader.GetString(1);
                    var prod = new
                    {
                        MaLoaiSP = id,
                        TenLoaiSP = name
                    };
                    list.Add(prod);
                }
                Reader.Close();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DisConnect(); 
            }
            return list;   
        }
    }
}
