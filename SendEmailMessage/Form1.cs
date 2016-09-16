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

namespace SendEmailMessage
{
    public partial class Form1 : Form
    {
        NetworkCredential login;
        SmtpClient client;
        MailMessage msg;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            login = new NetworkCredential(txtUsername.Text, txtPasswrod.Text);

            client = new SmtpClient(txtSmtp.Text);
            client.Port = Convert.ToInt32(txtPort.Text);
            client.EnableSsl = chkSSL.Checked;
            client.Credentials = login;

            msg = new MailMessage { From = new MailAddress(txtUsername.Text + txtSmtp.Text.Replace("smtp.", "@"), "Lucy", Encoding.UTF8) };
            msg.To.Add(new MailAddress(txtTo.Text));

            if (!string.IsNullOrEmpty(txtCC.Text))
            {
                msg.To.Add(txtCC.Text);
            }

            msg.Subject = txtSubject.Text;
            msg.Body = txtMessage.Text;
            msg.BodyEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.High;

            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallBack);
            string UserState = "Sending...";
            client.SendAsync(msg, UserState);

        }

        private static void SendCompletedCallBack(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show(e.UserState + " Send Cancelled", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            if (e.Error != null)
                MessageBox.Show(e.Error + " " + e.UserState + " Error Message Not Send", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("Your Message has been successfully send", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
