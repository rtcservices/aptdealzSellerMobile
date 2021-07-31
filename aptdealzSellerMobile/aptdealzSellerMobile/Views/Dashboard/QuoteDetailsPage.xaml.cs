using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
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
        #region Objects
        private string QuotationId;
        private bool isReveal = false;
        Quote mQuote;
        #endregion

        #region Constructor
        public QuoteDetailsPage(string quoteId)
        {
            InitializeComponent();
            QuotationId = quoteId;
            mQuote = new Quote();
            GetQuotes();

            MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
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
                            GetQuotesDetails();
                            GetRequirementsById();
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

        private async void GetQuotesDetails()
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
                lblValidityDate.Text = mQuote.ValidityDate.Date.ToString("dd/MM/yyyy");
                lblStatus.Text = mQuote.Status;

                if (!Common.EmptyFiels(mQuote.ShippingPinCode))
                {
                    lblQuotePinCode.Text = mQuote.ShippingPinCode;
                }

                if (mQuote.IsBuyerContactRevealed)
                {
                    RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
                    var jObject = (JObject)mQuote.BuyerContact;
                    if (jObject != null)
                    {
                        mRevealBuyerContact = jObject.ToObject<RevealBuyerContact>();
                    }

                    if (mRevealBuyerContact != null)
                        BtnRevealContact.Text = mRevealBuyerContact.PhoneNumber;
                }
                else
                {
                    if (!isReveal)
                    {
                        BtnRevealContact.Text = "Reveal Contact";
                    }
                }

                if (!Common.EmptyFiels(mQuote.Comments))
                {
                    lblComments.Text = mQuote.Comments;
                }

                if (mQuote.Status == QuoteStatus.Accepted.ToString() && mQuote.PaymentStatus == PaymentStatus.Success.ToString())
                    BtnEditSubmit.IsEnabled = false;
                else
                    BtnEditSubmit.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailsPage/GetQuotesDetails: " + ex.Message);
            }
        }

        private void RevealBuyerContact()
        {
            if (BtnRevealContact.Text == "Reveal Contact")
            {
                long revealRs = (long)App.Current.Resources["RevealContact"];
                string message = "You need to pay Rs " + revealRs + " to reveal the Seller contact information. Do you wish to continue making payment?";

                var contactPopup = new Popup.PaymentPopup(message);
                contactPopup.isRefresh += (s1, e1) =>
                {
                    bool isPay = (bool)s1;
                    if (isPay)
                    {
                        if (DeviceInfo.Platform == DevicePlatform.Android)
                        {
                            RevealContact(mQuote.RequirementId);
                            isReveal = true;
                        }
                        else
                        {
                            //PaidQuote(false);
                        }
                    }
                };
                PopupNavigation.Instance.PushAsync(contactPopup);

            }
            else
            {
                DependencyService.Get<IDialer>().Dial(App.Current.Resources["CountryCode"] + BtnRevealContact.Text);
            }
        }

        public void RevealContact(string RequirementId)
        {
            try
            {
                long amount = (long)App.Current.Resources["RevealContact"];
                RazorPayload payload = new RazorPayload();
                payload.amount = amount * 100;
                payload.currency = (string)App.Current.Resources["Currency"];
                payload.receipt = RequirementId; // quoteid
                payload.email = Common.mSellerDetails.Email;
                payload.contact = Common.mSellerDetails.PhoneNumber;
                MessagingCenter.Send<RazorPayload>(payload, "RevealPayNow");
                MessagingCenter.Subscribe<RazorResponse>(this, "PaidRevealResponse", async (razorResponse) =>
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
                    mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)PaymentStatus.Success : (int)PaymentStatus.Failed;
                    mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderNo;
                    mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;

                    RequirementAPI requirementAPI = new RequirementAPI();
                    var mResponse = await requirementAPI.RevealBuyerContact(mRevealBuyerContact);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mSellerContact = jObject.ToObject<RevealBuyerContact>();
                            if (mSellerContact != null)
                            {
                                var successPopup = new Views.Popup.SuccessPopup(Constraints.ContactRevealed);
                                await PopupNavigation.Instance.PushAsync(successPopup);

                                BtnRevealContact.Text = mSellerContact.PhoneNumber;
                            }
                        }
                    }
                    else
                    {
                        // "Reveal Contact";
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                });
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
        }

        private async void GetRequirementsById()
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
                                lblPinCode.Text = "Product Pickup PIN Code";
                            }
                            else
                            {
                                lblPinCode.Text = "Shipping PIN Code";
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

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage());
        }

        private void EditSubmit_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnEditSubmit);
            Navigation.PushAsync(new Dashboard.ProvideQuotePage(mQuote));

        }

        private void BackQuote_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnBackQuote);
            Navigation.PopAsync();
        }

        private void BtnRevealContact_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRevealContact);
            RevealBuyerContact();
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