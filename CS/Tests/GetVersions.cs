using System;
using ArtOfTest.WebAii.TestTemplates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS.CommonMethods;
using System.Threading;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CS.Tests
{
    /// <summary>
    /// This test is used to get the meta data of the system under test- version of SUT, browser used, SUT Url.
    /// Author :Tharinda 
    /// Date: 09.12.2016.
    /// </summary>
    [TestClass]
    public class GetVersions : BaseTest
    {
        private static string _fileVersion;
        private static string _custId;

        private static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                var start = strSource.IndexOf(strStart, 0, StringComparison.Ordinal) + strStart.Length;
                var end = strSource.IndexOf(strEnd, start, StringComparison.Ordinal);
                return strSource.Substring(start, end - start);
            }
            else
            {
                return "";
            }
        }

        private static async void DownloadPageAsync()
        {
            // ... Target page.
            var page = config.Default.About_Url;
            // ... Use HttpClient.
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(page))
            using (var content = response.Content)
            {
                // ... Read the string.
                var result = await content.ReadAsStringAsync();
                // ... Display the result.
                if (result != null)
                {
                    _fileVersion = GetBetween(result, "File version", "Netserver version");
                    _custId = GetBetween(result, "Program: ", "/CS/scripts/ticket.fcgi");
                }
                else
                {
                    _fileVersion = "Response not received";
                }

                if (_fileVersion == null)
                {
                    _fileVersion = "Not retrieved";
                }   
            }
        }
        
        
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
                //create an async task to download the html page
                var t = new Task(DownloadPageAsync);
                t.Start();
                Thread.Sleep(config.Default.SleepingTime* 10);                
                var con = new DbAccess();
                con.Create_DBConnection(config.Default.DBProvidestringSQL);
                con.Execute_SQLQuery("select prefvalue from userpreference where prefkey='CRMBaseURL'");                
                //string url = con.Return_Data_In_Array()[0].ToString();
                
                //write application version/url info to a file
                const string filePath = @"C:\GIT\Telerik\CS\TestResults\Version.log";
                using (var aFile = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(aFile))
                {                    
                    sw.WriteLine("\n");
                    sw.WriteLine("***Base Url: " + config.Default.Base_Url);
                    sw.WriteLine("\n");
                    sw.WriteLine("***Tenant Id: " + _custId);
                    sw.WriteLine("\n");
                    sw.WriteLine("***File Version: " + _fileVersion);
                    sw.WriteLine("\n");
                    sw.WriteLine("***Browser Used...: " + config.Default.BrowserType);
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
