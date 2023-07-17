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
    public partial class FrmFaturaUrunDuzenleme : Form
    {
        public FrmFaturaUrunDuzenleme()
        {
            InitializeComponent();
        }

        public string urunid;
        SqlBaglantisi bgl = new SqlBaglantisi();
        private void FrmFaturaUrunDuzenleme_Load(object sender, EventArgs e)
        {
            TxtUrunId.Text = urunid;

            SqlCommand komut = new SqlCommand("Select * From TBL_FATURADETAY Where FATURAURUNID=@P1",bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", urunid);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                TxtUrunAdi.Text = dr[1].ToString();
                TxtMiktar.Text = dr[2].ToString();
                TxtFiyat.Text = dr[3].ToString();
                TxtTutar.Text = dr[4].ToString();

                bgl.baglanti().Close();
            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncellekomut = new SqlCommand("Update TBL_FATURADETAY Set URUNAD=@P1,MIKTAR=@P2,FIYAT=@P3,TUTAR=@P4 Where FATURAURUNID=@P5", bgl.baglanti());
            guncellekomut.Parameters.AddWithValue("@p1", TxtUrunAdi.Text);
            guncellekomut.Parameters.AddWithValue("@p2", TxtMiktar.Text);
            guncellekomut.Parameters.AddWithValue("@p3", decimal.Parse(TxtFiyat.Text));
            guncellekomut.Parameters.AddWithValue("@p4", decimal.Parse(TxtTutar.Text));
            guncellekomut.Parameters.AddWithValue("@p5", TxtUrunId.Text);
            guncellekomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Yapılan değişiklikler kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            SqlCommand silkomut = new SqlCommand("Delete From TBL_FATURADETAY Where FATURAURUNID=@P1", bgl.baglanti());
            silkomut.Parameters.AddWithValue("@p1", TxtUrunId.Text);
            silkomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Ürün silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
