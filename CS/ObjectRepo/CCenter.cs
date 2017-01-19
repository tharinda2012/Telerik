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


       
        
    }
}
