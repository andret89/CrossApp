
using Android.App;
using Android.Content;
using System.IO;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(CrossApp.Droid.IntentSender))]

namespace CrossApp.Droid
{
    class IntentSender : ISenderService
    {
        Context context = Android.App.Application.Context;

        public string SendRequest()
        {
            var appDevice = "testot330";
            var application_id = "com.companyname.CrossApp";
            var parameter = "targetapplication=default";
            //var url = $"{appDevice}+{application_id}://data?userinfo=targetapplication=default&json=base64_encoded_data";
            // var url = $"{appDevice}://start?userinfo=targetapplication=default&bundleid={application_id}";
            var url = $"{appDevice}://start?userinfo={parameter}," +
                $"language=it_IT,tutorial=false&bundleid={application_id}";
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionView);
            var urlApp = Android.Net.Uri.Parse(url);
            intent.SetData(urlApp);
            context.StartActivity(intent);
            return urlApp.ToString();
        }

        public string OpenFile(FileStream fileStream)
        {
            string jsonString = null;
            //var uri = System.Uri.FromFile(filePath);
            //Stream stream = ContentResolver.OpenInputStream(filePath);
            using (var streamReader = new StreamReader(fileStream))
            {
                jsonString = streamReader.ReadToEnd();
            }
            return jsonString;
        }

        public void GetFileChoice()
        {
            var intent = new Intent();
            intent.SetType("application/*");
            intent.SetAction(Intent.ActionGetContent);
            var cur = (MainActivity)Forms.Context;
            cur.StartActivityForResult(Intent.CreateChooser(intent, "Select File JFT"), 1000);
        }
    }
}