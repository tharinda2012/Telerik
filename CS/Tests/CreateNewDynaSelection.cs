using System;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.TestTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.CommonMethods;
using CS.ObjectRepo.Selection;
using System.Threading;
using CS.ObjectRepo;

namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can create a dynamic selection in CS
    /// Author :Tharinda Liyanage..
    /// Date: 11.01.2017
    /// </summary>
    [TestClass]
    public class CreateDynamicSelection : BaseTest
    {
        readonly SessionManager _login = new SessionManager();
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
        public void TestMethod_Create_DynamicSelection()
        {

            try
            {

               //create a login object to invoke methods related to login/logout.    
               //login.Login_To_CS_Onsite();
                _login.Login_To_CS(false);

               //invoke new quick request screen from main "+" button                
               Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(_login.MyManager);      
               _login.MyManager.ActiveBrowser.RefreshDomTree();
               var selection = new Selection(_login.MyManager);
               var tm = new TopMenu(_login.MyManager);
               _login.MyManager.ActiveBrowser.RefreshDomTree();
               tm.newSpan.Wait.ForExists();
               _login.MyManager.ActiveBrowser.Actions.Click(tm.newSpan);
               _login.MyManager.ActiveBrowser.Actions.Click(tm.newSelection);


                

                //select source table as "request"
                var sourcetable = selection.sourcetable.As<HtmlInputText>();
                Utilities.Click_Event_For_Textfield(_login.MyManager, sourcetable);
                Utilities.Enter_SearchString_For_TextField(_login.MyManager, "request");
               

                

                //click continue button
                _login.MyManager.ActiveBrowser.Actions.Click(selection.btnContinue);

                //set selection title                
                var title = "Sel_" + Utilities.Generate_Random_String(10);
                _login.MyManager.ActiveBrowser.Actions.SetText(selection.title, title);

                //click execute button                
                _login.MyManager.ActiveBrowser.Actions.Click(selection.btnExecute);

                Thread.Sleep(config.Default.SleepingTime*2);

                //save the selection
                _login.MyManager.ActiveBrowser.Actions.Click(selection.btnOk);


                //verify that the data has been saved to the database using an assert
                var con = new DbAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select name, ej_table,last_count from crm7.ejselection  where name ='" + title + "' ");         
                Assert.AreEqual(title, con.Return_Data_In_Array()[0],"Selection was not saved");//checking selection is saved to the table
                Assert.AreEqual("ticket", con.Return_Data_In_Array()[1],"selection source table is not Ticket"); //checking selection source table is Ticket'
                var lastCount = Int32.Parse(con.Return_Data_In_Array()[2].ToString());
                if (lastCount<=0)
                {
                    Assert.Fail("Selection is empty"); //checking if selection is empty
                }
                
                
                con.Close_Connection();
                           
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
