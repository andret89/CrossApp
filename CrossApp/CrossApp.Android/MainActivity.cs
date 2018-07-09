
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content;
using Android;
using System.Threading.Tasks;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace CrossApp.Droid
{
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/json")]

    [Activity(Label = "CrossApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected async override void OnCreate(Bundle bundle)
        {

            //await TryToGetPermissions();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CrossCurrentActivity.Current.Init(this, bundle);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var jsonStr = ImportJson();
            if (jsonStr != null)
                LoadApplication(new App(jsonStr));
            else
                LoadApplication(new App());
            //((App)Xamarin.Forms.Application.Current).DisplayJSON(json);
        }

        public string ImportJson()
        {
            var clipboard = (ClipboardManager)Application.Context.GetSystemService(ClipboardService);
            var json = clipboard.Text;
            if (IsValidJson(json))
                return json;
            return null;
        }

        private static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (Exception) //some other exception
                { }
            }
            return false;
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Canceled)
            {
                // Notify user file picking was cancelled.
                Finish();
            }
            else
            {
                System.Diagnostics.Debug.Write(data.Data);
                try
                {
                    var _uri = data.Data;

                    var filePath = IOUtil.getPath(this, _uri);

                    if (string.IsNullOrEmpty(filePath))
                        filePath = _uri.Path;

                    var file = IOUtil.readFile(filePath);

                    //var fileName = GetFileName(this, _uri);

                    //OnFilePicked(new FilePickerEventArgs(file, fileName, filePath));
                }
                catch (Exception readEx)
                {
                    // Notify user file picking failed.
                    //OnFilePickCancelled();
                    System.Diagnostics.Debug.Write(readEx);
                }
                finally
                {
                    Finish();
                }
            }

        }

        public string HandlerIntenetToJsonOld()
        {
            Intent intent = Intent;
            string type = intent.Type;
            string jsonString = null;
            if (Intent.Action == Intent.ActionSend)
            {
                if (type.StartsWith("application/"))
                {
                    var key = "android.intent.extra.STREAM";
                    string jsonDecryptString = string.Empty;
                    var data = Intent.Data;
                    var filePathUri = Intent.GetParcelableExtra(key) as Android.Net.Uri;
                    Stream stream = ContentResolver.OpenInputStream(filePathUri);

                    using (var streamReader = new StreamReader(stream))
                    {
                        jsonString = streamReader.ReadToEnd();

                    }
                }
            }
            return jsonString;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

        /*
        #region RuntimePermissions

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }


        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.ReadUserDictionary,
                            Manifest.Permission.ReadProfile,
             };

        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }



        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        //{
        //    switch (requestCode)
        //    {
        //        case RequestLocationId:
        //            {
        //                if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
        //                {
        //                    Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

        //                }
        //                else
        //                {
        //                    //Permission Denied :(
        //                    //Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();

        //                }
        //            }
        //            break;
        //    }
        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        #endregion
        */

