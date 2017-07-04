using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.Controls.HtmlControls.HtmlAsserts;
using ArtOfTest.WebAii.Core;
using ArtOfTest.WebAii.ObjectModel;
using ArtOfTest.WebAii.TestAttributes;
using ArtOfTest.WebAii.TestTemplates;
using ArtOfTest.WebAii.Win32.Dialogs;
using ArtOfTest.WebAii.Silverlight;
using ArtOfTest.WebAii.Silverlight.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.ObjectRepo.Mailing;
using CS.CommonMethods;
using System.Threading;
using CS.ObjectRepo;

namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can perform a email mailing successfully in CS
    /// Author :Tharinda 
    /// Date: 09.12.2016
    /// </summary>
    [TestClass]
    public class CreateEmailMailing : BaseTest
    {
        readonly SessionManager _login = new SessionManager();
        #region [Setup / TearDown]

        private TestContext _testContextInstance = null;
        /// <summary>
        ///Gets or sets the VS test context which provides
        ///information about and functionality for the
        ///current test run.
        ///</summary>
        private TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }


        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        
        // Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            #region WebAii Initialization

            // Initializes WebAii manager to be used by the test case.
            // If a WebAii configuration section exists, settings will be
            // loaded from it. Otherwise, will create a default settings
            // object with system defaults.
            //
            // Note: We are passing in a delegate to the VisualStudio
            // testContext.WriteLine() method in addition to the Visual Studio
            // TestLogs directory as our log location. This way any logging
            // done from WebAii (i.e. Manager.Log.WriteLine()) is
            // automatically logged to the VisualStudio test log and
            // the WebAii log file is placed in the same location as VS logs.
            //
            // If you do not care about unifying the log, then you can simply
            // initialize the test by calling Initialize() with no parameters;
            // that will cause the log location to be picked up from the config
            // file if it exists or will use the default system settings (C:\WebAiiLog\)
            // You can also use Initialize(LogLocation) to set a specific log
            // location for this test.

            // Pass in 'true' to recycle the browser between test methods
            Initialize(false, this.TestContext.TestLogsDir, new TestContextWriteLine(this.TestContext.WriteLine));

            // If you need to override any other settings coming from the
            // config section you can comment the 'Initialize' line above and instead
            // use the following:

            /*

            // This will get a new Settings object. If a configuration
            // section exists, then settings from that section will be
            // loaded

            Settings settings = GetSettings();

            // Override the settings you want. For example:
            settings.Web.DefaultBrowser = BrowserType.FireFox;

            // Now call Initialize again with your updated settings object
            Initialize(settings, new TestContextWriteLine(this.TestContext.WriteLine));

            */

            // Set the current test method. This is needed for WebAii to discover
            // its custom TestAttributes set on methods and classes.
            // This method should always exist in [TestInitialize()] method.
            SetTestMethod(this, (string)TestContext.Properties["TestName"]);

            #endregion

            //
            // Place any additional initialization here
            //

        }

        [TestMethod]
        public void TestMethod_Create_EmailMailing()
        {
            try
            {

                var mailingname = "Mailing--" + Utilities.Generate_Random_String(6);
                // create a login object to invoke methods related to login/logout.    
                //login.Login_To_CS_Onsite();
                _login.Login_To_CS(false);
                Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(_login.MyManager);
                var email = new email(_login.MyManager);
                var tm = new TopMenu(_login.MyManager);
                //invoke new mailing screen from main "+" button
                _login.MyManager.ActiveBrowser.Actions.Click(tm.newSpan);

                _login.MyManager.ActiveBrowser.Actions.Click(tm.newmailing);
               
                //access mailing screen in the iframe
                
                _login.MyManager.ActiveBrowser.RefreshDomTree();
                Thread.Sleep(config.Default.SleepingTime*3);
                var t1Frame = _login.MyManager.ActiveBrowser.Frames[0];

                //setup stage=======================
                
                //set mailing name
                var mName = t1Frame.Find.ById<HtmlInputText>("mailingName");
                
                t1Frame.Actions.SetText(mName, mailingname);

                //set subject
                var subject = t1Frame.Find.ById<HtmlInputText>("subject");
                t1Frame.Actions.SetText(subject, mailingname);

                //set from address
                var from = t1Frame.Find.ById<HtmlInputText>("from");
                t1Frame.Actions.SetText(from, "tharindal@superoffice.com");


                //enter selection name for archive
                var archiveSelectionInput = t1Frame.Find.ByName<HtmlInputText>("archiveSelection_input");
                Utilities.Click_Event_For_Textfield(t1Frame, archiveSelectionInput);
                Utilities.Enter_SearchString_For_TextField(t1Frame, "email selection");
                
                //click to go to next screen
                Thread.Sleep(config.Default.SleepingTime * 2);
                var btnNextSetup = t1Frame.Find.ById<HtmlButton>("_id_37");
                btnNextSetup.Wait.ForExists();
                btnNextSetup.Click();
               
                //template stage=========================
                
                //select a templates
                var alltemplates = t1Frame.Find.ById<HtmlDiv>("HtmlSectionBar_templateExplorer_section_section_all");
                alltemplates.Wait.ForExists();
                alltemplates.Click();

                var templatefolder = t1Frame.Find.ByAttributes<HtmlDiv>("class=HtmlThumbnailList_icon");
                templatefolder.Wait.ForExists();
                templatefolder.Click();

                var selectedTemplate = t1Frame.Find.ByXPath<HtmlDiv>("//*[@id='templateExplorer_list_elementsDiv']/div[3]/div[1]");
                selectedTemplate.Wait.ForExists();
                selectedTemplate.Click();

                var btnnextTemplate = t1Frame.Find.ById<HtmlButton>("_id_56");
                btnnextTemplate.Wait.ForExists();
                btnnextTemplate.Click();

                //Content stage

                var btnNextContent = t1Frame.Find.ById<HtmlButton>("_id_77");
                btnNextContent.Wait.ForExists();
                btnNextContent.Click();

                //recipients stage
                                
                //waiting the recipient list is being generated from the selection. 
                                
                t1Frame.RefreshDomTree();
                var recipienttable = t1Frame.Find.ByXPath<HtmlTable>("//*[@id='recipientGrid']/section/table");
                var counter0 = 0;
                while (recipienttable==null && counter0 < 10) //this will try upto 10 times before fails
                {
                    Thread.Sleep(config.Default.SleepingTime * 10);
                    counter0 += 1;
                    t1Frame.RefreshDomTree();
                    recipienttable = t1Frame.Find.ByXPath<HtmlTable>("//*[@id='recipientGrid']/section/table");
                }
                
                //checking if the recipient list is populated
                if (recipienttable != null)
                {
                    IList<HtmlTableRow> list = recipienttable.Find.AllByTagName<HtmlTableRow>("tr");
                    var celltext = list[0].InnerText.ToString();
                    var counter1 = 0;
                    while (string.IsNullOrEmpty(celltext) && counter1 < 10) //this will try upto 10 times before fails
                    {        
                                
                        Thread.Sleep(config.Default.SleepingTime * 10);
                        list = recipienttable.Find.AllByTagName<HtmlTableRow>("tr");
                        celltext = list[0].InnerText.ToString();
                        counter1 += 1;                
                    }
                }

                var btnNextRecipients = t1Frame.Find.ById<HtmlButton>("_id_116");
                btnNextRecipients.Click();


                //sending the mailiing
                var btnSend = t1Frame.Find.ById<HtmlButton>("_id_122");
                Thread.Sleep(config.Default.SleepingTime * 2);
                btnSend.Wait.ForExists();
                btnSend.Click(true);         
     
               
                //verify that the data has been saved to the database using an assert
                
                var con = new DbAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select description,status from crm7.s_shipment where description ='" + mailingname + "'");
                var counter2 = 0;
                while (con.Return_Data_In_Array()[1].ToString() != "1" && counter2 < 10) //this will try upto 10 times before fails
                {
                    con.Create_DBConnection(config.Default.DBProvidestringSQL);
                    con.Execute_SQLQuery("select description,status from crm7.s_shipment where description ='" + mailingname + "'");           
                    Thread.Sleep(config.Default.SleepingTime * 10);
                    counter2 += 1;
                }

                Assert.AreEqual(mailingname, con.Return_Data_In_Array()[0],"Mailing was not saved"); //checking mailing has been saved with the given name
                Assert.AreEqual("1", con.Return_Data_In_Array()[1],"mailing is not finished"); // checking mailing status is 'finished' and complete
                con.Close_Connection();
            }

            catch (Exception e)
            {

                //saving error and failing test       
                Utilities.Save_Screenshot_with_log(_login.MyManager.ActiveBrowser, e, TestContext.TestName);
                Assert.Fail();
                
            }

        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

            //
            // Place any additional cleanup here
            //
            
            SessionManager.Logout_From_CS(_login.MyManager);
            
            #region WebAii CleanUp

            // Shuts down WebAii manager and closes all browsers currently running
            // after each test. This call is ignored if recycleBrowser is set
            this.CleanUp();

            #endregion
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            // This will shut down all browsers if
            // recycleBrowser is turned on. Else
            // will do nothing.
            ShutDown();
        }

        #endregion

    }
}
