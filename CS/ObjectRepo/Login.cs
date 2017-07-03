using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.ObjectRepo
{
    class Login
    {

        private Manager _manager;

        public Login(Manager m)
        {
            _manager = m;
        }

        //elements in CS for Onsite
        public Element userName { get { return _manager.ActiveBrowser.Find.ById("id_login_username"); } }
        public Element passWord { get { return _manager.ActiveBrowser.Find.ByName("login_password"); } }
        public Element loginButton { get { return _manager.ActiveBrowser.Find.ByXPath("/html/body/form/div/button"); } }

        public Element logoutDiv { get { return _manager.ActiveBrowser.Find.ByAttributes("class=HtmlPage_personWrapper"); } }
        //public Element logoutDiv { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='HtmlPage_topDiv']/div[2]/div[2]/div/div"); } }
        public Element logoutAnchor { get { return _manager.ActiveBrowser.Find.ByAttributes("href=/scripts/rms.fcgi?_sf=0&action=logout", "class=HtmlPageDropDown_selectedItem"); } }
       

        //elemets for Online - Access crm.web first and then launch CS from Navigator

        public Element onlineUsername { get { return _manager.ActiveBrowser.Find.ById("Username"); } }
        public Element onlinePassword { get { return _manager.ActiveBrowser.Find.ById("Password"); } }
        public Element onlineLoginButton { get { return _manager.ActiveBrowser.Find.ById("loginButton"); } }

        //crm.23b navigator elements to find and launch CS

        public Element anchrorCS { get { return _manager.ActiveBrowser.Find.ById("csButton"); } }
        
    }
}
