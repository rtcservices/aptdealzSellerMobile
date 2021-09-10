using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.OtherPage;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Services
{
    public class RequirementRepository : IRequirementRepository
    {
        RequirementAPI requirementAPI = new RequirementAPI();

        public async Task<Requirement> GetRequirementById(string RequirmentId)
        {
            Requirement mRequirement = new Requirement();
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await requirementAPI.GetRequirementById(RequirmentId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mRequirement = jObject.ToObject<Requirement>();
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
                Common.DisplayErrorMessage("RequirementRepository/GetRequirementById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return mRequirement;
        }

        public async Task<string> RevealContact(RevealBuyerContact mRevealBuyerContact)
        {
            string PhoneNumber = Constraints.Str_RevealContact;
            try
            {


                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await requirementAPI.RevealBuyerContact(mRevealBuyerContact);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mBuyerContact = jObject.ToObject<RevealBuyerContact>();
                        if (mBuyerContact != null)
                        {
                            await PopupNavigation.Instance.PushAsync(new Views.Popup.SuccessPopup(Constraints.ContactRevealed));
                            PhoneNumber = mBuyerContact.PhoneNumber;
                        }
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
            catch (ArgumentNullException)
            {
                Common.DisplayErrorMessage(Constraints.Number_was_null);
            }
            catch (FeatureNotSupportedException)
            {
                Common.DisplayErrorMessage(Constraints.Phone_Dialer_Not_Support);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteRepository/RevealContact: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
            return PhoneNumber;
        }

        //public async Task RevealContactPay(string RequirementId)
        //{
        //    try
        //    {
        //        long amount = (long)App.Current.Resources["RevealContact"];
        //        RazorPayload payload = new RazorPayload();
        //        payload.amount = amount * 100;
        //        payload.currency = (string)App.Current.Resources["Currency"];
        //        payload.receipt = RequirementId; // quoteid
        //        payload.email = Common.mSellerDetails.Email;
        //        payload.contact = Common.mSellerDetails.PhoneNumber;
        //        if (DeviceInfo.Platform == DevicePlatform.Android)
        //        {
        //            MessagingCenter.Send<RazorPayload>(payload, Constraints.RP_RevealPayNow);
        //            MessagingCenter.Subscribe<RazorResponse>(this, Constraints.RP_PaidRevealResponse, async (razorResponse) =>
        //            {
        //                if (razorResponse != null && !razorResponse.isPaid)
        //                {
        //                    string message = "Payment failed ";

        //                    if (!Common.EmptyFiels(razorResponse.OrderId))
        //                        message += "OrderId: " + razorResponse.OrderId + " ";

        //                    if (!Common.EmptyFiels(razorResponse.PaymentId))
        //                        message += "PaymentId: " + razorResponse.PaymentId + " ";

        //                    if (message != null)
        //                        Common.DisplayErrorMessage(message);
        //                }

        //                UserDialogs.Instance.ShowLoading(Constraints.Loading);

        //                RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
        //                mRevealBuyerContact.RequirementId = RequirementId;
        //                mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)RevealContactStatus.Success : (int)RevealContactStatus.Failure;
        //                mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderId;
        //                mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;

        //                BtnRevealContact.Text = RevealContact(mRevealBuyerContact);
        //                MessagingCenter.Unsubscribe<RazorResponse>(this, Constraints.RP_PaidRevealResponse);
        //            });
        //        }
        //        else
        //        {
        //            RequestPayLoad mPayLoad = new RequestPayLoad()
        //            {
        //                amount = payload.amount,
        //                currency = payload.currency,
        //                accept_partial = false,
        //                description = Constraints.Str_RevealContact,
        //                customer = new Customer()
        //                {
        //                    contact = Common.mSellerDetails.PhoneNumber,
        //                    email = Common.mSellerDetails.Email,
        //                    name = Common.mSellerDetails.FullName
        //                },
        //                callback_method = "get",
        //                callback_url = "https://purple-field-04c774300.azurestaticapps.net/login",
        //            };
        //            RazorPayUtility razorPayUtility = new RazorPayUtility();
        //            var urls = await razorPayUtility.PayViaRazor(payload, mPayLoad, Constraints.RP_UserName, Constraints.RP_Password);
        //            if (urls != null && urls.Count > 0)
        //            {
        //                var url = urls.FirstOrDefault();
        //                var orderId = urls.LastOrDefault();
        //                var checkoutPage = new CheckOutPage(url);
        //                checkoutPage.PaidEvent += async (s1, e1) =>
        //                {
        //                    RazorResponse razorResponse = new RazorResponse();
        //                    var keyValuePairs = (Dictionary<string, string>)s1;
        //                    if (keyValuePairs != null)
        //                    {
        //                        razorResponse.isPaid = true;
        //                        razorResponse.PaymentId = keyValuePairs.Where(x => x.Key == "razorpay_payment_id").FirstOrDefault().Value;
        //                        razorResponse.OrderId = keyValuePairs.Where(x => x.Key == "razorpay_payment_link_reference_id").FirstOrDefault().Value;
        //                        razorResponse.Signature = keyValuePairs.Where(x => x.Key == "razorpay_signature").FirstOrDefault().Value;
        //                    }
        //                    else
        //                    {
        //                        razorResponse.isPaid = false;
        //                        razorResponse.OrderId = orderId;
        //                    }
        //                    RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
        //                    mRevealBuyerContact.RequirementId = RequirementId;
        //                    mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)RevealContactStatus.Success : (int)RevealContactStatus.Failure;
        //                    mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderId;
        //                    mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;
        //                    BtnRevealContact.Text = RevealContact(mRevealBuyerContact);
        //                };
        //                await Navigation.PushAsync(checkoutPage);
        //            }
        //        }
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
        //        Common.DisplayErrorMessage("RequirementDetailPage/RevealContact: " + ex.Message);
        //    }
        //    finally
        //    {
        //        UserDialogs.Instance.HideLoading();
        //    }
        //}

    }
}
