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
using DevExpress.Charts;

namespace WindowsFormsApp1
{
    public partial class FrmKasa : Form
    {
        public FrmKasa()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void musterihareketlistele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Exec pr_MusteriHareketleri", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void firmahareketlistele()
        {
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("Exec pr_FirmaHareketleri", bgl.baglanti());
            da2.Fill(dt2);
            gridControl3.DataSource = dt2;
        }

        void giderlistele()
        {
            DataTable dt3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter("Select * from TBL_GIDERLER", bgl.baglanti());
            da3.Fill(dt3);
            gridControl2.DataSource = dt3;
        }

        void kasagirishareketleri()
        {
            // Toplam tutarı hesaplama
            SqlCommand komut1 = new SqlCommand("Select Sum(TUTAR) From TBL_FATURADETAY", bgl.baglanti());
            SqlDataReader dr1 = komut1.ExecuteReader();
            while (dr1.Read())
            {
                LblKasaToplam.Text = dr1[0].ToString() + " ₺ (TL)";
            }
            bgl.baglanti().Close();

            // Son ayın faturaları yani ödemeleri
            SqlCommand komut2 = new SqlCommand("Select (ELEKTRIK+SU+DOGALGAZ+INTERNET+EKSTRA) from TBL_GIDERLER Order By ID ASC", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                LblOdemeler.Text = dr2[0].ToString() + " ₺";
            }
            bgl.baglanti().Close();

            // Son ayın personel maaşları (en sonki maaş değerini verir)
            SqlCommand komut3 = new SqlCommand("Select MAASLAR from TBL_GIDERLER Order By ID ASC", bgl.baglanti());
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                LblPersonalMaaslari.Text = dr3[0].ToString() + " ₺";
            }
            bgl.baglanti().Close();

            // Müşteri sayısı (toplamda kaç müşterimiz var)
            SqlCommand komut4 = new SqlCommand("Select Count(*) from TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr4 = komut4.ExecuteReader();
            while (dr4.Read())
            {
                LblMusteriSayisi.Text = dr4[0].ToString();
            }
            bgl.baglanti().Close();


            // Firmaların toplam sayısı
            SqlCommand komut5 = new SqlCommand("Select Count(*) from TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr5 = komut5.ExecuteReader();
            while (dr5.Read())
            {
                LblFirmaSayisi.Text = dr5[0].ToString();
            }
            bgl.baglanti().Close();


            // Firmalarda bulunan toplam şehir sayısı
            //distinct ile şehirleri tekrarsız saydık
            SqlCommand komut6 = new SqlCommand("Select Count(Distinct(IL)) from TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr6 = komut6.ExecuteReader();
            while (dr6.Read())
            {
                LblSehirSayisi.Text = dr6[0].ToString();
            }
            bgl.baglanti().Close();

            // Müşterilerde bulunan toplam şehir sayısı
            SqlCommand komut7 = new SqlCommand("Select Count(Distinct(IL)) from TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr7 = komut7.ExecuteReader();
            while (dr7.Read())
            {
                LblSehirSayisi2.Text = dr7[0].ToString();
            }
            bgl.baglanti().Close();

            // Personel sayısı (tablodaki bütün kayıtları alır, hesaplar ve sayar)
            SqlCommand komut8 = new SqlCommand("Select Count(*) from TBL_PERSONELLER", bgl.baglanti());
            SqlDataReader dr8 = komut8.ExecuteReader();
            while (dr8.Read())
            {
                LblPersonelSayisi.Text = dr8[0].ToString();
            }
            bgl.baglanti().Close();

            // Toplam ürün sayısı
            SqlCommand komut9 = new SqlCommand("Select Sum(ADET) from TBL_URUNLER", bgl.baglanti());
            SqlDataReader dr9 = komut9.ExecuteReader();
            while (dr9.Read())
            {
                LblStokSayisi.Text = dr9[0].ToString();
            }
            bgl.baglanti().Close();

            // Aktif Kullanıcı
            // Ana modulu köprü olarak kullandık
            //ad değişkeninden gelecek olan değeri, aktifKullanıcı textine yazacak
            LblAktifKullanici.Text = ad;
        }

        public string ad;
        private void FrmKasa_Load(object sender, EventArgs e)
        {
            musterihareketlistele();
            firmahareketlistele();
            giderlistele();

            kasagirishareketleri();

        }


       // -----------------------------CHART CONTROL 1 İÇİN--------------------------------
        int sayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer basladıgında sayac bir bir artsın
            sayac++;

            //1. Chart Controle Elektrik faturası son 4 ay için listeleyelim
            if (sayac > 0 && sayac <= 5)
            {
                groupControl11.Text = "Son 4 Ayın Elektrik Faturası";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut10 = new SqlCommand("SELECT TOP 4 AY,ELEKTRIK FROM TBL_GIDERLER ORDER BY ID DESC", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    //series içinde yeni bir konum alanı tanımladık
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }

            //1. chart controle Su faturasının son 4 ayını listeleyelim
            if (sayac > 5 && sayac <= 10)
            {
                groupControl11.Text = "Son 4 Ayın Su Faturası";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut10 = new SqlCommand("Select Top 4 AY,SU from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }

            // 1.Chart controle son 4 ayın Doğalgaz faturasını listeleme
            if (sayac > 10 && sayac <= 15)
            {
                groupControl11.Text = "Son 4 Ayın Doğalgaz Faturası";
                chartControl1.Series["Aylar"].Points.Clear();

                SqlCommand komut10 = new SqlCommand("Select Top 4 AY,DOGALGAZ from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }

            // 1.Chart controle son 4 ayın Internet faturasını listeleme
            if (sayac > 15 && sayac <= 20)
            {
                groupControl11.Text = "Son 4 Ayın İnternet Faturası";
                chartControl1.Series["Aylar"].Points.Clear();

                SqlCommand komut10 = new SqlCommand("Select Top 4 AY,INTERNET from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }

            // 1.Chart controle son 4 ayın Ekstra giderlerini listeleme
            if (sayac > 20 && sayac <= 25)
            {
                groupControl11.Text = "Son 4 Ayın Ekstra Giderleri";
                chartControl1.Series["Aylar"].Points.Clear();

                SqlCommand komut10 = new SqlCommand("Select Top 4 AY,EKSTRA from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }

            // Sayaç 26 olduğunda sayacı sıfırlayıp döngüye alan kod
            if (sayac == 26)
            {
                sayac = 0;
            }
        }


        //--------------------------------CHART CONTROL 2 İÇİN-----------------------------------------

        int sayac2 = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac2++;

            // 2.Chart controle son 6 ayın Elektrik faturasını listeleme
            if (sayac2 >= 0 && sayac2 <= 5)
            {

                groupControl12.Text = "Son 6 Ayın Elektrik Faturası";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut11 = new SqlCommand("Select Top 6 AY,ELEKTRIK from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }

            // 2.Chart controle son 6 ayın Su faturasını listeleme
            if (sayac2 > 5 && sayac2 <= 10)
            {
                groupControl12.Text = "Son 6 Ayın Su Faturası";
                chartControl2.Series["Aylar"].Points.Clear();

                SqlCommand komut11 = new SqlCommand("Select Top 6 AY,SU from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }

            // 2.Chart controle son 6 ayın Doğalgaz faturasını listeleme
            if (sayac2 > 10 && sayac2 <= 15)
            {
                groupControl12.Text = "Son 6 Ayın Doğalgaz Faturası";
                chartControl2.Series["Aylar"].Points.Clear();

                SqlCommand komut11 = new SqlCommand("Select Top 6 AY,DOGALGAZ from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }

            // 2.Chart controle son 6 ayın Internet faturasını listeleme
            if (sayac2 > 15 && sayac2 <= 20)
            {
                groupControl12.Text = "Son 6 ayın Internet Faturası";
                chartControl2.Series["Aylar"].Points.Clear();

                SqlCommand komut11 = new SqlCommand("Select Top 6 AY,INTERNET from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }

            // 2.Chart controle son 6 ayın Ekstra giderlerini listeleme
            if (sayac2 > 20 && sayac2 <= 25)
            {
                groupControl12.Text = "Son 6 Ayın Ekstra Giderleri";
                chartControl2.Series["Aylar"].Points.Clear();

                SqlCommand komut11 = new SqlCommand("Select Top 6 AY,EKSTRA from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
                SqlConnection.ClearPool(bgl.baglanti());
            }

            // Sayaç 26 olduğunda sayacı sıfırlayıp döngüye alan kod
            if (sayac2 == 26)
            {
                sayac2 = 0;
            }
        }
    }
}
