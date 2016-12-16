using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CS.ObjectRepo.Company
{
    class Company
    {

         private Manager _manager;

         public Company(Manager m)
        {
            _manager = m;
        }

         // Main + add button in the top area of the home screen
         public Element newItemIcon
         { get { return _manager.ActiveBrowser.Find.ById("HtmlPage_newItem"); } }

         //Company menu item
         public Element newCompany
         { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newCompany']/span"); } }

         public Element companyName { get { return _manager.ActiveBrowser.Find.ById("name"); } }
         public Element department { get { return _manager.ActiveBrowser.Find.ById("department"); } }
         public Element phone { get { return _manager.ActiveBrowser.Find.ById("phone"); } }
         public Element fax { get { return _manager.ActiveBrowser.Find.ById("fax"); } }
         public Element add0 { get { return _manager.ActiveBrowser.Find.ById("adr_0_0"); } }
         
        //country field
        public Element countryfield { get { return _manager.ActiveBrowser.Find.ById("country_label"); } }
        //priority field
        public Element priorityfield { get { return _manager.ActiveBrowser.Find.ById("priority_label"); } }
        //category field
        public Element categotyfield { get { return _manager.ActiveBrowser.Find.ById("contactCategory_label"); } }
        //business field
        public Element businessfield { get { return _manager.ActiveBrowser.Find.ById("contactBusiness_label"); } }
       
        
        
        public Element note { get { return _manager.ActiveBrowser.Find.ById("note"); } }
        public Element okBut { get { return _manager.ActiveBrowser.Find.ById("_id_40"); } }

        //dropdown hidden fields
        //public Element country { get { return _manager.ActiveBrowser.Find.ById("country"); } }
        //public Element priority { get { return _manager.ActiveBrowser.Find.ById("priority"); } }
        //public Element contactCategory { get { return _manager.ActiveBrowser.Find.ById("contactCategory"); } }
        //public Element contactBusiness { get { return _manager.ActiveBrowser.Find.ById("contactBusiness"); } }


        
    }
}
