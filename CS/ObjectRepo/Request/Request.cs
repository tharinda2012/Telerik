using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CS.ObjectRepo.Request
{
    class Request
    {
         private Manager _manager;

         public Request(Manager m)
                    {
                        _manager = m;

                    }

       
        public Element title { get { return _manager.ActiveBrowser.Find.ById("title"); } }

        //supportfield
        public Element supportfield   { get { return _manager.ActiveBrowser.Find.ById("category_label"); } }

        //message tab
        public Element messageTab { get { return _manager.ActiveBrowser.Find.ById("panes_tab_1"); } }

        public Element okBut { get { return _manager.ActiveBrowser.Find.ById("_id_4"); } }

        //delete request elements
        //action menu
        public HtmlSpan actionmenu { get { return _manager.ActiveBrowser.Find.ByAttributes<HtmlSpan>("class=HtmlIconDropDown_contextMenu"); } }

        //edit request menu
        public HtmlSpan editrequest { get { return _manager.ActiveBrowser.Find.ByXPath<HtmlSpan>("//*[@id='HtmlPageDropDown_menuItems']/a[4]/span"); } }

        //delete button
        public HtmlButton btnDelete { get { return _manager.ActiveBrowser.Find.ById<HtmlButton>("_id_7"); ; } }

        //recipient

        public HtmlInputText recipient
        {
            get { return _manager.ActiveBrowser.Find.ByExpression<HtmlInputText>("TabIndex=5"); }
        }
    }
}
