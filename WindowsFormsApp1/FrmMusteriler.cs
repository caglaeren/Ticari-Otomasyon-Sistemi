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
    public partial class FrmMusteriler : Form
    {
        public FrmMusteriler()
        {
            InitializeComponent();
        }

        SqlBaglantisi bagl = new SqlBaglantisi();

        void musterilistele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_MUSTERILER Order By ID ASC", bagl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void sehirListesi()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR from TBL_ILLER", bagl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            //okuma işlemi sürdüğü müddetçe:
            while(dr.Read())
            {
                //dr'den gelen 0. indeks
                Cmbil.Properties.Items.Add(dr[0]);
            }
            bagl.baglanti().Close();
        }

        void temizle()
        {
            TxtId.Text = "";
            TxtAd.Text = "";
            TxtMail.Text = "";
            TxtSoyad.Text = "";
            TxtVergidairesi.Text = "";
            MskTC.Text = "";
            MskTelefon.Text = "";
            MskTelefon2.Text = "";
            RchAdres.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
        }
        private void FrmMusteriler_Load(object sender, EventArgs e)
        {
            musterilistele();

            sehirListesi();

            temizle();
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            //İlçe seçilince daha önce seçilmiş olan ilçeleri temizlemek istersek de:
            Cmbilce.Properties.Items.Clear();
            SqlCommand komut = new SqlCommand("Select ILCE From TBL_ILCELER Where SEHIR = @p1", bagl.baglanti());
            //Cmbil'in seçilen indeksinden ilçenin gelmesini ayarlamak istedik
            //Comboboxdaki indeks değeri 0 dan baslıyor. ama sqldeki sehir id'imiz 1 den baslıyor bu yüzden 1 arttırdık
            //1 arttırınca dogru ilin dogru sekilde ilçelerini verdi
            komut.Parameters.AddWithValue("@p1", Cmbil.SelectedIndex+1);
            //executereader select komutlarında calısır
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                //Cmbilce'nin içerisine ekleme yapacak
                //0. indeks bizim ilce adımızdır
                Cmbilce.Properties.Items.Add(dr[0]);
            }
            bagl.baglanti().Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Insert into TBL_MUSTERILER (AD, SOYAD, TELEFON, TELEFON2, TC, MAIL, IL, ILCE, ADRES, VERGIDAIRE)" +
                "VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9, @p10)", bagl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", MskTelefon.Text);
            komut.Parameters.AddWithValue("@p4", MskTelefon2.Text);
            komut.Parameters.AddWithValue("@p5", MskTC.Text);
            komut.Parameters.AddWithValue("@p6", TxtMail.Text);
            komut.Parameters.AddWithValue("@p7", Cmbil.Text);
            komut.Parameters.AddWithValue("@p8", Cmbilce.Text);
            komut.Parameters.AddWithValue("@p9", RchAdres.Text);
            komut.Parameters.AddWithValue("@p10", TxtVergidairesi.Text);
            komut.ExecuteNonQuery();
            bagl.baglanti().Close();
            MessageBox.Show("Müşteri eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            musterilistele();



        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr!=null)
            {
                TxtId.Text = dr["ID"].ToString();
                TxtAd.Text = dr["AD"].ToString();
                TxtSoyad.Text = dr["SOYAD"].ToString();
                MskTelefon.Text = dr["TELEFON"].ToString();
                MskTelefon2.Text = dr["TELEFON2"].ToString();
                MskTC.Text = dr["TC"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                TxtVergidairesi.Text = dr["VERGIDAIRE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
               
                
            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
          
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Müşteri Kaydınız Sileceksiniz. Silmek İstediğinizden Emin Misiniz?", "Müşteri Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand silkomut = new SqlCommand("Delete From TBL_MUSTERILER Where ID=@p1", bagl.baglanti());
                silkomut.Parameters.AddWithValue("@p1", TxtId.Text);
                //execute non query, insert,delete ve update komutlarında calısır
                silkomut.ExecuteNonQuery();
                bagl.baglanti().Close();
                MessageBox.Show("Müşteri silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                musterilistele();
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncellekomut = new SqlCommand("Update TBL_MUSTERILER Set AD=@P1, SOYAD=@P2, TELEFON=@P3, TELEFON2=@P4, TC=@P5," +
                " MAIL=@P6, IL=@P7, ILCE=@P8, ADRES=@P9, VERGIDAIRE=@P10  Where ID=@P11 ", bagl.baglanti());
            guncellekomut.Parameters.AddWithValue("@p1", TxtAd.Text);
            guncellekomut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            guncellekomut.Parameters.AddWithValue("@p3", MskTelefon.Text);
            guncellekomut.Parameters.AddWithValue("@p4", MskTelefon2.Text);
            guncellekomut.Parameters.AddWithValue("@p5", MskTC.Text);
            guncellekomut.Parameters.AddWithValue("@p6", TxtMail.Text);
            guncellekomut.Parameters.AddWithValue("@p7", Cmbil.Text);
            guncellekomut.Parameters.AddWithValue("@p8", Cmbilce.Text);
            guncellekomut.Parameters.AddWithValue("@p9", RchAdres.Text);
            guncellekomut.Parameters.AddWithValue("@p10", TxtVergidairesi.Text);
            guncellekomut.Parameters.AddWithValue("@p11", TxtId.Text);
            guncellekomut.ExecuteNonQuery();
            bagl.baglanti().Close();
            MessageBox.Show("Müşteri bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            musterilistele();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }
    }
}
