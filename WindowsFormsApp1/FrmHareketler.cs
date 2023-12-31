﻿using System;
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
    public partial class FrmHareketler : Form
    {
        public FrmHareketler()
        {
            InitializeComponent();
        }

        SqlBaglantisi bgl = new SqlBaglantisi();

        void firmaHareketiListesi()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Exec pr_FirmaHareketleri",bgl.baglanti());
            da.Fill(dt);
            gridControl2.DataSource = dt;
        }

        void musteriHareketiListesi()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Exec pr_MusteriHareketleri", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        private void FrmHareketler_Load(object sender, EventArgs e)
        {
            firmaHareketiListesi();
            musteriHareketiListesi();
        }
    }
}
