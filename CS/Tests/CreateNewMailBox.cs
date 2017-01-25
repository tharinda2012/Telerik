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
    public class CreateMailBox : BaseTest
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
        public void TestMethod_Create_MailBox()
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

               //select email from the menu
               login.myManager.ActiveBrowser.Actions.Click(tm.adminemail);
               
               //mailbox creation
               MailBox mb=new MailBox(login.myManager);
               login.myManager.ActiveBrowser.Actions.Click(mb.newMailBox);
               String title = "Mail_" + Utilities.Random_String_Generated(6);
               
               //add address name
               HtmlInputText address = mb.address.As<HtmlInputText>();
               Utilities.Click_Event_For_Textfield(login.myManager, address);
               Utilities.Enter_SearchString_For_TextField(login.myManager, title);

               //set category
               HtmlInputText category = mb.cateogry.As<HtmlInputText>();
               Utilities.Click_Event_For_Textfield(login.myManager, category);
               Utilities.Enter_SearchString_For_TextField(login.myManager, "Support");
               
               //set priority
               HtmlInputText priority = mb.priority.As<HtmlInputText>();
               //Utilities.Click_EventFor_Textfield(login.myManager, priority);
               Utilities.Enter_SearchString_For_TextField(login.myManager, "High");

                //retrieve email address
                String fulladdress= address.Text;

               // save the mailbox
               login.myManager.ActiveBrowser.Actions.Click(mb.btnOK);
               Thread.Sleep(config.Default.SleepingTime * 3); 

                //verify that the data has been saved to the database using an assert
                DBAccess con = new DBAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select address,category_id,priority from crm7.MAIL_IN_FILTER where address = '" + fulladdress + "' ");
                Assert.AreEqual(fulladdress, con.Return_Data_In_Array()[0]);//checking mailbox is saved to the table
                Assert.AreEqual("1", con.Return_Data_In_Array()[1]); //checking category 1 = support
                Assert.AreEqual("3", con.Return_Data_In_Array()[2]); //checking priority 3 = high

                con.Close_Connection();

                //send mail to the new address
                Utilities.Send_Mail("tharindal@99x.lk", fulladdress, title, "this is my test email body", "SRI-QA-FILESERVER");

                //check if the request is generated by CS from the mail sent above
                DBAccess con2 = new DBAccess();
                con2.Create_DBConnection(config.Default.DBProvidestringSQL);
                con2.Execute_SQLQuery("select title, author from crm7.ticket where title= '" + title + "'");
                int counter = 0;
                while (con2.Return_Data_In_Array()[0].ToString() != title && counter < 10) //this will try upto 10 times before fails
                {
                    con2.Create_DBConnection(config.Default.DBProvidestringSQL);
                    con2.Execute_SQLQuery("select title, author from crm7.ticket where title= '" + title + "'");
                    Thread.Sleep(config.Default.SleepingTime * 10);
                    counter += 1;
                }

                Assert.AreEqual(title, con2.Return_Data_In_Array()[0].ToString());

                con2.Close_Connection();

                //clean the mailbox by deleting the newly added record in the database
                DBAccess con3 = new DBAccess();
                con3.Create_DBConnection(config.Default.DBProvidestringSQL);
                con3.Execute_SQLQuery("delete crm7.MAIL_IN_FILTER where address = '" + fulladdress + "'");                
                con3.Close_Connection();
                           
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
