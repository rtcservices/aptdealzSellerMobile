using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MainTabbedPages;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProvideQuotePage : ContentPage, INotifyPropertyChanged
    {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private ObservableCollection<string> _mCountriesData;
        public ObservableCollection<string> mCountriesData
        {
            get { return _mCountriesData; }
            set
            {
                _mCountriesData = value;
                OnPropertyChanged("mCountriesData");
            }
        }
        #endregion

        #region Objects
        private string RequirementId;
        bool isFirstLoad = true;
        private string ErrorMessage = string.Empty;
        private Requirement mRequirement;
        private Quote mQuote;
        #endregion

        #region Constructor
        public ProvideQuotePage(string requirementId)
        {
            InitializeComponent();
            lblTotalAmount.Text = "Rs 0";
            RequirementId = requirementId;
            GetRequirementsById();
        }

        public ProvideQuotePage(Quote mQuote)
        {
            InitializeComponent();
            lblTotalAmount.Text = "Rs 0";
            this.mQuote = mQuote;
            RequirementId = this.mQuote.RequirementId;
            GetRequirementsById();
            BindQuoteDetails();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            dpValidityDate.NullableDate = null;
            dpValidityDate.MinimumDate = DateTime.Today;
        }

        public bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtUnitPrice.Text) || Common.EmptyFiels(pckCountry.Text) || dpValidityDate.NullableDate == null)
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtUnitPrice.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_UnitPrice);
                }
                else if (dpValidityDate.NullableDate == null)
                {
                    Common.DisplayErrorMessage(Constraints.Required_QuoteValidityDate);
                }
                else if (Common.EmptyFiels(pckCountry.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_CountryofOrigin);
                }
                else if (Common.mCountries.Where(x => x.Name.ToLower() == pckCountry.Text.ToLower()).Count() == 0)
                {
                    Common.DisplayErrorMessage(Constraints.InValid_CountryOfOrigin);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtUnitPrice.Text))
                {
                    BoxUnitPrice.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                if (dpValidityDate.NullableDate == null)
                {
                    BoxValidityDate.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
                if (Common.EmptyFiels(pckCountry.Text))
                {
                    BoxCountry.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/EnableRequiredFields: " + ex.Message);
            }
        }

        async void BindProperties()
        {
            try
            {
                lblReqId.Text = mRequirement.RequirementNo;
                txtQuantity.Text = mRequirement.Quantity.ToString();

                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                await GetCountries();

                if (mRequirement.PickupProductDirectly)
                {
                    if (mRequirement.NeedInsuranceCoverage)
                        StkInsuranceCharge.IsVisible = true;
                    else
                        StkInsuranceCharge.IsVisible = false;

                    StkShippingCharge.IsVisible = false;
                    lblPinCode.Text = "Product Pickup PIN Code";
                }
                else
                {
                    StkShippingCharge.IsVisible = true;
                    StkInsuranceCharge.IsVisible = true;
                    lblPinCode.Text = "Shipping PIN Code";
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/BindProperties: " + ex.Message);
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
                Common.DisplayErrorMessage("QuoteDetailPage/GetCountries: " + ex.Message);
            }
        }

        async void BindQuoteDetails()
        {
            try
            {
                txtUnitPrice.Text = mQuote.UnitPrice.ToString();
                txtHandlingCharge.Text = mQuote.HandlingCharges.ToString();
                txtShippingCharge.Text = mQuote.ShippingCharges.ToString();
                txtInsuranceCharge.Text = mQuote.InsuranceCharges.ToString();
                txtShippingPinCode.Text = mQuote.ShippingPinCode;
                dpValidityDate.NullableDate = mQuote.ValidityDate;
                dpValidityDate.Date = mQuote.ValidityDate;
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
                mCountriesData = new ObservableCollection<string>(Common.mCountries.Select(x => x.Name));
                pckCountry.ItemsSource = Common.mCountries.ToList();
                pckCountry.Text = countryName;

                lblTotalAmount.Text = "Rs " + mQuote.TotalQuoteAmount;
                if (!Common.EmptyFiels(mQuote.Comments))
                {
                    edtrComment.Text = mQuote.Comments;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/GetCountries: " + ex.Message);
            }
        }

        Model.Request.Quote FillQuote()
        {
            if (this.mQuote == null)
                mQuote = new Quote();
            try
            {
                mQuote.RequirementId = mRequirement.RequirementId;
                mQuote.SellerId = mRequirement.UserId;

                mQuote.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                mQuote.HandlingCharges = Convert.ToDecimal(txtHandlingCharge.Text);
                mQuote.ShippingCharges = Convert.ToDecimal(txtShippingCharge.Text);
                mQuote.InsuranceCharges = Convert.ToDecimal(txtInsuranceCharge.Text);
                mQuote.ShippingPinCode = txtShippingPinCode.Text;


                if (dpValidityDate.NullableDate != null && dpValidityDate.NullableDate != DateTime.MinValue)
                {
                    mQuote.ValidityDate = dpValidityDate.NullableDate.Value;
                }

                if (!Common.EmptyFiels(pckCountry.Text))
                {
                    mQuote.CountryId = (int)(Common.mCountries.Where(x => x.Name.ToLower() == pckCountry.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);
                }

                if (!Common.EmptyFiels(edtrComment.Text))
                {
                    mQuote.Comments = edtrComment.Text;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return mQuote;
        }

        void FieldsTrim()
        {
            txtUnitPrice.Text = txtUnitPrice.Text.Trim();
            if (!Common.EmptyFiels(txtHandlingCharge.Text))
            {
                txtHandlingCharge.Text = txtHandlingCharge.Text.Trim();
            }
            if (!Common.EmptyFiels(txtShippingCharge.Text))
            {
                txtShippingCharge.Text = txtShippingCharge.Text.Trim();
            }
            if (!Common.EmptyFiels(txtInsuranceCharge.Text))
            {
                txtInsuranceCharge.Text = txtInsuranceCharge.Text.Trim();
            }
            if (!Common.EmptyFiels(txtShippingPinCode.Text))
            {
                txtShippingPinCode.Text = txtShippingPinCode.Text.Trim();
            }
            if (!Common.EmptyFiels(edtrComment.Text))
            {
                edtrComment.Text = edtrComment.Text.Trim();
            }
        }

        async void SaveQuote()
        {
            try
            {
                if (Validations())
                {
                    if (!await PinCodeValidation())
                        return;

                    FieldsTrim();
                    var mQuote = FillQuote();
                    if (mQuote != null)
                    {
                        QuoteAPI quoteAPI = new QuoteAPI();
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        Response mResponse = new Response();

                        if (Common.EmptyFiels(mQuote.QuoteId))
                        {
                            mResponse = await quoteAPI.SaveQuote(mQuote);
                        }
                        else
                        {
                            mResponse = await quoteAPI.UpdateQuote(mQuote);
                        }

                        if (mResponse != null && mResponse.Succeeded)
                        {
                            SuccessfullSavedQuote(mResponse.Message);
                        }
                        else
                        {
                            if (mResponse != null && !Common.EmptyFiels(mResponse.Message))
                                Common.DisplayErrorMessage(mResponse.Message);
                            else
                                Common.DisplayErrorMessage(Constraints.Something_Wrong);
                        }
                    }
                    else
                    {
                        if (ErrorMessage != null)
                            Common.DisplayErrorMessage(ErrorMessage);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        void SuccessfullSavedQuote(string MessageString)
        {
            try
            {
                var successPopup = new Popup.SuccessPopup(MessageString);
                successPopup.isRefresh += (s1, e1) =>
                {
                    bool res = (bool)s1;
                    if (res)
                    {
                        if (!Common.EmptyFiels(mQuote.QuoteId))
                        {
                            Navigation.PopAsync();
                        }
                        else
                        {
                            Navigation.PushAsync(new MainTabbedPage("Submitted"));
                        }
                        ClearPropeties();
                    }
                };

                PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/SuccessSavedQuote: " + ex.Message);
            }
        }

        void ClearPropeties()
        {
            txtUnitPrice.Text = string.Empty;
            txtHandlingCharge.Text = string.Empty;
            txtShippingCharge.Text = string.Empty;
            txtInsuranceCharge.Text = string.Empty;
            txtShippingPinCode.Text = string.Empty;
            pckCountry.Text = string.Empty;
            edtrComment.Text = string.Empty;
        }

        void CalculateQuoteAmount()
        {
            try
            {
                double unitPrice = 0;
                double handlingCharge = 0;
                double shippingCharge = 0;
                double insuranceCharge = 0;

                if (!Common.EmptyFiels(txtUnitPrice.Text))
                {
                    var totalAmount = mRequirement.Quantity * Convert.ToDecimal(txtUnitPrice.Text);
                    double.TryParse(totalAmount.ToString(), out unitPrice);
                }

                if (!Common.EmptyFiels(txtHandlingCharge.Text))
                {
                    double.TryParse(txtHandlingCharge.Text, out handlingCharge);
                }

                if (!Common.EmptyFiels(txtShippingCharge.Text))
                {
                    double.TryParse(txtShippingCharge.Text, out shippingCharge);
                }

                if (!Common.EmptyFiels(txtInsuranceCharge.Text))
                {
                    double.TryParse(txtInsuranceCharge.Text, out insuranceCharge);
                }

                var amount = unitPrice + handlingCharge + shippingCharge + insuranceCharge;
                lblTotalAmount.Text = "Rs " + amount;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/CalculateQuoteAmount: " + ex.Message);
            }
        }

        async Task<bool> PinCodeValidation()
        {
            bool isValid = false;
            try
            {
                if (!Common.EmptyFiels(txtShippingPinCode.Text))
                {
                    txtShippingPinCode.Text = txtShippingPinCode.Text.Trim();

                    isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(txtShippingPinCode.Text);
                    if (isValid)
                    {
                        BoxPinCode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else
                    {
                        BoxPinCode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/PinCodeValidation: " + ex.Message);
            }
            return isValid;
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
                            BindProperties();
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
                Common.DisplayErrorMessage("QuoteDetailPage/GetRequirementsById: " + ex.Message);
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

        private void ImgCoutryPck_Clicked(object sender, EventArgs e)
        {
            pckCountry.Focus();
        }

        private void Submit_Quote_Click(object sender, EventArgs e)
        {
            //SubmitQuote.Focus();
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

        private void BtnSubmitQuote_Clicked(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: BtnSubmitQuote);
                SaveQuote();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/Submit_QuoteTapped: " + ex.Message);
            }
        }

        private void ValidityDate_Tapped(object sender, EventArgs e)
        {
            dpValidityDate.Focus();
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                if (e.ChosenSuggestion != null)
                {
                    pckCountry.Text = e.ChosenSuggestion.ToString();
                }
                else
                {
                    // User hit Enter from the search box. Use args.QueryText to determine what to do.
                    pckCountry.Unfocus();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/AutoSuggestBox_QuerySubmitted: " + ex.Message);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pckCountry.Text = e.SelectedItem.ToString();
        }

        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    if (isFirstLoad || i < 2)
                    {
                        isFirstLoad = false;
                        pckCountry.IsSuggestionListOpen = false;
                        i++;
                        return;
                    }
                }

                if (mCountriesData == null)
                    mCountriesData = new ObservableCollection<string>();

                mCountriesData.Clear();
                if (!string.IsNullOrEmpty(pckCountry.Text))
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Where(x => x.Name.ToLower().Contains(pckCountry.Text.ToLower())).Select(x => x.Name));
                }
                else
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Select(x => x.Name));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/AutoSuggestBox_TextChanged: " + ex.Message);
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            CalculateQuoteAmount();
            var entry = (Extention.ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                BoxUnitPrice.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }

        private void pckCountry_Unfocused(object sender, FocusEventArgs e)
        {
            var autoSuggestBox = (Extention.ExtAutoSuggestBox)sender;
            if (!Common.EmptyFiels(autoSuggestBox.Text))
            {
                BoxCountry.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }

        private async void txtShippingPinCode_Unfocused(object sender, FocusEventArgs e)
        {
            await PinCodeValidation();
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        private void dpValidityDate_Unfocused(object sender, FocusEventArgs e)
        {
            var date = (Extention.ExtDatePicker)sender;
            if (date.NullableDate != null)
            {
                BoxValidityDate.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }
    }
}