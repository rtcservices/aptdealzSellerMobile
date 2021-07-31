using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
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
        private ProfileAPI profileAPI;
        private SellerDetails mSellerDetail;
        private BillingAddress mBillingAddress;

        private List<Category> mCategories;
        private List<SubCategory> mSubCategories;
        private List<BillingAddress> mBillingAddresses;
        private List<string> selectedSubCategory;
        private List<string> documentList;

        private string newCategory;
        private string relativePath = string.Empty;
        private string relativeDocumentPath = string.Empty;
        private string ErrorMessage = string.Empty;
        private bool isFirstLoad = true;
        private bool isUpdatPhoto = false;
        private bool isDeleteSubCategory = true;
        private bool isAddBillingAddress = false;
        private bool isEditBillingAddress = false;
        private bool isDeleteBillingAddress = false;
        #endregion

        #region Constructor
        public AccountView()
        {
            InitializeComponent();
            BtnUpdate.IsEnabled = false;
            BindObjects();

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
        private async void BindObjects()
        {
            profileAPI = new ProfileAPI();
            mCategories = new List<Category>();
            mSubCategories = new List<SubCategory>();
            mBillingAddresses = new List<BillingAddress>();
            selectedSubCategory = new List<string>();
            documentList = new List<string>();

            await Task.Run(() =>
             {
                 UserDialogs.Instance.ShowLoading(Constraints.Loading);
             });
            await GetCategory();
            await GetCountries();
            await GetProfile();
            UserDialogs.Instance.HideLoading();
        }

        #region [ Get / Bind Data ]
        private async Task GetCategory()
        {
            try
            {
                mCategories = await DependencyService.Get<ICategoryRepository>().GetCategory();
                if (mCategories != null || mCategories.Count > 0)
                {
                    pkCategory.ItemsSource = mCategories.Select(x => x.Name).ToList();
                }

                if (pkCategory.SelectedItem != null)
                {
                    var categoryId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.CategoryId;
                    GetSubCategoryByCategoryId(categoryId);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindCategoriesList: " + ex.Message);
            }
        }

        private async void GetSubCategoryByCategoryId(string categoryId)
        {
            try
            {
                mSubCategories = await DependencyService.Get<ICategoryRepository>().GetSubCategory(categoryId);
                pkSubCategory.ItemsSource = mSubCategories.Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetSubCategoryByCategoryId: " + ex.Message);
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
                if (Common.mSellerDetails == null || Common.EmptyFiels(Common.mSellerDetails.SellerId))
                {
                    var mResponse = await profileAPI.GetMyProfileData();
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            mSellerDetail = jObject.ToObject<Model.Request.SellerDetails>();
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
                    mSellerDetail = Common.mSellerDetails;
                }

                BindProfileDetails(mSellerDetail);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetProfile: " + ex.Message);
            }
        }

        private void BindProfileDetails(SellerDetails mSellerDetail)
        {
            try
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
                if (mSellerDetail.BillingAddresses != null && mSellerDetail.BillingAddresses.Count > 0)
                {
                    mBillingAddresses = mSellerDetail.BillingAddresses;
                    if (mBillingAddresses.Count == 0)
                    {
                        lstBilling.IsVisible = false;
                        lstBilling.HeightRequest = 0;
                    }
                    else
                    {
                        lstBilling.IsVisible = true;
                        lstBilling.ItemsSource = mBillingAddresses.ToList();
                        lstBilling.HeightRequest = mBillingAddresses.Count * 120;
                    }

                    var efBAddress = mBillingAddresses.FirstOrDefault();
                    if (efBAddress == null)
                    {
                        txtBuildingNumber.Text = mSellerDetail.Building;
                        txtStreet.Text = mSellerDetail.Street;
                        txtCity.Text = mSellerDetail.City;
                        txtState.Text = mSellerDetail.State;
                        txtPinCode.Text = mSellerDetail.PinCode;
                        txtLandmark.Text = mSellerDetail.Landmark;
                        pkNationality.Text = mSellerDetail.Nationality;
                    }
                }
                else
                {
                    txtBuildingNumber.Text = mSellerDetail.Building;
                    txtStreet.Text = mSellerDetail.Street;
                    txtCity.Text = mSellerDetail.City;
                    txtState.Text = mSellerDetail.State;
                    txtPinCode.Text = mSellerDetail.PinCode;
                    txtLandmark.Text = mSellerDetail.Landmark;
                    pkNationality.Text = mSellerDetail.Nationality;
                }
                #endregion

                #region Company Profile
                if (mSellerDetail.CompanyProfile.Category != null)
                {
                    var category = mCategories.Where(x => x.Name == mSellerDetail.CompanyProfile.Category).FirstOrDefault()?.Name;

                    if (!Common.EmptyFiels(category))
                    {
                        pkCategory.SelectedItem = category;
                    }
                    else
                    {
                        txtOtherCategory.Text = category;
                    }
                }

                if (selectedSubCategory != null)
                {
                    selectedSubCategory = mSellerDetail.CompanyProfile.SubCategories;
                    wlSubCategory.ItemsSource = selectedSubCategory.ToList();
                }

                txtDescription.Text = mSellerDetail.CompanyProfile.Description;
                txtExperience.Text = mSellerDetail.CompanyProfile.Experience;
                txtSupplyArea.Text = mSellerDetail.CompanyProfile.AreaOfSupply;
                #endregion

                #region Bank Information
                txtGstNumber.Text = mSellerDetail.BankInformation.Gstin;
                txtPan.Text = mSellerDetail.BankInformation.Pan;
                txtBankAccount.Text = mSellerDetail.BankInformation.BankAccountNumber;
                txtBankName.Text = mSellerDetail.BankInformation.Branch;
                txtIfsc.Text = mSellerDetail.BankInformation.Ifsc;
                #endregion

                #region Document List
                documentList = mSellerDetail.Documents;
                AddDocuments();
                #endregion
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BindProfileDetails: " + ex.Message);
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
                Common.DisplayErrorMessage("AccountView/BindDocumentList: " + ex.Message);
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
                    Common.DisplayErrorMessage(Constraints.Required_All);
                    return false;
                }

                if (mBillingAddresses.Count == 0)
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
                else if (!Common.IsValidGSTPIN(txtGstNumber.Text))
                {
                    Common.DisplayErrorMessage(Constraints.InValid_GST);
                }
                else if (Common.EmptyFiels(txtPan.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_PAN);
                }
                else if (!Common.IsValidPAN(txtPan.Text))
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
                        if (panFromGSTIN != txtPan.Text)
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
                if (mBillingAddresses.Count == 0)
                {
                    if (Common.EmptyFiels(txtBuildingNumber.Text) || Common.EmptyFiels(txtStreet.Text)
                           || Common.EmptyFiels(txtCity.Text) || Common.EmptyFiels(txtState.Text)
                           || Common.EmptyFiels(pkNationality.Text) || Common.EmptyFiels(txtPinCode.Text)
                           || Common.EmptyFiels(txtLandmark.Text))
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
                    BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxState.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["LightGray"];
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
                else if (mSellerDetail.BankInformation.Gstin != txtGstNumber.Text)
                    isUpdate = true;
                else if (mSellerDetail.BankInformation.Pan != txtPan.Text)
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
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.BuildingNumber != txtBuildingNumber.Text)
                    isUpdate = true;
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.Street != txtStreet.Text)
                    isUpdate = true;
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.City != txtCity.Text)
                    isUpdate = true;
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.State != txtState.Text)
                    isUpdate = true;
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.PinCode != txtPinCode.Text)
                    isUpdate = true;
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.Nationality != pkNationality.Text)
                    isUpdate = true;
                else if (isEditBillingAddress && mBillingAddress != null && mBillingAddress.Landmark != txtLandmark.Text)
                    isUpdate = true;
                else if (isAddBillingAddress)
                    isUpdate = true;
                else if (isDeleteBillingAddress)
                    isUpdate = true;
                else if (isDeleteSubCategory)
                    isUpdate = true;
                else if (isUpdatPhoto)
                    isUpdate = true;
                else
                    isUpdate = false;
                //else if (isUploadDoc)
                //    isUpdate = true;

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
                    BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtStreet.Text))
                {
                    BoxStreet.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtCity.Text))
                {
                    BoxCity.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtState.Text))
                {
                    BoxState.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPinCode.Text))
                {
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(pkNationality.Text))
                {
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtLandmark.Text))
                {
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["LightRed"];
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
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtDescription.Text))
                {
                    BoxDescription.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (pkCategory.SelectedIndex == -1)
                {
                    BoxCategory.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (selectedSubCategory.Count == 0 && pkSubCategory.SelectedIndex == -1)
                {
                    BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtExperience.Text))
                {
                    BoxExperience.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtSupplyArea.Text))
                {
                    BoxSupplyArea.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtGstNumber.Text))
                {
                    BoxGstNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPan.Text))
                {
                    BoxPan.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtBankAccount.Text))
                {
                    BoxBankAccount.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtBankName.Text))
                {
                    BoxBankName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtIfsc.Text))
                {
                    BoxIfsc.BackgroundColor = (Color)App.Current.Resources["LightRed"];
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
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Experience")
                    {
                        BoxExperience.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "SupplyArea")
                    {
                        BoxSupplyArea.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "GstNumber")
                    {
                        BoxGstNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Pan")
                    {
                        BoxPan.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BankAccount")
                    {
                        BoxBankAccount.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BankName")
                    {
                        BoxBankName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "IFSC")
                    {
                        BoxIfsc.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BABuildingNumber")
                    {
                        BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BAStreet")
                    {
                        BoxStreet.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BACity")
                    {
                        BoxCity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BAState")
                    {
                        BoxState.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BAPincode")
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BALandmark")
                    {
                        BoxLandmark.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                if (editor != null)
                {
                    if (editor.ClassId == "Description")
                    {
                        BoxDescription.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                if (picker != null)
                {
                    if (picker.ClassId == "Category")
                    {
                        BoxCategory.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (picker.ClassId == "SubCategory")
                    {
                        BoxSubCategory.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                }

                if (autoSuggestBox != null)
                {
                    if (autoSuggestBox.ClassId == "BANationality")
                    {
                        BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightGray"];
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
                txtGstNumber.Text = txtGstNumber.Text.Trim();
                txtPan.Text = txtPan.Text.Trim();
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
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else
                    {
                        BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_PinCode);
                    BoxPincode.BackgroundColor = (Color)App.Current.Resources["LightRed"];
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
                #region Profile
                if (!Common.EmptyFiels(relativePath))
                {
                    string baseURL = (string)App.Current.Resources["BaseURL"];
                    mSellerDetail.ProfilePhoto = relativePath.Replace(baseURL, "");
                }
                #endregion

                #region User Details            
                mSellerDetail.FullName = txtFullName.Text;
                mSellerDetail.Email = txtEmail.Text;
                mSellerDetail.PhoneNumber = txtPhoneNumber.Text;
                if (!Common.EmptyFiels(txtAltPhoneNumber.Text))
                {
                    mSellerDetail.AlternativePhoneNumber = txtAltPhoneNumber.Text;
                }
                #endregion

                #region Billing Address
                AddBillingAddress();
                mSellerDetail.BillingAddresses = mBillingAddresses;
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
                #endregion

                #region Bank Information
                mSellerDetail.Gstin = txtGstNumber.Text;
                mSellerDetail.Pan = txtPan.Text;
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

        private void AddBillingAddress()
        {
            try
            {
                if (BillingAddressValidations())
                {
                    int id = (mBillingAddresses.Count == 0) ? 0 : mBillingAddresses.Max(x => x.Id);
                    if (id == 0)
                        id = 1;
                    else
                        id++;

                    bool isAdd = false;

                    if (mBillingAddress == null)
                    {
                        isAdd = true;
                        mBillingAddress = new BillingAddress();
                        mBillingAddress.Id = id;
                    }

                    if (!Common.EmptyFiels(txtBuildingNumber.Text) && !Common.EmptyFiels(txtStreet.Text)
                     && !Common.EmptyFiels(txtCity.Text) && !Common.EmptyFiels(txtState.Text)
                     && !Common.EmptyFiels(txtPinCode.Text) && !Common.EmptyFiels(txtLandmark.Text)
                     && !Common.EmptyFiels(pkNationality.Text))
                    {
                        mBillingAddress.BuildingNumber = txtBuildingNumber.Text;
                        mBillingAddress.Street = txtStreet.Text;
                        mBillingAddress.City = txtCity.Text;
                        mBillingAddress.State = txtState.Text;
                        mBillingAddress.PinCode = txtPinCode.Text;
                        mBillingAddress.Landmark = txtLandmark.Text;
                        mBillingAddress.Nationality = pkNationality.Text;
                        mBillingAddress.CountryId = (int)(Common.mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);

                        if (isAdd)
                        {
                            mBillingAddresses.Add(mBillingAddress);
                        }

                        isAddBillingAddress = true;
                        ClearBillingAddress();
                    }
                }

                mBillingAddress = null;

                if (mBillingAddresses.Count == 0)
                {
                    lstBilling.IsVisible = false;
                    lstBilling.HeightRequest = 0;
                }
                else
                {
                    lstBilling.IsVisible = true;
                    lstBilling.ItemsSource = mBillingAddresses.ToList();
                    lstBilling.HeightRequest = mBillingAddresses.Count * 120;
                }

                HasUpdateProfileDetail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/AddBillingAddress: " + ex.Message);
            }
        }

        private async void UpdateProfile()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    //if (!Common.EmptyFiels(txtPinCode.Text))
                    //{
                    //    if (!await PinCodeValidation())
                    //        return;
                    //}

                    FieldsTrim();
                    var mSellerDetails = FillProfileDetails();

                    if (mSellerDetails != null)
                    {
                        var mResponse = await profileAPI.SaveProfile(mSellerDetails);
                        if (mResponse != null && mResponse.Succeeded)
                        {
                            GetProfile();
                            isAddBillingAddress = false;
                            isDeleteBillingAddress = false;
                            isEditBillingAddress = false;
                            isDeleteSubCategory = false;
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

        private void SuccessfullUpdate(string MessageString)
        {
            try
            {
                var successPopup = new Popup.SuccessPopup(MessageString);
                PopupNavigation.Instance.PushAsync(successPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/SuccessfullUpdate: " + ex.Message);
            }
        }
        #endregion

        private void ClearBillingAddress()
        {
            txtBuildingNumber.Text = string.Empty;
            txtStreet.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtPinCode.Text = string.Empty;
            txtLandmark.Text = string.Empty;
            pkNationality.Text = string.Empty;
            mBillingAddress = null;
        }

        private async void DoLogout()
        {
            try
            {
                var isClose = await App.Current.MainPage.DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
                if (isClose)
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await authenticationAPI.Logout(Settings.RefreshToken, Settings.LoginTrackingKey);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                    }
                    else
                    {
                        if (mResponse != null && !mResponse.Message.Contains("TrackingKey"))
                            Common.DisplayErrorMessage(mResponse.Message);
                    }

                    Settings.EmailAddress = string.Empty;
                    Settings.UserToken = string.Empty;
                    Settings.RefreshToken = string.Empty;
                    Settings.LoginTrackingKey = string.Empty;

                    App.Current.MainPage = new NavigationPage(new Views.Accounts.LoginPage());
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/DoLogout: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Events    
        #region [ Header Navigation ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Dashboard.NotificationPage());
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
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
                Common.BindAnimation(image: ImgCamera);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                ImageConvertion.SelectedImagePath = imgUser;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfilePicture);
                await ImageConvertion.SelectImage();

                if (ImageConvertion.SelectedImageByte != null)
                {
                    relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfilePicture);
                    isUpdatPhoto = true;
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
                Common.BindAnimation(imageButton: ImgUplodeDocument);
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                await FileSelection.FilePickup();
                relativeDocumentPath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

                if (!Common.EmptyFiels(relativeDocumentPath))
                {
                    if (documentList == null)
                        documentList = new List<string>();

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
        private void pkCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkCategory.SelectedIndex > -1 && pkCategory.SelectedItem != null)
                {
                    newCategory = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).Select(x => x.CategoryId).FirstOrDefault();
                    if (newCategory != null)
                    {
                        GetSubCategoryByCategoryId(newCategory);
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

        private async void txtOtherCategory_Completed(object sender, EventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mCategories = await DependencyService.Get<ICategoryRepository>().CreateCategory(txtOtherCategory.Text);
                    if (mCategories != null)
                    {
                        var lastAddedCategory = mCategories.Select(x => x.Name).ToList();
                        if (lastAddedCategory.Any(x => x.ToLower().Trim() == txtOtherCategory.Text.ToLower().Trim()))
                        {
                            newCategory = mCategories.Where(x => x.Name == txtOtherCategory.Text).FirstOrDefault()?.CategoryId;
                            pkCategory.ItemsSource = lastAddedCategory.ToList();
                            pkCategory.SelectedItem = txtOtherCategory.Text;
                            txtOtherCategory.Text = string.Empty;
                            GetSubCategoryByCategoryId(newCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/txtOtherCategory_Completed: " + ex.Message);
            }
        }

        private async void txtOtherSubCategory_Completed(object sender, EventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(txtOtherSubCategory.Text))
                {
                    mSubCategories = await DependencyService.Get<ICategoryRepository>().CreateSubCategory(txtOtherSubCategory.Text, newCategory);
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
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/txtOtherSubCategory_Completed: " + ex.Message);
            }
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

        private void BtnAddMore_Clicked(object sender, EventArgs e)
        {
            lstBilling.IsVisible = true;
            AddBillingAddress();
        }

        private void BtnEdit_Clicked(object sender, EventArgs e)
        {
            try
            {
                var btn = (ImageButton)sender;
                mBillingAddress = (BillingAddress)btn.BindingContext;
                if (mBillingAddress != null)
                {
                    txtBuildingNumber.Text = mBillingAddress.BuildingNumber;
                    txtStreet.Text = mBillingAddress.Street;
                    txtCity.Text = mBillingAddress.City;
                    txtState.Text = (string)mBillingAddress.State;
                    txtPinCode.Text = (string)mBillingAddress.PinCode;
                    txtLandmark.Text = mBillingAddress.Landmark;
                    pkNationality.Text = (string)mBillingAddress.Nationality;
                    isEditBillingAddress = true;
                }

                if (mBillingAddresses.Count == 0)
                {
                    lstBilling.IsVisible = false;
                    lstBilling.HeightRequest = 0;
                }
                else
                {
                    lstBilling.IsVisible = true;
                    lstBilling.ItemsSource = mBillingAddresses.ToList();
                    lstBilling.HeightRequest = mBillingAddresses.Count * 120;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnEditAddress: " + ex.Message);
            }
        }

        private async void BtnDeleteBillding_Clicked(object sender, EventArgs e)
        {
            try
            {
                var isDelete = await App.Current.MainPage.DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantDelete, Constraints.Yes, Constraints.No);
                if (isDelete)
                {
                    isDeleteBillingAddress = true;
                    var btn = (Button)sender;
                    var mBillingAddress = (BillingAddress)btn.BindingContext;
                    if (mBillingAddress != null && mBillingAddresses != null && mBillingAddresses.Count > 0)
                    {
                        mBillingAddresses.Remove(mBillingAddress);
                    }

                    if (mBillingAddresses.Count == 0)
                    {
                        lstBilling.IsVisible = false;
                        lstBilling.HeightRequest = 0;
                    }
                    else
                    {
                        lstBilling.IsVisible = true;
                        lstBilling.ItemsSource = mBillingAddresses.ToList();
                        lstBilling.HeightRequest = mBillingAddresses.Count * 120;
                    }
                    HasUpdateProfileDetail();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnDeleteBillding: " + ex.Message);
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

        private void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnUpdate);
            UpdateProfile();
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            DoLogout();
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            rfView.IsRefreshing = true;
            BindObjects();
            rfView.IsRefreshing = false;
        }

        private void BtnDeactivateAccount_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnDeactivateAccount);
            Navigation.PushAsync(new OtherPage.DeactivateAccountPage());
        }
        #endregion

        private void CopyString_Tapped(object sender, EventArgs e)
        {
            try
            {
                string message = Constraints.CopiedSellerId;
                Common.CopyText(lblSellerId, message);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("OrderDetailsPage/StkOrderId_Tapped: " + ex.Message);
            }
        }
    }
}