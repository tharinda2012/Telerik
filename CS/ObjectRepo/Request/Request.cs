﻿using ArtOfTest.WebAii.Controls.HtmlControls;
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

        // Main + add button in the top area of the home screen
        public Element newItemIcon
        { get { return _manager.ActiveBrowser.Find.ById("HtmlPage_newItem"); } }

        //quick request 
        public Element newRequest
        { get { return _manager.ActiveBrowser.Find.ByXPath("//*[@id='newItem_newTicket']/span"); } }

        public Element title
        { get { return _manager.ActiveBrowser.Find.ById("title"); } }

        //supportfield
        public Element supportfield
        { get { return _manager.ActiveBrowser.Find.ById("category_label"); } }

        //message tab
        public Element messageTab
        { get { return _manager.ActiveBrowser.Find.ById("panes_tab_1"); } }



        public Element okBut
        { get { return _manager.ActiveBrowser.Find.ById("_id_4"); } }
    }
}