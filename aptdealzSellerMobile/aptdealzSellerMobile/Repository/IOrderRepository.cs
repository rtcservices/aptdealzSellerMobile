using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderDetails(string orderId);
        Task UpdateOrder(OrderUpdate mOrder);
        Task ScanQRCodeAndUpdateOrder(string OrderId);
    }
}
