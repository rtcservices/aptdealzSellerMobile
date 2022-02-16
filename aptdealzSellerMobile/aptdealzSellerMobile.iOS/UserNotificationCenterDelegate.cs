using System;
using System.Linq;
using UserNotifications;

namespace aptdealzSellerMobile.iOS
{
    public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public UserNotificationCenterDelegate()
        {

        }

        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);

            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.
           
            //updated by BK 01-12-2022
            completionHandler(UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge);
            //completionHandler(UNNotificationPresentationOptions.Alert);
        }

        //public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        //{
        //    // Here you handle the user taps
        //    completionHandler();
        //    var remotePushData = response.Notification.Request.Content.UserInfo.ToDictionary(i => i.Key.ToString(), i => i.Value.ToString());

        //}
    }
}