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
                Credentials = new NetworkCredential("mokhovtema@yandex.ru", "elvmujhniwvuwofh"),
                EnableSsl = true,
            };
            smtpClient.Send("mokhovtema@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}