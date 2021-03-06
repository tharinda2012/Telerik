using System;
using ArtOfTest.WebAii.Controls.HtmlControls;
using ArtOfTest.WebAii.TestTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.ObjectRepo.Customer;
using CS.CommonMethods;
using System.Threading;
using CS.ObjectRepo;
namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can create a new person in CS
    /// Author :Tharinda 
    /// Date: 09.12.2016
    /// </summary>
    [TestClass]
    public class CreatePerson : BaseTest
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
        public void TestMethod_Create_Person()
        {

            try
            {

                // create a login object to invoke methods related to login/logout.    
                //login.Login_To_CS_Onsite();
                _login.Login_To_CS(false);
                var person = new Person(_login.MyManager);

                Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(_login.MyManager);
                var company = new Company(_login.MyManager);
                var tm = new TopMenu(_login.MyManager);
                //invoke new quick request screen from main "+" button
                _login.MyManager.ActiveBrowser.RefreshDomTree();
                tm.newSpan.Wait.ForExists();
                _login.MyManager.ActiveBrowser.Actions.Click(tm.newSpan);
                _login.MyManager.ActiveBrowser.Actions.Click(tm.newPerson);

                //add first and last names
                person.firstname.Wait.ForExists();
                var fname = "Person--" + Utilities.Generate_Random_String(6);
                _login.MyManager.ActiveBrowser.Actions.SetText(person.firstname, fname);

                _login.MyManager.ActiveBrowser.Actions.SetText(person.lastname, "LASTNAME");

                Thread.Sleep(config.Default.SleepingTime*2);
                //create a company to accomodate the contact
                var newcomp = person.newCompany.As<HtmlImage>();
                newcomp.Wait.ForExists();
                newcomp.MouseClick();

                //new company screen opnes in an iFrame
                _login.MyManager.ActiveBrowser.RefreshDomTree();
                Thread.Sleep(config.Default.SleepingTime*3);
                var t1Frame = _login.MyManager.ActiveBrowser.Frames[0];

                //add data to the new company
                var compname = t1Frame.Find.ById<HtmlInputText>("name");
                var cname = Utilities.Generate_Random_String(10);
                compname.Wait.ForExists();
                t1Frame.Actions.SetText(compname, cname);
                var department = t1Frame.Find.ById<HtmlInputText>("department");
                t1Frame.Actions.SetText(department, "DEPT-QA");
                var phone = t1Frame.Find.ById<HtmlInputText>("phone");
                t1Frame.Actions.SetText(phone, "123456789");
                var fax = t1Frame.Find.ById<HtmlInputText>("fax");
                t1Frame.Actions.SetText(fax, "4444444444");
                var okbtn = t1Frame.Find.ById<HtmlButton>("_id_41");
                //save company
                okbtn.Wait.ForExists();
                okbtn.MouseClick();
                Thread.Sleep(config.Default.SleepingTime*4);

                //finally saving person
                _login.MyManager.ActiveBrowser.RefreshDomTree();
                _login.MyManager.ActiveBrowser.Actions.Click(person.okBut);
                //verify that the data has been saved to the database using an assert
                var con = new DbAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select person.firstname, contact.name from crm7.person inner join crm7.contact on person.contact_id=contact.contact_id where person.firstname='" + fname + "'");
                Assert.AreEqual(fname, con.Return_Data_In_Array()[0],"Person is not saved");//checking person issaved
                Assert.AreEqual(cname, con.Return_Data_In_Array()[1],"Company is not saved");//checking company is saved
                con.Close_Connection();
            }

            catch (Exception e)
            {

                //saving error and logging out       
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
