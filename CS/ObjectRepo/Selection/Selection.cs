using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.ObjectRepo.Selection
{
    class Selection
    {

         private Manager _manager;

         public Selection(Manager m)
        {
            _manager = m;
        }

        

        //continue button
         public Element btnContinue { get { return _manager.ActiveBrowser.Find.ById("_id_15"); } }

        //selection title
        public Element title { get { return _manager.ActiveBrowser.Find.ById("name"); } }

        //execute button
        public Element btnExecute { get { return _manager.ActiveBrowser.Find.ById("_id_77"); } }

        //selection save button
        public Element btnOk { get { return _manager.ActiveBrowser.Find.ById("_id_76"); } }
    
        //source table field

        public Element sourcetable { get { return _manager.ActiveBrowser.Find.ByName("sourceTable_input"); } }
    }

}
