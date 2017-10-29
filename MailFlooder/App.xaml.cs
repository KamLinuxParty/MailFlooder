using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MailFlooder
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        
        internal static bool ChkHost(string v)
        {
            return tstFormat(v,RegexpPatterns.HostFormat);
        }

        private static bool tstFormat(string v, string format)
        {
            Regex r = new Regex(format);
            return r.IsMatch(v);
        }

        internal static bool ChkMail(string v)
        {
            return tstFormat(v, RegexpPatterns.MailFormat);
        }

        internal static async Task SendMail(string host, short port, MailMessage message, NetworkCredential accunt)
        {
            SmtpClient smtp = new SmtpClient(host, port);
            smtp.Credentials = accunt;
            smtp.EnableSsl = true;
            try
            {
                await smtp.SendMailAsync(message);
                //lock (Logger.Logs)
                //{
                //    Logger.WriteLine($"[{DateTime.Now}]{Task.CurrentId}: Рассылка сообшений окончена! ");
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //lock (Logger.Logs)
                //{
                //    Logger.WriteLine($"[{DateTime.Now}]{Task.CurrentId}: Писмо не отправлено с ошибкой:{e.Message} ");
                //}
            }
            
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {

            //Logger.Logs.Dispose();
        }
    }
}
