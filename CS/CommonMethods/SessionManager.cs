using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Controls.HtmlControls.HtmlAsserts;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using ArtOfTest.WebAii.TestAttributes;
using ArtOfTest.WebAii.TestTemplates;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.ObjectRepo.Login;
using CS.ObjectRepo.WebTools;
using ArtOfTest.WebAii.Win32.Dialogs;

namespace CS.CommonMethods
{
      class SessionManager
    {
        public Settings mySettings;
        public Manager myManager;

        public void Login_To_CS()
        {

            mySettings = new Settings();
            mySettings.Web.DefaultBrowser = BrowserType.Chrome;
            myManager = new Manager(mySettings);
            myManager.Start();
            myManager.LaunchNewBrowser();
            myManager.Browsers[0].Window.Maximize();
            Thread.Sleep(config.Default.SleepingTime);
            myManager.ActiveBrowser.ClearCache(ArtOfTest.WebAii.Core.BrowserCacheType.Cookies);
            myManager.ActiveBrowser.NavigateTo(config.Default.Base_Url);
            obj_login obj = new obj_login(myManager);
            obj.onlineUsername.Wait.ForExists();
            myManager.ActiveBrowser.Actions.SetText(obj.onlineUsername, config.Default.Username);
            obj.onlinePassword.Wait.ForExists();
            myManager.ActiveBrowser.Actions.SetText(obj.onlinePassword, config.Default.Password);
            obj.onlineLoginButton.Wait.ForExists();
            myManager.ActiveBrowser.Actions.Click(obj.onlineLoginButton);

            //try to handle webtools wizard
            WebTools wt = new WebTools(myManager);
            HtmlSpan sharedtext = myManager.ActiveBrowser.Find.ById<HtmlSpan>("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheFirstLoadDialog_Page1SharedComputerText");
            int counter0 = 0;
            while (sharedtext == null && counter0 < 10) //this will try upto 10 times before fails
            {
                Thread.Sleep(config.Default.SleepingTime * 10);
                counter0 += 1;
                myManager.ActiveBrowser.RefreshDomTree();
                sharedtext = myManager.ActiveBrowser.Find.ById<HtmlSpan>("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheFirstLoadDialog_Page1SharedComputerText");
            }

            myManager.ActiveBrowser.RefreshDomTree();
            if (wt.SharedComputer.InnerText != null)
            {
                Thread.Sleep(config.Default.SleepingTime * 5);
                wt.SharedComputer.Wait.ForExists();
                myManager.ActiveBrowser.Actions.Click(wt.SharedComputer);
                Thread.Sleep(config.Default.SleepingTime * 3);
                myManager.ActiveBrowser.Actions.Click(wt.WTNextBtn);
                Thread.Sleep(config.Default.SleepingTime * 3);
                myManager.ActiveBrowser.Actions.Click(wt.WTCloseBtn);
                Thread.Sleep(config.Default.SleepingTime * 2);
            }

            Thread.Sleep(config.Default.SleepingTime * 2);
            myManager.SetNewBrowserTracking(true);

            //launch CS from web navigator
            myManager.ActiveBrowser.Actions.Click(obj.anchrorCS);
            myManager.SetNewBrowserTracking(false);
            //myManager.WaitForNewBrowserConnect("https://sod.superoffice.com/Cust21693/CS/scripts/ticket.fcgi?action=mainMenu&login_language=en-US", true, 10000);
            Thread.Sleep(config.Default.SleepingTime * 5);
            int browsercount = myManager.Browsers.Count;

            //try to maximise browser if its not maximised.
            if (browsercount == 2)
            {
                myManager.Browsers[1].Window.SetFocus();
                myManager.Browsers[1].Window.Maximize();
                Thread.Sleep(config.Default.SleepingTime * 3);
            }

            else
            {
                myManager.Browsers[0].Window.SetFocus();
                myManager.Browsers[0].Window.Maximize();
            }




        }
        public void Logout_From_CS(Manager myManager)
        {
            try
            {
                obj_login obj = new obj_login(myManager);
                myManager.ActiveBrowser.RefreshDomTree();
                System.Threading.Thread.Sleep(config.Default.SleepingTime * 3);
                HtmlDiv logout = obj.logoutDiv.As<HtmlDiv>();
                logout.Wait.ForExists();
                logout.MouseClick();

                System.Threading.Thread.Sleep(config.Default.SleepingTime);
                myManager.ActiveBrowser.RefreshDomTree();
                HtmlDiv logouttags = myManager.ActiveBrowser.Find.ById("HtmlPageDropDown_menuItems").As<HtmlDiv>();

                HtmlAnchor logoutspan = logouttags.ChildNodes[1].As<HtmlAnchor>();
                logoutspan.Wait.ForExists();

                //handling javascript window popups. Ex: the navigate away dialog appears from browser
                OnBeforeUnloadDialog dialog = OnBeforeUnloadDialog.CreateOnBeforeUnloadDialog(myManager.ActiveBrowser, DialogButton.CLOSE);
                myManager.DialogMonitor.AddDialog(dialog);
                myManager.DialogMonitor.Start();
                logoutspan.Click();

                //If more than one browser instalance are invoked then we handle them seperately. 
                int browsercount = myManager.Browsers.Count;
                if (browsercount == 2)
                {
                    myManager.Browsers[1].Window.Close();
                    myManager.Browsers[0].Window.Close();
                }

                else
                {
                    myManager.Browsers[0].Window.Close();
                }


                myManager.DialogMonitor.Stop();
                //this.Manager.Settings.UnexpectedDialogAction = UnexpectedDialogAction.HandleAndContinue;
            }

            catch (Exception e)
            {
                myManager.Browsers[0].Window.Close();
                Console.Write(e.Message);
            }
        }



    }
}
