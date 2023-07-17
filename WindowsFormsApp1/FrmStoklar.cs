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
    public partial class FrmStoklar : Form
    {
        public FrmStoklar()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();
        private void FrmStoklar_Load(object sender, EventArgs e)
        {
           
            //Ürün niktarı listeleme
            SqlDataAdapter da = new SqlDataAdapter("Select URUNAD As 'Ürün Adı',Sum(ADET) As 'Adet' FROM TBL_URUNLER GROUP BY URUNAD",bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;

            //Chart'a Stok miktarını Listeliyoruz
            SqlCommand komut = new SqlCommand("Select URUNAD As 'Ürün Adı',Sum(ADET) As 'Adet' FROM TBL_URUNLER GROUP BY URUNAD", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                //0. indekste şehrin adı var
                //1. indekste de kaç tane olduğu var (dr[1] i önce string formatta aldık ve sonra da int a dönüştürdük)
                chartControl1.Series["Series 1"].Points.AddPoint(Convert.ToString(dr[0]), int.Parse(dr[1].ToString()));
            }
            bgl.baglanti().Close();

            //Chart'a Firma ve Sehir sayısını da listeleyelim
            SqlCommand komut2 = new SqlCommand("Select IL as 'İl', Count(*) As 'Adet' From TBL_FIRMALAR Group By IL", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while(dr2.Read())
            {
                chartControl2.Series["Series 1"].Points.AddPoint(Convert.ToString(dr2[0]), int.Parse(dr2[1].ToString()));

            }
            bgl.baglanti().Close();


        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmStokDetay fr = new FrmStokDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (dr != null)
            {
                fr.ad = dr["Ürün Adı"].ToString();
            }
            fr.Show();
        }
    }
}
