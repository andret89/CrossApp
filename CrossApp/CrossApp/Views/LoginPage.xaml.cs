﻿using CrossApp.ViewModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public static LoginPage loginPage;
        private LoginViewModel vm;
        public LoginPage()
        {
            loginPage = this;
            vm = new LoginViewModel();
            this.BindingContext = vm;
            InitializeComponent();
            vm.DisplayInvalidLoginPrompt += () => DisplayAlert("Error", "Invalid Login, try again", "OK");
            vm.StartLoader += () => SetLoader(true);
            vm.StopLoader += () => SetLoader(false);
            EmailEntry.Text = "opti@optisoft.it";
            EmailEntry.Completed += (object sender, EventArgs e) => { PasswordEntry.Focus(); };
            PasswordEntry.Completed += (object sender, EventArgs e) => { vm.SubmitCommand.Execute(null); };

            var assembly = typeof(LoginPage).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
        }
        void SetLoader(bool enable)
        {
            loader.IsVisible = enable;
            loader.IsRunning = enable;
        }
    }
}