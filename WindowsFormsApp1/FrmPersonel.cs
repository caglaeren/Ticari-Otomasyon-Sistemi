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
    public partial class FrmPersonel : Form
    {
        public FrmPersonel()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void personellistesi()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_PERSONELLER  Order By ID ASC", bgl.baglanti());
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


        void temizle()
        {
            TxtId.Text = "";
            TxtAd.Text = "";
            TxtSoyad.Text = "";
            MskTelefon.Text = "";
            MskTC.Text = "";
            TxtMail.Text = "";
            Cmbil.Text = "";
            Cmbilce.Text = "";
            RchAdres.Text = "";
            TxtGorev.Text = "";
            TxtAd.Focus(); //imleci focuslatıyoruz

        }
        private void FrmPersonel_Load(object sender, EventArgs e)
        {
            personellistesi();

            sehirListesi();

            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Insert Into TBL_PERSONELLER (AD,SOYAD,TELEFON,TC,MAIL,IL,ILCE,ADRES,GOREV) " +
                "Values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)", bgl.baglanti());
            komut.Parameters.AddWithValue("@P1", TxtAd.Text);
            komut.Parameters.AddWithValue("@P2", TxtSoyad.Text );
            komut.Parameters.AddWithValue("@P3", MskTelefon.Text );
            komut.Parameters.AddWithValue("@P4", MskTC.Text );
            komut.Parameters.AddWithValue("@P5", TxtMail.Text );
            komut.Parameters.AddWithValue("@P6", Cmbil.Text );
            komut.Parameters.AddWithValue("@P7", Cmbilce.Text );
            komut.Parameters.AddWithValue("@P8", RchAdres.Text );
            komut.Parameters.AddWithValue("@P9", TxtGorev.Text );
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Personel bilgileri sisteme kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            personellistesi();
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {

            //İlçe seçilince daha önce seçilmiş olan ilçeleri temizlemek istersek de:
            Cmbilce.Properties.Items.Clear();
            SqlCommand komut = new SqlCommand("Select ILCE From TBL_ILCELER Where SEHIR = @p1", bgl.baglanti());
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

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if(dr!=null)
            {
                TxtId.Text = dr["ID"].ToString();
                TxtAd.Text = dr["AD"].ToString();
                TxtSoyad.Text = dr["SOYAD"].ToString();
                MskTelefon.Text = dr["TELEFON"].ToString();
                MskTC.Text = dr["TC"].ToString();
                TxtMail.Text = dr["MAIL"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                RchAdres.Text = dr["ADRES"].ToString();
                TxtGorev.Text = dr["GOREV"].ToString();

            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Personel kaydınızı sileceksiniz. Silmek istediğinizden emin misiniz?", "Personel Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand silkomut = new SqlCommand("Delete From TBL_PERSONELLER Where ID=@p1", bgl.baglanti());
                silkomut.Parameters.AddWithValue("@p1", TxtId.Text);
                //execute non query, insert,delete ve update komutlarında calısır
                silkomut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Personel listeden silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                personellistesi();
                temizle();
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncellekomut = new SqlCommand("Update TBL_PERSONELLER set AD=@P1, SOYAD=@P2, TELEFON=@P3, TC=@P4, MAIL=@P5, IL=@P6, ILCE=@P7, ADRES=@P8, GOREV=@P9 WHERE ID=@P10", bgl.baglanti());
            guncellekomut.Parameters.AddWithValue("@P1", TxtAd.Text);
            guncellekomut.Parameters.AddWithValue("@P2", TxtSoyad.Text);
            guncellekomut.Parameters.AddWithValue("@P3", MskTelefon.Text);
            guncellekomut.Parameters.AddWithValue("@P4", MskTC.Text);
            guncellekomut.Parameters.AddWithValue("@P5", TxtMail.Text);
            guncellekomut.Parameters.AddWithValue("@P6", Cmbil.Text);
            guncellekomut.Parameters.AddWithValue("@P7", Cmbilce.Text);
            guncellekomut.Parameters.AddWithValue("@P8", RchAdres.Text);
            guncellekomut.Parameters.AddWithValue("@P9", TxtGorev.Text);
            guncellekomut.Parameters.AddWithValue("@P10", TxtId.Text);
            guncellekomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Personel bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            personellistesi();
            temizle();

        }
    }
}
