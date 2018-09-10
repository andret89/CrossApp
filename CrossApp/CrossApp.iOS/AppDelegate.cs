using System.Diagnostics;
using System.IO;

using Foundation;
using Plugin.Toasts;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace CrossApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            DependencyService.Register<ToastNotification>(); // Register your dependency
            ToastNotification.Init();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Ask the user for permission to get notifications on iOS 10.0+
                UNUserNotificationCenter.Current.RequestAuthorization(
                    UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                    (approved, error) => { });
                UNUserNotificationCenter.Current.Delegate = new UserNotwwificationCenterDelegate();
                               
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                // Ask the user for permission to get notifications on iOS 8.0+
                var settings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                    new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            App.PackageName = NSBundle.MainBundle.BundleIdentifier; 
            return base.FinishedLaunching(app, options);
        }

        [Export("application:openURL:sourceApplication:annotation:")]
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            string absPath = url.Path;
            Stream fs = new FileStream(absPath, FileMode.Open, FileAccess.Read);
            string jsonString = null;
            string type = "application/json";
            using (StreamReader sr = new StreamReader(fs))
            {
                jsonString = sr.ReadToEnd();
            }
            fs.Close();
            if(!string.IsNullOrEmpty(jsonString))
                ((App)Xamarin.Forms.Application.Current).SendFileData(jsonString, type);

            Debug.WriteLine($"OpenUrl iOS {sourceApplication}");
            return true;
        }

    }
}
