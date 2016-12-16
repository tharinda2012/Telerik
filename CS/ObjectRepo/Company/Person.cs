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
    class Person
    {
        private Manager _manager;

         public Person(Manager m)
        {
            _manager = m;
        }

         // Main + add button in the top area of the home screen
         public Element newItemIcon { get { return _manager.ActiveBrowser.Find.ById("HtmlPage_newItem"); } }

         //person menu item
         public Element newPerson { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newCustomer']/span"); } }

        //firstname
         public Element firstname { get { return _manager.ActiveBrowser.Find.ById("firstname"); } }

         //lastname
         public Element lastname { get { return _manager.ActiveBrowser.Find.ById("lastname"); } }

        //create new company
         public Element newCompany { get { return _manager.ActiveBrowser.Find.ByAttributes("class=HtmlSelectRelation2_button"); } }

        public Element okBut { get { return _manager.ActiveBrowser.Find.ById("_id_36"); } }

       
        //controls for find person 



    }
}



