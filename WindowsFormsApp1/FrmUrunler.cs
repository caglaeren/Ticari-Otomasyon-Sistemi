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
    public partial class FrmUrunler : Form
    {
        public FrmUrunler()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi(); //SqlBaglantisi sınıfından nesne türettik
        void listele()
        {
            DataTable dt = new DataTable();
            //Ürünler tablosundan çektik ve baglanti adresi ile ilişkilendirdik
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_URUNLER ORDER BY ID ASC", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt; //dt'den gelen deger olur
        }

        void temizle()
        {
            TxtId.Text = "";
            TxtAd.Text = "";
            TxtMarka.Text = "";
            TxtModel.Text = "";
            MskYil.Text = "";
            NudAdet.Value = 0;
            TxtAlisFiyat.Text = "";
            TxtSatisFiyat.Text = "";
            RchDetay.Text = "";

        }
        private void FrmUrunler_Load(object sender, EventArgs e)
        {
            //From yüklendiğinde de listele metodumuzu cagırdık
            listele();
            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //Verileri kaydediyoruz:
            SqlCommand komut = new SqlCommand("insert into TBL_URUNLER (URUNAD,MARKA,MODEL,YIL,ADET,ALISFIYAT,SATISFIYAT,DETAY) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtMarka.Text);
            komut.Parameters.AddWithValue("@p3", TxtModel.Text);
            komut.Parameters.AddWithValue("@p4", MskYil.Text);
            komut.Parameters.AddWithValue("@p5", int.Parse((NudAdet.Value).ToString()));
            komut.Parameters.AddWithValue("@p6", decimal.Parse(TxtAlisFiyat.Text));
            komut.Parameters.AddWithValue("@p7", decimal.Parse(TxtSatisFiyat.Text));
            komut.Parameters.AddWithValue("@p8", RchDetay.Text);
            komut.ExecuteNonQuery(); //DML komutlarını gerceklestirir yani sorguyu calıstırır
            bgl.baglanti().Close();
            //Birinci parametre mesajımız, ikincisi messagebox'ın başlığı, üçüncüsü buton, dödrüncüsü icon
            MessageBox.Show("Ürün sisteme eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele(); //listele metodu ile verilerimizi listeleyelim
            temizle();

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            //silmeyi id'ye göre yapacağız
            //bu parametreyi bgl.baglantidan alacak
            SqlCommand komutsil = new SqlCommand("Delete From TBL_URUNLER Where ID=@p1", bgl.baglanti());
            komutsil.Parameters.AddWithValue("@p1", TxtId.Text);
            komutsil.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Ürün silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            listele();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //DataRow bizim veri satırımızdır. dr komutuna bir görev atayacağız.
            //GetDataRow ile satırın verisini alacağız
            //metodun içinde satırın indeksini alacağız aslında
            //gridView'daki imleçle tıklanan FocusedRowHandle'ı aldık
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            TxtId.Text = dr["ID"].ToString();
            TxtAd.Text = dr["URUNAD"].ToString();
            TxtMarka.Text = dr["MARKA"].ToString();
            TxtModel.Text = dr["MODEL"].ToString();
            MskYil.Text = dr["YIL"].ToString();
            NudAdet.Value = int.Parse(dr["ADET"].ToString());
            TxtAlisFiyat.Text = dr["ALISFIYAT"].ToString();
            TxtSatisFiyat.Text = dr["SATISFIYAT"].ToString();
            RchDetay.Text = dr["DETAY"].ToString();
            TxtModel.Text = dr["MODEL"].ToString();

        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            //id'ye göre güncelleyeceğiz
            SqlCommand komutguncelle = new SqlCommand("Update TBL_URUNLER Set URUNAD=@P1, MARKA=@P2, MODEL=@P3, YIL=@P4, ADET=@P5, ALISFIYAT=@P6, SATISFIYAT=@P7, DETAY=@P8 Where ID=@P9", bgl.baglanti());
            komutguncelle.Parameters.AddWithValue("@p1", TxtAd.Text);
            komutguncelle.Parameters.AddWithValue("@p2", TxtMarka.Text);
            komutguncelle.Parameters.AddWithValue("@p3", TxtModel.Text);
            komutguncelle.Parameters.AddWithValue("@p4", MskYil.Text);
            komutguncelle.Parameters.AddWithValue("@p5", int.Parse((NudAdet.Value).ToString()));
            komutguncelle.Parameters.AddWithValue("@p6", decimal.Parse(TxtAlisFiyat.Text));
            komutguncelle.Parameters.AddWithValue("@p7", decimal.Parse(TxtSatisFiyat.Text));
            komutguncelle.Parameters.AddWithValue("@p8", RchDetay.Text);
            komutguncelle.Parameters.AddWithValue("@p9", TxtId.Text);
            komutguncelle.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Ürün bilgisi güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            listele();
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }
    }
}
