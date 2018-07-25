
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Android.Content;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using System;

namespace CrossApp.Droid
{
    //[IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/pdf")]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/json")]

    [Activity(Label = "CrossApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //[Activity(Label = "CrossApp", Icon = "@mipmap/icon", Theme = "@andoird:style/Theme.Material.Light", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CrossCurrentActivity.Current.Init(this, bundle);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
            HandlerIntentToJson();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                string dataString = null;
                try
                {
                    var uFile = data.Data;
                    Stream stream = ContentResolver.OpenInputStream(uFile);
                    using (var streamReader = new StreamReader(stream))
                    {
                        dataString = streamReader.ReadToEnd();
                    }
                    var type = ContentResolver.GetType(uFile);
                    ((App)Xamarin.Forms.Application.Current).SendFileData(dataString, type);
                }
                catch (Exception readEx)
                {
                    System.Diagnostics.Debug.Write(readEx);
                }
                
            }

        }

        public void HandlerIntentToJson()
        {
            Intent intent = Intent;
            string type = intent.Type;
            string jsonString = null;
            if (Intent.Action == Intent.ActionSend)
            {
                if (type.StartsWith("application/"))
                {
                    var key = "android.intent.extra.STREAM";
                    var filePathUri = Intent.GetParcelableExtra(key) as Android.Net.Uri;
                    Stream stream = ContentResolver.OpenInputStream(filePathUri);
                    System.Diagnostics.Debug.Write(filePathUri);

                    using (var streamReader = new StreamReader(stream))
                    {
                        jsonString = streamReader.ReadToEnd();
                    }
                    ((App)Xamarin.Forms.Application.Current).SendFileData(jsonString, type);
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
