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
using System.Threading;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        SqlConnection sCon = new SqlConnection();
        SqlDataAdapter dta = new SqlDataAdapter();
        SqlCommand kayitGetir = new SqlCommand();
        SqlCommand hareket = new SqlCommand();
        DataTable dt = new DataTable();
        delegate void dtSbagla(DataTable dts);
        string[] ports = SerialPort.GetPortNames();

        public Form3()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            serialPort1.BaudRate = 9600;

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //if (!serialPort1.IsOpen)
            //    serialPort1.Open();
            foreach(string port in ports)
            {
                comboBox1.Items.Add(port);
               // comboBox1.SelectedIndex = 0;
            }
            
        }
        private void datagrideDTbagla(DataTable dts)
        {
            

            if (dataGridView1.InvokeRequired)
            /*
             * form nesnesine başvuru aynı iş parçacığından mı
             * değilmi kontrolu yapılıyor.
             */
            {
                dtSbagla dtbag = new dtSbagla(datagrideDTbagla);
                //datagrideBagla adındaki metot için işaretçi oluşturulyor.
                dataGridView1.Invoke(dtbag, new object[] { dts });
                //nesne referansı üzerinden yeniden tetikleniyor
            }
            else
            {
                dataGridView1.DataSource = dts;
                dataGridView1.Refresh();
                /* 
                 * aynı iş parçacığından gelen başvurularda 
                 * doğrudan erişim sağlanıyor
                 */
            }
        }

        //static string ConvertStringArrayToStringJoin(string[] dizi)
        //{
        //    //
        //    // Use string Join to concatenate the string elements.
        //    //
        //    string result = string.Join("", dizi);
        //    return result;
        //}
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
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


                var result = string.Join("", sequence);//int dizisini stringe çeviriyorum.
                //  string dizi1 = ConvertStringArrayToStringJoin(dizi); 

                // textBox1.Text = rfIdkart;

                sCon.ConnectionString = "Data Source=BATUHANOZEN\\SQL_2014;Initial Catalog=personel;Integrated Security=True;";


                kayitGetir.CommandType = CommandType.StoredProcedure;
                kayitGetir.Parameters.Add("@rfId", SqlDbType.NVarChar).Value = result;

                kayitGetir.Parameters.Add("@sonuc", SqlDbType.Int).Direction = ParameterDirection.Output;
                kayitGetir.Connection = sCon;
                kayitGetir.CommandText = "rfIdSorgula";
                sCon.Open();
                kayitGetir.ExecuteNonQuery();


                int kayitVarmi = Convert.ToInt32(kayitGetir.Parameters["@sonuc"].Value);
                kayitGetir.Parameters.Clear();
                sCon.Close();
                if (kayitVarmi > 0)
                {


                    serialPort1.WriteLine("A");
                    hareket.Connection = sCon;
                    hareket.CommandText = "select personel.personelAd,personel.personelSoyad,bolum,hareket.tarihSaat from hareket,personel where hareket.personelId=personel.personelId";

                    dta.SelectCommand = hareket;
                    dt.Clear();

                    dta.Fill(dt);
                    //  dataGridView1.DataSource = dt;

                    new Thread(new ThreadStart(basla)).Start();

                }

                else
                {

                    serialPort1.WriteLine("K");
                   MessageBox.Show("Geçersiz Kart Algılandı");
                }

            }
            else
            {
                MessageBox.Show("Hatalı Veri");
                serialPort1.WriteLine("K");
            }
        }

        void basla()
                             {
                           datagrideDTbagla(dt);
                             }



      
        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            MessageBox.Show("Port Kapatıldı");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                if (comboBox1.Text == "")
                    return;
                serialPort1.PortName = comboBox1.Text;
                try
                {
                    serialPort1.Open();
                    MessageBox.Show("Port Açıldı");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
