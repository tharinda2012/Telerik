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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CS.Tests
{
    /// <summary>
    /// This test is used to verify whether user can create a new company in CS
    /// Author :Tharinda 
    /// Date: 09.12.2016
    /// </summary>
    [TestClass]
    public class GetVersions : BaseTest
    {

        public static string FileVersion;
        
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        static async void DownloadPageAsync()
        {
            // ... Target page.
            string page = config.Default.About_Url;

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string result = await content.ReadAsStringAsync();

                // ... Display the result.
                if (result != null)
                {
                    //FileVersion = result;
                    FileVersion = getBetween(result, "File version", "Netserver version");
                    
                }

                if (FileVersion == null)
                {
                    FileVersion = "Not retrieved";
                }   
            }
        }
        
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
        public void TestMethod_getVersions()
        {
            try
            {
                Task t = new Task(DownloadPageAsync);
                t.Start();
                Thread.Sleep(config.Default.SleepingTime* 10);
                DBAccess con = new DBAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select prefvalue from crm7.userpreference where prefkey='CRMBaseURL'");                
                string filePath = @"C:\GIT\Telerik\CS\TestResults\Version.log";
                //string url = con.Return_Data_In_Array()[0].ToString();

                string url = config.Default.Base_Url;
                using (FileStream aFile = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(aFile))
                {
                    
                    sw.WriteLine("\n");
                    sw.WriteLine("***Base Url: " + url);
                    sw.WriteLine("\n");
                    sw.WriteLine("***File Version: " + FileVersion);
                    

                }

                con.Close_Connection();


                
                    

            }

            catch (Exception e)
            {

                //saving error and logging out   
                Console.WriteLine(e.Message);
                
            }

        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

            //
            // Place any additional cleanup here
            //
            
            
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
