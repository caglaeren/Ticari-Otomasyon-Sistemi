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
    public partial class FrmAyarlar : Form
    {
        public FrmAyarlar()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void ayarlarilistele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TBL_ADMIN ",bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
            
        }

        void temizle()
        {
            TxtId.Text = "";
            Txtkullaniciad.Text = "";
            Txtsifre.Text = "";
          

        }
        private void FrmAyarlar_Load(object sender, EventArgs e)
        {
            ayarlarilistele();
            temizle();
        }

        private void Btnislem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Txtkullaniciad.Text) || string.IsNullOrEmpty(Txtsifre.Text))
            {
                MessageBox.Show("Kullanıcı adı ve şifre alanları boş olamaz.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Admin bilgisi sisteme eklenecektir.Gerçekten eklemek istiyor musunuz?", "Admin Bilgisi Ekleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand komut = new SqlCommand("Insert into TBL_ADMIN (KULLANICIADI,SIFRE) Values (@P1,@P2)", bgl.baglanti());
                komut.Parameters.AddWithValue("@P1",Txtkullaniciad.Text);
                komut.Parameters.AddWithValue("@P2", Txtsifre.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Admin bilgisi sisteme başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ayarlarilistele();
                temizle();
            }
            else
            {
                MessageBox.Show("Admin bilgisi ekleme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr!=null)
            {
                TxtId.Text = dr["ID"].ToString();
                Txtkullaniciad.Text = dr["KULLANICIADI"].ToString();
                Txtsifre.Text = dr["SIFRE"].ToString();
            }
        }

        

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Admin bilgisi güncellenecektir.Gerçekten güncellemek istiyor musunuz?", "Admin Bilgisi Güncelleme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand komut = new SqlCommand("Update TBL_ADMIN set KULLANICIADI=@P1,SIFRE=@P2 where ID=@P3", bgl.baglanti());
                komut.Parameters.AddWithValue("@P1", Txtkullaniciad.Text);
                komut.Parameters.AddWithValue("@P2", Txtsifre.Text);
                komut.Parameters.AddWithValue("@P3", TxtId.Text);
                komut.ExecuteNonQuery();
                MessageBox.Show("Admin bilgisi başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ayarlarilistele();
                temizle();
            }
            else
            {
                MessageBox.Show("Admin bilgisi güncelleme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult secim2 = new DialogResult();
            secim2 = MessageBox.Show("Admin bilgisi sistemden silinecektir.Gerçekten silmek istiyor musunuz?", "Admin Bilgisi Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim2 == DialogResult.Yes)
            {
                SqlCommand komut = new SqlCommand("Delete from TBL_ADMIN where KULLANICIADI=@P1", bgl.baglanti());
                komut.Parameters.AddWithValue("@P1",Txtkullaniciad.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Admin bilgisi sistemden başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ayarlarilistele();
                temizle();
            }
            else
            {
                MessageBox.Show("Admin bilgisi silme işlemi iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
