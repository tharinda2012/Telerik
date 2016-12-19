# Telerik

UI automation Unit tests (VsUnit) using Telerik free Test Framework - written in C#
=====================================================================

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

How tests are written
======================
1.	Install Telerik test framework.
2.	Create a new VS project select “Unit test project” from “Test” under Template.
3.	Use VsUnit test to write the tests.
