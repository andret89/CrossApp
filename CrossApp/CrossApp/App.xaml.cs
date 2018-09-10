using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CrossApp
{
    public partial class App : Application
    {
        internal static NavigationPage _navigationRoot;
        internal static string PackageName;
        public bool IsLogged()
        {
            var ret = false;
            if (Current.Properties.ContainsKey("IsLogin"))
            {
                Debug.WriteLine("----Login: ContainsKey('IsLogin') == true");
                if ((bool)Current.Properties["IsLogin"])
                    ret = true;
            }
            return ret;
        }

        public App()
        {
            InitializeComponent();
            Current.Properties["IsLogin"] = false;
            Current.SavePropertiesAsync();

            if (IsLogged())
            {
                _navigationRoot = new NavigationPage(new Views.InterventiPage());
                MainPage = _navigationRoot;
            }
            else
                MainPage = new Views.LoginPage();
            //NavigationPage.SetHasNavigationBar(_navigationRoot.CurrentPage, false);
           

        }

        public async System.Threading.Tasks.Task OnLoginAsync()
        {
            if (!IsLogged())
            {
                Application.Current.Properties["isLogin"] = true;
                await Current.SavePropertiesAsync();
                MainPage = new NavigationPage(new Views.InterventiPage());
            }
        }

        public async System.Threading.Tasks.Task OnLogoutAsync()
        {
            Application.Current.Properties["isLogin"] = false;
            await Current.SavePropertiesAsync();
            MainPage = new Views.LoginPage();
        }

        public void OnShare()
        {
            //    CrossShare.Current.Share(new ShareMessage
            //    {
            //        Title = "Motz Cod.es",
            //        Text = "Checkout Motz Cod.es! for all sorts of goodies",
            //        Url = "http://motzcod.es"
            //    },
            //    new ShareOptions
            //    {
            //        ChooserTitle = "Share Blog",
            //        ExcludedUIActivityTypes = new[] { ShareUIActivityType.Mail }
            //    });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        internal void SendFileData(string dataString, string type)
        {
            if (IsLogged())
                MainPage = new NavigationPage(new Views.InterventiPage(dataString, type));
            else
            {
                MainPage = new NavigationPage(new Views.LoginPage());
                MainPage.DisplayAlert("Errore", "Login necessario", null, "OK");
            }
        }
        public delegate void Del();

        public static void MethodWithCallback(Del callback)
        {
            try
            {
                callback();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }


    }
}
