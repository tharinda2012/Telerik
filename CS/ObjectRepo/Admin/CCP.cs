using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.ObjectRepo.Admin
{
    class CCP  //class to hold elements in customer center pages screen
    {

         private Manager _manager;

         public CCP(Manager m)
        {
            _manager = m;
        }

         //new mailbox icon
         public Element linktoCC { get { return _manager.ActiveBrowser.Find.ById("first_element"); } }
       
    }

}
