using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace MailFlooder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }        

        private void AddAttachments(object sender, RoutedEventArgs e)
        {
            ShowSelectFileDialog("Выбор прекрипляемых файлов",true, Ofd_AttachFileOk);
            
            void Ofd_AttachFileOk(object s, CancelEventArgs a)
            {
                OpenFileDialog os = ((OpenFileDialog)s);
                try
                {
                    LoadAttachments(os.FileNames);
                }
                catch (Exception)
                {
                    a.Cancel = true;
                }

            }
        }

        private static void ShowSelectFileDialog(string title,bool mul,CancelEventHandler Ofd_AttachFileOk)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileOk += Ofd_AttachFileOk;
            ofd.Title = title;
            ofd.Multiselect = mul;
            ofd.ShowDialog();
        }

        private void LoadAttachments(string[] fileNames)
        {
            foreach (string item in fileNames)
            {
                Attachments.Children.Add(MakeBtn(item));
            }
        }

        private UIElement MakeBtn(string text)
        {
            Button newBtn = new Button();
            newBtn.Content = text;
            newBtn.Click += new RoutedEventHandler((object sender, RoutedEventArgs e) => { ((Panel)newBtn.Parent).Children.Remove(newBtn); });
            return newBtn;
        }

        

        private void AddTarget(object sender, RoutedEventArgs e)
        {
            Regex r = new Regex(RegexpPatterns.MailFormat);
            if (Target.Text.Length > 0 && r.IsMatch(Target.Text)) Targets.Children.Add(MakeBtn(Target.Text));
            else MessageBox.Show("Адресат не заполнен, или заполнен не верно!");
        }
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            ShowSelectFileDialog("Выбор файлов источников получателей", true, Ofd_DbFileOk);


            void Ofd_DbFileOk(object s, CancelEventArgs a)
            {
                OpenFileDialog os = ((OpenFileDialog)s);
                try
                {
                    LoadDb(os.FileNames);
                }
                catch (Exception)
                {
                    a.Cancel = true;
                }
            }
        }

        

        private void LoadDb(string[] fileNames)
        {            
                Regex r = new Regex(RegexpPatterns.MailFormat);
                foreach (string item in fileNames)
                {
                    using (StreamReader sr = new StreamReader(item))
                    {
                        if (!sr.EndOfStream)
                        {
                        foreach (Match to in r.Matches(sr.ReadToEnd()))
                        {
                            Targets.Children.Add(MakeBtn(to.Value));
                        }
                    
                        }
                    }
                }            
        }

        private void Send_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from.Text);
            mail.Subject = subject.Text;
            mail.Body = body.Text;
            mail.IsBodyHtml = true;

            if (Attachments.Children.Count>0)
            { attach(mail.Attachments); }
            if (Targets.Children.Count > 0)
            { target(mail.To); }

            NetworkCredential accunt = new NetworkCredential(from.Text, password.SecurePassword);
            App.SendMail(host.Text, Convert.ToInt16(port.Text), mail, accunt).GetAwaiter();


            void attach(AttachmentCollection collect)
            {
                foreach (Button item in Attachments.Children)
                {
                    collect.Add(new Attachment(item.Content.ToString()));
                }
            }
            void target(MailAddressCollection collect)
            {
                foreach (Button item in Targets.Children)
                {
                    collect.Add(new MailAddress(item.Content.ToString()));
                }
            }
        }
        private void CanSend_Executed(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                e.CanExecute = (App.ChkHost($"{host.Text}:{port.Text}") && App.ChkMail(from.Text));

            }
            catch (Exception)
            {
                
            }
           
        }
        private void TrySend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                App.SendMail(host.Text,Convert.ToInt16(port.Text),new MailMessage(from.Text,from.Text),new System.Net.NetworkCredential(from.Text,password.SecurePassword));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            
        }

        private void SettingChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void ShowPeview(object sender, RoutedEventArgs e)
        {
            TogglePreview();
            ShowHTMLOnPreview();
        }

        private void TogglePreview()
        {
            preSlider.Visibility = preview.Visibility = (preview.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
            
            bodyDelim.Width = new GridLength(1.0, GridUnitType.Star);
            preDelim.Width = new GridLength(0, GridUnitType.Auto);
        }

        private void body_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowHTMLOnPreview();
        }

        private void ShowHTMLOnPreview()
        {
            if (preview != null && body.Text != "") preview.NavigateToString(body.Text);
        }
    }
}
