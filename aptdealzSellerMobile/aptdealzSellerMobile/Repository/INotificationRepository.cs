using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface INotificationRepository
    {
        Task<string> GetNotificationCount();
        Task<bool> SetUserNoficiationAsRead(string NotificationId);
    }
}
