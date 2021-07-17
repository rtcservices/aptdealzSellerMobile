using aptdealzSellerMobile.Model.Reponse;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Repository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderDetails(string orderId);
        Task UpdateOrder(Order mOrder);
    }
}
