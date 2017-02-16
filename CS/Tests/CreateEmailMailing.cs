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

        
        SessionManager login = new SessionManager();
        #region [Setup / TearDown]

        private TestContext testContextInstance = null;
        /// <summary>
        ///Gets or sets the VS test context which provides
        ///information about and functionality for the
        ///current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
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

                String mailingname = "Mailing--" + Utilities.Generate_Random_String(6);
                // create a login object to invoke methods related to login/logout.    
                //login.Login_To_CS_Onsite();
                login.Login_To_CS();
                Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(login.myManager);
                email email = new email(login.myManager);
                TopMenu tm = new TopMenu(login.myManager);
                //invoke new mailing screen from main "+" button
                login.myManager.ActiveBrowser.Actions.Click(tm.newSpan);

                login.myManager.ActiveBrowser.Actions.Click(tm.newmailing);
               
                //access mailing screen in the iframe
                
                login.myManager.ActiveBrowser.RefreshDomTree();
                Thread.Sleep(config.Default.SleepingTime*3);
                ArtOfTest.WebAii.Core.Browser t1_frame = login.myManager.ActiveBrowser.Frames[0];

                ///setup stage=======================
                
                //set mailing name
                HtmlInputText mName = t1_frame.Find.ById<HtmlInputText>("mailingName");
                
                t1_frame.Actions.SetText(mName, mailingname);

                //set subject
                HtmlInputText subject = t1_frame.Find.ById<HtmlInputText>("subject");
                t1_frame.Actions.SetText(subject, mailingname);

                //set from address
                HtmlInputText from = t1_frame.Find.ById<HtmlInputText>("from");
                t1_frame.Actions.SetText(from, "tharindal@superoffice.com");


                //enter selection name for archive
                HtmlInputText archiveSelection_input = t1_frame.Find.ByName<HtmlInputText>("archiveSelection_input");
                Utilities.Click_Event_For_Textfield(t1_frame, archiveSelection_input);
                Utilities.Enter_SearchString_For_TextField(t1_frame, "email selection");
                
                //click to go to next screen
                Thread.Sleep(config.Default.SleepingTime * 2);
                HtmlButton btnNextSetup = t1_frame.Find.ById<HtmlButton>("_id_37");
                btnNextSetup.Wait.ForExists();
                btnNextSetup.Click();
               
                //template stage=========================
                
                //select a templates
                HtmlDiv Alltemplates = t1_frame.Find.ById<HtmlDiv>("HtmlSectionBar_templateExplorer_section_section_all");
                Alltemplates.Wait.ForExists();
                Alltemplates.Click();

                HtmlDiv templatefolder = t1_frame.Find.ByAttributes<HtmlDiv>("class=HtmlThumbnailList_icon");
                templatefolder.Wait.ForExists();
                templatefolder.Click();

                HtmlDiv selectedTemplate = t1_frame.Find.ByXPath<HtmlDiv>("//*[@id='templateExplorer_list_elementsDiv']/div[3]/div[1]");
                selectedTemplate.Wait.ForExists();
                selectedTemplate.Click();

                HtmlButton btnnextTemplate = t1_frame.Find.ById<HtmlButton>("_id_56");
                btnnextTemplate.Wait.ForExists();
                btnnextTemplate.Click();

                //Content stage

                HtmlButton btnNextContent = t1_frame.Find.ById<HtmlButton>("_id_77");
                btnNextContent.Wait.ForExists();
                btnNextContent.Click();

                //recipients stage
                                
                //waiting the recipient list is being generated from the selection. 
                                
                t1_frame.RefreshDomTree();
                HtmlTable recipienttable = t1_frame.Find.ByXPath<HtmlTable>("//*[@id='recipientGrid']/section/table");
                int counter0 = 0;
                while (recipienttable==null && counter0 < 10) //this will try upto 10 times before fails
                {
                    Thread.Sleep(config.Default.SleepingTime * 10);
                    counter0 += 1;
                    t1_frame.RefreshDomTree();
                    recipienttable = t1_frame.Find.ByXPath<HtmlTable>("//*[@id='recipientGrid']/section/table");
                }
                
                //checking if the recipient list is populated
                IList<HtmlTableRow> list = recipienttable.Find.AllByTagName<HtmlTableRow>("tr");
                String celltext = list[0].InnerText.ToString();
                int counter1 = 0;
                while (string.IsNullOrEmpty(celltext) && counter1 < 10) //this will try upto 10 times before fails
                {        
                                
                    Thread.Sleep(config.Default.SleepingTime * 10);
                    list = recipienttable.Find.AllByTagName<HtmlTableRow>("tr");
                    celltext = list[0].InnerText.ToString();
                    counter1 += 1;                
                }

                HtmlButton btnNextRecipients = t1_frame.Find.ById<HtmlButton>("_id_116");
                btnNextRecipients.Click();


                //sending the mailiing
                HtmlButton btnSend = t1_frame.Find.ById<HtmlButton>("_id_122");
                Thread.Sleep(config.Default.SleepingTime * 2);
                btnSend.Wait.ForExists();
                btnSend.Click(true);         
     
               
                //verify that the data has been saved to the database using an assert
                
                DBAccess con = new DBAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select description,status from crm7.s_shipment where description ='" + mailingname + "'");
                int counter2 = 0;
                while (con.Return_Data_In_Array()[1].ToString() != "1" && counter2 < 10) //this will try upto 10 times before fails
                {
                    con.Create_DBConnection(config.Default.DBProvidestringSQL);
                    con.Execute_SQLQuery("select description,status from crm7.s_shipment where description ='" + mailingname + "'");           
                    Thread.Sleep(config.Default.SleepingTime * 10);
                    counter2 += 1;
                }

                Assert.AreEqual(mailingname, con.Return_Data_In_Array()[0]); //checking mailing has been saved with the given name
                Assert.AreEqual("1", con.Return_Data_In_Array()[1]); // checking mailing status is 'finished' and complete
                con.Close_Connection();
            }

            catch (Exception e)
            {

                //saving error and failing test       
                Utilities.Save_Screenshot_with_log(login.myManager.ActiveBrowser, e, TestContext.TestName, login.myManager);
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
            SessionManager logout = new SessionManager();
            logout.Logout_From_CS(login.myManager);
            
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
