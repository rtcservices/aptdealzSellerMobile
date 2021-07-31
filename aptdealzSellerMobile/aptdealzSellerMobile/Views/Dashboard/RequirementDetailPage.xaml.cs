using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequirementDetailPage : ContentPage
    {
        #region Objects
        private List<string> subcaregories;
        private string RequirementId;
        private bool isReveal = false;
        private Requirement mRequirement;
        private RequirementAPI requirementAPI;
        #endregion

        #region Constructor
        public RequirementDetailPage(string redId)
        {
            InitializeComponent();
            RequirementId = redId;
            requirementAPI = new RequirementAPI();

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
            GetRequirementDetails();
        }

        private async Task GetRequirementDetails()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await requirementAPI.GetRequirementDetailsWithoutQuoteDetails(RequirementId);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mRequirement = jObject.ToObject<Requirement>();
                        if (mRequirement != null)
                        {
                            BindDetails();
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
                Common.DisplayErrorMessage("RequirementDetailPage/GetRequirementsById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindDetails()
        {
            try
            {
                if (!Common.EmptyFiels(mRequirement.ProductImage))
                {
                    imgProductImage.Source = mRequirement.ProductImage;
                }
                else
                {
                    imgProductImage.Source = "iconProductBanner.png";
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

                if (mRequirement.NeedInsuranceCoverage)
                {
                    lblNeedInsurance.Text = "✓";
                }
                else
                {
                    lblNeedInsurance.Text = "✕";
                }

                if (mRequirement.PreferInIndiaProducts)
                {
                    lblPreferInIndiaProducts.Text = "✓";
                }
                else
                {
                    lblPreferInIndiaProducts.Text = "✕";
                }

                if (mRequirement.PickupProductDirectly)
                {
                    lblPreferSeller.Text = "✓";
                    lblDeliveryDate.Text = "Expected Pickup Date";
                }
                else
                {
                    lblPreferSeller.Text = "✕";
                    lblDeliveryDate.Text = "Expected Delivery Date";
                }

                if (mRequirement.IsBuyerContactRevealed)
                {
                    RevealBuyerContact mRevealBuyerContact = new RevealBuyerContact();
                    var jObject = (JObject)mRequirement.BuyerContact;
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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementDetailPage/GetDetails: " + ex.Message);
            }
        }

        private void RevealBuyerContact()
        {
            if (BtnRevealContact.Text == "Reveal Contact")
            {
                RevealContact(RequirementId);
                isReveal = true;
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
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void BtnRevealContact_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRevealContact);
            RevealBuyerContact();
        }

        private void BtnProvideQuote_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnProvideQoute);
            Navigation.PushAsync(new ProvideQuotePage(mRequirement.RequirementId));
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

        //private void BtnReject_Tapped(object sender, EventArgs e)
        //{
        //    Common.BindAnimation(button: BtnReject);
        //}
        #endregion
    }
}