﻿#pragma checksum "..\..\MainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A9A76173ED62A2A568CC97ED2FE7CD0F0D00269E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WatsUpClient;


namespace WatsUpClient {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lv_chatslist;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lv_chat;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bt_logout;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_messaggio;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bt_allega;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bt_invia;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button bt_createchat;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tb_creachat;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WatsUpClient;component/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\MainPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 7 "..\..\MainPage.xaml"
            ((WatsUpClient.MainPage)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lv_chatslist = ((System.Windows.Controls.ListBox)(target));
            
            #line 22 "..\..\MainPage.xaml"
            this.lv_chatslist.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lv_chatslist_Selected);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lv_chat = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.bt_logout = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\MainPage.xaml"
            this.bt_logout.Click += new System.Windows.RoutedEventHandler(this.bt_logout_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tb_messaggio = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\MainPage.xaml"
            this.tb_messaggio.KeyDown += new System.Windows.Input.KeyEventHandler(this.tb_messaggio_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.bt_allega = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\MainPage.xaml"
            this.bt_allega.Click += new System.Windows.RoutedEventHandler(this.bt_allega_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.bt_invia = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\MainPage.xaml"
            this.bt_invia.Click += new System.Windows.RoutedEventHandler(this.Send);
            
            #line default
            #line hidden
            return;
            case 8:
            this.bt_createchat = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\MainPage.xaml"
            this.bt_createchat.Click += new System.Windows.RoutedEventHandler(this.bt_createchat_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.tb_creachat = ((System.Windows.Controls.TextBox)(target));
            
            #line 51 "..\..\MainPage.xaml"
            this.tb_creachat.KeyDown += new System.Windows.Input.KeyEventHandler(this.tb_creachat_KeyDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

