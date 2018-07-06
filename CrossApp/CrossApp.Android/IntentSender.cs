using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(CrossApp.Droid.IntentSender))]

namespace CrossApp.Droid
{
    class IntentSender : ISenderService
    {
        Context context = Android.App.Application.Context;

        public string sendRequest()
        {
            var appDevice = "testot330i";
            var application_id = "com.companyname.CrossApp";
            var parameter = "targetapplication=default";
            // var url = $"{appDevice}+{application_id}://data?userinfo=targetapplication=default&json=base64_encoded_data";
            // var url = $"{appDevice}://start?userinfo=targetapplication=default&bundleid={application_id}";
            var url = $"{appDevice}://start?userinfo=parameter," +
                $"language=en_GB,tutorial=false&bundleid={application_id}";
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionView);
            var urlApp = Android.Net.Uri.Parse(url);
            intent.SetData(urlApp);
            context.StartActivity(intent);
            return urlApp.ToString();
        }
    }
}