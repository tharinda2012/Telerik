using System;
using ArtOfTest.WebAii.Core;
using System.IO;
using System.Windows.Forms;
using ArtOfTest.WebAii.Controls.HtmlControls;
using System.Threading;
using System.Net.Mail;
using System.Linq;
using CS.ObjectRepo;

namespace CS.CommonMethods
{
    internal static class Utilities
    {
        //screenshot function
        public static void Save_Screenshot_with_log(Browser browser, Exception e, string testemethodname)
        {
            try
            {
                //create a directory with the methodname to store screenshot and error log
                var directoryname = testemethodname + "_" + string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now);
                const string errordumppath = "C:\\GIT\\Telerik\\CS\\TestResults";
                Directory.CreateDirectory(errordumppath + "\\" + directoryname);
                //save screenshot
                browser.RefreshDomTree();
                var pic = browser.Capture();
                //pic.Save(config.Default.errordumppath + "\\" + directoryname + "\\" + "Error_" + testemethodname + ".png");            
                var pic2 = browser.Capture();
                pic2.Save(errordumppath + "\\" + directoryname + "\\" + "Error_" + testemethodname + ".png");
                File.AppendAllText(errordumppath + "\\" + directoryname + "\\" + "error.txt",
                    "\r\n*************" + DateTime.Now + "---" + testemethodname + "**************\r\n");
                File.AppendAllText(errordumppath + "\\" + directoryname + "\\" + "error.txt", e.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

        }

        //keyboard key simulate functions for non iFrame dialogs

        public static void Enter_SearchString_For_TextField(Manager m, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return;

            foreach (var key in searchText)
            {
                Thread.Sleep(config.Default.SleepingTime / 4);
                m.Desktop.KeyBoard.KeyPress((Keys) char.ToUpper(key));
            }

            m.Desktop.KeyBoard.KeyPress(Keys.Tab);

        }


        public static void Click_Event_For_Textfield(Manager m, HtmlInputText element)
        {
            m.Desktop.Mouse.Click(MouseClickType.LeftClick, element.GetRectangle());

        }


        //keyboard key simulate functions for iFrame dialogs
        public static void Enter_SearchString_For_TextField(Browser browser, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return;
            foreach (var key in searchText)
            {
                Thread.Sleep(config.Default.SleepingTime / 4);
                browser.Desktop.KeyBoard.KeyPress((Keys) char.ToUpper(key));
            }
            browser.Desktop.KeyBoard.KeyPress(Keys.Tab);
        }

        public static void Click_Event_For_Textfield(Browser browser, HtmlInputText element)
        {
            browser.Desktop.Mouse.Click(MouseClickType.LeftClick, element.GetRectangle());

        }

        /// handle confirmation dialogs
        // ConfirmDialog confirmDialog = ConfirmDialog.CreateConfirmDialog(ActiveBrowser, DialogButton.OK);
        //Manager.DialogMonitor.AddDialog(confirmDialog);
        //Manager.DialogMonitor.Start();
        // Wait until dialog handled
        //confirmDialog.WaitUntilHandled(10000);
        //confirmDialog.CurrentState = DialogCurrentState.NotActive;

        //// Add dialog handler for AlertDialog.
        //AlertDialog alertDialog = AlertDialog.CreateAlertDialog(manager.ActiveBrowser, DialogButton.OK);
        //alertDialog.HandlerDelegate = new DialogHandlerDelegate(MyCustomAlertHandler);
        //manager.DialogMonitor.AddDialog(alertDialog);

        public static void Wait_CS_to_Load_Then_Invoke_NewItem(Manager m)
        {
            m.ActiveBrowser.RefreshDomTree();
            var counter0 = 0;
            //HtmlDiv newItemIcon = m.ActiveBrowser.Find.ById<HtmlDiv>("HtmlPage_newItem");
            var tm = new TopMenu(m);
            while (tm.newSpan == null && counter0 < 10) //this will try upto 10 times before fails
            {
                Thread.Sleep(config.Default.SleepingTime * 10);
                counter0 += 1;
                m.ActiveBrowser.RefreshDomTree();
                //newItemIcon = m.ActiveBrowser.Find.ById<HtmlDiv>("HtmlPage_newItem");
                tm = new TopMenu(m);
            }
        }

        public static void Send_Mail(string fromaddr, string toaddr, string subject, string mailbody, string smtphost)
        {
            //send mail to the new address

            var mail = new MailMessage(fromaddr, toaddr);
            var client = new SmtpClient
            {
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = smtphost
            };
            mail.Subject = subject;
            mail.Body = mailbody;
            client.Send(mail);
        }


        public static string Generate_Random_String(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var ranstring = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            var date = DateTime.Now;

            return "Auto--" + date.ToString("yy-MM-dd-HH-mm-ss");
        }



        //send email address as key strokes
        public static void Enter_emailAddress_as_keystrokes(Manager m, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return;

            foreach (var key in searchText)
            {
                if (key.ToString() == ".")
                {
                    m.Desktop.KeyBoard.KeyPress(Keys.OemPeriod);

                }

                if (key.ToString() == "@")
                {
                    m.Desktop.KeyBoard.KeyDown(Keys.LShiftKey);
                    m.Desktop.KeyBoard.KeyPress(Keys.D2);
                    m.Desktop.KeyBoard.KeyUp((Keys.LShiftKey));
                }

                Thread.Sleep(config.Default.SleepingTime / 4);
                m.Desktop.KeyBoard.KeyPress((Keys) char.ToUpper(key));

            }
            m.Desktop.KeyBoard.KeyPress(Keys.Enter);

        }
    }
}
