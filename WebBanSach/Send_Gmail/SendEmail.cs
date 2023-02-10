using System.Net.Mail;
using System.Net;

namespace WebBanSach.Send_Gmail
{
    public static class SendEmail
    {
        public static void SendMail2Step(string From, string To, string Subject, string Body, string pass)
        {
            var fromAddress = new MailAddress(From, "Web Ban Sach");
            var toAddress = new MailAddress(To, To);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(From, pass),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = Subject,
                Body = Body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
