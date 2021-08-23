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
        #region [ Objects ]
        private List<string> subcaregories;
        private string RequirementId;
        private Requirement mRequirement;
        #endregion

        #region [ Constructor ]
        public RequirementDetailPage(string redId)
        {
            try
            {
                InitializeComponent();
                RequirementId = redId;

                MessagingCenter.Unsubscribe<string>(this, "NotificationCount"); MessagingCenter.Subscribe<string>(this, "NotificationCount", (count) =>
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

        #region Methods
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
                        if (mRequirement.BuyerContact != null)
                            BtnRevealContact.Text = mRequirement.BuyerContact.PhoneNumber;
                    }
                    else
                    {

                        BtnRevealContact.Text = "Reveal Contact";

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
                                RevealContact(RequirementId);
                            }
                            else
                            {
                                //PaidQuote(false);
                            }
                        }
                    };
                    await PopupNavigation.Instance.PushAsync(contactPopup);

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
                    mRevealBuyerContact.PaymentStatus = razorResponse.isPaid ? (int)RevealContactStatus.Success : (int)RevealContactStatus.Failure;
                    mRevealBuyerContact.RazorPayOrderId = razorResponse.OrderId;
                    mRevealBuyerContact.RazorPayPaymentId = razorResponse.PaymentId;

                    BtnRevealContact.Text = await DependencyService.Get<IRequirementRepository>().RevealContact(mRevealBuyerContact);
                    MessagingCenter.Unsubscribe<RazorResponse>(this, "PaidRevealResponse");
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
                Common.DisplayErrorMessage("RequirementDetailPage/RevealContact: " + ex.Message);
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

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RequirementDetailPage/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private async void BtnRevealContact_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnRevealContact);
                    await RevealBuyerContact();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RequirementDetailPage/BtnRevealContact_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnProvideQuote_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    Common.BindAnimation(button: BtnProvideQoute);
                    await Navigation.PushAsync(new ProvideQuotePage(mRequirement.RequirementId));
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RequirementDetailPage/BtnProvideQuote_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
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

        //private void BtnReject_Tapped(object sender, EventArgs e)
        //{
        //    Common.BindAnimation(button: BtnReject);
        //}
        #endregion
    }
}