﻿#pragma checksum "..\..\Logowanie.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8ECC98F1A3CA97F648E2C15F7ACD478F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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
using System.Windows.Forms.Integration;
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
using interfejs;


namespace interfejs {
    
    
    /// <summary>
    /// Logowanie
    /// </summary>
    public partial class Logowanie : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\Logowanie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox loginBox;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\Logowanie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passwordBox;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\Logowanie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button logInButton;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\Logowanie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\Logowanie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label1;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\Logowanie.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label errorInfo;
        
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
            System.Uri resourceLocater = new System.Uri("/interfejs;component/logowanie.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Logowanie.xaml"
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
            
            #line 9 "..\..\Logowanie.xaml"
            ((interfejs.Logowanie)(target)).AddHandler(System.Windows.Input.Keyboard.KeyDownEvent, new System.Windows.Input.KeyEventHandler(this.Window_KeyDown));
            
            #line default
            #line hidden
            return;
            case 2:
            this.loginBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 11 "..\..\Logowanie.xaml"
            this.loginBox.AddHandler(System.Windows.Input.Keyboard.GotKeyboardFocusEvent, new System.Windows.Input.KeyboardFocusChangedEventHandler(this.loginBox_GotFocus));
            
            #line default
            #line hidden
            
            #line 11 "..\..\Logowanie.xaml"
            this.loginBox.AddHandler(System.Windows.Input.Mouse.GotMouseCaptureEvent, new System.Windows.Input.MouseEventHandler(this.loginBox_GotFocus));
            
            #line default
            #line hidden
            return;
            case 3:
            this.passwordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 12 "..\..\Logowanie.xaml"
            this.passwordBox.AddHandler(System.Windows.Input.Keyboard.GotKeyboardFocusEvent, new System.Windows.Input.KeyboardFocusChangedEventHandler(this.passwordBox_GotFocus));
            
            #line default
            #line hidden
            
            #line 12 "..\..\Logowanie.xaml"
            this.passwordBox.AddHandler(System.Windows.Input.Mouse.GotMouseCaptureEvent, new System.Windows.Input.MouseEventHandler(this.passwordBox_GotFocus));
            
            #line default
            #line hidden
            return;
            case 4:
            this.logInButton = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\Logowanie.xaml"
            this.logInButton.Click += new System.Windows.RoutedEventHandler(this.logInButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.label1 = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.errorInfo = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

