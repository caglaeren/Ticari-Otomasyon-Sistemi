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
    public partial class FrmRehber : Form
    {
        public FrmRehber()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();
        private void FrmRehber_Load(object sender, EventArgs e)
        {
            //Müşteri bilgileri
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select AD,SOYAD,TELEFON,TELEFON2 AS 'TELEFON 2',MAIL From TBL_MUSTERILER ", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;


            //Firma bilgileri
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("Select AD,YETKILIADSOYAD AS 'YETKİLİ AD SOYAD',TELEFON1 As 'TELEFON 1',TELEFON2 As 'TELEFON 2'," +
                "TELEFON3 As 'TELEFON 3',MAIL,FAX From TBL_FIRMALAR", bgl.baglanti());
            da2.Fill(dt2);
            gridControl2.DataSource = dt2;

        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmMail frmMail = new FrmMail();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle); //sectigimiz satırın degerini dr ye atar

            if(dr!=null)
            {
                frmMail.mail = dr["MAIL"].ToString();
            }
            frmMail.Show();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            FrmMail frmMail = new FrmMail();
            DataRow dr = gridView2.GetDataRow(gridView2.FocusedRowHandle); //sectigimiz satırın degerini dr ye atar

            if (dr != null)
            {
                frmMail.mail = dr["MAIL"].ToString();
            }
            frmMail.Show();
        }
    }
}
