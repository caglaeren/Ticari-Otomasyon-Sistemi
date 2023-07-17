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


namespace WindowsFormsApp1
{
    public partial class FrmAdmin : Form
    {
        public FrmAdmin()
        {
            InitializeComponent();
        }


        SqlBaglantisi bgl = new SqlBaglantisi();
        private void button1_MouseHover(object sender, EventArgs e)
        {
            //fareyle butonun üstüne gelince ne olsun ayarlaması 
            BtnGiris.BackColor = Color.BlueViolet;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            //fareyle üstünden ayrılınca
            BtnGiris.BackColor = Color.FromArgb(222, 226, 255);
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select * From TBL_ADMIN Where KULLANICIADI=@P1 AND SIFRE=@P2",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtKullaniciAdi.Text);
            komut.Parameters.AddWithValue("@p2", TxtSifre.Text);
            SqlDataReader dr = komut.ExecuteReader();
            //eger dogru sekilde okuma yapıyorsa
            if(dr.Read())
            {
                //ana modülü aç ve admin panelini gizle
                FrmAnaModul fr = new FrmAnaModul();
                //eğerki ad değerinde giriş yapma işlemi doğru gerçekleşirse (FrmKasalarda yazılan kodda)
                fr.kullanici = TxtKullaniciAdi.Text;

                fr.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Kullanıcı adınızı ya da şifrenizi hatalı girdiniz.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            bgl.baglanti().Close();


        }
    }
}
