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
    public partial class FrmNotlar : Form
    {
        public FrmNotlar()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();
        void notlarlistele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_NOTLAR Order By NOTID", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;

        }

        void temizle()
        {
            TxtNotBaslik.Text = "";
            MskNotSaat.Text = "";
            MskNotTarih.Text = "";
            TxtNotId.Text = "";
            TxtHitap.Text = "";
            TxtNotOlusturan.Text = "";
            RchNotDetay.Text = "";
            
        }
        private void FrmNotlar_Load(object sender, EventArgs e)
        {
            notlarlistele();
            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Insert Into TBL_NOTLAR (NOTTARIH,NOTSAAT,NOTBASLIK,NOTDETAY,NOTOLUSTURAN,NOTHITAP) " +
                "VALUES (@P1,@P2,@P3,@P4,@P5,@P6)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", MskNotTarih.Text);
            komut.Parameters.AddWithValue("@p2", MskNotSaat.Text);
            komut.Parameters.AddWithValue("@p3", TxtNotBaslik.Text);
            komut.Parameters.AddWithValue("@p4", RchNotDetay.Text);
            komut.Parameters.AddWithValue("@p5", TxtNotOlusturan.Text);
            komut.Parameters.AddWithValue("@p6", TxtHitap.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Notlar kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            notlarlistele();

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr!=null)
            {
                TxtNotId.Text = dr["NOTID"].ToString();
                MskNotTarih.Text = dr["NOTTARIH"].ToString();
                MskNotSaat.Text = dr["NOTSAAT"].ToString();
                TxtNotBaslik.Text = dr["NOTBASLIK"].ToString();
                RchNotDetay.Text = dr["NOTDETAY"].ToString();
                TxtNotOlusturan.Text = dr["NOTOLUSTURAN"].ToString();
                TxtHitap.Text = dr["NOTHITAP"].ToString();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Notunuzu sileceksiniz. Silmek istediğinizden emin misiniz?", "Notlar Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand silkomut = new SqlCommand("Delete From TBL_NOTLAR Where NOTID=@P1", bgl.baglanti());
                silkomut.Parameters.AddWithValue("@p1", TxtNotId.Text);
                silkomut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Notunuz listeden silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                notlarlistele();
                temizle();

            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncellekomut = new SqlCommand("Update TBL_NOTLAR Set NOTTARIH=@P1,NOTSAAT=@P2,NOTBASLIK=@P3,NOTDETAY=@P4,NOTOLUSTURAN=@P5,NOTHITAP=@P6 " +
                "WHERE NOTID=@P7", bgl.baglanti());
            guncellekomut.Parameters.AddWithValue("@p1", MskNotTarih.Text);
            guncellekomut.Parameters.AddWithValue("@p2", MskNotSaat.Text);
            guncellekomut.Parameters.AddWithValue("@p3", TxtNotBaslik.Text);
            guncellekomut.Parameters.AddWithValue("@p4", RchNotDetay.Text);
            guncellekomut.Parameters.AddWithValue("@p5", TxtNotOlusturan.Text);
            guncellekomut.Parameters.AddWithValue("@p6", TxtHitap.Text);
            guncellekomut.Parameters.AddWithValue("@p7", TxtNotId.Text);
            guncellekomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Notlar güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            notlarlistele();
            temizle();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmNotDetay fr = new FrmNotDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr!=null)
            {
                fr.metin = dr["NOTDETAY"].ToString();
            }
            fr.Show();

        }
    }
}
