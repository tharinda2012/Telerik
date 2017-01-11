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
    class MailBox
    {

         private Manager _manager;

         public MailBox(Manager m)
        {
            _manager = m;
        }

        //new mailbox icon
         public Element newMailBox { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='HtmlAnchorLine__id_9']/ul/li[1]/a/span"); } }

        //email address
         public Element address { get { return _manager.ActiveBrowser.Find.ById("address_address"); } }

        //category
         public Element cateogry { get { return _manager.ActiveBrowser.Find.ById("category_label"); } }

        //priority
         public Element priority { get { return _manager.ActiveBrowser.Find.ById("priority_label"); } }

        //save
         public Element btnOK { get { return _manager.ActiveBrowser.Find.ById("_id_35"); } }
       
    }

}
