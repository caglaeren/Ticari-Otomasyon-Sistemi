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
    public partial class FrmGiderler : Form
    {
        public FrmGiderler()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void giderlistesi()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_GIDERLER Order By ID ASC", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle()
        {
            TxtId.Text = "";
            Cmbay.Text = "";
            Cmbyil.Text = "";
            TxtElektrik.Text = "";
            TxtSu.Text = "";
            TxtDogalgaz.Text = "";
            TxtInternet.Text = "";
            TxtMaaslar.Text = "";
            TxtEkstra.Text = "";
            RchNotlar.Text = "";

        }
        private void FrmGiderler_Load(object sender, EventArgs e)
        {
            giderlistesi();

            temizle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Insert into TBL_GIDERLER (AY,YIL,ELEKTRIK,SU,DOGALGAZ,INTERNET,MAASLAR,EKSTRA,NOTLAR)" +
                "VALUES (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", Cmbay.Text);
            komut.Parameters.AddWithValue("@p2", Cmbyil.Text);
            komut.Parameters.AddWithValue("@p3",decimal.Parse(TxtElektrik.Text));
            komut.Parameters.AddWithValue("@p4",decimal.Parse(TxtSu.Text));
            komut.Parameters.AddWithValue("@p5",decimal.Parse(TxtDogalgaz.Text));
            komut.Parameters.AddWithValue("@p6",decimal.Parse(TxtInternet.Text));
            komut.Parameters.AddWithValue("@p7",decimal.Parse(TxtMaaslar.Text));
            komut.Parameters.AddWithValue("@p8",decimal.Parse(TxtEkstra.Text));
            komut.Parameters.AddWithValue("@p9",RchNotlar.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Bu ayki giderler sisteme eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            giderlistesi();
          

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr!=null)
            {
                TxtId.Text = dr["ID"].ToString();
                Cmbay.Text = dr["AY"].ToString();
                Cmbyil.Text = dr["YIL"].ToString();
                TxtElektrik.Text = dr["ELEKTRIK"].ToString();
                TxtSu.Text = dr["SU"].ToString();
                TxtDogalgaz.Text = dr["DOGALGAZ"].ToString();
                TxtInternet.Text = dr["INTERNET"].ToString();
                TxtMaaslar.Text = dr["MAASLAR"].ToString();
                TxtEkstra.Text = dr["EKSTRA"].ToString();
                RchNotlar.Text = dr["NOTLAR"].ToString();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {

            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Giderler kaydınızı sileceksiniz. Silmek istediğinizden emin misiniz?", "Giderler Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand silkomut = new SqlCommand("Delete From TBL_GIDERLER Where ID=@P1", bgl.baglanti());
                silkomut.Parameters.AddWithValue("@p1", TxtId.Text);
                silkomut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Gider kaydı listeden silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                giderlistesi();
                temizle();

            }

        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncellekomut = new SqlCommand("Update TBL_GIDERLER Set AY=@P1,YIL=@P2,ELEKTRIK=@P3,SU=@P4,DOGALGAZ=@P5,INTERNET=@P6," +
                "MAASLAR=@P7,EKSTRA=@P8,NOTLAR=@P9 where ID=@P10", bgl.baglanti());
            guncellekomut.Parameters.AddWithValue("@p1", Cmbay.Text);
            guncellekomut.Parameters.AddWithValue("@p2", Cmbyil.Text);
            guncellekomut.Parameters.AddWithValue("@p3", decimal.Parse(TxtElektrik.Text));
            guncellekomut.Parameters.AddWithValue("@p4", decimal.Parse(TxtSu.Text));
            guncellekomut.Parameters.AddWithValue("@p5", decimal.Parse(TxtDogalgaz.Text));
            guncellekomut.Parameters.AddWithValue("@p6", decimal.Parse(TxtInternet.Text));
            guncellekomut.Parameters.AddWithValue("@p7", decimal.Parse(TxtMaaslar.Text));
            guncellekomut.Parameters.AddWithValue("@p8", decimal.Parse(TxtEkstra.Text));
            guncellekomut.Parameters.AddWithValue("@p9", RchNotlar.Text);
            guncellekomut.Parameters.AddWithValue("@p10", TxtId.Text);
            guncellekomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Giderler güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            giderlistesi();
            temizle();

        }
    }
}
