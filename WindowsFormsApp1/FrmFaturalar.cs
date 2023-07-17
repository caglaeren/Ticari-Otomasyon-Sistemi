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
    public partial class FrmFaturalar : Form
    {
        public FrmFaturalar()
        {
            InitializeComponent();
        }


        SqlBaglantisi bgl = new SqlBaglantisi();

        void faturalistele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_FATURABILGI Order By FATURABILGIID asc", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            TxtId.Text = "";
            TxtSeri.Text = "";
            TxtSiraNo.Text = "";
            Msktarih.Text = "";
            MskSaat.Text = "";
            TxtVeriDairesi.Text = "";
            TxtAlici.Text = "";
            TxtTeslimAlan.Text = "";
            TxtTeslimEden.Text = "";
            //cmbCariTuru.Text = "";
        }
        private void FrmFaturalar_Load(object sender, EventArgs e)
        {
            faturalistele();
            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            //eğer ki Fatura ID kısmı boşsa demektir
            if (TxtFaturaId.Text == "")
            {
                SqlCommand komut = new SqlCommand("Insert into TBL_FATURABILGI (SERI,SIRANO,TARIH,SAAT,VERGIDAIRE,ALICI,TESLIMEDEN,TESLIMALAN)" +
                    "VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8)", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", TxtSeri.Text);
                komut.Parameters.AddWithValue("@p2", TxtSiraNo.Text);
                komut.Parameters.AddWithValue("@p3", Msktarih.Text);
                komut.Parameters.AddWithValue("@p4", MskSaat.Text);
                komut.Parameters.AddWithValue("@p5", TxtVeriDairesi.Text);
                komut.Parameters.AddWithValue("@p6", TxtAlici.Text);
                komut.Parameters.AddWithValue("@p7", TxtTeslimEden.Text);
                komut.Parameters.AddWithValue("@p8", TxtTeslimAlan.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Fatura bilgileri kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                faturalistele();
                temizle();

            }

            //Firma carisi
            if(TxtFaturaId.Text!="" && cmbCariTuru.Text=="Firma")
            {
                double miktar, tutar, fiyat;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = miktar * fiyat;
                TxtTutar.Text = tutar.ToString();

                SqlCommand komut2 = new SqlCommand("Insert Into TBL_FATURADETAY (URUNAD,MIKTAR,FIYAT,TUTAR,FATURAID)" +
                    "VALUES (@P1,@P2,@P3,@P4,@P5)", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1", TxtUrunAdi.Text);
                komut2.Parameters.AddWithValue("@p2",TxtMiktar.Text);
                komut2.Parameters.AddWithValue("@p3",decimal.Parse(TxtFiyat.Text));
                komut2.Parameters.AddWithValue("@p4",decimal.Parse(TxtTutar.Text));
                komut2.Parameters.AddWithValue("@p5",TxtFaturaId.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();


                //Hareket tablosuna veri girişi
                SqlCommand komut3 = new SqlCommand("insert into TBL_FIRMAHAREKETLER (URUNID,ADET,SATICIPERSONEL,FIRMAALICI,FIYAT,TOPLAM,FATURAID,TARIH) values(@H1,@H2,@H3,@H4,@H5,@H6,@H7,@H8)", bgl.baglanti());
                komut3.Parameters.AddWithValue("@H1", TxtUrunId.Text);
                komut3.Parameters.AddWithValue("@H2", TxtMiktar.Text);
                komut3.Parameters.AddWithValue("@H3", TxtPersonel.Text);
                komut3.Parameters.AddWithValue("@H4", TxtFirma.Text);
                komut3.Parameters.AddWithValue("@H5", decimal.Parse(TxtFiyat.Text));
                komut3.Parameters.AddWithValue("@H6", decimal.Parse(TxtTutar.Text));
                komut3.Parameters.AddWithValue("@H7", TxtFaturaId.Text);
                komut3.Parameters.AddWithValue("@H8", Msktarih.Text);
                komut3.ExecuteNonQuery();
                bgl.baglanti().Close();

                //Stok sayısı düşme -- güncelleme yaptık
                SqlCommand komut4 = new SqlCommand("Update TBL_URUNLER set ADET=ADET-@U1 where ID=@U2", bgl.baglanti());
                komut4.Parameters.AddWithValue("@U1", TxtMiktar.Text);
                komut4.Parameters.AddWithValue("@U2", TxtUrunId.Text);
                komut4.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Faturaya ait ürün bilgileri kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                faturalistele();
                temizle();
                
            }

            //--------------------------------------------------------------------------------------------------

            //Müşteri carisi
            
            if (TxtFaturaId.Text != "" && cmbCariTuru.Text =="Müşteri")
            {
                double miktar, tutar, fiyat;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = miktar * fiyat;
                TxtTutar.Text = tutar.ToString();

                SqlCommand komut2 = new SqlCommand("Insert Into TBL_FATURADETAY (URUNAD,MIKTAR,FIYAT,TUTAR,FATURAID)" +
                    "VALUES (@m1,@m2,@m3,@m4,@m5)", bgl.baglanti());
                komut2.Parameters.AddWithValue("@m1", TxtUrunAdi.Text);
                komut2.Parameters.AddWithValue("@m2", TxtMiktar.Text);
                komut2.Parameters.AddWithValue("@m3", decimal.Parse(TxtFiyat.Text));
                komut2.Parameters.AddWithValue("@m4", decimal.Parse(TxtTutar.Text));
                komut2.Parameters.AddWithValue("@m5", TxtFaturaId.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();


                //Hareket tablosuna veri girişi
                SqlCommand komut3 = new SqlCommand("insert into TBL_MUSTERIHAREKETLER (URUNID,ADET,SATICIPERSONEL,MUSTERIALICI,FIYAT,TOPLAM,FATURAID,TARIH) values(@a1,@a2,@a3,@a4,@a5,@a6,@a7,@a8)", bgl.baglanti());
                komut3.Parameters.AddWithValue("@a1", TxtUrunId.Text);
                komut3.Parameters.AddWithValue("@a2", TxtMiktar.Text);
                komut3.Parameters.AddWithValue("@a3", TxtPersonel.Text);
                komut3.Parameters.AddWithValue("@a4", TxtFirma.Text);
                komut3.Parameters.AddWithValue("@a5", decimal.Parse(TxtFiyat.Text));
                komut3.Parameters.AddWithValue("@a6", decimal.Parse(TxtTutar.Text));
                komut3.Parameters.AddWithValue("@a7", TxtFaturaId.Text);
                komut3.Parameters.AddWithValue("@a8", Msktarih.Text);
                komut3.ExecuteNonQuery();
                bgl.baglanti().Close();

                //Stok sayısı düşme -- güncelleme yaptık
                SqlCommand komut4 = new SqlCommand("Update TBL_URUNLER set ADET=ADET-@U1 where ID=@U2", bgl.baglanti());
                komut4.Parameters.AddWithValue("@U1", TxtMiktar.Text);
                komut4.Parameters.AddWithValue("@U2", TxtUrunId.Text);
                komut4.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Faturaya ait ürün bilgileri kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                faturalistele();
                temizle();
                
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                TxtId.Text = dr["FATURABILGIID"].ToString();
                TxtSeri.Text = dr["SERI"].ToString();
                TxtSiraNo.Text = dr["SIRANO"].ToString();
                Msktarih.Text = dr["TARIH"].ToString();
                MskSaat.Text = dr["SAAT"].ToString();
                TxtVeriDairesi.Text = dr["VERGIDAIRE"].ToString();
                TxtAlici.Text = dr["ALICI"].ToString();
                TxtTeslimEden.Text = dr["TESLIMEDEN"].ToString();
                TxtTeslimAlan.Text = dr["TESLIMALAN"].ToString();

            }
        }


        private void BtnTemizle_Click_1(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            //burada bir hata var 
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Fatura kaydınızı sileceksiniz. Silmek istediğinizden emin misiniz?", "Fatura Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                //bu hataya bakılmalı
                SqlCommand komut = new SqlCommand("Delete From TBL_FATURABILGI Where FATURABILGIID=@P1", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", TxtId.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Fatura listeden silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                faturalistele();
                temizle();
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Update TBL_FATURABILGI Set SERI=@P1,SIRANO=@P2,TARIH=@P3,SAAT=@P4,VERGIDAIRE=@P5,ALICI=@P6,TESLIMEDEN=@P7,TESLIMALAN=@P8" +
                " WHERE FATURABILGIID=@P9", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtSeri.Text);
            komut.Parameters.AddWithValue("@p2", TxtSiraNo.Text);
            komut.Parameters.AddWithValue("@p3", Msktarih.Text);
            komut.Parameters.AddWithValue("@p4", MskSaat.Text);
            komut.Parameters.AddWithValue("@p5", TxtVeriDairesi.Text);
            komut.Parameters.AddWithValue("@p6", TxtAlici.Text);
            komut.Parameters.AddWithValue("@p7", TxtTeslimEden.Text);
            komut.Parameters.AddWithValue("@p8", TxtTeslimAlan.Text);
            komut.Parameters.AddWithValue("@p9", TxtId.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Fatura bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            faturalistele();

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmFaturaUrunDetay fr = new FrmFaturaUrunDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr!=null)
            {
                fr.id = dr["FATURABILGIID"].ToString();
            }
            fr.Show();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Select URUNAD,SATISFIYAT from TBL_URUNLER where ID=@P1", bgl.baglanti());
            komut.Parameters.AddWithValue("@P1", TxtUrunId.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunAdi.Text = dr[0].ToString();
                TxtFiyat.Text = dr[1].ToString();
            }
            bgl.baglanti().Close();
        }
    }
}
