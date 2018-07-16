using Android.Content;
using Android.Support.V4.Content;
using Plugin.CurrentActivity;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossApp.Droid.AppHandler))]

namespace CrossApp.Droid
{
    class AppHandler : IAppHandler
    {
        Context context = Android.App.Application.Context;

        public string ReadFile(string filePath)
        {
            string retStr = null;
            var file = new Java.IO.File(filePath);
            var uri = Android.Net.Uri.FromFile(file);
            Stream stream = context.ContentResolver.OpenInputStream(uri);
            using (var streamReader = new StreamReader(stream))
            {
                retStr = streamReader.ReadToEnd();
            }
            return retStr;
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

        public void OpenPDF(string filePath)
        {
            var file = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.Path + filePath);
            Android.Net.Uri uri = null;
            if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.M)
            {
                try
                {
                    uri = FileProvider.GetUriForFile(context,
                        "com.companyname.CrossApp" + ".fileprovider", file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
                uri = Android.Net.Uri.FromFile(file);

            if(uri!=null)
            {
                var intent = new Intent(Intent.ActionView);
                intent.PutExtra(Intent.ExtraStream, uri);
                intent.SetDataAndType(uri, "application/pdf");
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);

                var intentChooser = Intent.CreateChooser(intent, "Open PDF");
                context.StartActivity(intentChooser);
            }
        }

        public void DownloadFile(string fileName_, Byte[] document_)
        {
            //PermissionsDroidBusiness.CheckReadAndWriteExternalStorage();

            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" +
            global::Android.OS.Environment.DirectoryDownloads + "/" + fileName_;
            File.WriteAllBytes(externalPath, document_);
            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);

            var contentUri = FileProvider.GetUriForFile(
                context, "com.companyname.CrossApp.fileprovider", file);
            var intent = new Intent(Intent.ActionView);
            intent.PutExtra(Intent.ExtraStream, contentUri);
            var type = contentUri.GetType();
            intent.SetDataAndType(contentUri, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            intent.SetFlags(ActivityFlags.GrantReadUriPermission);

            var intentChooser = Intent.CreateChooser(intent, "Open File");
            context.StartActivity(intentChooser);
        }

        public bool IsAppInstalled(string packageName)
        {
            bool installed = false;
            try
            {
                context.PackageManager.GetPackageInfo(packageName,Android.Content.PM.PackageInfoFlags.Activities);
                installed = true;
            }
            catch (Android.Content.PM.PackageManager.NameNotFoundException)
            {
                installed = false;
            }

            return installed;
        }
    }
}