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
        #region [ Properties ]
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

        #region [ Objects ]
        private string RequirementId;
        bool isFirstLoad = true;
        private string ErrorMessage = string.Empty;
        private Requirement mRequirement;
        private Quote mQuote;
        private RequestQuote mRequestQuote;
        #endregion

        #region [ Constructor ]
        public ProvideQuotePage(string requirementId)
        {
            try
            {
                InitializeComponent();
                lblTotalAmount.Text = "Rs 0";
                RequirementId = requirementId;
                mQuote = new Quote();
                mRequestQuote = new RequestQuote();


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
                Common.DisplayErrorMessage("ProvideQuotePage/Ctor: " + ex.Message);
            }
        }

        public ProvideQuotePage(Quote mQuote)
        {
            try
            {
                InitializeComponent();
                lblTotalAmount.Text = "Rs 0";
                this.mQuote = mQuote;
                RequirementId = this.mQuote.RequirementId;
                mRequestQuote = new RequestQuote();
                GetRequirementsById();
                BindQuoteDetails();

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
                Common.DisplayErrorMessage("ProvideQuotePage/ctor: " + ex.Message);
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
            dpValidityDate.MinimumDate = DateTime.Today;
            if (mQuote.ValidityDate == DateTime.MinValue)
            {
                dpValidityDate.NullableDate = null;
            }
            else
            {
                dpValidityDate.NullableDate = mQuote.ValidityDate;
                dpValidityDate.Date = mQuote.ValidityDate;
            }
            GetRequirementsById();
        }

        #region [ Binding ]
        private async void BindPropertiesByRequirementId()
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
                    lblPinManditory.IsVisible = true;
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
                Common.DisplayErrorMessage("ProvideQuotePage/BindProperties: " + ex.Message);
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
                Common.DisplayErrorMessage("ProvideQuotePage/GetCountries: " + ex.Message);
            }
        }

        private async void BindQuoteDetails()
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
                Common.DisplayErrorMessage("ProvideQuotePage/GetCountries: " + ex.Message);
            }
        }
        #endregion

        #region [ Validations ]
        private bool Validations()
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
                    if (mRequirement.PickupProductDirectly && Common.EmptyFiels(txtShippingPinCode.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.Required_PickUp_Product);
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProvideQuotePage/Validations: " + ex.Message);
            }
            return isValid;
        }

        private void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtUnitPrice.Text))
                {
                    BoxUnitPrice.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
                if (dpValidityDate.NullableDate == null)
                {
                    BoxValidityDate.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
                if (Common.EmptyFiels(pckCountry.Text))
                {
                    BoxCountry.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
                if (mRequirement.PickupProductDirectly && Common.EmptyFiels(txtShippingPinCode.Text))
                {
                    BoxPinCode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProvideQuotePage/EnableRequiredFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            try
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProvideQuotePage/FieldsTrim: " + ex.Message);
            }
        }

        private void CalculateQuoteAmount()
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
                Common.DisplayErrorMessage("ProvideQuotePage/CalculateQuoteAmount: " + ex.Message);
            }
        }

        private async Task<bool> PinCodeValidation()
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
                        BoxPinCode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else
                    {
                        BoxPinCode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                    }

                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProvideQuotePage/PinCodeValidation: " + ex.Message);
            }
            return isValid;
        }
        #endregion

        #region [ Save Quote ]
        private Model.Request.RequestQuote FillQuote()
        {
            try
            {
                mRequestQuote.RequirementId = mRequirement.RequirementId;

                mRequestQuote.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                mRequestQuote.HandlingCharges = Convert.ToDecimal(txtHandlingCharge.Text);
                mRequestQuote.ShippingCharges = Convert.ToDecimal(txtShippingCharge.Text);
                mRequestQuote.InsuranceCharges = Convert.ToDecimal(txtInsuranceCharge.Text);


                if (Common.EmptyFiels(txtShippingPinCode.Text))
                    mRequestQuote.ShippingPinCode = "";
                else
                    mRequestQuote.ShippingPinCode = txtShippingPinCode.Text;

                if (dpValidityDate.NullableDate != null && dpValidityDate.NullableDate != DateTime.MinValue)
                {
                    mRequestQuote.ValidityDate = dpValidityDate.NullableDate.Value;
                }

                if (!Common.EmptyFiels(pckCountry.Text))
                {
                    mRequestQuote.CountryId = (int)(Common.mCountries.Where(x => x.Name.ToLower() == pckCountry.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);
                }

                if (!Common.EmptyFiels(edtrComment.Text))
                {
                    mRequestQuote.Comments = edtrComment.Text;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return mRequestQuote;
        }

        private async Task SaveQuote()
        {
            try
            {
                if (Validations())
                {
                    if (!await PinCodeValidation())
                        return;

                    FieldsTrim();
                    var mRequestQuote = FillQuote();
                    if (mRequestQuote != null)
                    {
                        QuoteAPI quoteAPI = new QuoteAPI();
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        Response mResponse = new Response();

                        if (Common.EmptyFiels(mQuote.QuoteId))
                        {
                            mResponse = await quoteAPI.SaveQuote(mRequestQuote);
                        }
                        else
                        {
                            mRequestQuote.QuoteId = mQuote.QuoteId;
                            mResponse = await quoteAPI.UpdateQuote(mRequestQuote);
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
                Common.DisplayErrorMessage("ProvideQuotePage/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void SuccessfullSavedQuote(string MessageString)
        {
            try
            {
                var successPopup = new Popup.SuccessPopup(MessageString);
                successPopup.isRefresh += async (s1, e1) =>
                {
                    bool res = (bool)s1;
                    if (res)
                    {
                        if (!Common.EmptyFiels(mRequestQuote.QuoteId))
                        {
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await Navigation.PushAsync(new MainTabbedPage("Quotes"));
                        }
                        ClearPropeties();
                    }
                };

                PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProvideQuotePage/SuccessSavedQuote: " + ex.Message);
            }
        }

        private void ClearPropeties()
        {
            txtUnitPrice.Text = string.Empty;
            txtHandlingCharge.Text = string.Empty;
            txtShippingCharge.Text = string.Empty;
            txtInsuranceCharge.Text = string.Empty;
            txtShippingPinCode.Text = string.Empty;
            pckCountry.Text = string.Empty;
            edtrComment.Text = string.Empty;
        }
        #endregion

        private async void GetRequirementsById()
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
                            BindPropertiesByRequirementId();
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
                Common.DisplayErrorMessage("ProvideQuotePage/GetRequirementsById: " + ex.Message);
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
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ProvideQuotePage/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("ProvideQuotePage/ImgNotification_Tapped: " + ex.Message);
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

        private async void BtnSubmitQuote_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnSubmitQuote);
                BtnSubmitQuote.IsEnabled = false;
                await SaveQuote();
                BtnSubmitQuote.IsEnabled = true;
            }
            catch (Exception ex)
            {
                BtnSubmitQuote.IsEnabled = true;
                Common.DisplayErrorMessage("ProvideQuotePage/Submit_QuoteTapped: " + ex.Message);
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
                Common.DisplayErrorMessage("ProvideQuotePage/AutoSuggestBox_QuerySubmitted: " + ex.Message);
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
                Common.DisplayErrorMessage("ProvideQuotePage/AutoSuggestBox_TextChanged: " + ex.Message);
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

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            CalculateQuoteAmount();
            var entry = (Extention.ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                BoxUnitPrice.BackgroundColor = (Color)App.Current.Resources["appColor8"];
            }
        }

        private void pckCountry_Unfocused(object sender, FocusEventArgs e)
        {
            var autoSuggestBox = (Extention.ExtAutoSuggestBox)sender;
            if (!Common.EmptyFiels(autoSuggestBox.Text))
            {
                BoxCountry.BackgroundColor = (Color)App.Current.Resources["appColor8"];
            }
        }

        private void dpValidityDate_Unfocused(object sender, FocusEventArgs e)
        {
            var date = (Extention.ExtDatePicker)sender;
            if (date.NullableDate != null)
            {
                BoxValidityDate.BackgroundColor = (Color)App.Current.Resources["appColor8"];
            }
        }
        #endregion
    }
}