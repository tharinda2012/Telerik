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
    class CCenter
    {

        private Manager _manager;

        public CCenter(Manager m)
        {
            _manager = m;
        }

        //login submit button
        public HtmlInputSubmit btnLogin { get { return _manager.ActiveBrowser.Find.ById<HtmlInputSubmit>("doLogin"); } }

        //new request tab 
        public HtmlAnchor newRequest { get { return _manager.ActiveBrowser.Find.ByExpression<HtmlAnchor>("tagname=a", "class=menuRegisterTicket", "InnerText=New request"); } }
       
        //yourname
        public HtmlInputText yourName { get { return _manager.ActiveBrowser.Find.ByName<HtmlInputText>("custName"); } }

        //email
        public HtmlInputText email { get { return _manager.ActiveBrowser.Find.ByName<HtmlInputText>("custEmail"); } }

        //category dropdown
        public HtmlSelect category { get { return _manager.ActiveBrowser.Find.ByName<HtmlSelect>("category"); } }

        //subject
        public HtmlInputText subject { get { return _manager.ActiveBrowser.Find.ByName<HtmlInputText>("title"); } }

        //message
        public HtmlTextArea message { get { return _manager.ActiveBrowser.Find.ByName<HtmlTextArea>("message"); } }

        //submit button

        public HtmlDiv submitbutton { get { return _manager.ActiveBrowser.Find.ByExpression<HtmlDiv>("tagname=div", "class=submitButton"); } }

        //success msg
        public HtmlDiv success { get { return _manager.ActiveBrowser.Find.ById<HtmlDiv>("mainContent"); } }

    }
}
