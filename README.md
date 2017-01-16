# Telerik

Simple End-To-End UI Test Automation solution (with Telerik, C#, Python etc).
=====================================================================

Following technologies/ tools were used .

1. Telerik Testing Framework from Progress Inc - this is the tool used for writing the UI tests
2. Visual Studio 2013 or  above with C#
3. Python scripting language - DO automate soem background tasks like mail sending, cleanup etc
4. DOS batch files- To initiate the test building and running
5. SMTP Server - I used free version of MailEnable - This is to send out the status mail that iis composed at the end of the test run
6 . Windows Task Scheduler _To schedule test runs
7. Command line tools - MSBuild.exe, MSTest.exe - This is to build and run the tests in 'Test Runner' machine
8. Dedicated VM for running the tests- Which I call it the 'Test Runner'
9. Github repository to version controlling and  pushing the tests to 'Test Runner' after they are being developed and tested locally.

The project is organised in the following manner
- References
    - all the dll references required to run the project including telerik files
- common methods
    - c# classes that are shared by the unit tests
- object repo
    - all the page oblect elements/locators are stoed as class properties
- Tests
    - VsUnit tests- actual test cases
- app config
    - values like db provider string, credentials etc are stored
    
- Supporting Scripts to achive the background automatoin tasks
    

How tests are written
======================
1.	Install Telerik test framework.
2.	Create a new VS project select “Unit test project” from “Test” under Template.
3.	Use VsUnit test to write the tests.

