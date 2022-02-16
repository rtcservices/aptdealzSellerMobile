using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface INotificationRepository
    {
        Task<string> GetNotificationCount();
        Task<bool> SetUserNoficiationAsRead(string NotificationId);
        Task<bool> SetUserNoficiationAsReadAndDelete(string NotificationId);
        //Task<bool> SendTestPushNotification(string SoundName, string Message);
    }
}
