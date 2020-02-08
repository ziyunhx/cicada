using Cicada.Core.Enums;
using Cicada.Core.Helper;
using Cicada.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace Cicada.Core.Notication
{
    public class MailNotication : INotication
    {
        private static string smtpServer = "smtp.126.com";
        private static int smtpPort = 25;
        private static bool enableSsl = false;
        private static string username = "";
        private static string password = "";


        public bool Notify(string content, string[] toUser = null, string[] toParty = null, string[] toTag = null, EventLevel level = EventLevel.Normal, int timeOut = 10000)
        {
            List<string> emails = null;// UserHelper.GetEmailFromMembers(toUser, toParty, toTag);

            try
            {
                if (!string.IsNullOrEmpty(content) && emails != null && emails.Count > 0)
                {
                    SmtpClient client = new SmtpClient(smtpServer, smtpPort)
                    {
                        EnableSsl = enableSsl,
                        Credentials = new NetworkCredential(username, password),
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(username, "Cicada Alerted");
                    message.Body = content;
                    message.Subject = "Cicada 预警通知";
                    message.IsBodyHtml = CheckHtml(content);
                    message.BodyEncoding = Encoding.UTF8;

                    foreach (string email in emails)
                    {
                        if (!string.IsNullOrEmpty(email))
                            message.To.Add(email);
                    }
                    client.Send(message);
                    return true;
                }
            }
            catch (Exception ex)
            { }

            return false;
        }

        /// <summary>
        /// 检查是否有Html标签
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns>存在为True</returns>
        private static bool CheckHtml(string html)
        {
            string filter = "<[\\s\\S]*?>";
            if (Regex.IsMatch(html, filter))
            {
                return true;
            }
            filter = "[<>][\\s\\S]*?";
            if (Regex.IsMatch(html, filter))
            {
                return true;
            }
            return false;
        }

    }
}
