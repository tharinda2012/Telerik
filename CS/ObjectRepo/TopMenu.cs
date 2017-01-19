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
    class TopMenu
    {

        private Manager _manager;

        public TopMenu(Manager m)
        {
            _manager = m;
        }

        // Main + add button in the top area of the home screen
        public Element newItemIcon { get { return _manager.ActiveBrowser.Find.ById("HtmlPage_newItem"); } }

        //New selection menu item
        public Element newSelection{ get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_addSelection']/span"); } }

        //New Request menu item
        public Element newRequest { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newTicket']/span"); } }

        //New Qrequest menu item
        public Element newQuickRequest { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newQuickTicket']/span"); } }

        //New Company menu item
        public Element newCompany { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newCompany']/span"); } }

        //New person menu item
        public Element newPerson { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newCustomer']/span"); } }

        //New mailing menu item
        public Element newmailing { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newEmailMailing']/span"); } } 
        
        //New FAQ menu item
        public Element newFAQ { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_editKbEntry']/span"); } }

        //admin cogwheel
        public Element AdmincogWheel { get { return _manager.ActiveBrowser.Find.ById("HtmlPage_adminMenuImage"); } }

        //admin-email
        public Element adminemail { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='area_admin_listFilters']/span"); } }

        //admin-customer center pages
        public Element admincustcenterpages { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='area_admin_listCustomerCenterTemplates']/span"); } }
    }

}
