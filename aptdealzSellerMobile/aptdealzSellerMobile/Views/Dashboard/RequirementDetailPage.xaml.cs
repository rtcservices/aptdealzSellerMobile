using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
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
        private Requirement mRequirement;
        private RequirementAPI requirementAPI;
        #endregion

        #region Constructor
        public RequirementDetailPage(string redId)
        {
            InitializeComponent();
            RequirementId = redId;
            requirementAPI = new RequirementAPI();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetRequirementDetailsWithoutQuoteDetails();
        }

        public async void GetRequirementDetailsWithoutQuoteDetails()
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
                            GetDetails();
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
                Common.DisplayErrorMessage("ViewRequirememntPage/GetRequirementsById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void GetDetails()
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

                lblDeliveryDate.Text = mRequirement.DeliveryDate;
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
                }
                else
                {
                    lblPreferSeller.Text = "✕";
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/GetDetails: " + ex.Message);
            }
        }

        async void RevealBuyerContact()
        {
            try
            {
                if (BtnRevealContact.Text == "Reveal Contact")
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await requirementAPI.RevealBuyerContact(RequirementId, (int)PaymentStatus.Failed);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mBuyerContact = jObject.ToObject<RevealBuyerContact>();
                            if (mBuyerContact != null)
                            {
                                var successPopup = new Popup.SuccessPopup(Constraints.ContactRevealed);
                                await PopupNavigation.Instance.PushAsync(successPopup);

                                BtnRevealContact.Text = mBuyerContact.PhoneNumber;
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
                else
                {
                    PhoneDialer.Open(BtnRevealContact.Text);
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
                Common.DisplayErrorMessage("QuoteDetailsPage/RevealBuyerContact: " + ex.Message);
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

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
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

        //private void BtnReject_Tapped(object sender, EventArgs e)
        //{
        //    Common.BindAnimation(button: BtnReject);
        //}
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}