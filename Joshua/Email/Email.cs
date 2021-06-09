using System;
using System.Configuration;
using System.Net.Mail;

namespace Joshua.Email
{
    public static class Email
    {
        public static string Send(string recipient, string message, string subject = "no subject", string sender = "noreply@joshuasimacek.com")
        {
            string host = ConfigurationManager.AppSettings["emailrelay"];
            int port = 25;

            using (var emailClient = new SmtpClient())
            {
                emailClient.Port = port;
                emailClient.Host = host;
                emailClient.EnableSsl = false;
                emailClient.DeliveryMethod = global::System.Net.Mail.SmtpDeliveryMethod.Network;
                emailClient.Timeout = int.MaxValue;
                try
                {
                    emailClient.Send(sender, recipient, subject, message);
                    return "Success";
                }
                catch (Exception ex)
                {
                    global::System.Diagnostics.Debug.WriteLine("Exception Caught: \r\n" + ex);
                    return ex.ToString();
                }
            }
        }
    }
}
