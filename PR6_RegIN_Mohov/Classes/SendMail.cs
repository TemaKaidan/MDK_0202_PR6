using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PR6_RegIN_Mohov.Classes
{
    public class SendMail
    {
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("artemmohov05@mail.ru", "112211qQFortnite"),
                EnableSsl = true,
            };
            smtpClient.Send("artemmohov05@mail.ru", To, "Проект RegIn", Message);
        }
    }
}