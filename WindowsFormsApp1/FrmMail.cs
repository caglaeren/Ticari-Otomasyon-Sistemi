using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace WindowsFormsApp1
{
    public partial class FrmMail : Form
    {
        public FrmMail()
        {
            InitializeComponent();
        }

        public string mail;

        private void FrmMail_Load(object sender, EventArgs e)
        {
            TxtMailAdres.Text = mail;
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            MailMessage mesajim = new MailMessage();
            //bir tane istemci nesnesi türetelim
            SmtpClient istemci = new SmtpClient();
            //istemcinin kimligi (nttworkcredential ise ag kimligidir)
            //birinci cift tırnak icine mail adresiniz yazılacak
            //ikinci cift tırnak icine ise sifreniz yazılacak
            //istemci.Credentials = new System.Net.NetworkCredential("Mail", "Şifre");
            //NetWorkCredential ağ kimliğidir
            istemci.Credentials = new System.Net.NetworkCredential("mailadresiniz.com", "sifreniz");
            //türkiyede kullanılan mail adresi port numarası 587
            istemci.Port = 587;
            //istemcinin sunucusu
            //gmailin sunucusunu yazmaya calıstım
            istemci.Host = "smtp.gmail.com";
            //EnableSsl yol boyunca şifrelesin mi diyor ve evet şifrelesin istedik
            istemci.EnableSsl = true;
            //mesajımı atan kişiden göndermiş olacagım
            //bu mail adresine gönderilmiş olacak
            mesajim.To.Add(TxtMailAdres.Text);

            //peki mesajım kimden gidecek
            //Bu yukarıda yazdıgımız system.net içindeki mail adresi ile aynı olacak
            //mesajim.From = new MailAddress("Mail");
            //bu bizim kendi mail adresimiz, yani bu mailden göndereceği
            mesajim.From = new MailAddress("mailadresimiz.com");
            mesajim.Subject = TxtKonu.Text; // konudan gelen degerdir
            mesajim.Body = RchMesaj.Text;//içerikten gelen mesaj
            istemci.Send(mesajim); //mesajımı gönderme islemini gerceklesitrecek
        }
    }
}
