using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Share;
using Plugin.Share.Abstractions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CrossApp
{
    public partial class App : Application
	{
        internal static NavigationPage _navigationRoot;
        internal static string PackageName;

        public App()
        {
            InitializeComponent();
            _navigationRoot = new NavigationPage(new Views.LoginPage());
            //NavigationPage.SetHasNavigationBar(_navigationRoot.CurrentPage, false);
            MainPage = _navigationRoot;

        }

        public void OnLogin()
        {
            MainPage = new NavigationPage(new Views.InterventiPage());
        }

        public void OnLogout()
        {
            MainPage = new NavigationPage(new Views.LoginPage());
        }

        public void OnShare()
        {
            CrossShare.Current.Share(new ShareMessage
            {
                Title = "Motz Cod.es",
                Text = "Checkout Motz Cod.es! for all sorts of goodies",
                Url = "http://motzcod.es"
            },
            new ShareOptions
            {
                ChooserTitle = "Share Blog",
                ExcludedUIActivityTypes = new[] { ShareUIActivityType.Mail }
            });
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

        internal void SendFileData(string dataString, string type)
        {
            MainPage = new NavigationPage(new Views.InterventiPage(dataString,type));

        }
    }
}
