using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Reponse;
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
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteDetailsPage : ContentPage
    {
        #region [ Objects ]
        private string QuotationId;
        Quote mQuote;
        #endregion

        #region [ Constructor ]
        public QuoteDetailsPage(string quoteId)
        {
            try
            {
                InitializeComponent();
                QuotationId = quoteId;
                mQuote = new Quote();

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount); MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
                {
                    if (!Common.EmptyFiels(Common.NotificationCount))
                    {
                        lblNotificationCount.Text = count;
                        frmNotification.IsVisible = true;
                    }
                    else
                    {
                        frmNotification.IsVisible = false;
                        lblNotificationCount.Text = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetQuotes();
        }

        private async Task GetQuotes()
        {
            try
            {
                QuoteAPI quoteAPI = new QuoteAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                var mResponse = await quoteAPI.GetQuotesById(QuotationId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mQuote = jObject.ToObject<Quote>();
                        if (mQuote != null)
                        {
                            await GetQuotesDetails();
                            await GetRequirementsById();
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/GetRequirementsById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task GetCountries()
        {
            try
            {
                if (Common.mCountries == null || Common.mCountries.Count == 0)
                {
                    Common.mCountries = await DependencyService.Get<IProfileRepository>().GetCountry();

                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/GetCountries: " + ex.Message);
            }
        }

        private async Task GetQuotesDetails()
        {
            try
            {
                lblQuoteRequirementId.Text = mQuote.RequirementNo;
                lblQuoteReferenceNo.Text = mQuote.QuoteNo;
                lblQuoteBuyerId.Text = mQuote.BuyerId;
                lblQuoteSellerId.Text = mQuote.SellerId;
                lblQuoteQuntity.Text = "" + mQuote.RequestedQuantity + " " + mQuote.Unit;
                lblQuoteUnitPrice.Text = "Rs " + mQuote.UnitPrice;
                lblQuoteNetAmount.Text = "Rs " + mQuote.NetAmount;
                lblQuoteHandlingCharge.Text = "Rs " + mQuote.HandlingCharges;
                lblQuoteShippingCharge.Text = "Rs " + mQuote.ShippingCharges;
                lblQuoteInsuranceCharge.Text = "Rs " + mQuote.InsuranceCharges;
                lblTotalAmount.Text = "Rs " + mQuote.TotalQuoteAmount;
                lblIsReseller.Text = mQuote.IsReseller ? Constraints.Str_Right : Constraints.Str_Wrong;

                string countryName = "";
                if (Common.mCountries == null || Common.mCountries.Count == 0)
                {
                    await GetCountries();
                }

                if (Common.mCountries != null)
                {
                    if (mQuote.CountryId > 0)
                        countryName = Common.mCountries.Where(x => x.CountryId == mQuote.CountryId).FirstOrDefault()?.Name;
                    else
                        countryName = Common.mCountries.Where(x => x.Name == mQuote.Country).FirstOrDefault()?.Name;
                }

                lblQuoteCountry.Text = countryName;
                lblValidityDate.Text = mQuote.ValidityDate.Date.ToString(Constraints.Str_DateFormate);
                lblStatus.Text = mQuote.Status;

                if (!Common.EmptyFiels(mQuote.ShippingPinCode))
                {
                    lblQuotePinCode.Text = mQuote.ShippingPinCode;
                }

                #region [ Reveal Contact ]
                if (mQuote.Status == QuoteStatus.Accepted.ToString())
                {
                    BtnRevealContact.IsVisible = false;
                    lblBuyerContact.IsVisible = true;
                    if (mQuote.BuyerContact != null)
                    {
                        lblBuyerContact.Text = mQuote.BuyerContact.PhoneNumber;
                    }
                    else
                    {
                        lblBuyerContact.Text = Constraints.Str_NotRevealContact;
                    }
                }
                else
                {
                    if (mQuote.IsBuyerContactRevealed)
                    {
                        lblBuyerContact.IsVisible = true;
                        BtnRevealContact.IsVisible = false;
                        if (mQuote.BuyerContact != null)
                            lblBuyerContact.Text = mQuote.BuyerContact.PhoneNumber;
                        else
                            lblBuyerContact.Text = Constraints.Str_NotRevealContact;
                    }
                    else
                    {
                        lblBuyerContact.IsVisible = false;
                        BtnRevealContact.IsVisible = true;
                        BtnRevealContact.Text = Constraints.Str_RevealContact;
                    }
                }
                #endregion

                if (!Common.EmptyFiels(mQuote.Comments))
                {
                    lblComments.Text = mQuote.Comments;
                }
                //mQuote.Status == QuoteStatus.Accepted.ToString() && mQuote.PaymentStatus == PaymentStatus.Success.ToString()
                if (mQuote.Status == QuoteStatus.Accepted.ToString())
                {
                    BtnEditSubmit.IsVisible = false;
                }
                else
                {
                    BtnEditSubmit.IsVisible = true;
                }

            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/GetQuotesDetails: " + ex.Message);
            }
        }

        private async Task RevealBuyerContact()
        {
            try
            {
                long revealRs = 0;
                if (BtnRevealContact.Text == Constraints.Str_RevealContact)
                {
                    decimal amount = 0;

                    RequirementAPI requirementAPI = new RequirementAPI();
                    var mResponse = await requirementAPI.GetAmountToBePaidToRevealSellerContact(mQuote.QuoteId);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mAmount = jObject.ToObject<Data>();
                            if (mAmount != null)
                                amount = mAmount.amount;
                        }

                        revealRs = (long)App.Current.Resources["RevealContact"];
                        long.TryParse(amount.ToString(), out revealRs);
                        string message = "You need to pay Rs " + revealRs + " to reveal the Seller contact information. Do you wish to continue making payment?";

                        var contactPopup = new Popup.PaymentPopup(message);
                        contactPopup.isRefresh += async (s1, e1) =>
                        {
                            bool isPay = (bool)s1;
                            if (isPay)
                            {
                                await RevealContact(mQuote.RequirementId, revealRs);
                            }
                        };
                        await PopupNavigation.Instance.PushAsync(contactPopup);
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    DependencyService.Get<IDialer>().Dial(App.Current.Resources["CountryCode"] + BtnRevealContact.Text);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/RevealBuyerContact: " + ex.Message);
            }
        }

        public async Task RevealContact(string RequirementId, long revealRs)
        {
            try
            {
                RazorPayload payload = new RazorPayload();
                payload.amount = revealRs * 100;
                payload.currency = (string)App.Current.Resources["Currency"];
                payload.receipt = RequirementId; // quoteid
                payload.email = Common.mSellerDetails.Email;
                payload.contact = Common.mSellerDetails.PhoneNumber;

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    MessagingCenter.Send<RazorPayload>(payload, Constraints.RP_RevealPayNow);
                    MessagingCenter.Subscribe<RazorResponse>(this, Constraints.RP_PaidRevealResponse, async (razorResponse) =>
                     {
                         if (razorResponse != null && !razorResponse.isPaid)
                         {
                             string message = "Payment failed ";

                             if (!Common.EmptyFiels(razorResponse.OrderId))
                                 message += "OrderId: " + razorResponse.OrderId + " ";

                             if (!Common.EmptyFiels(razorResponse.PaymentId))
                                 message += "PaymentId: " + razorResponse.PaymentId + " ";

                             if (message != null)
                                 Common.DisplayErrorMessage(message);
                         }

                         UserDialogs.Instance.ShowLoading(Constraints.Loading);

                         RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
                         mRevealBuyerContact.RequirementId = RequirementId;
                         mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)RevealContactStatus.Success : (int)RevealContactStatus.Failure;
                         mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderId;
                         mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;

                         BtnRevealContact.Text = await DependencyService.Get<IRequirementRepository>().RevealContact(mRevealBuyerContact);
                         MessagingCenter.Unsubscribe<RazorResponse>(this, Constraints.RP_PaidRevealResponse);
                     });
                }
                else
                {
                    RequestPayLoad mPayLoad = new RequestPayLoad()
                    {
                        amount = payload.amount,
                        currency = payload.currency,
                        accept_partial = false,
                        description = Constraints.Str_RevealContact,
                        customer = new Customer()
                        {
                            contact = Common.mSellerDetails.PhoneNumber,
                            email = Common.mSellerDetails.Email,
                            name = Common.mSellerDetails.FullName
                        },
                        callback_method = "get",
                        callback_url = "https://purple-field-04c774300.azurestaticapps.net/login",
                    };
                    RazorPayUtility razorPayUtility = new RazorPayUtility();
                    var urls = await razorPayUtility.PayViaRazor(payload, mPayLoad, Constraints.RP_UserName, Constraints.RP_Password);
                    if (urls != null && urls.Count > 0)
                    {
                        var url = urls.FirstOrDefault();
                        var orderId = urls.LastOrDefault();
                        var checkoutPage = new CheckOutPage(url);
                        checkoutPage.PaidEvent += async (s1, e1) =>
                        {
                            MessagingCenter.Unsubscribe<RazorResponse>(this, Constraints.RP_PaidRevealResponse);

                            RazorResponse razorResponse = new RazorResponse();
                            var keyValuePairs = (Dictionary<string, string>)s1;
                            if (keyValuePairs != null)
                            {
                                razorResponse.isPaid = true;
                                razorResponse.PaymentId = keyValuePairs.Where(x => x.Key == "razorpay_payment_id").FirstOrDefault().Value;
                                razorResponse.OrderId = keyValuePairs.Where(x => x.Key == "razorpay_payment_link_reference_id").FirstOrDefault().Value;
                                razorResponse.Signature = keyValuePairs.Where(x => x.Key == "razorpay_signature").FirstOrDefault().Value;
                            }
                            else
                            {
                                razorResponse.isPaid = false;
                                razorResponse.OrderId = orderId;
                            }
                            RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
                            mRevealBuyerContact.RequirementId = RequirementId;
                            mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)RevealContactStatus.Success : (int)RevealContactStatus.Failure;
                            mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderId;
                            mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;
                            BtnRevealContact.Text = await DependencyService.Get<IRequirementRepository>().RevealContact(mRevealBuyerContact);
                        };
                        await Navigation.PushAsync(checkoutPage);
                    }
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
                Common.DisplayErrorMessage("QuoteDetailsPage/RevealContact: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task GetRequirementsById()
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                var mResponse = await requirementAPI.GetRequirementById(mQuote.RequirementId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        var mRequirement = jObject.ToObject<Requirement>();
                        if (mRequirement != null)
                        {
                            if (mRequirement.PickupProductDirectly)
                            {
                                lblPinCode.Text = Constraints.Str_PickupPINCode;
                            }
                            else
                            {
                                lblPinCode.Text = Constraints.Str_ShippingPINCode;
                            }
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/GetRequirementsById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region [ Events ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private async void EditSubmit_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnEditSubmit);
                await Navigation.PushAsync(new Dashboard.ProvideQuotePage(mQuote));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private async void BackQuote_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnBackQuote);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/BackQuote_Tapped: " + ex.Message);
            }
        }

        private async void BtnRevealContact_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnRevealContact);
                await RevealBuyerContact();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/BtnRevealContact_Tapped: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            try
            {
                rfView.IsRefreshing = true;
                await GetQuotes();
                rfView.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/RefreshView_Refreshing: " + ex.Message);
            }
        }

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                var stackLayout = (StackLayout)sender;
                if (!Common.EmptyFiels(stackLayout.ClassId))
                {
                    if (stackLayout.ClassId == "RequirementId")
                    {
                        string message = Constraints.CopiedRequirementId;
                        Common.CopyText(lblQuoteRequirementId, message);

                        Navigation.PushAsync(new RequirementDetailPage(mQuote.RequirementId));
                    }
                    else if (stackLayout.ClassId == "QuoteRefNo")
                    {
                        string message = Constraints.CopiedQuoteRefNo;
                        Common.CopyText(lblQuoteReferenceNo, message);
                    }
                    else if (stackLayout.ClassId == "BuyerId")
                    {
                        string message = Constraints.CopiedBuyerId;
                        Common.CopyText(lblQuoteBuyerId, message);
                    }
                    else if (stackLayout.ClassId == "SellerId")
                    {
                        string message = Constraints.CopiedSellerId;
                        Common.CopyText(lblQuoteSellerId, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/StkQuoteId_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}