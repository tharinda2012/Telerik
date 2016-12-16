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
using CS.ObjectRepo.Company;
using CS.CommonMethods;
using System.Threading;

namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can create a new company in CS
    /// Author :Tharinda 
    /// Date: 09.12.2016
    /// </summary>
    [TestClass]
    public class CreateCompany : BaseTest
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
        public void TestMethod_Create_NewCompany()
        {
            try
            {


                // create a login object to invoke methods related to login/logout.    
                //login.Login_To_CS_Onsite();
                login.Login_To_CS();
                Utilities.Wait_CS_to_Load_Then_Invoke_NewItem(login.myManager);
                Company company = new Company(login.myManager);

                //invoke new quick request screen from main "+" button
                login.myManager.ActiveBrowser.Actions.Click(company.newItemIcon);
                login.myManager.ActiveBrowser.Actions.Click(company.newCompany);

                //add value for name
                company.companyName.Wait.ForExists();
                String compname = RandomDataGen.Random_String_Generated(10);
                login.myManager.ActiveBrowser.Actions.SetText(company.companyName, compname);

                //add value to department
                login.myManager.ActiveBrowser.Actions.SetText(company.department, "Dept-QA");

                //add value for Phone
                login.myManager.ActiveBrowser.Actions.SetText(company.phone, "123456789");

                // add value for Fax
                login.myManager.ActiveBrowser.Actions.SetText(company.fax, "2222222");

                //add value for address field 0
                login.myManager.ActiveBrowser.Actions.SetText(company.add0, "ADDR0");


                ////assign country value
                HtmlInputText countryfield = company.countryfield.As<HtmlInputText>();
                Utilities.Click_EventFor_Textfield(login.myManager, countryfield);
                Utilities.Enter_SearchStringFor_TextField(login.myManager, "bahamas");
                


                //assign priority  value

                HtmlInputText priorityfield = company.priorityfield.As<HtmlInputText>();
                Utilities.Click_EventFor_Textfield(login.myManager, priorityfield);
                Utilities.Enter_SearchStringFor_TextField(login.myManager, "High");
                
                //assign category  value
                HtmlInputText categotyfield = company.categotyfield.As<HtmlInputText>();
                Utilities.Click_EventFor_Textfield(login.myManager, categotyfield);
                Utilities.Enter_SearchStringFor_TextField(login.myManager, "Customer");
                

                //assign business  value

                HtmlInputText businessfield = company.businessfield.As<HtmlInputText>();
                Utilities.Click_EventFor_Textfield(login.myManager, businessfield);
                Utilities.Enter_SearchStringFor_TextField(login.myManager, "IT");
                


                //add a note
                login.myManager.ActiveBrowser.Actions.SetText(company.note, "TEST NOTE");
                Thread.Sleep(config.Default.SleepingTime*2);


                //save company 
                login.myManager.ActiveBrowser.Actions.Click(company.okBut);
                Thread.Sleep(config.Default.SleepingTime*2);

                //verify that the data has been saved to the database using an assert
                DBAccess con = new DBAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select name from crm7.contact where name ='" + compname + "'");
                Assert.AreEqual(compname, con.Return_Data_In_Array()[0]);//checking company is saved to the table
                con.Close_Connection();
            }

            catch (Exception e)
            {

                //saving error and logging out       
                Utilities.Save_Screenshot_withlog(login.myManager.ActiveBrowser, e, TestContext.TestName, login.myManager);
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
