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
    class Mobile
    {

        private Manager _manager;

        public Mobile(Manager m)
        {
            _manager = m;
        }

        //elements in CS for Mobile page
        public Element compactString { get { return _manager.ActiveBrowser.Find.ByExpression("class=application-name","InnerText=CS Compact Mode"); } }
        
        //create new request  + button
        public Element addNew { get { return _manager.ActiveBrowser.Find.ByExpression("class=ui-btn ui-btn-icon-right ui-icon-plus"); } }

        // request title
        public Element title { get { return _manager.ActiveBrowser.Find.ById("editTicket_title"); } }

        //request message
        public Element message { get { return _manager.ActiveBrowser.Find.ById("editTicket_message"); } }

        //ok button
        public Element btnOK { get { return _manager.ActiveBrowser.Find.ByExpression("class=ui-btn ui-shadow"); } }

        //verify title

        public Element verifyTitle { get { return _manager.ActiveBrowser.Find.ByExpression("class=ui-collapsible-heading-toggle ui-btn ui-icon-plus ui-btn-icon-left ui-btn-b"); } }


    }
}
