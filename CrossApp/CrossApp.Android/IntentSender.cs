
using Android.App;
using Android.Content;

[assembly: Xamarin.Forms.Dependency(typeof(CrossApp.Droid.IntentSender))]

namespace CrossApp.Droid
{
    class IntentSender : ISenderService
    {
        Context context = Application.Context;

        public string sendRequest()
        {
            var appDevice = "testot330i";
            var application_id = "com.companyname.CrossApp";
            var parameter = "targetapplication=default";
            var url = $"{appDevice}://start?userinfo={parameter}," +
                $"language=it_IT,tutorial=false&bundleid={application_id}";
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionView);
            var urlApp = Android.Net.Uri.Parse(url);
            intent.SetData(urlApp);
            context.StartActivity(intent);
            return urlApp.ToString();
        }
    }
}