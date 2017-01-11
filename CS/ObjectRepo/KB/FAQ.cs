using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.ObjectRepo.KB
{
    class FAQ
    {

         private Manager _manager;

         public FAQ(Manager m)
        {
            _manager = m;
        }
                      

        //faq name
         public Element faqname { get { return _manager.ActiveBrowser.Find.ById("title"); } }

        //access field
         public Element access { get { return _manager.ActiveBrowser.Find.ById("access_label"); } }

        //keywords
         public Element keyword { get { return _manager.ActiveBrowser.Find.ById("keywords"); } }

        //workflow
         public Element workflow { get { return _manager.ActiveBrowser.Find.ById("workflow_label"); } }

        //question tab
         public Element questiontab { get { return _manager.ActiveBrowser.Find.ById("panes_tab_1"); } }

        //answer tab
         public Element answertab { get { return _manager.ActiveBrowser.Find.ById("panes_tab_2"); } }

        //save OK button
         public Element btnOK { get { return _manager.ActiveBrowser.Find.ById("_id_60"); } }

        //move forward workflow

         public Element moveForwardWF { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='HtmlAnchorLine__id_8']/ul/li[3]/a/span"); } }
    }
 }


