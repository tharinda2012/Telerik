using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CS.ObjectRepo.Mailing
{
    class email
    {

        private Manager _manager;

        public email(Manager m)
        {
            _manager = m;
        }

        // Main + add button in the top area of the home screen
        public Element newItemIcon
        { get { return _manager.ActiveBrowser.Find.ById("HtmlPage_newItem"); } }

        //emaili maliling menu item
        public Element newmailing
        { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newEmailMailing']/span"); } }       



    }
}
