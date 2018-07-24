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
            _navigationRoot = new NavigationPage(new BluetoothPage());
            MainPage = _navigationRoot;

        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            MessagingCenter.Send<App>(this, "Sleep");
        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
            MessagingCenter.Send<App>(this, "Resume");
        }

        internal async void SendFileData(string dataString, string type)
        {
            if(type.Equals("text/xml"))
                await ((MainPage)_navigationRoot.CurrentPage).SetXmlToViewAsync(dataString);
            else
                 if (type.Equals("application/json"))
                    await ((MainPage)_navigationRoot.CurrentPage).SetJsonToViewAsync(dataString);
        }
    }
}
