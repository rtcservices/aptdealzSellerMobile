using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using Newtonsoft.Json.Linq;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequirementDetailPage : ContentPage
    {
        #region Objects
        private string RequirementId;
        private Requirement mRequirement;
        #endregion

        #region Constructor
        public RequirementDetailPage(string redId)
        {
            InitializeComponent();
            RequirementId = redId;

        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetRequirementsById();
        }

        public async void GetRequirementsById()
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                var mResponse = await requirementAPI.GetRequirementById(RequirementId);
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
                lblBuyerId.Text = mRequirement.BuyerNo;
                lblCategory.Text = mRequirement.Category;
                lblQuantity.Text = mRequirement.Quantity.ToString() + " " + mRequirement.Unit;
                lblEstimatePrice.Text = mRequirement.TotalPriceEstimation.ToString();
                lblPreferredSource.Text = mRequirement.PreferredSourceOfSupply;
                lblDeliveryDate.Text = mRequirement.ExpectedDeliveryDate.Date.ToString("dd.MM.yyyy");
                lblLocPinCode.Text = mRequirement.DeliveryLocationPinCode;

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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/GetDetails: " + ex.Message);
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
        }

        private void BtnProvideQuote_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnProvideQoute);
            Navigation.PushAsync(new QuoteDetailPage(mRequirement));
        }

        //private void BtnReject_Tapped(object sender, EventArgs e)
        //{
        //    Common.BindAnimation(button: BtnReject);
        //}
        #endregion
    }
}