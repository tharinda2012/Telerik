using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.ObjectRepo.WebTools
{
    class WebTools
    {
        private Manager _manager;

        public WebTools(Manager m)
                    {
                        _manager = m;

                    }

        public Element SharedComputer { get { return _manager.ActiveBrowser.Find.ById("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheFirstLoadDialog_Page1SharedComputerText"); } }

        public Element WTNextBtn { get { return _manager.ActiveBrowser.Find.ById("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheSplashScreen_btnNext"); } }
        public Element WTCloseBtn { get { return _manager.ActiveBrowser.Find.ById("_ctl0__ctl0_MasterMessageBoxPlaceHolder_MessageBoxPlaceHolder_TheSplashScreen_btnClose"); } }



    }
}
