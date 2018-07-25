using Android.Content;
using Android.Support.V4.Content;
using CrossApp.Services;
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

        public void GetFileChoice()
        {
            var intent = new Intent();
            intent.SetType("*/*");
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

        public void OpenPDF(string fileName)
        {
            var filePath = Android.OS.Environment.ExternalStorageDirectory.Path + fileName;
            var uri = GetUriForAllVersion(filePath);

            if (uri != null)
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
                context.PackageManager.GetPackageInfo(packageName, Android.Content.PM.PackageInfoFlags.Activities);
                installed = true;
            }
            catch (Android.Content.PM.PackageManager.NameNotFoundException)
            {
                installed = false;
            }
            return installed;
        }

        public Android.Net.Uri GetUriForAllVersion(string filePath)
        {
            Android.Net.Uri uri = null;
            var file = new Java.IO.File(filePath);
            if (Android.OS.Build.VERSION.SdkInt > Android.OS.BuildVersionCodes.M)
            {
                try
                {
                    uri = FileProvider.GetUriForFile(context, "com.companyname.CrossApp.fileprovider", file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
                uri = Android.Net.Uri.FromFile(file);
            return uri;
        }

        public void InstallApplication(string appPackageName)
        {
            Intent marketIntent = new Intent(Intent.ActionView);
            marketIntent.SetData(Android.Net.Uri.Parse($"market://details?id={appPackageName}"));
            marketIntent.AddFlags(ActivityFlags.NoHistory | ActivityFlags.ClearWhenTaskReset |
                ActivityFlags.MultipleTask | ActivityFlags.NewTask);
            context.StartActivity(marketIntent);
        }

        public void OpenURL(string url)
        {
            var contentUri = GetUriForAllVersion(url);
            if (contentUri != null)
            {
                var intent = new Intent(Intent.ActionView);
                intent.PutExtra(Intent.ExtraStream, contentUri);
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
                intent.SetFlags(ActivityFlags.GrantReadUriPermission);
                context.StartActivity(intent);
            }

        }
    }

}