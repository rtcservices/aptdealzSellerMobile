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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequirementDetailPage : ContentPage
    {
        #region [ Objects ]
        private List<string> subcaregories;
        private string RequirementId;
        private Requirement mRequirement;
        private string ProductImageUrl = string.Empty;
        #endregion

        #region [ Constructor ]
        public RequirementDetailPage(string redId)
        {
            try
            {
                InitializeComponent();
                RequirementId = redId;

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
                Common.DisplayErrorMessage("RequirementDetailPage/Ctor: " + ex.Message);
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
            GetRequirementDetails();
        }

        private async Task GetRequirementDetails()
        {
            try
            {
                mRequirement = await DependencyService.Get<IRequirementRepository>().GetRequirementById(RequirementId);
                if (mRequirement != null)
                {
                    if (!Common.EmptyFiels(mRequirement.ProductImage))
                    {
                        imgProductImage.Source = mRequirement.ProductImage;
                        ProductImageUrl = mRequirement.ProductImage;
                    }
                    else
                    {
                        imgProductImage.Source = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.ImgProductBanner : Constraints.ImgProductBannerWhite;
                    }

                    lblTitle.Text = mRequirement.Title;
                    lblRequirementId.Text = mRequirement.RequirementNo;
                    lblDescription.Text = mRequirement.ProductDescription;
                    lblBuyerId.Text = mRequirement.BuyerNo;
                    lblCategory.Text = mRequirement.Category;
                    lblQuantity.Text = mRequirement.Quantity.ToString() + " " + mRequirement.Unit;
                    lblEstimatePrice.Text = "Rs " + mRequirement.TotalPriceEstimation.ToString();
                    lblPreferredSource.Text = (string)mRequirement.PreferredSourceOfSupply;

                    lblDeliveryDateValue.Text = mRequirement.DeliveryDate;
                    lblLocPinCode.Text = mRequirement.DeliveryLocationPinCode;

                    if (mRequirement.SubCategories != null)
                    {
                        subcaregories = mRequirement.SubCategories;
                        lblSubCategory.Text = string.Join(",", subcaregories);
                    }

                    lblNeedInsurance.Text = mRequirement.NeedInsuranceCoverage ? Constraints.Str_Right : Constraints.Str_Wrong;
                    lblPreferInIndiaProducts.Text = mRequirement.PreferInIndiaProducts ? Constraints.Str_Right : Constraints.Str_Wrong;
                    lblIsReseller.Text = mRequirement.IsReseller ? Constraints.Str_Right : Constraints.Str_Wrong;


                    if (mRequirement.PickupProductDirectly)
                    {
                        lblPreferSeller.Text = Constraints.Str_Right;
                        lblDeliveryDate.Text = Constraints.Str_ExpectedPickupDate;
                        stkLocPinCode.IsVisible = false;
                    }
                    else
                    {
                        stkLocPinCode.IsVisible = true;
                        lblPreferSeller.Text = Constraints.Str_Wrong;
                        lblDeliveryDate.Text = Constraints.Str_ExpectedDeliveryDate;
                    }

                    if (mRequirement.IsBuyerContactRevealed)
                    {
                        if (mRequirement.BuyerContact != null)
                            BtnRevealContact.Text = mRequirement.BuyerContact.PhoneNumber;
                    }
                    else
                    {
                        BtnRevealContact.Text = Constraints.Str_RevealContact;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementDetailPage/GetRequirementDetails: " + ex.Message);
            }
        }

        private async Task RevealBuyerContact()
        {
            try
            {
                long revealRs = 0;
                decimal amount = 0;

                if (BtnRevealContact.Text == Constraints.Str_RevealContact)
                {
                    RequirementAPI requirementAPI = new RequirementAPI();
                    var mResponse = await requirementAPI.GetAmountToBePaidToRevealBuyerContact(mRequirement.RequirementId);
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
                        string message = "You need to pay Rs " + revealRs + " to reveal the Buyer contact information. Do you wish to continue making payment?";

                        var contactPopup = new Popup.PaymentPopup(message);
                        contactPopup.isRefresh += async (s1, e1) =>
                        {
                            bool isPay = (bool)s1;
                            if (isPay)
                            {
                                await RevealContact(RequirementId, revealRs);
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
                Common.DisplayErrorMessage("RequirementDetailPage/RevealBuyerContact: " + ex.Message);
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
                        MessagingCenter.Unsubscribe<RazorResponse>(this, Constraints.RP_PaidRevealResponse);
                        if (razorResponse != null && !razorResponse.isPaid)
                        {
                            string message = "Payment failed ";

                            if (!Common.EmptyFiels(razorResponse.OrderId))
                                message += "OrderId: " + razorResponse.OrderId + " ";

                            if (!Common.EmptyFiels(razorResponse.PaymentId))
                                message += "PaymentId: " + razorResponse.PaymentId + " ";

                            if (message != null)
                                Common.DisplayErrorMessage(message);
                             return;
                        }

                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
                        mRevealBuyerContact.RequirementId = RequirementId;
                        mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)RevealContactStatus.Success : (int)RevealContactStatus.Failure;
                        mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderId;
                        mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;
                        BtnRevealContact.Text = await DependencyService.Get<IRequirementRepository>().RevealContact(mRevealBuyerContact);
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
                        var isPaid = false;
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
                                if (isPaid) return;
                                isPaid = true;
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
                Common.DisplayErrorMessage("RequirementDetailPage/RevealContact: " + ex.Message);
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
                Common.DisplayErrorMessage("RequirementDetailPage/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementDetailPage/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
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
                Common.DisplayErrorMessage("RequirementDetailPage/BtnRevealContact_Tapped: " + ex.Message);
            }
        }

        private async void BtnProvideQuote_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnProvideQoute);
                await Navigation.PushAsync(new ProvideQuotePage(mRequirement.RequirementId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementDetailPage/BtnProvideQuote_Tapped: " + ex.Message);
            }
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            await GetRequirementDetails();
            rfView.IsRefreshing = false;
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
                        Common.CopyText(lblRequirementId, message);
                    }
                    else if (stackLayout.ClassId == "BuyerId")
                    {
                        string message = Constraints.CopiedBuyerId;
                        Common.CopyText(lblBuyerId, message);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementDetailPage/CopyString_Tapped: " + ex.Message);
            }
        }

        private async void FrmProductImage_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                if (!Common.EmptyFiels(ProductImageUrl))
                {
                    var base64File = ImageConvertion.ConvertImageURLToBase64(ProductImageUrl);
                    string extension = Path.GetExtension(ProductImageUrl).ToLower();

                    var successPopup = new Popup.DisplayDocumentPopup(base64File, extension);
                    await PopupNavigation.Instance.PushAsync(successPopup);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementDetailPage/FrmProductImage_Tapped: " + ex.Message);
            }
            IsBusy = false;
        }
        #endregion
    }
}