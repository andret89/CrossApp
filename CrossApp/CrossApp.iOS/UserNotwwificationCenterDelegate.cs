using System;
using ObjCRuntime;
using UserNotifications;

namespace CrossApp.iOS
{
    internal class UserNotwwificationCenterDelegate : UNUserNotificationCenterDelegate
    {
       
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, 
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            //base.WillPresentNotification(center, notification, completionHandler);
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

    }
}