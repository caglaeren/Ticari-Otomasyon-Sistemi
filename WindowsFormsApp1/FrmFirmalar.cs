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
    public partial class FrmFirmalar : Form
    {
        public FrmFirmalar()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void firmalistesi()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_FIRMALAR", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;

        }

        void sehirListesi()
        {
            SqlCommand komut = new SqlCommand("Select SEHIR from TBL_ILLER", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            //okuma işlemi sürdüğü müddetçe:
            while (dr.Read())
            {
                //dr'den gelen 0. indeks
                Cmbil.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        void carikodaciklamalari()
        {
            SqlCommand komut = new SqlCommand("Select FIRMAKOD1 From TBL_KODLAR", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                //RchKod1.Text = dr[0].ToString();
            }
            bgl.baglanti().Close();
        }
        void temizle()
        {
            TxtAd.Text = "";
            TxtId.Text = "";
            
            TxtMail.Text = "";
            TxtSektor.Text = "";
            TxtVergidairesi.Text = "";
            TxtYetkili.Text = "";
            TxtYetkiliGorev.Text = "";
            MskFax.Text = "";
            MskTelefon1.Text = "";
            MskTelefon2.Text = "";
            MskTelefon3.Text = "";
            MskYetkiliTC.Text = "";
            RchAdres.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            TxtAd.Focus(); //imleci focuslatıyoruz

        }


        private void FrmFirmalar_Load(object sender, EventArgs e)
        {
            firmalistesi();

            sehirListesi();

//            carikodaciklamalari();

            temizle();
        }

        
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtId.Text = dr["ID"].ToString();
                TxtAd.Text = dr["AD"].ToString();
                TxtYetkiliGorev.Text = dr["YETKILISTATU"].ToString();
                TxtYetkili.Text = dr["YETKILIADSOYAD"].ToString();
                MskYetkiliTC.Text = dr["YETKILITC"].ToString();
                TxtSektor.Text = dr["SEKTOR"].ToString();
                MskTelefon1.Text = dr["TELEFON1"].ToString();
                MskTelefon2.Text = dr["TELEFON2"].ToString();
                MskTelefon3.Text = dr["TELEFON3"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                MskFax.Text = dr["FAX"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                TxtVergidairesi.Text = dr["VERGIDAIRE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
                //TxtKod1.Text = dr["OZELKOD1"].ToString();
                //TxtKod2.Text = dr["OZELKOD2"].ToString();
                //TxtKod3.Text = dr["OZELKOD3"].ToString();
 
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Insert into TBL_FIRMALAR (AD, YETKILISTATU,YETKILIADSOYAD,YETKILITC,SEKTOR,TELEFON1,TELEFON2,TELEFON3," +
                "MAIL,FAX,IL,ILCE,VERGIDAIRE,ADRES,OZELKOD1,OZELKOD2,OZELKOD3) Values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17)", bgl.baglanti());
            komut.Parameters.AddWithValue("@P1", TxtAd.Text);
            komut.Parameters.AddWithValue("@P2", TxtYetkiliGorev.Text);
            komut.Parameters.AddWithValue("@P3", TxtYetkili.Text);
            komut.Parameters.AddWithValue("@P4", MskYetkiliTC.Text);
            komut.Parameters.AddWithValue("@P5", TxtSektor.Text);
            komut.Parameters.AddWithValue("@P6", MskTelefon1.Text);
            komut.Parameters.AddWithValue("@P7", MskTelefon2.Text);
            komut.Parameters.AddWithValue("@P8", MskTelefon3.Text);
            komut.Parameters.AddWithValue("@P9", TxtMail.Text);
            komut.Parameters.AddWithValue("@P10", MskFax.Text);
            komut.Parameters.AddWithValue("@P11", Cmbil.Text);
            komut.Parameters.AddWithValue("@P12", Cmbilce.Text);
            komut.Parameters.AddWithValue("@P13", TxtVergidairesi.Text);
            komut.Parameters.AddWithValue("@P14", RchAdres.Text);
            //komut.Parameters.AddWithValue("@P15", TxtKod1.Text);
            //komut.Parameters.AddWithValue("@P16", TxtKod2.Text);
            //komut.Parameters.AddWithValue("@P17", TxtKod3.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Firma Sisteme Kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            firmalistesi();
            temizle(); //kaydettikten sonra da temizlemiş olur
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            //İlçe seçilince daha önce seçilmiş olan ilçeleri temizlemek istersek de:
            Cmbilce.Properties.Items.Clear();
            SqlCommand komut = new SqlCommand("Select ILCE From TBL_ILCELER Where SEHIR = @p1", bgl.baglanti());
            //Cmbil'in seçilen indeksinden ilçenin gelmesini ayarlamak istedik
            komut.Parameters.AddWithValue("@p1", Cmbil.SelectedIndex + 1);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                //Cmbilce'nin içerisine ekleme yapacak
                //0. indeks bizim ilce adımızdır
                Cmbilce.Properties.Items.Add(dr[0]);
            }
            bgl.baglanti().Close();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Firma kaydınızı sileceksiniz. Silmek istediğinizden emin misiniz?", "Firma Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand silkomut = new SqlCommand("Delete From TBL_FIRMALAR Where ID=@p1", bgl.baglanti());
                silkomut.Parameters.AddWithValue("@p1", TxtId.Text);
                //execute non query, insert,delete ve update komutlarında calısır
                silkomut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Firma listeden silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                firmalistesi();
                temizle();
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Update TBL_FIRMALAR Set AD=@P1, YETKILISTATU=@P2, YETKILIADSOYAD=@P3, YETKILITC=@P4, SEKTOR=@P5," +
                " TELEFON1=@P6, TELEFON2=@P7, TELEFON3=@P8, MAIL=@P9, FAX=@P10, IL=@P11, ILCE=@P12, VERGIDAIRE=@P13, ADRES=@P14," +
                " OZELKOD1=@P15, OZELKOD2=@P16, OZELKOD3=@P17  Where ID=@P18 ", bgl.baglanti());


            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtYetkiliGorev.Text);
            komut.Parameters.AddWithValue("@p3", TxtYetkili.Text);
            komut.Parameters.AddWithValue("@p4", MskYetkiliTC.Text);
            komut.Parameters.AddWithValue("@p5", TxtSektor.Text);
            komut.Parameters.AddWithValue("@p6", MskTelefon1.Text);
            komut.Parameters.AddWithValue("@p7", MskTelefon2.Text);
            komut.Parameters.AddWithValue("@p8", MskTelefon3.Text);
            komut.Parameters.AddWithValue("@p9", TxtMail.Text);
            komut.Parameters.AddWithValue("@p10", MskFax.Text);
            komut.Parameters.AddWithValue("@p11", Cmbil.Text);
            komut.Parameters.AddWithValue("@p12", Cmbilce.Text);
            komut.Parameters.AddWithValue("@p13", TxtVergidairesi.Text);
            komut.Parameters.AddWithValue("@p14", RchAdres.Text);
            //komut.Parameters.AddWithValue("@p15", TxtKod1.Text);
            //komut.Parameters.AddWithValue("@p16", TxtKod2.Text);
            //komut.Parameters.AddWithValue("@p17", TxtKod3.Text);
            komut.Parameters.AddWithValue("@p18", TxtId.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Firma bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            firmalistesi();
            temizle();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        
    }
}
