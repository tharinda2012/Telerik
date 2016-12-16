using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ArtOfTest.WebAii.Core;
using System.Drawing;
using ArtOfTest.WebAii.ObjectModel;
using System.IO;
using CS.CommonMethods;
using System.Windows.Forms;
using ArtOfTest.WebAii.Controls.HtmlControls;
using System.Threading;
namespace CS.CommonMethods
{
    class Utilities
    {
        //screenshot function
        public static void Save_Screenshot_withlog(ArtOfTest.WebAii.Core.Browser browser, Exception e, string testemethodname, ArtOfTest.WebAii.Core.Manager myManager)
        {
            try
            {
                //create a directory with the methodname to store screenshot and error log
                String directoryname = testemethodname + "_" + string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now);
                String errordumppath = config.Default.errordumppath;
                errordumppath.Replace(@"\", @"\\");
                System.IO.Directory.CreateDirectory(errordumppath + "\\" + directoryname);
                //save screenshot
                browser.RefreshDomTree();
                Bitmap pic = browser.Capture();
                //pic.Save(config.Default.errordumppath + "\\" + directoryname + "\\" + "Error_" + testemethodname + ".png");            
                Bitmap pic2 = browser.Capture();
                pic2.Save(config.Default.errordumppath + "\\" + directoryname + "\\" + "Error_" + testemethodname + ".png");
                File.AppendAllText(errordumppath + "\\" + directoryname + "\\" + "error.txt", "\r\n*************" + DateTime.Now + "---" + testemethodname + "**************\r\n");
                File.AppendAllText(errordumppath + "\\" + directoryname + "\\" + "error.txt", e.ToString());
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

        }

        //keyboard key simulate functions for non iFrame dialogs

        public static void Enter_SearchStringFor_TextField(Manager m, string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                foreach (char key in searchText)
                {
                    Thread.Sleep(config.Default.SleepingTime / 4);
                    m.Desktop.KeyBoard.KeyPress((Keys)char.ToUpper(key));
                }
                m.Desktop.KeyBoard.KeyPress(Keys.Enter);
            }


        }


        public static void Click_EventFor_Textfield(Manager m, HtmlInputText element)
        {
            m.Desktop.Mouse.Click(MouseClickType.LeftClick, element.GetRectangle());

        }


        //keyboard key simulate functions for iFrame dialogs
        public static void Enter_SearchStringFor_TextField(ArtOfTest.WebAii.Core.Browser browser, string searchText)
        {
            if (!string.IsNullOrEmpty(searchText))
            {
                foreach (char key in searchText)
                {
                    Thread.Sleep(config.Default.SleepingTime / 4);
                    browser.Desktop.KeyBoard.KeyPress((Keys)char.ToUpper(key));
                }
                browser.Desktop.KeyBoard.KeyPress(Keys.Tab);
            }



        }

        public static void Click_EventFor_Textfield(ArtOfTest.WebAii.Core.Browser browser, HtmlInputText element)
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
            int counter0 = 0;
            HtmlDiv newItemIcon = m.ActiveBrowser.Find.ById<HtmlDiv>("HtmlPage_newItem");
            while (newItemIcon == null && counter0 < 10) //this will try upto 10 times before fails
            {
                Thread.Sleep(config.Default.SleepingTime * 10);
                counter0 += 1;
                m.ActiveBrowser.RefreshDomTree();
                newItemIcon = m.ActiveBrowser.Find.ById<HtmlDiv>("HtmlPage_newItem");
            }
        }


    }
}
