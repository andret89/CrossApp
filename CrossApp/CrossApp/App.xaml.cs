using CrossApp.Models;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CrossApp
{
	public partial class App : Application
	{
        private NavigationPage _navigationRoot;
        public App()
        {
            InitializeComponent();
            _navigationRoot = new NavigationPage(new MainPage());
            MainPage = _navigationRoot;

        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        internal async void SendFileData(string dataString, string type)
        {
            if(type == "text/xml")
                await ((MainPage)_navigationRoot.CurrentPage).SetXmlToViewAsync(dataString);
            else
                await ((MainPage)_navigationRoot.CurrentPage).SetJsonToViewAsync(dataString);
        }
    }
}
