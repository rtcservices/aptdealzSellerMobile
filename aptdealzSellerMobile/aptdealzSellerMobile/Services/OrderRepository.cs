using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Services
{
    public class OrderRepository : IOrderRepository
    {
        OrderAPI orderAPI = new OrderAPI();
        public async Task<Order> GetOrderDetails(string orderId)
        {
            Order mOrder = new Order();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.GetOrderDetailsForSeller(orderId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mOrder = jObject.ToObject<Order>();
                    }
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/GetOrderDetails: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mOrder;
        }

        public async Task UpdateOrder(Order mOrder)
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await orderAPI.UpdateOrder(mOrder);
                if (mResponse != null && mResponse.Succeeded)
                {
                    SuccessfullSavedQuote(mResponse.Message);
                }
                else
                {
                    if (mResponse != null && mResponse.Message != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/UpdateOrder: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void SuccessfullSavedQuote(string MessageString)
        {
            try
            {
                var successPopup = new Views.Popup.SuccessPopup(MessageString);
                PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderRepository/SuccessSavedQuote: " + ex.Message);
            }
        }
    }
}
