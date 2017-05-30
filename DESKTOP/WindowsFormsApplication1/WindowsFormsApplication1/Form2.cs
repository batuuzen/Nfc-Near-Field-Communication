using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        string[] ports = SerialPort.GetPortNames();

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (comboBox2.Text == "")
                    return;
                serialPort1.PortName = comboBox2.Text;
                try
                {
                    serialPort1.Open();
                    MessageBox.Show("Port Açıldı, lütfen telefonunuzu okutunuz");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
          
            string b;
          
            string rfIdkart = serialPort1.ReadLine();

            string rfIdkart1 = serialPort1.ReadLine();
            string rfIdkart2 = serialPort1.ReadLine();
            string rfIdkart3 = serialPort1.ReadLine();

            string rfIdkart4 = serialPort1.ReadLine();
            string rfIdkart5 = serialPort1.ReadLine();
            string rfIdkart6 = serialPort1.ReadLine();

            string[] dizi = new string[7];


            dizi[0] = rfIdkart;
            dizi[1] = rfIdkart1;
            dizi[2] = rfIdkart2;
            dizi[3] = rfIdkart3;
            dizi[4] = rfIdkart4;
            dizi[5] = rfIdkart5;
            dizi[6] = rfIdkart6;

            
            int[] myInts = Array.ConvertAll(dizi, int.Parse);//diziyi int e çeviriyorum

            char[] chars = myInts.Select(x => (char)x).ToArray();
            string str = new string(chars); //int e çevirdiğim dizinin decimal degerlerini ascıı karşılıklarını yazıyorum.

            int[] sequence = chars.Select(c => Convert.ToInt32(c.ToString())).ToArray();
            for (int i = 0; i < chars.Length; i++)
            {
                sequence[i] = Convert.ToInt32(chars[i].ToString());//ascıı karakterli char dizisini int dizisine ceviriyorum. bunu yapmamdaki amaç kontrol edebilmek
            }

            if (Math.Abs(sequence[5] - sequence[0]) == Math.Abs(sequence[6]))//dizini 6.elemanı ile ilk elemanı arasındaki fark eger 7. elemana eşit ise kayıt ediyorum.
            {


                for (int i = 0; i < 7; i++)
                {
                    textBox7.Text = textBox7.Text + sequence[i];
                }

                
            }
            else MessageBox.Show("hatalı veri!");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            personelEkle();
            personelListele();
        }   
        private void personelEkle()
        {
            SqlConnection sCon = new SqlConnection();
            sCon.ConnectionString = "Data Source=BATUHANOZEN\\SQL_2014;Initial Catalog=personel;Integrated Security=True;";
            SqlCommand kayitEkle = new SqlCommand();
            kayitEkle.Connection = sCon;


            kayitEkle.Parameters.AddWithValue("@personelAd", textBox1.Text);
            kayitEkle.Parameters.AddWithValue("@personelSoyad", textBox2.Text);
            kayitEkle.Parameters.AddWithValue("@tcKimlik", textBox3.Text);
            kayitEkle.Parameters.AddWithValue("@bolum", comboBox1.Text);
            kayitEkle.Parameters.AddWithValue("@adres", textBox5.Text);
            kayitEkle.Parameters.AddWithValue("@telefon", textBox6.Text);
            kayitEkle.Parameters.AddWithValue("@rfId", textBox7.Text);
            kayitEkle.CommandText = "INSERT INTO personel values(@personelAd,@personelSoyad,@tcKimlik,@bolum,@adres,@telefon,@rfId)";

            sCon.Open();
            int sonuc = kayitEkle.ExecuteNonQuery();
            sCon.Close();
            if (sonuc != null)
            {
                MessageBox.Show("KAYIT EKLENDİ");
                textBox7.Clear();
                textBox6.Clear();
                textBox5.Clear();
             //  comboBox1.Clear();
                textBox3.Clear();
                textBox2.Clear();
                textBox1.Clear();
            }


        }

        private void Form2_Load(object sender, EventArgs e)
        {
            personelListele();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            comboBox1.Items.Add("Arge");

            comboBox1.Items.Add("Bilgi İşlem");

            comboBox1.Items.Add("Yönetim");
            foreach (string port in ports)
            {
                comboBox2.Items.Add(port);
                // comboBox1.SelectedIndex = 0;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            personelGuncelle();
            personelListele();

        }
        private void personelListele()
        {
            SqlConnection con = new SqlConnection("Data Source = BATUHANOZEN\\SQL_2014; Initial Catalog = personel; Integrated Security = True;");
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "select personel.personelId,personelAd,personelSoyad,tcKimlik,bolum,adres,telefon,rfId  from personel";
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void personelGuncelle()
        {
            SqlConnection sCon = new SqlConnection();
            sCon.ConnectionString = "Data Source=BATUHANOZEN\\SQL_2014;Initial Catalog=personel;Integrated Security=True;";
            SqlCommand Guncelle = new SqlCommand();
            Guncelle.Connection = sCon;
            Guncelle.CommandText = "Update personel set personelAd=@personelAd,personelSoyad=@personelSoyad,tcKimlik=@tcKimlik,bolum=@bolum,adres=@adres,telefon=@telefon,rfId=@rfId Where personelId=@personelId";

            Guncelle.Parameters.AddWithValue("@PersonelId", dataGridView1.CurrentRow.Cells[0].Value);
            Guncelle.Parameters.AddWithValue("@personelAd", textBox1.Text);
            Guncelle.Parameters.AddWithValue("@personelSoyad", textBox2.Text);
            Guncelle.Parameters.AddWithValue("@tcKimlik", textBox3.Text);
            Guncelle.Parameters.AddWithValue("@bolum", comboBox1.Text);
            Guncelle.Parameters.AddWithValue("@adres", textBox5.Text);
            Guncelle.Parameters.AddWithValue("@telefon", textBox6.Text);
            Guncelle.Parameters.AddWithValue("@rfId", textBox7.Text);

            sCon.Open();
            Guncelle.ExecuteNonQuery();
            MessageBox.Show("Kayıt Güncelledi");

            textBox7.Clear();
            textBox6.Clear();
            textBox5.Clear();
            //  comboBox1.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox1.Clear();
            sCon.Close();
        }
        private void personelsil()
        {
            SqlConnection sCon = new SqlConnection();
            sCon.ConnectionString = "Data Source=BATUHANOZEN\\SQL_2014;Initial Catalog=personel;Integrated Security=True;";
            SqlCommand Sil = new SqlCommand();
            Sil.Connection = sCon;
            Sil.CommandText = "Delete From personel  Where personelId=@personelId";

            Sil.Parameters.AddWithValue("@PersonelId", dataGridView1.CurrentRow.Cells[0].Value);
            sCon.Open();
            Sil.ExecuteNonQuery();
            MessageBox.Show("Kayıt Silindi");
            textBox7.Clear();
            textBox6.Clear();
            textBox5.Clear();
            //  comboBox1.Clear();
            textBox3.Clear();
            textBox2.Clear();
            textBox1.Clear();
            sCon.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            personelsil();
            personelListele();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (comboBox2.Text == "")
                    return;
                serialPort1.PortName = comboBox2.Text;
                try
                {
                    serialPort1.Open();
                    MessageBox.Show("port açıldı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            MessageBox.Show("Port Kapatıldı");
        }
    }
}
