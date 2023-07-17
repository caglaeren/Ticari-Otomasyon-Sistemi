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
    public partial class FrmAnaSayfa : Form
    {
        public FrmAnaSayfa()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        //STOKLAR
        void Stoklar()
        {
            // Azalan Stoklar kısmına 20'den az stoklu ürünleri yazdırıyoruz.
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select URUNAD as 'Ürün Adı', SUM(ADET) as 'Adet' from TBL_URUNLER group by URUNAD Having Sum(ADET)<=20 order by sum(ADET)", bgl.baglanti());
            da.Fill(dt);
            gridControlStoklar.DataSource = dt;
        }

        //AJANDA
        void Ajanda()
        {
            // Ajanda kısmına son 12 toplantıyı yazdırıyoruz.
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("Select top 10 NOTTARIH as 'Not Tarihi' ,NOTSAAT as 'Not Saati',NOTBASLIK as 'Not Başlığı',NOTHITAP as 'Not Hitap',NOTDETAY as 'Not Detayı' " +
                "from TBL_NOTLAR order by NOTID desc", bgl.baglanti());
            da2.Fill(dt2);
            gridControlAjanda.DataSource = dt2;
        }

        void FirmaHareketleri()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Exec pr_FirmaHareketleri2", bgl.baglanti());
            da.Fill(dt);
            gridControlFirmaHareket.DataSource = dt;
        }

        void FirmaNotuListesi()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select AD as 'Firma Adı',TELEFON1 As 'Telefon 1',MAIL from TBL_FIRMALAR", bgl.baglanti());
            da.Fill(dt);
            gridControliNotListe.DataSource = dt;
        }

        //void MusteriNotListesi()
        //{
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter da = new SqlDataAdapter("Select (AD + ' ' + SOYAD) as 'AD SOYAD',TELEFON,MAIL from TBL_MUSTERILER", bgl.baglanti());
        //    da.Fill(dt);
        //    gridControlNotListe.DataSource = dt;
        //}
        private void FrmAnaSayfa_Load(object sender, EventArgs e)
        {
            Stoklar();
            Ajanda();
            FirmaHareketleri();
            //MusteriNotListesi();
            FirmaNotuListesi();

            webBrowserDovizKurlari.Navigate("https://tcmb.gov.tr/kurlar/today.xml");
        }
    }
}
