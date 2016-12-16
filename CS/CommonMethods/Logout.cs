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
using ArtOfTest.WebAii.Win32.Dialogs;
using ArtOfTest.WebAii.Silverlight;
using ArtOfTest.WebAii.Silverlight.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.ObjectRepo.Login;

namespace CS.CommonMethods
{
    class Logout
    {



        public void Logout_From_CS(Manager myManager)
        {
            try
            {
                obj_login obj = new obj_login(myManager);
                myManager.ActiveBrowser.RefreshDomTree();
                System.Threading.Thread.Sleep(config.Default.SleepingTime*3);
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
