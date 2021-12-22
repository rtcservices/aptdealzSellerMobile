using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using dotMorten.Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountView : ContentView, INotifyPropertyChanged
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
        private ProfileAPI profileAPI;
        private SellerDetails mSellerDetail;

        private List<Category> mCategories;
        private List<SubCategory> mSubCategories;
        private List<string> selectedSubCategory;
        private List<string> documentList;

        private string relativePath = string.Empty;
        private string relativeDocumentPath = string.Empty;
        private string ErrorMessage = string.Empty;

        private bool isFirstLoad = true;
        private bool isUpdatePhoto = false;
        private bool isDeleteSubCategory = true;
        private bool isUpdateProfile = false;
        #endregion

        #region [ Constructor ]
        public AccountView()
        {
            try
            {
                InitializeComponent();
                BtnUpdate.IsEnabled = false;
                BindObjects();

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount);
                MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
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
                Common.DisplayErrorMessage("AccountView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]

        #region [ Get / Bind Data ]
        private async void BindObjects()
        {
            profileAPI = new ProfileAPI();
            mCategories = new List<Category>();
            mSubCategories = new List<SubCategory>();
            selectedSubCategory = new List<string>();
            documentList = new List<string>();

            await GetCategory();
            await GetCountries();
            await GetProfile();
            CapitalizeWord();
        }

        private void CapitalizeWord()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                txtFullName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                txtStreet.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtCity.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtState.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtLandmark.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                txtDescription.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtSupplyArea.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);

                txtGstNumber.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtPan.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtBankName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
                txtIfsc.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeWord);
            }
        }

        private async Task GetCategory()
        {
            try
            {
                mCategories = await DependencyService.Get<IProfileRepository>().GetCategory();
                if (mCategories != null || mCategories.Count > 0)
                {
                    pkCategory.ItemsSource = mCategories.Select(x => x.Name).ToList();
                }

                if (pkCategory.SelectedItem != null)
                {
                    var categoryId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.CategoryId;
                    await GetSubCategoryByCategoryId(categoryId);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetCategory: " + ex.Message);
            }
        }

        private async Task GetSubCategoryByCategoryId(string categoryId)
        {
            try
            {
                if (!Common.EmptyFiels(categoryId))
                {
                    mSubCategories = await DependencyService.Get<IProfileRepository>().GetSubCategory(categoryId);
                    if (mSubCategories != null)
                    {
                        pkSubCategory.ItemsSource = mSubCategories.Select(x => x.Name).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetSubCategoryByCategoryId: " + ex.Message);
            }
        }

        private async Task CreateSubCategory(bool isNewSubCategory = false)
        {
            try
            {
                string CategoryId = "";

                if (pkCategory.SelectedIndex > -1)
                {
                    mCategories = await DependencyService.Get<IProfileRepository>().GetCategory();
                    var mCategory = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault();
                    if (mCategory != null)
                    {
                        CategoryId = mCategory.CategoryId;
                    }
                }

                if (isNewSubCategory)
                {
                    if (selectedSubCategory != null && selectedSubCategory.Count > 0)
                    {
                        foreach (var subCategory in selectedSubCategory)
                        {
                            if (!Common.EmptyFiels(subCategory) && !Common.EmptyFiels(CategoryId))
                            {
                                await DependencyService.Get<IProfileRepository>().CreateSubCategoryByCategoryId(subCategory, CategoryId);
                            }
                        }

                    }
                }
                else
                {
                    if (!Common.EmptyFiels(txtOtherSubCategory.Text) && !Common.EmptyFiels(CategoryId))
                    {
                        await DependencyService.Get<IProfileRepository>().CreateSubCategoryByCategoryId(txtOtherSubCategory.Text, CategoryId);
                    }
                }

                await GetSubCategoryByCategoryId(CategoryId);

                if (mSubCategories != null)
                {
                    if (selectedSubCategory.Where(x => x == txtOtherSubCategory.Text).Count() == 0)
                    {
                        selectedSubCategory = mSubCategories.Select(x => x.Name).ToList();
                        pkSubCategory.ItemsSource = mSubCategories.Select(x => x.Name).ToList();
                        pkSubCategory.SelectedItem = txtOtherSubCategory.Text;
                        txtOtherSubCategory.Text = string.Empty;
                    }

                    wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/CreateSubCategory: " + ex.Message);
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
                Common.DisplayErrorMessage("AccountView/GetCountries: " + ex.Message);
            }
        }

        private async Task GetProfile()
        {
            try
            {
                if (Common.mSellerDetails == null || Common.EmptyFiels(Common.mSellerDetails.SellerId) || isUpdateProfile)
                {
                    mSellerDetail = await DependencyService.Get<IProfileRepository>().GetMyProfileData();
                    Common.mSellerDetails = mSellerDetail;
                }
                else
                {
                    mSellerDetail = Common.mSellerDetails;
                }

                if (Common.mSellerDetails != null && !Common.EmptyFiels(Common.mSellerDetails.SellerId))
                {
                    GetProfileDetails(Common.mSellerDetails);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetProfile: " + ex.Message);
            }
        }

        private async void GetProfileDetails(SellerDetails mSellerDetail)
        {
            try
            {
                if (mSellerDetail != null)
                {
                    #region Profile
                    if (!Common.EmptyFiels(mSellerDetail.ProfilePhoto))
                    {
                        string baseURL = (string)App.Current.Resources["BaseURL"];
                        mSellerDetail.ProfilePhoto = baseURL + mSellerDetail.ProfilePhoto.Replace(baseURL, "");
                        imgUser.Source = mSellerDetail.ProfilePhoto;
                    }
                    else
                    {
                        imgUser.Source = "iconUserAccount.png";
                    }
                    #endregion

                    #region User Details
                    lblSellerId.Text = mSellerDetail.SellerNo;
                    txtFullName.Text = mSellerDetail.FullName;
                    txtEmail.Text = mSellerDetail.Email;
                    txtPhoneNumber.Text = mSellerDetail.PhoneNumber;
                    if (!Common.EmptyFiels((string)mSellerDetail.AlternativePhoneNumber))
                    {
                        txtAltPhoneNumber.Text = (string)mSellerDetail.AlternativePhoneNumber;
                    }
                    #endregion

                    #region Billing Address
                    txtBuildingNumber.Text = mSellerDetail.Building;
                    txtStreet.Text = mSellerDetail.Street;
                    txtCity.Text = mSellerDetail.City;
                    txtState.Text = mSellerDetail.State;
                    txtPinCode.Text = mSellerDetail.PinCode;
                    txtLandmark.Text = mSellerDetail.Landmark;
                    pkNationality.Text = mSellerDetail.Nationality;
                    #endregion

                    #region Company Profile

                    if (selectedSubCategory != null)
                    {
                        selectedSubCategory = mSellerDetail.CompanyProfile.SubCategories;
                        wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                    }

                    if (mSellerDetail.CompanyProfile.Category != null)
                    {
                        var category = mCategories.Where(x => x.Name == mSellerDetail.CompanyProfile.Category).FirstOrDefault()?.Name;

                        if (!Common.EmptyFiels(category))
                        {
                            pkCategory.SelectedItem = category;
                        }
                        else
                        {
                            txtOtherCategory.Text = mSellerDetail.CompanyProfile?.Category;
                            //
                            mCategories = await DependencyService.Get<IProfileRepository>().CreateCategory(txtOtherCategory.Text);
                            if (mCategories != null)
                            {
                                var lastAddedCategory = mCategories.Select(x => x.Name).ToList();
                                if (lastAddedCategory.Any(x => x.ToLower().Trim() == txtOtherCategory.Text.ToLower().Trim()))
                                {
                                    pkCategory.ItemsSource = lastAddedCategory.ToList();
                                    pkCategory.SelectedItem = txtOtherCategory.Text;

                                    txtOtherCategory.Text = string.Empty;
                                    await CreateSubCategory(true);
                                }
                            }
                        }
                    }

                    txtDescription.Text = mSellerDetail.CompanyProfile.Description;
                    txtExperience.Text = mSellerDetail.CompanyProfile.Experience;
                    txtSupplyArea.Text = mSellerDetail.CompanyProfile.AreaOfSupply;
                    lblSellerCommission.Text = "" + mSellerDetail.CompanyProfile.CommissionRate + "%";
                    #endregion

                    #region Bank Information
                    txtGstNumber.Text = mSellerDetail.BankInformation.Gstin.ToUpper();
                    txtPan.Text = mSellerDetail.BankInformation.Pan.ToUpper();
                    txtBankAccount.Text = mSellerDetail.BankInformation.BankAccountNumber;
                    txtBankName.Text = mSellerDetail.BankInformation.Branch;
                    txtIfsc.Text = mSellerDetail.BankInformation.Ifsc;
                    #endregion

                    #region Document List
                    documentList = mSellerDetail.Documents;
                    AddDocuments();
                    #endregion 
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetProfileDetails: " + ex.Message);
            }
        }

        private void AddDocuments()
        {
            try
            {
                if (documentList != null && documentList.Count > 0)
                {
                    lblAttachDocument.IsVisible = false;
                    lstDocument.ItemsSource = documentList.ToList();
                    lstDocument.IsVisible = true;
                }
                else
                {
                    lblAttachDocument.IsVisible = true;
                    lstDocument.ItemsSource = null;
                    lstDocument.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AddDocuments: " + ex.Message);
            }
        }
        #endregion

        #region [ Validations ]
        private bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtFullName.Text) || Common.EmptyFiels(txtEmail.Text)
                        || Common.EmptyFiels(txtPhoneNumber.Text) || Common.EmptyFiels(txtDescription.Text)
                        || pkCategory.SelectedIndex == -1 || selectedSubCategory == null
                        || Common.EmptyFiels(txtExperience.Text) || Common.EmptyFiels(txtSupplyArea.Text)
                        || Common.EmptyFiels(txtGstNumber.Text) || Common.EmptyFiels(txtPan.Text)
                        || Common.EmptyFiels(txtBankAccount.Text) || Common.EmptyFiels(txtBankName.Text)
                        || Common.EmptyFiels(txtIfsc.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtBuildingNumber.Text) || Common.EmptyFiels(txtStreet.Text)
                           || Common.EmptyFiels(txtCity.Text) || Common.EmptyFiels(txtState.Text)
                           || Common.EmptyFiels(pkNationality.Text) || Common.EmptyFiels(txtPinCode.Text)
                           || Common.EmptyFiels(txtLandmark.Text))
                {
                    return BillingAddressValidations();
                }

                if (Common.EmptyFiels(txtFullName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_FullName);
                }
                else if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PhoneNumber);
                }
                else if (!txtPhoneNumber.Text.IsValidPhone())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                }
                else if (Common.EmptyFiels(txtDescription.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Description);
                }
                else if (pkCategory.SelectedIndex == -1 && Common.EmptyFiels(txtOtherCategory.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Category);
                }
                else if (selectedSubCategory != null && selectedSubCategory.Count == 0 && pkSubCategory.SelectedIndex == -1)
                {
                    Common.DisplayErrorMessage(Constraints.Required_Subcategory);
                }
                else if (selectedSubCategory != null && selectedSubCategory.Count == 0)
                {
                    Common.DisplayErrorMessage(Constraints.Required_Subcategory);
                }
                else if (Common.EmptyFiels(txtExperience.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Experience);
                }
                else if (Common.EmptyFiels(txtSupplyArea.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_SupplyArea);
                }
                else if (Common.EmptyFiels(txtGstNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_GST);
                }
                else if (!Common.IsValidGSTPIN(txtGstNumber.Text.ToUpper()))
                {
                    Common.DisplayErrorMessage(Constraints.InValid_GST);
                }
                else if (Common.EmptyFiels(txtPan.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PAN);
                }
                else if (!Common.IsValidPAN(txtPan.Text.ToUpper()))
                {
                    Common.DisplayErrorMessage(Constraints.InValid_PAN);
                }
                else if (Common.EmptyFiels(txtBankAccount.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Bank_Account);
                }
                else if (Common.EmptyFiels(txtBankName.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Bank_Name);
                }
                else if (Common.EmptyFiels(txtIfsc.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_IFSC);
                }
                else if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    if (!Common.IsValidPhone(txtAltPhoneNumber.Text))
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_AltNumber);
                    }
                    else if (txtAltPhoneNumber.Text == txtPhoneNumber.Text)
                    {
                        Common.DisplayErrorMessage(Constraints.Same_Phone_AltPhone_Number);
                    }
                    else
                    {
                        isValid = true;
                    }
                }
                else
                {
                    isValid = true;
                }

                if (isValid)
                {
                    if (Common.IsValidGSTPIN(txtGstNumber.Text) && Common.IsValidPAN(txtPan.Text))
                    {
                        string panFromGSTIN = txtGstNumber.Text.Substring(2, 10);
                        if (panFromGSTIN.ToUpper() != txtPan.Text.ToUpper())
                        {
                            Common.DisplayErrorMessage(Constraints.InValid_PAN_GSTIN);
                            isValid = false;
                        }
                        else
                        {
                            isValid = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Validations: " + ex.Message);
            }
            return isValid;
        }

        private bool BillingAddressValidations()
        {
            bool isValid = false;

            try
            {
                RequiredBillingFields();
                if (Common.EmptyFiels(txtBuildingNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_BuildingNumber);
                }
                else if (Common.EmptyFiels(txtStreet.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Street);
                }
                else if (Common.EmptyFiels(txtCity.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_City);
                }
                else if (Common.EmptyFiels(txtState.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_State);
                }
                else if (Common.EmptyFiels(pkNationality.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Nationality);
                }
                else if (Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower()).Count() == 0)
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Nationality);
                }
                else if (Common.EmptyFiels(txtLandmark.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Landmark);
                }
                else if (Common.EmptyFiels(txtPinCode.Text))
                {
                    isValid = PinCodeValidation().Result;
                }
                else
                {
                    isValid = true;
                }

                if (isValid)
                {
                    BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxState.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BillingAddressValidations: " + ex.Message);
            }

            return isValid;
        }

        private void HasUpdateProfileDetail()
        {
            try
            {
                bool isUpdate = false;

                if (mSellerDetail == null)
                    isUpdate = true;

                if (mSellerDetail.FullName != txtFullName.Text)
                    isUpdate = true;
                else if (mSellerDetail.Email != txtEmail.Text)
                    isUpdate = true;
                else if (mSellerDetail.PhoneNumber != txtPhoneNumber.Text)
                    isUpdate = true;
                else if (mSellerDetail.AlternativePhoneNumber != txtAltPhoneNumber.Text)
                    isUpdate = true;
                else if (mSellerDetail.CompanyProfile.Description != txtDescription.Text)
                    isUpdate = true;
                else if (mSellerDetail.CompanyProfile.Experience != txtExperience.Text)
                    isUpdate = true;
                else if (mSellerDetail.CompanyProfile.AreaOfSupply != txtSupplyArea.Text)
                    isUpdate = true;
                else if (mSellerDetail.BankInformation.Gstin.ToUpper() != txtGstNumber.Text.ToUpper())
                    isUpdate = true;
                else if (mSellerDetail.BankInformation.Pan.ToUpper() != txtPan.Text.ToUpper())
                    isUpdate = true;
                else if (mSellerDetail.BankInformation.BankAccountNumber != txtBankAccount.Text)
                    isUpdate = true;
                else if (mSellerDetail.BankInformation.Branch != txtBankName.Text)
                    isUpdate = true;
                else if (mSellerDetail.BankInformation.Ifsc != txtIfsc.Text)
                    isUpdate = true;
                else if (mSellerDetail.Documents != documentList)
                    isUpdate = true;
                else if (mSellerDetail.CompanyProfile.Category != (string)pkCategory.SelectedItem)
                    isUpdate = true;
                else if (mSellerDetail.CompanyProfile.SubCategories != selectedSubCategory)
                    isUpdate = true;
                else if (mSellerDetail.Building != txtBuildingNumber.Text)
                    isUpdate = true;
                else if (mSellerDetail.Street != txtStreet.Text)
                    isUpdate = true;
                else if (mSellerDetail.City != txtCity.Text)
                    isUpdate = true;
                else if (mSellerDetail.State != txtState.Text)
                    isUpdate = true;
                else if (mSellerDetail.PinCode != txtPinCode.Text)
                    isUpdate = true;
                else if (mSellerDetail.Nationality != pkNationality.Text)
                    isUpdate = true;
                else if (mSellerDetail.Landmark != txtLandmark.Text)
                    isUpdate = true;
                else if (isDeleteSubCategory)
                    isUpdate = true;
                else if (isUpdatePhoto)
                    isUpdate = true;
                else
                    isUpdate = false;
                BtnUpdate.IsEnabled = isUpdate;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/HasUpdateProfileDetail: " + ex.Message);
            }
        }

        private void RequiredBillingFields()
        {
            try
            {
                if (Common.EmptyFiels(txtBuildingNumber.Text))
                {
                    BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtStreet.Text))
                {
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtCity.Text))
                {
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtState.Text))
                {
                    BoxState.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPinCode.Text))
                {
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(pkNationality.Text))
                {
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtLandmark.Text))
                {
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/RequiredBillingFields: " + ex.Message);
            }
        }

        private void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtDescription.Text))
                {
                    BoxDescription.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (pkCategory.SelectedIndex == -1)
                {
                    BoxCategory.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (selectedSubCategory.Count == 0 && pkSubCategory.SelectedIndex == -1)
                {
                    BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtExperience.Text))
                {
                    BoxExperience.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtSupplyArea.Text))
                {
                    BoxSupplyArea.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtGstNumber.Text))
                {
                    BoxGstNumber.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtPan.Text))
                {
                    BoxPan.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtBankAccount.Text))
                {
                    BoxBankAccount.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtBankName.Text))
                {
                    BoxBankName.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtIfsc.Text))
                {
                    BoxIfsc.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/RequiredFields: " + ex.Message);
            }
        }

        private void UnfocussedFields(Entry entry = null, ExtAutoSuggestBox autoSuggestBox = null, Editor editor = null, Picker picker = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "FullName")
                    {
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Experience")
                    {
                        BoxExperience.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "SupplyArea")
                    {
                        BoxSupplyArea.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "GstNumber")
                    {
                        BoxGstNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Pan")
                    {
                        BoxPan.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BankAccount")
                    {
                        BoxBankAccount.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BankName")
                    {
                        BoxBankName.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "IFSC")
                    {
                        BoxIfsc.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BABuildingNumber")
                    {
                        BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BAStreet")
                    {
                        BoxStreet.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BACity")
                    {
                        BoxCity.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BAState")
                    {
                        BoxState.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BAPincode")
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "BALandmark")
                    {
                        BoxLandmark.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (editor != null)
                {
                    if (editor.ClassId == "Description")
                    {
                        BoxDescription.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (picker != null)
                {
                    if (picker.ClassId == "Category")
                    {
                        BoxCategory.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (picker.ClassId == "SubCategory")
                    {
                        BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                if (autoSuggestBox != null)
                {
                    if (autoSuggestBox.ClassId == "BANationality")
                    {
                        BoxNationality.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }

                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UnfocussedFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            try
            {
                #region User Details            
                txtFullName.Text = txtFullName.Text.Trim();
                txtEmail.Text = txtEmail.Text.Trim();
                txtPhoneNumber.Text = txtPhoneNumber.Text.Trim();
                if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    txtAltPhoneNumber.Text = txtAltPhoneNumber.Text.Trim();
                }
                #endregion

                #region Billing Address
                if (!Common.EmptyFiels(txtBuildingNumber.Text))
                    txtBuildingNumber.Text = txtBuildingNumber.Text.Trim();
                if (!Common.EmptyFiels(txtStreet.Text))
                    txtStreet.Text = txtStreet.Text.Trim();
                if (!Common.EmptyFiels(txtCity.Text))
                    txtCity.Text = txtCity.Text.Trim();
                if (!Common.EmptyFiels(txtState.Text))
                    txtState.Text = txtState.Text.Trim();
                if (!Common.EmptyFiels(txtPinCode.Text))
                    txtPinCode.Text = txtPinCode.Text.Trim();
                if (!Common.EmptyFiels(txtLandmark.Text))
                    txtLandmark.Text = txtLandmark.Text.Trim();
                #endregion

                #region Company Profile
                txtDescription.Text = txtDescription.Text.Trim();
                txtExperience.Text = txtExperience.Text.Trim();
                txtSupplyArea.Text = txtSupplyArea.Text.Trim();
                #endregion

                #region Bank Information
                txtGstNumber.Text = txtGstNumber.Text.ToUpper().Trim();
                txtPan.Text = txtPan.Text.ToUpper().Trim();
                txtBankAccount.Text = txtBankAccount.Text.Trim();
                txtBankName.Text = txtBankName.Text.Trim();
                txtIfsc.Text = txtIfsc.Text.Trim();
                #endregion
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/FieldsTrim: " + ex.Message);
            }
        }

        private async Task<bool> PinCodeValidation()
        {
            bool isValid = false;
            try
            {
                if (!Common.EmptyFiels(txtPinCode.Text))
                {
                    txtPinCode.Text = txtPinCode.Text.Trim();
                    isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(txtPinCode.Text);
                    if (isValid)
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_PinCode);
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/PinCodeValidation: " + ex.Message);
            }
            return isValid;
        }
        #endregion

        #region [ Update Profile Process ]
        private Model.Request.SellerDetails FillProfileDetails()
        {
            try
            {
                #region Profile Image
                if (!Common.EmptyFiels(relativePath))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mSellerDetail.ProfilePhoto = relativePath.Replace(baseURL, "");
                }
                #endregion

                #region User Details            
                mSellerDetail.UserId = Settings.UserId;
                mSellerDetail.FullName = txtFullName.Text;
                mSellerDetail.Email = txtEmail.Text;
                mSellerDetail.PhoneNumber = txtPhoneNumber.Text;
                if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    mSellerDetail.AlternativePhoneNumber = txtAltPhoneNumber.Text;
                }
                #endregion

                #region Billing Address
                //AddBillingAddress();
                //mSellerDetail.BillingAddresses = mBillingAddresses;
                mSellerDetail.Building = txtBuildingNumber.Text;
                mSellerDetail.Street = txtStreet.Text;
                mSellerDetail.City = txtCity.Text;
                mSellerDetail.State = txtState.Text;
                mSellerDetail.PinCode = txtPinCode.Text;
                mSellerDetail.Landmark = txtLandmark.Text;
                mSellerDetail.Nationality = pkNationality.Text;
                #endregion

                #region Company Profile
                mSellerDetail.Description = txtDescription.Text;
                mSellerDetail.Experience = txtExperience.Text;
                mSellerDetail.AreaOfSupply = txtSupplyArea.Text;

                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mSellerDetail.Category = txtOtherCategory.Text;
                }
                else
                {
                    mSellerDetail.Category = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.Name;
                }

                if (selectedSubCategory != null)
                {
                    mSellerDetail.SubCategories = selectedSubCategory;
                }
                if (!Common.EmptyFiels(pkNationality.Text))
                {
                    mSellerDetail.CountryId = (Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId ?? 0);
                }
                #endregion

                #region Bank Information
                mSellerDetail.Gstin = txtGstNumber.Text.ToUpper();
                mSellerDetail.Pan = txtPan.Text.ToUpper();
                mSellerDetail.BankAccountNumber = txtBankAccount.Text;
                mSellerDetail.Branch = txtBankName.Text;
                mSellerDetail.Ifsc = txtIfsc.Text;
                #endregion

                #region Document List
                mSellerDetail.Documents = documentList;
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    ErrorMessage = ex.Message;
                }
                else
                {
                    ErrorMessage = Constraints.Something_Wrong;
                }
                return null;
            }

            return mSellerDetail;
        }

        private async void UpdateProfile()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    FieldsTrim();
                    var mSellerDetails = FillProfileDetails();

                    if (mSellerDetails != null)
                    {
                        var mResponse = await profileAPI.SaveProfile(mSellerDetails);
                        if (mResponse != null && mResponse.Succeeded)
                        {
                            isUpdateProfile = true;
                            await GetProfile();

                            isUpdatePhoto = false;
                            var updateId = mResponse.Data;
                            if (updateId != null)
                            {
                                SuccessfullUpdate(mResponse.Message);
                                BtnUpdate.IsEnabled = false;
                            }
                        }
                        else
                        {
                            if (mResponse == null)
                                return;

                            Common.DisplayErrorMessage(mResponse.Message);
                        }
                    }
                    else
                    {
                        Common.DisplayErrorMessage(ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UpdateProfile: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void SuccessfullUpdate(string MessageString)
        {
            try
            {
                var successPopup = new Popup.SuccessPopup(MessageString);
                await PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/SuccessfullUpdate: " + ex.Message);
            }
        }
        #endregion
        #endregion

        #region [ Events ]    
        #region [ Header Navigation ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgMenu_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("AccountView/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        #region [ Upload Photo | Documents ]
        private async void ImgCamera_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgCamera);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                ImageConvertion.SelectedImagePath = imgUser;
                //ImageConvertion.SelectedXFImagePath = imgUser;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfilePicture);
                await ImageConvertion.SelectImage();

                if (ImageConvertion.SelectedImageByte != null)
                {
                    relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfilePicture);
                    isUpdatePhoto = true;
                    HasUpdateProfileDetail();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgCamera_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void ImgUplodeDocument_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(imageButton: ImgUplodeDocument);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                await FileSelection.FilePickup();
                relativeDocumentPath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

                if (!Common.EmptyFiels(relativeDocumentPath))
                {
                    if (documentList == null)
                        documentList = new List<string>();
                    isUpdatePhoto = true;
                    documentList.Add(relativeDocumentPath);
                    AddDocuments();
                    HasUpdateProfileDetail();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgUplodeDocument_Clicked: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region [ AutoSuggestBox-Country ]
        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    if (isFirstLoad || i < 2)
                    {
                        isFirstLoad = false;
                        pkNationality.IsSuggestionListOpen = false;
                        i++;
                        return;
                    }
                }

                if (mCountriesData == null)
                    mCountriesData = new ObservableCollection<string>();

                mCountriesData.Clear();
                if (!string.IsNullOrEmpty(pkNationality.Text))
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Where(x => x.Name.ToLower().Contains(pkNationality.Text.ToLower())).Select(x => x.Name));
                }
                else
                {
                    mCountriesData = new ObservableCollection<string>(Common.mCountries.Select(x => x.Name));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AutoSuggestBox_TextChanged: " + ex.Message);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            try
            {
                if (e.ChosenSuggestion != null)
                {
                    pkNationality.Text = e.ChosenSuggestion.ToString();
                }
                else
                {
                    // User hit Enter from the search box. Use args.QueryText to determine what to do.
                    pkNationality.Unfocus();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AutoSuggestBox_QuerySubmitted: " + ex.Message);
            }
        }

        private void AutoSuggestBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }
        #endregion

        #region [ Category-SubCategory ]
        private async void pkCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkCategory.SelectedIndex > -1 && pkCategory.SelectedItem != null)
                {
                    var newCategoryId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).Select(x => x.CategoryId).FirstOrDefault();
                    if (newCategoryId != null)
                    {
                        await GetSubCategoryByCategoryId(newCategoryId);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Category_SelectedIndex: " + ex.Message);
            }
        }

        private void pkSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkSubCategory.SelectedIndex > -1 && pkSubCategory.SelectedItem != null)
                {
                    if (selectedSubCategory.Where(x => x == pkSubCategory.SelectedItem.ToString()).Count() == 0)
                    {
                        selectedSubCategory.Add(pkSubCategory.SelectedItem.ToString());
                    }

                    wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/pkSubCategory_SelectedIndexChanged: " + ex.Message);
            }
        }

        private void BtnDeleteSubCategory_Clicked(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                var subName = (string)btn.BindingContext;
                if (selectedSubCategory != null && selectedSubCategory.Count > 0)
                {
                    selectedSubCategory.Remove(subName);
                    isDeleteSubCategory = true;
                    HasUpdateProfileDetail();
                }

                wlSubCategory.ItemsSource = selectedSubCategory.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnDeleteSubCategory: " + ex.Message);
            }
        }

        private async void txtOtherCategory_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mCategories = await DependencyService.Get<IProfileRepository>().CreateCategory(txtOtherCategory.Text);
                    if (mCategories != null)
                    {
                        selectedSubCategory = new List<string>();
                        wlSubCategory.ItemsSource = selectedSubCategory.ToList();

                        var lastAddedCategory = mCategories.Select(x => x.Name).ToList();
                        if (lastAddedCategory.Any(x => x.ToLower().Trim() == txtOtherCategory.Text.ToLower().Trim()))
                        {
                            pkCategory.ItemsSource = lastAddedCategory.ToList();
                            pkCategory.SelectedItem = txtOtherCategory.Text;

                            txtOtherCategory.Text = string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/txtOtherCategory_Completed: " + ex.Message);
            }
        }

        private async void txtOtherSubCategory_Unfocused(object sender, FocusEventArgs e)
        {
            await CreateSubCategory();
        }
        #endregion

        #region [ Billing Address ]
        private void GrdBilling_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdBilling.IsVisible == false)
                {
                    GrdBilling.IsVisible = true;
                    ImgDropDownBilling.Source = Constraints.Arrow_Up;
                    ScrPrimary.ScrollToAsync(GrdBillingAddress, ScrollToPosition.Start, true);
                }
                else
                {
                    GrdBilling.IsVisible = false;
                    ImgDropDownBilling.Source = Constraints.Arrow_Down;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GrdBilling_Tapped: " + ex.Message);
            }
        }

        private void BillingAddressEntry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                UnfocussedFields(entry: entry);
            }
        }

        private async void BillingPincode_Unfocused(object sender, FocusEventArgs e)
        {
            await PinCodeValidation();
        }
        #endregion

        #region [ Unfocus ]
        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                UnfocussedFields(entry: entry);
            }
        }

        private void AutoSuggestBox_Unfocused(object sender, FocusEventArgs e)
        {
            var autoSuggestBox = (ExtAutoSuggestBox)sender;
            if (!Common.EmptyFiels(autoSuggestBox.Text))
            {
                UnfocussedFields(autoSuggestBox: autoSuggestBox);
            }
        }

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            var editor = (Editor)sender;
            if (!Common.EmptyFiels(editor.Text))
            {
                UnfocussedFields(editor: editor);
            }
        }

        private void Picker_Unfocused(object sender, FocusEventArgs e)
        {
            var picker = (Picker)sender;
            if (picker.SelectedIndex != -1)
            {
                UnfocussedFields(picker: picker);
            }
        }
        #endregion

        private void GrdCompanyProfile_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdCompanyProfile.IsVisible == false)
                {
                    GrdCompanyProfile.IsVisible = true;
                    ImgDownDownCompanyProfile.Source = Constraints.Arrow_Up;
                    ScrPrimary.ScrollToAsync(GrdCompnyProfile, ScrollToPosition.Start, true);
                }
                else
                {
                    GrdCompanyProfile.IsVisible = false;
                    ImgDownDownCompanyProfile.Source = Constraints.Arrow_Down;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GrdCompanyProfile_Tapped: " + ex.Message);
            }
        }

        private void GrdBankInfo_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (GrdBankInfo.IsVisible == false)
                {
                    GrdBankInfo.IsVisible = true;
                    ImgDropDownBankInfo.Source = Constraints.Arrow_Up;
                    ScrPrimary.ScrollToAsync(GrdBankInformation, ScrollToPosition.Start, true);
                }
                else
                {
                    GrdBankInfo.IsVisible = false;
                    ImgDropDownBankInfo.Source = Constraints.Arrow_Down;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GrdBankInfo_Tapped: " + ex.Message);
            }
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            await Common.BindAnimation(button: BtnUpdate);
            UpdateProfile();
        }

        private async void Logout_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnLogout);
                await DependencyService.Get<IAuthenticationRepository>().DoLogout();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/Logout_Tapped: " + ex.Message);
            }
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            BindObjects();
            rfView.IsRefreshing = false;
        }

        private async void BtnDeactivateAccount_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnDeactivateAccount);
                await Navigation.PushAsync(new OtherPage.DeactivateAccountPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnDeactivateAccount: " + ex.Message);
            }
        }

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                string message = Constraints.CopiedSellerId;
                Common.CopyText(lblSellerId, message);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/CopyString_Tapped: " + ex.Message);
            }
        }

        private async void ImgDocument_Clicked(object sender, EventArgs e)
        {
            var imgButton = (ImageButton)sender;
            try
            {
                var url = imgButton.BindingContext as string;
                await GenerateWebView.GenerateView(url);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgDocument_Clicked: " + ex.Message);
            }
        }
        #endregion
    }
}