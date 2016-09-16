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
            progressBar1.Value = 0;

            login = new NetworkCredential(txtUsername.Text, txtPasswrod.Text);  //Send The Credential to smtp Credential
            progressBar1.PerformStep();
            client = new SmtpClient(txtSmtp.Text); // Give the smtp the (Google smtp)
            client.Port = Convert.ToInt32(txtPort.Text); // Give the smtp port (Google Port)
            client.EnableSsl = chkSSL.Checked; // Give the smtp the SSL if we need it or not!
            client.Credentials = login; // Give the smtp the Login(Credential)
            progressBar1.PerformStep();

            msg = new MailMessage
            {
                From = new MailAddress(txtUsername.Text + txtSmtp.Text.Replace("smtp.", "@"), "Lucy", Encoding.UTF8)
            }; // Send From
            progressBar1.PerformStep();

            msg.To.Add(new MailAddress(txtTo.Text)); // Send To

            if (!string.IsNullOrEmpty(txtCC.Text)) 
            {
                msg.To.Add(txtCC.Text); // Send to Many Email
            }

            msg.Subject = txtSubject.Text; // Message Subject
            progressBar1.PerformStep();
            msg.Body = txtMessage.Text; // Message Content
            progressBar1.PerformStep();
            msg.BodyEncoding = Encoding.UTF8; // Message Encoding
            progressBar1.PerformStep();
            msg.IsBodyHtml = true; // Message have HTML Formate or not 
            progressBar1.PerformStep();
            msg.Priority = MailPriority.High; // Message Priority (الاهميه او الاولويه)
            progressBar1.PerformStep();

            msg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallBack); // Create Event Name (SendCompletedCallBack)
            string Userstate = "Sending..."; // Must u add it Becoz the Event need UserState
            client.SendAsync(msg, Userstate); // Send Message From Smtp to Sender(USER HOW SEND My EMAIL) 
            progressBar1.PerformStep();
            progressBar1.PerformStep();

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
