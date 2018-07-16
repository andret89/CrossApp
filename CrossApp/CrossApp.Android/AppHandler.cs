
using Android.App;
using Android.Content;
using Android.Net;
using Android.Support.Compat;
using Android.Support.V4.Content;
using Android.Widget;
using Plugin.CurrentActivity;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(CrossApp.Droid.AppHandler))]

namespace CrossApp.Droid
{
    class AppHandler : IAppHandler
    {
        Context context = Android.App.Application.Context;

        public string SendRequest()
        {
            var appDevice = "testot330i";
            var application_id = context.PackageName;
            var parameter = "targetapplication=default";
            var url = $"{appDevice}+{application_id}://data?userinfo=parameter&json=base64_encoded_data";
            // var url = $"{appDevice}://start?userinfo=targetapplication=default&bundleid={application_id}";
            //var url = $"{appDevice}://start?userinfo={parameter}," +
            //    $"language=it_IT,tutorial=false&bundleid={application_id}";
            Intent intent = new Intent();
            intent.SetAction(Intent.ActionSend);
            var urlApp = Android.Net.Uri.Parse(url);
            intent.SetData(urlApp);
            context.StartActivity(intent);
            return urlApp.ToString();
        }

        public string OpenFile(string filePath)
        {
            string jsonString = null;
            var file = new Java.IO.File(filePath);
            var uri = Android.Net.Uri.FromFile(file);
            Stream stream = context.ContentResolver.OpenInputStream(uri);
            using (var streamReader = new StreamReader(stream))
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
            intent.AddCategory(Intent.CategoryOpenable);
            var activity = CrossCurrentActivity.Current.Activity;
            activity.StartActivityForResult(Intent.CreateChooser(intent, "Select File JFT"), 1000);
        }

        public string GetTextFromClipboard()
        {
            var clipboard = (ClipboardManager)Android.App.Application.Context.GetSystemService(Context.ClipboardService);
            return clipboard.Text;
        }

        public Task<bool> LaunchApp(string uri)
        {
            bool result = false;

            try
            {
                var aUri = Android.Net.Uri.Parse(uri.ToString());
                var intent = new Intent(Intent.ActionView, aUri);
                context.StartActivity(intent);
                result = true;
            }
            catch (ActivityNotFoundException)
            {
                result = false;
            }

            return Task.FromResult(result);
        }

        public void OpenPDF(string filePath)
        {
            //var file = new Java.IO.File(filePath);
            //Android.Net.Uri uri;
            //if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.M)
            //{
            //    try
            //    {
            //        uri = FileProvider.GetUriForFile(context,
            //            "com.companyname.CrossApp" + ".fileprovider", file);
            //    }catch(Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}
            //else
            //{
            //    uri = Android.Net.Uri.FromFile(file);
            //}

            var intent = new Intent(Intent.ActionView);
            //var ld = context.GetDir("Download","Prova.pdf");
            var contentUri = Android.Net.Uri.Parse("content://" + filePath);
            intent.PutExtra(Intent.ExtraStream, contentUri);
            intent.SetDataAndType(contentUri, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            var intentChooser = Intent.CreateChooser(intent, "Open PDF");
            context.StartActivity(intentChooser);
        }

        public void DownloadFile(string fileName_, Byte[] document_)
        {
            //PermissionsDroidBusiness.CheckReadAndWriteExternalStorage();
            /*
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" +
            global::Android.OS.Environment.DirectoryDownloads + "/" + fileName_;
            File.WriteAllBytes(externalPath, document_);
            */
            var externalPath = fileName_;
            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);
            /*
            var contentUri = Android.Support.V4.Content.FileProvider.GetUriForFile(
                context, "com.companyname.CrossApp", file);
                */
            var contentUri = Android.Net.Uri.Parse("content://" + externalPath);

            var intent = new Intent(Intent.ActionView);
            intent.PutExtra(Intent.ExtraStream, contentUri);
            intent.SetDataAndType(contentUri, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            var intentChooser = Intent.CreateChooser(intent, "Open PDF");

            context.StartActivity(intentChooser);
            /*
            Android.Net.Uri pdfPath = FileProvider.GetUriForFile(context,"com.companyname.CrossApp", file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(pdfPath, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);
            context.StartActivity(intent);
            */
        }
    }
}