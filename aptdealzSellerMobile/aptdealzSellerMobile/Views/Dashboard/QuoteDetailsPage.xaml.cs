using Acr.UserDialogs;
using aptdealzSellerMobile.API;
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
        private string RequirmentId;
        Quote mQuote;
        #endregion

        #region Constructor
        public QuoteDetailsPage(string requirmentId, string quoteId)
        {
            InitializeComponent();
            QuotationId = quoteId;
            RequirmentId = requirmentId;
            mQuote = new Quote();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetQuotesById();
        }

        public async void GetQuotesById()
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
                Common.DisplayErrorMessage("ViewRequirememntPage/GetRequirementsById: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async Task GetCountries()
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
                Common.DisplayErrorMessage("AccountView/GetCountries: " + ex.Message);
            }
        }

        async void GetQuotesDetails()
        {
            try
            {
                lblQuoteRequirementId.Text = mQuote.RequirementNo;
                lbllblQuoteReferenceNo.Text = mQuote.QuoteNo;
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

                if (!Common.EmptyFiels(mQuote.ShippingPinCode))
                {
                    lblQuotePinCode.Text = mQuote.ShippingPinCode;
                }

                if (mQuote.Status == Utility.QuoteStatus.Accepted.ToString())
                {
                    lblStatus.Text = "Buyer " + mQuote.Status + " Another Quote";
                }
                else
                {
                    lblStatus.Text = "Buyer " + mQuote.Status + " Quote";
                }

                if (!Common.EmptyFiels(mQuote.Comments))
                {
                    lblComments.Text = mQuote.Comments;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteEditPage/GetQuotesDetails: " + ex.Message);
            }
        }

        async void RevealBuyerContact()
        {
            try
            {
                if (BtnRevealContact.Text == "Reveal Contact")
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    RequirementAPI requirementAPI = new RequirementAPI();
                    var mResponse = await requirementAPI.RevealBuyerContact(RequirmentId, (int)PaymentStatus.Failed);
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

        public async void GetRequirementsById()
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                //UserDialogs.Instance.ShowLoading(Constraints.Loading);
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
                Common.DisplayErrorMessage("QuoteEditPage/GetRequirementsById: " + ex.Message);
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
            Navigation.PopAsync();
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

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
        #endregion

        private void BtnRevealContact_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnRevealContact);
            RevealBuyerContact();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}