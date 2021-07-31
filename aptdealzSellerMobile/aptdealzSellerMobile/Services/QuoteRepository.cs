using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Services
{
    public class QuoteRepository : IQuoteRepository
    {
        RequirementAPI requirementAPI = new RequirementAPI();

        //public async Task<string> RevealContact(string RequirementId)
        //{
        //    string PhoneNumber = "Reveal Contact";
        //    try
        //    {
        //        long amount = (long)App.Current.Resources["RevealContact"];
        //        RazorPayload payload = new RazorPayload();
        //        payload.amount = amount * 100;
        //        payload.currency = (string)App.Current.Resources["Currency"];
        //        payload.receipt = RequirementId; // quoteid
        //        payload.email = Common.mSellerDetails.Email;
        //        payload.contact = Common.mSellerDetails.PhoneNumber;
        //        MessagingCenter.Send<RazorPayload>(payload, "RevealPayNow");
        //        MessagingCenter.Subscribe<RazorResponse>(this, "PaidRevealResponse", async (razorResponse) =>
        //        {
        //            if (razorResponse != null && !razorResponse.isPaid)
        //            {
        //                string message = "Payment failed ";

        //                if (!Common.EmptyFiels(razorResponse.OrderId))
        //                    message += "OrderId: " + razorResponse.OrderId + " ";

        //                if (!Common.EmptyFiels(razorResponse.PaymentId))
        //                    message += "PaymentId: " + razorResponse.PaymentId + " ";

        //                if (message != null)
        //                    Common.DisplayErrorMessage(message);
        //            }

        //            UserDialogs.Instance.ShowLoading(Constraints.Loading);

        //            RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
        //            mRevealBuyerContact.RequirementId = RequirementId;
        //            mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)PaymentStatus.Success : (int)PaymentStatus.Failed;
        //            mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderNo;
        //            mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;

        //            var mResponse = await requirementAPI.RevealBuyerContact(mRevealBuyerContact);
        //            if (mResponse != null && mResponse.Succeeded)
        //            {
        //                var jObject = (JObject)mResponse.Data;
        //                if (jObject != null)
        //                {
        //                    var mSellerContact = jObject.ToObject<RevealBuyerContact>();
        //                    if (mSellerContact != null)
        //                    {
        //                        var successPopup = new Views.Popup.SuccessPopup(Constraints.ContactRevealed);
        //                        PopupNavigation.Instance.PushAsync(successPopup);

        //                        PhoneNumber = mSellerContact.PhoneNumber;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (mResponse != null)
        //                    Common.DisplayErrorMessage(mResponse.Message);
        //                else
        //                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
        //            }
        //        });
        //    }
        //    catch (ArgumentNullException)
        //    {
        //        Common.DisplayErrorMessage(Constraints.Number_was_null);
        //    }
        //    catch (FeatureNotSupportedException)
        //    {
        //        Common.DisplayErrorMessage(Constraints.Phone_Dialer_Not_Support);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.DisplayErrorMessage("QuoteRepository/RevealContact: " + ex.Message);
        //    }
        //    finally
        //    {
        //        UserDialogs.Instance.HideLoading();
        //    }
        //    return PhoneNumber;
        //}
    }
}
