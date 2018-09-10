using System;
using System.Linq;
using System.Threading.Tasks;
using CrossApp.Services;
using Foundation;
using MobileCoreServices;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossApp.iOS.AppHandler))]

namespace CrossApp.iOS
{
    class AppHandler : IAppHandler
    {
        public void DownloadFile(string fileName_, byte[] document_)
        {
            throw new NotImplementedException();
        }

        public string GetTextFromClipboard()
        {
            throw new NotImplementedException();
        }

        public async void InstallApplication(string appIDstore, string appName)
        {
            Foundation.NSUrl urlApp = Foundation.NSUrl.FromString(appName);
            try
            {
                if(UIApplication.SharedApplication.CanOpenUrl(urlApp))
                    UIApplication.SharedApplication.OpenUrl(urlApp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Create Alert
                bool result = await ((App)Xamarin.Forms.Application.Current).MainPage.DisplayAlert("Errore", 
                    $"Verifica l'installazione di {appName}", "ok", "cancel");

                if (!result)
                {

                    NSUrl itunesLink = new NSUrl($"https://itunes.apple.com/us/app/testo/{appIDstore}?mt=8/");
                    UIApplication.SharedApplication.OpenUrl(itunesLink);

                }
            }
        }

        public bool IsAppInstalled(string pkgName, string appName)
        {
            bool ret = false;

            Foundation.NSUrl Url = Foundation.NSUrl.FromString(appName);

            if (UIApplication.SharedApplication.CanOpenUrl(Url))
            {
                // App is installed
                ret = true;
            }
            else
            {
                // App is not installed
                Console.WriteLine($"{appName} App is not installed");
            }
            return ret;
        }

        public void OpenPDF(string file)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetFileChoice()
        {

            var task = new TaskCompletionSource<string>();
            try
            {
                OpenDoc(GetCurrentUIController(), (obj) =>
                {
                    if (obj == null)
                    {
                        task.SetResult(null);
                        return;
                    }
                    var aa = obj.AbsoluteUrl;
                    task.SetResult(aa.Path);
                });
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
            return task.Task;
        }

        static Action<NSUrl> _callbackDoc;

        public static void OpenDoc(UIViewController parent, Action<NSUrl> callback)
        {
            _callbackDoc = callback;
            var version = UIDevice.CurrentDevice.SystemVersion;
            int verNum = 0;
            Int32.TryParse(version.Substring(0, 2), out verNum);

            var allowedUTIs = new string[]
            {
        UTType.UTF8PlainText,
        UTType.PlainText,
        UTType.RTF,
        UTType.XML,
        UTType.JSON,
        UTType.PNG,
        UTType.Text,
        UTType.PDF,
        UTType.Image,
        UTType.Spreadsheet,
        "com.microsoft.word.doc",
        "org.openxmlformats.wordprocessingml.document",
        "com.microsoft.powerpoint.ppt",
        "org.openxmlformats.spreadsheetml.sheet",
        "org.openxmlformats.presentationml.presentation",
        "com.microsoft.excel.xls",

            };

            // Display the picker
            var pickerMenu = new UIDocumentMenuViewController(allowedUTIs, UIDocumentPickerMode.Import);
            pickerMenu.DidPickDocumentPicker += (sender, args) =>
            {
                if (verNum < 11)
                {
                    args.DocumentPicker.DidPickDocument += (sndr, pArgs) =>
                    {
                        UIApplication.SharedApplication.OpenUrl(pArgs.Url);
                        pArgs.Url.StopAccessingSecurityScopedResource();

                        var cb = _callbackDoc;
                        _callbackDoc = null;
                        pickerMenu.DismissModalViewController(true);
                        cb(pArgs.Url.AbsoluteUrl);
                    };
                }
                else
                {
                    args.DocumentPicker.DidPickDocumentAtUrls += (sndr, pArgs) =>
                    {
                        UIApplication.SharedApplication.OpenUrl(pArgs.Urls[0]);
                        pArgs.Urls[0].StopAccessingSecurityScopedResource();

                        var cb = _callbackDoc;
                        _callbackDoc = null;
                        pickerMenu.DismissModalViewController(true);
                        cb(pArgs.Urls[0].AbsoluteUrl);
                    };
                }
                // Display the document picker
                parent.PresentViewController(args.DocumentPicker, true, null);
            };

            pickerMenu.ModalPresentationStyle = UIModalPresentationStyle.Popover;
            parent.PresentViewController(pickerMenu, true, null);
            UIPopoverPresentationController presentationPopover = pickerMenu.PopoverPresentationController;
            if (presentationPopover != null)
            {
                presentationPopover.SourceView = parent.View;
                presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Down;
            }
        }

        public void OpenURL(string url)
        {
            throw new NotImplementedException();
        }

        private void DocPicker_DidPickDocumentAtUrls(object sender, UIDocumentPickedAtUrlsEventArgs e)
        {
            //Action to perform on document pick
        }

        public string FileChoice()
        {
            try
            {
                var docPicker = new UIDocumentPickerViewController(new string[]
                { UTType.Data, UTType.Content }, UIDocumentPickerMode.Import);
                docPicker.WasCancelled += (sender, wasCancelledArgs) =>
                {
                };
                docPicker.DidPickDocumentAtUrls += DocPicker_DidPickDocumentAtUrls;
                var _currentViewController = GetCurrentUIController();
                if (_currentViewController != null)
                    _currentViewController.PresentViewController(docPicker, true, null);
            }
            catch (Exception ex)
            {
                //Exception Logging
            }
            return "prova";
        }


        public UIViewController GetCurrentUIController()
        {
            UIViewController viewController;
            var window = UIApplication.SharedApplication.KeyWindow;
            if (window == null)
            {
                return null;
            }

            if (window.RootViewController.PresentedViewController == null)
            {
                window = UIApplication.SharedApplication.Windows
                         .First(i => i.RootViewController != null &&
                                     i.RootViewController.GetType().FullName
                                     .Contains(typeof(Xamarin.Forms.Platform.iOS.Platform).FullName));
            }

            viewController = window.RootViewController;

            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}