using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.ObjectRepo.Reqeust
{
    class QReqeust
    {
        private Manager _manager;

        public QReqeust(Manager m)
                    {
                        _manager = m;

                    }
       
        public Element title
        { get { return _manager.ActiveBrowser.Find.ById("title"); } }

        //category_label

        public Element categorylabel
        { get { return _manager.ActiveBrowser.Find.ById("category_label"); } }

        //message text
        public Element message
        { get { return _manager.ActiveBrowser.Find.ById("message"); } }

        public Element okButton
        { get { return _manager.ActiveBrowser.Find.ByAttributes("class=HtmlSubmitButton HtmlSubmitButton_green"); } }

    }
}
