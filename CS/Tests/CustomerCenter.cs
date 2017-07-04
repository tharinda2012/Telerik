using System;
using ArtOfTest.WebAii.TestTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.CommonMethods;
using CS.ObjectRepo.Admin;
using System.Threading;
using CS.ObjectRepo;



namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can invoke 'Customer Center' in a new browser, by clicking the link in 'customer center pages' screen in CS and create a request there
    /// Author :Tharinda Liyanage..
    /// Date: 19.01.2017
    /// </summary>
    [TestClass]
    public class CustomerCenter : BaseTest
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
        public void TestMethod_Create_Request_In_CustomerCenter()
        {

            try
            {

               //create a login object to invoke methods related to login/logout.    
                _login.Login_To_CS(false);       
            
               //click the admin cogwheel
               Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(_login.MyManager);      
               _login.MyManager.ActiveBrowser.RefreshDomTree();
               var tm = new TopMenu(_login.MyManager);
               _login.MyManager.ActiveBrowser.RefreshDomTree();
               tm.newSpan.Wait.ForExists();               
               _login.MyManager.ActiveBrowser.Actions.Click(tm.AdmincogWheel);                              
               //select customer center pages menu
               _login.MyManager.ActiveBrowser.Actions.Click(tm.admincustcenterpages);

                //click CC link
               var cc = new CCP(_login.MyManager);

               //handle new browser window for CC
               _login.MyManager.SetNewBrowserTracking(true);
               _login.MyManager.ActiveBrowser.Actions.Click(cc.linktoCC);
               _login.MyManager.SetNewBrowserTracking(false);

               var browsercount = _login.MyManager.Browsers.Count;

               //try to maximise browser if its not maximised.
               if (browsercount == 3)
               {
                   _login.MyManager.Browsers[2].Window.SetFocus();
                   _login.MyManager.Browsers[2].Window.Maximize();
                   Thread.Sleep(config.Default.SleepingTime * 3);
               }
               
               _login.MyManager.ActiveBrowser.RefreshDomTree();
               var ccenter = new CCenter(_login.MyManager);  
 
                //asserting that the 'login' button is available in the page whihc confirms that the test has land on 'Customer Center' page
               //Assert.AreEqual("Login", ccenter.btnLogin.Value.ToString()); 
       
                //click new request tab and add a new request and save the request
               ccenter.newRequest.Wait.ForExists();
               ccenter.newRequest.Click();
               ccenter.yourName.Text = "Tharinda";
               ccenter.email.Text = "tharindal@99x.lk";
               ccenter.category.SelectByValue("1");
               ccenter.subject.Text="Customer Center Request";
               ccenter.message.Text="CC request message";
               
               //ccenter.btnOk.MouseClick();
               ccenter.submitbutton.MouseClick();

               Thread.Sleep(config.Default.SleepingTime * 2);
               _login.MyManager.ActiveBrowser.RefreshDomTree();

               //check if the request is created by verifing that the string "Your inquiry has been registered..." appears in the following screen.
               Assert.IsTrue(ccenter.success.TextContent.Contains("Your inquiry has been registered with request ID"),"Request in Customer Center was not created successfully");
               
                           
            }
            catch (Exception error)
            {
                //saving error and logging out       
                Utilities.Save_Screenshot_with_log(_login.MyManager.ActiveBrowser, error, TestContext.TestName);
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
