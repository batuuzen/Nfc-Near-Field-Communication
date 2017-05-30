using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //            string donustur = dateTimePicker1.Value.ToString("yyyy - MM - dd HH: mm:ss.FFF");

            string donustur = dateTimePicker1.Value.ToString("yyyy - MM - dd HH: mm:ss.FFF ");
            string donustur2 = dateTimePicker2.Value.ToString("yyyy - MM - dd HH: mm:ss.FFF");
            //string[] dizi = new string[2];
            //dizi[0] = donustur;
            //dizi[1] = donustur2;
            //int[] myInts = Array.ConvertAll(dizi, int.Parse);
            SqlConnection con = new SqlConnection("Data Source = BATUHANOZEN\\SQL_2014; Initial Catalog = personel; Integrated Security = True;");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            //if (myInts[0]==myInts[1])
            //{
            //    cmd.CommandText = "select personel.personelAd,personelSoyad,hareket.tarihsaat  from personel join hareket on personel.personelId=hareket.personelId    where tarihSaat    @Tarih1   ";//
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Parameters.AddWithValue("@Tarih1", donustur);


            //    con.Close();
            //}
            
                cmd.CommandText = "select personel.personelAd,personelSoyad,hareket.tarihsaat  from personel join hareket on personel.personelId=hareket.personelId    where tarihSaat   between @Tarih1 and @Tarih2  ";//
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Tarih1", donustur);
                cmd.Parameters.AddWithValue("@Tarih2", donustur2);
            
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source = BATUHANOZEN\\SQL_2014; Initial Catalog = personel; Integrated Security = True;");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select personel.personelAd,personelSoyad,hareket.tarihsaat  from personel join hareket on personel.personelId=hareket.personelId    where personelAd=@personelAd ";

            //cmd.CommandText = "select * from hareket where PersonelId=@PersonelId";
            cmd.CommandType = CommandType.Text;
            //cmd.Parameters.AddWithValue("@PersonelId", textBox1.Text);
            cmd.Parameters.AddWithValue("@personelAd", textBox1.Text);

            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            con.Close();



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
