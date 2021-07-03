using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MainTabbedPages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
    public partial class QuoteDetailPage : ContentPage, INotifyPropertyChanged
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
        private Requirement mRequirement;
        private List<Country> mCountries;
        bool isFirstLoad = true;
        private string ErrorMessage = string.Empty;
        ProfileAPI profileAPI;
        #endregion

        #region Constructor
        public QuoteDetailPage(Requirement requirement)
        {
            InitializeComponent();
            mRequirement = requirement;
            BindProperties();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblTotalAmount.Text = "Rs 0";
            dpValidityDate.MinimumDate = DateTime.Today;
        }

        public bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtUnitPrice.Text) || Common.EmptyFiels(pckCountry.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_All);
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtUnitPrice.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_UnitPrice);
                }
                else if (Common.EmptyFiels(pckCountry.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Nationality);
                }
                else if (mCountries.Where(x => x.Name.ToLower() == pckCountry.Text.ToLower()).Count() == 0)
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Nationality);
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
                profileAPI = new ProfileAPI();
                UserDialogs.Instance.ShowLoading(Constraints.Loading);

                if (mCountries == null || mCountries.Count == 0)
                    mCountries = await DependencyService.Get<IProfileRepository>().GetCountry();

                if (mRequirement.PickupProductDirectly)
                {
                    StkPinCode.Margin = new Thickness(0, -15, 0, 0);
                    GrdOtherCharges.IsVisible = false;
                    lblPinCode.Text = "Seller's PIN Code";
                }
                else
                {
                    StkPinCode.Margin = new Thickness(0, 0, 0, 0);
                    GrdOtherCharges.IsVisible = true;
                    lblPinCode.Text = "Shiping PIN Code";
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

        Model.Request.Quote FillQuote()
        {
            Quote mQuote = new Quote();
            try
            {
                mQuote.RequirementId = mRequirement.RequirementId;
                mQuote.SellerId = mRequirement.UserId;

                mQuote.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                mQuote.HandlingCharges = Convert.ToDecimal(txtHandlingCharge.Text);
                mQuote.ShippingCharges = Convert.ToDecimal(txtShippingCharge.Text);
                mQuote.InsuranceCharges = Convert.ToDecimal(txtInsuranceCharge.Text);
                mQuote.ShippingPinCode = txtShippingPinCode.Text;
                mQuote.ValidityDate = dpValidityDate.Date;

                if (!Common.EmptyFiels(pckCountry.Text))
                {
                    mQuote.CountryId = (int)(mCountries.Where(x => x.Name == pckCountry.Text.ToString()).FirstOrDefault()?.CountryId);
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
                        var mResponse = await quoteAPI.SaveQuote(mQuote);
                        if (mResponse != null && mResponse.Succeeded)
                        {
                            //await Navigation.PushAsync(new MainTabbedPage("QrCodeScan"));
                            Common.DisplaySuccessMessage(mResponse.Message);
                            ClearPropeties();
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
                    if (Common.IsValidPincode(txtShippingPinCode.Text))
                    {
                        isValid = true;
                        //isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(Convert.ToInt32(txtShippingPinCode.Text));
                    }
                    else
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteDetailPage/PinCodeValidation: " + ex.Message);
            }
            return isValid;
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
                    mCountriesData = new ObservableCollection<string>(mCountries.Where(x => x.Name.ToLower().Contains(pckCountry.Text.ToLower())).Select(x => x.Name));
                }
                else
                {
                    mCountriesData = new ObservableCollection<string>(mCountries.Select(x => x.Name));
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
        #endregion
    }
}