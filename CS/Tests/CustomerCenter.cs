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
using CS.ObjectRepo.Admin;
using System.Threading;
using CS.ObjectRepo;



namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can create anew mailbox and send a mail to it and verify a request gets created out of it
    /// Author :Tharinda Liyanage..
    /// Date: 11.01.2017
    /// </summary>
    [TestClass]
    public class CustomerCenter : BaseTest
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
        public void TestMethod_ViewCustomerCenter()
        {

            try
            {

               //create a login object to invoke methods related to login/logout.    
               login.Login_To_CS();       
            
               //click the admin cogwheel
               Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(login.myManager);      
               login.myManager.ActiveBrowser.RefreshDomTree();
               TopMenu tm = new TopMenu(login.myManager);
               tm.newItemIcon.Wait.ForExists();
               login.myManager.ActiveBrowser.Actions.Click(tm.AdmincogWheel);

                              
               //select customer center pages menu
               login.myManager.ActiveBrowser.Actions.Click(tm.admincustcenterpages);

                //click CC link
               CCP cc = new CCP(login.myManager);

               //handle new browser window for CC
               login.myManager.SetNewBrowserTracking(true);
               login.myManager.ActiveBrowser.Actions.Click(cc.linktoCC);
               login.myManager.SetNewBrowserTracking(false);

               int browsercount = login.myManager.Browsers.Count;

               //try to maximise browser if its not maximised.
               if (browsercount == 3)
               {
                   login.myManager.Browsers[2].Window.SetFocus();
                   login.myManager.Browsers[2].Window.Maximize();
                   Thread.Sleep(config.Default.SleepingTime * 2);
               }
               
               login.myManager.ActiveBrowser.RefreshDomTree();
               CCenter ccenter = new CCenter(login.myManager);  
 
                //asserting that the 'login' button is available in the page whihc confirms that the test has land on 'Customer Center' page
               Assert.AreEqual("Login", ccenter.btnLogin.Value.ToString());        
                           
            }
            catch (Exception error)
            {
                //saving error and logging out       
                Utilities.Save_Screenshot_withlog(login.myManager.ActiveBrowser, error, TestContext.TestName, login.myManager);
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
