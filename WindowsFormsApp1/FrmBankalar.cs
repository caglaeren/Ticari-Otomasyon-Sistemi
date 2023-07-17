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
    public partial class FrmBankalar : Form
    {
        public FrmBankalar()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void bankalistele()
        {
            //SQL tarafında prosedür kullanarak oluşturduk
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Exec pr_BankaBilgileri", bgl.baglanti());
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

        void firmalistesi()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select ID,AD From TBL_FIRMALAR", bgl.baglanti());
            da.Fill(dt);
            //lookUpEdit1.Properties.NullText = "Lütfen bir ad seçiniz";
            lookUpEdit1.Properties.ValueMember = "ID";
            lookUpEdit1.Properties.DisplayMember = "AD"; //firmanın adı gözükecek
            lookUpEdit1.Properties.DataSource = dt;
        }

        void temizle()
        {
            TxtId.Text = "";
            TxtBankaAdi.Text = ""; 
            Cmbil.Text = "";
            Cmbilce.Text = "";
            TxtSube.Text = "";
            MskIBAN.Text = "";
            MskHesapno.Text = "";
            TxtYetkili.Text = "";
            MskTelefon.Text = "";
            MskTarih.Text = "";
            TxtHesapturu.Text = "";
            lookUpEdit1.Text = "";


        }
        private void FrmBankalar_Load(object sender, EventArgs e)
        {
            bankalistele();

            firmalistesi();

            sehirListesi();

            temizle();
       
                
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Insert into TBL_BANKALAR (BANKAADI,IL,ILCE,SUBE,IBAN,HESAPNO,YETKILI,TELEFON,TARIH,HESAPTURU,FIRMAID) " +
                "values (@P1,@P2,@P3,@P4,@P5,@P6,@P7,@P8,@P9,@P10,@P11)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtBankaAdi.Text);
            komut.Parameters.AddWithValue("@p2", Cmbil.Text);
            komut.Parameters.AddWithValue("@p3", Cmbilce.Text);
            komut.Parameters.AddWithValue("@p4", TxtSube.Text);
            komut.Parameters.AddWithValue("@p5", MskIBAN.Text);
            komut.Parameters.AddWithValue("@p6", MskHesapno.Text);
            komut.Parameters.AddWithValue("@p7", TxtYetkili.Text);
            komut.Parameters.AddWithValue("@p8", MskTelefon.Text);
            komut.Parameters.AddWithValue("@p9", MskTarih.Text);
            komut.Parameters.AddWithValue("@p10", TxtHesapturu.Text);
            komut.Parameters.AddWithValue("@p11", lookUpEdit1.EditValue);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Banka bilgileri sisteme kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            bankalistele();
        }

        private void Cmbil_SelectedIndexChanged(object sender, EventArgs e)
        {
            //İlçe seçilince daha önce seçilmiş olan ilçeleri temizlemek istersek de:
            Cmbilce.Properties.Items.Clear();
            SqlCommand komut = new SqlCommand("Select ILCE From TBL_ILCELER Where SEHIR = @p1", bgl.baglanti());
            //Cmbil'in seçilen indeksinden ilçenin gelmesini ayarlamak istedik
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
            if (dr != null)
            {
                TxtId.Text = dr["ID"].ToString(); //stored procedure'e eklemeyince hata veriyor bu kısım
                TxtBankaAdi.Text = dr["BANKAADI"].ToString();
                Cmbil.Text = dr["IL"].ToString();
                Cmbilce.Text = dr["ILCE"].ToString();
                TxtSube.Text = dr["SUBE"].ToString();
                MskIBAN.Text = dr["IBAN"].ToString();
                MskHesapno.Text = dr["HESAPNO"].ToString();
                TxtYetkili.Text = dr["YETKILI"].ToString();
                MskTelefon.Text = dr["TELEFON"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                TxtHesapturu.Text = dr["HESAPTURU"].ToString();
                

            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Banka kaydınızı sileceksiniz. Silmek istediğinizden emin misiniz?", "Banka Kaydı Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                SqlCommand silkomut = new SqlCommand("Delete From TBL_BANKALAR Where ID=@P1", bgl.baglanti());
                silkomut.Parameters.AddWithValue("@p1", TxtId.Text);
                silkomut.ExecuteNonQuery();
                bgl.baglanti().Close();
                temizle();
                MessageBox.Show("Bankanın kaydı listeden silindi.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bankalistele();

            }
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            SqlCommand guncellekomut = new SqlCommand("Update TBL_BANKALAR set BANKAADI=@P1,IL=@P2,ILCE=@P3,SUBE=@P4,IBAN=@P5,HESAPNO=@P6,YETKILI=@P7,TELEFON=@P8,TARIH=@P9," +
                "HESAPTURU=@P10,FIRMAID=@P11 WHERE ID=@P12", bgl.baglanti());
            guncellekomut.Parameters.AddWithValue("@p1", TxtBankaAdi.Text);
            guncellekomut.Parameters.AddWithValue("@p2", Cmbil.Text);
            guncellekomut.Parameters.AddWithValue("@p3", Cmbilce.Text);
            guncellekomut.Parameters.AddWithValue("@p4", TxtSube.Text);
            guncellekomut.Parameters.AddWithValue("@p5", MskIBAN.Text);
            guncellekomut.Parameters.AddWithValue("@p6", MskHesapno.Text);
            guncellekomut.Parameters.AddWithValue("@p7", TxtYetkili.Text);
            guncellekomut.Parameters.AddWithValue("@p8", MskTelefon.Text);
            guncellekomut.Parameters.AddWithValue("@p9", MskTarih.Text);
            guncellekomut.Parameters.AddWithValue("@p10", TxtHesapturu.Text);
            guncellekomut.Parameters.AddWithValue("@p11", lookUpEdit1.EditValue);
            guncellekomut.Parameters.AddWithValue("@p12", TxtId.Text);
            guncellekomut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Banka bilgileri güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            bankalistele();
            temizle();
        }
    }
}
