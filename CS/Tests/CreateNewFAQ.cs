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
using CS.CommonMethods;
using CS.ObjectRepo.KB;
using System.Threading;
using CS.ObjectRepo;

namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can create a quick new FAQ in CS
    /// Author :Tharinda Liyanage..
    /// Date: 11.01.2017
    /// </summary>
    [TestClass]
    public class CreateFAQ : BaseTest
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
        public void TestMethod_Create_FAQ()
        {

            try
            {

               //create a login object to invoke methods related to login/logout.    
               //login.Login_To_CS_Onsite();
                login.Login_To_CS(false);

               //invoke new quick request screen from main "+" button                
               Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(login.myManager);      
               login.myManager.ActiveBrowser.RefreshDomTree();
               FAQ faq = new FAQ(login.myManager);
               TopMenu tm = new TopMenu(login.myManager);
               login.myManager.ActiveBrowser.RefreshDomTree();
               tm.newSpan.Wait.ForExists();
               login.myManager.ActiveBrowser.Actions.Click(tm.newSpan);
               login.myManager.ActiveBrowser.Actions.Click(tm.newFAQ);

               //Add faq name in properties tab
                              
               String title = "FAQ_" + Utilities.Generate_Random_String(10);
               login.myManager.ActiveBrowser.Actions.SetText(faq.faqname, title);

                //set access to everyone
               HtmlInputText access = faq.access.As<HtmlInputText>();
               Utilities.Click_Event_For_Textfield(login.myManager, access);
               Utilities.Enter_SearchString_For_TextField(login.myManager, "Accessible to everyone");

               //set keyword
               login.myManager.ActiveBrowser.Actions.SetText(faq.keyword,title);

                //set work flow to publish
                HtmlInputText workflow = faq.workflow.As<HtmlInputText>();
                Utilities.Click_Event_For_Textfield(login.myManager, workflow);
                Utilities.Enter_SearchString_For_TextField(login.myManager, "Published");

                
                //add question to iframe element in question tab
                login.myManager.ActiveBrowser.Actions.Click(faq.questiontab);
                login.myManager.ActiveBrowser.RefreshDomTree();
                ArtOfTest.WebAii.Core.Browser t1_frame = login.myManager.ActiveBrowser.Frames[0];
                Element questioneditor = t1_frame.Find.ByXPath("/html/body");
                login.myManager.ActiveBrowser.Actions.SetText(questioneditor, title);


                //add answer to iframe element in answer tab
                login.myManager.ActiveBrowser.Actions.Click(faq.answertab);
                login.myManager.ActiveBrowser.RefreshDomTree();
                ArtOfTest.WebAii.Core.Browser t2_frame = login.myManager.ActiveBrowser.Frames[0];
                Element answereditor = t2_frame.Find.ByXPath("/html/body");
                login.myManager.ActiveBrowser.Actions.SetText(answereditor, title);

                //save the faq
                Thread.Sleep(config.Default.SleepingTime * 1);
                login.myManager.ActiveBrowser.Actions.Click(faq.btnOK);    
                //publish the faq
                Thread.Sleep(config.Default.SleepingTime * 1);
                login.myManager.ActiveBrowser.Actions.Click(faq.moveForwardWF); 

     

                //verify that the data has been saved to the database using an assert
                DBAccess con = new DBAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select title,keywords,access_level,status from crm7.kb_entry  where title ='" + title + "' ");         
                Assert.AreEqual(title, con.Return_Data_In_Array()[0]);//checking faq is saved to the table
                Assert.AreEqual(title, con.Return_Data_In_Array()[1]); //checking keyword is saved
                Assert.AreEqual("4", con.Return_Data_In_Array()[2]); //checking access level is 4 = accessible to all
                Assert.AreEqual("1", con.Return_Data_In_Array()[3]); //checking status is 1 = published
                con.Close_Connection();
                           
            }
            catch (Exception error)
            {
                //saving error and logging out       
                Utilities.Save_Screenshot_with_log(login.myManager.ActiveBrowser, error, TestContext.TestName, login.myManager);
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
