﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CS {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class config : global::System.Configuration.ApplicationSettingsBase {
        
        private static config defaultInstance = ((config)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new config())));
        
        public static config Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\GIT\\Telerik\\CS\\TestResults")]
        public string errordumppath {
            get {
                return ((string)(this["errordumppath"]));
            }
            set {
                this["errordumppath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://sod.00000.com/login")]
        public string Base_Url {
            get {
                return ((string)(this["Base_Url"]));
            }
            set {
                this["Base_Url"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0000")]
        public string Password {
            get {
                return ((string)(this["Password"]));
            }
            set {
                this["Password"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int SleepingTime {
            get {
                return ((int)(this["SleepingTime"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Provider=SQLOLEDB;Data Source=10.51.8.81;Initial Catalog=00021693;Integrated Secu" +
            "rity=SSPI")]
        public string DBProvidestringSQL {
            get {
                return ((string)(this["DBProvidestringSQL"]));
            }
            set {
                this["DBProvidestringSQL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tharindal@0000000.com")]
        public string Username {
            get {
                return ((string)(this["Username"]));
            }
            set {
                this["Username"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://sod.000000.com/Cust0000/CS/scripts/ticket.fcgi?action=about")]
        public string About_Url {
            get {
                return ((string)(this["About_Url"]));
            }
            set {
                this["About_Url"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Chrome")]
        public global::ArtOfTest.WebAii.Core.BrowserType BrowserType {
            get {
                return ((global::ArtOfTest.WebAii.Core.BrowserType)(this["BrowserType"]));
            }
            set {
                this["BrowserType"] = value;
            }
        }
    }
}
