using System;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.ObjectRepo;

using ArtOfTest.WebAii.Win32.Dialogs;

namespace CS.CommonMethods
{
    internal class SessionManager
    {
        private Settings _mySettings;
        public Manager MyManager;

        public void Login_To_CS(bool isMobile)
        {

            _mySettings = new Settings
            {
                Web = {DefaultBrowser = isMobile ? BrowserType.InternetExplorer : config.Default.BrowserType}
            };
            //assign different browsers based on CS client (desktop or mobile)


            MyManager = new Manager(_mySettings);
            MyManager.Start();
            MyManager.LaunchNewBrowser();
            MyManager.Browsers[0].Window.Maximize();
            Thread.Sleep(config.Default.SleepingTime);
            MyManager.ActiveBrowser.ClearCache(BrowserCacheType.Cookies);

            //assign different urls based on CS client (desktop or mobile)
            if (!isMobile)
            {
                MyManager.ActiveBrowser.NavigateTo(config.Default.Base_Url);
            }

            else
            {
                MyManager.ActiveBrowser.NavigateTo("http://support.81onsitecd.com/scripts/rms.fcgi/compactMode");
                // assert compactmode is landed
                var m= new Mobile(MyManager);
                Assert.AreEqual("CS Compact Mode",m.compactString.InnerText);
            }
            
            //login with credentials
            var obj = new Login(MyManager);
            obj.onlineUsername.Wait.ForExists();
            MyManager.ActiveBrowser.Actions.SetText(obj.onlineUsername, config.Default.Username);
            obj.onlinePassword.Wait.ForExists();
            MyManager.ActiveBrowser.Actions.SetText(obj.onlinePassword, config.Default.Password);
            obj.onlineLoginButton.Wait.ForExists();
            MyManager.ActiveBrowser.Actions.Click(obj.onlineLoginButton);


            if (isMobile) return;
            //try to handle webtools wizard
            var wt = new WebTools(MyManager);
            var sharedtext = MyManager.ActiveBrowser.Find.ById<HtmlSpan>("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheFirstLoadDialog_Page1SharedComputerText");
            var counter0 = 0;
            while (sharedtext == null && counter0 < 10) //this will try upto 10 times before fails
            {
                Thread.Sleep(config.Default.SleepingTime * 10);
                counter0 += 1;
                MyManager.ActiveBrowser.RefreshDomTree();
                sharedtext = MyManager.ActiveBrowser.Find.ById<HtmlSpan>("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheFirstLoadDialog_Page1SharedComputerText");
            }

            MyManager.ActiveBrowser.RefreshDomTree();
            if (wt.SharedComputer.InnerText != null)
            {
                Thread.Sleep(config.Default.SleepingTime * 5);
                wt.SharedComputer.Wait.ForExists();
                MyManager.ActiveBrowser.Actions.Click(wt.SharedComputer);
                Thread.Sleep(config.Default.SleepingTime * 3);
                MyManager.ActiveBrowser.Actions.Click(wt.WTNextBtn);
                Thread.Sleep(config.Default.SleepingTime * 3);
                MyManager.ActiveBrowser.Actions.Click(wt.WTCloseBtn);
                Thread.Sleep(config.Default.SleepingTime * 2);
            }

            Thread.Sleep(config.Default.SleepingTime * 2);
            MyManager.SetNewBrowserTracking(true);

            //launch CS from web navigator
            MyManager.ActiveBrowser.Actions.Click(obj.anchrorCS);
            MyManager.SetNewBrowserTracking(false);
                
            Thread.Sleep(config.Default.SleepingTime * 5);
            var browsercount = MyManager.Browsers.Count;

            //try to maximise browser if its not maximised.
            if (browsercount == 2)
            {
                MyManager.Browsers[1].Window.SetFocus();
                MyManager.Browsers[1].Window.Maximize();
                Thread.Sleep(config.Default.SleepingTime * 3);
            }

            else
            {
                MyManager.Browsers[0].Window.SetFocus();
                MyManager.Browsers[0].Window.Maximize();
            }
        }


        
        public static void Logout_From_CS(Manager myManager)
        {
            try
            {
                var obj = new Login(myManager);
                myManager.ActiveBrowser.RefreshDomTree();
                Thread.Sleep(config.Default.SleepingTime * 3);
                var logout = obj.logoutDiv.As<HtmlDiv>();
                logout.Wait.ForExists();
                logout.MouseClick();

                Thread.Sleep(config.Default.SleepingTime);
                myManager.ActiveBrowser.RefreshDomTree();
                var logouttags = myManager.ActiveBrowser.Find.ById("HtmlPageDropDown_menuItems").As<HtmlDiv>();

                var logoutspan = logouttags.ChildNodes[1].As<HtmlAnchor>();
                logoutspan.Wait.ForExists();

                //handling javascript window popups. Ex: the navigate away dialog appears from browser
                var dialog = OnBeforeUnloadDialog.CreateOnBeforeUnloadDialog(myManager.ActiveBrowser, DialogButton.CLOSE);
                myManager.DialogMonitor.AddDialog(dialog);
                myManager.DialogMonitor.Start();
                logoutspan.Click();

                //If more than one browser instalance are invoked then we handle them seperately. 
                var browsercount = myManager.Browsers.Count;
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
