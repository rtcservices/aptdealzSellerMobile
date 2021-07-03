using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using dotMorten.Xamarin.Forms;
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
        ProfileAPI profileAPI;
        private List<Country> mCountries;
        private List<Category> mCategories;
        private List<SubCategory> mSubCategories;
        private List<BillingAddress> mBillingAddresses;
        private List<string> selectedSubCategory;
        private string relativePath = string.Empty;
        private List<string> documentList;
        private SellerDetails mSellerDetail;
        private BillingAddress mBillingAddress;
        private bool isFirstLoad = true;
        #endregion

        #region Constructor
        public AccountView()
        {
            InitializeComponent();
            BindObjects();
        }
        #endregion

        #region Methods
        async void BindObjects()
        {
            profileAPI = new ProfileAPI();
            mCountries = new List<Country>();
            mCategories = new List<Category>();
            mSubCategories = new List<SubCategory>();
            mBillingAddresses = new List<BillingAddress>();
            selectedSubCategory = new List<string>();
            documentList = new List<string>();

            UserDialogs.Instance.ShowLoading(Constraints.Loading);
            await GetCategory();
            await GetCountries();
            await GetProfile();
            UserDialogs.Instance.HideLoading();
        }

        public bool Validations()
        {
            bool isValid = false;
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
                isValid = false;
            }

            if (Common.EmptyFiels(txtBuildingNumber.Text) || Common.EmptyFiels(txtStreet.Text)
                || Common.EmptyFiels(txtCity.Text) || Common.EmptyFiels(txtLandmark.Text)
                || Common.EmptyFiels(pkNationality.Text))
            {
                if (mBillingAddresses.Count == 0)
                {
                    Common.DisplayErrorMessage(Constraints.Required_BillingAddress);
                    RequiredBillingFields();
                    isValid = false;
                }
            }

            if (Common.EmptyFiels(txtFullName.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_FullName);
            }
            else if (Common.EmptyFiels(txtEmail.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Email);
            }
            else if (!txtEmail.Text.IsValidEmail())
            {
                Common.DisplayErrorMessage(Constraints.InValid_Email);
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
            else if (pkCategory.SelectedIndex == -1)
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
            else if (Common.EmptyFiels(txtPan.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_PAN);
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
                else
                {
                    isValid = true;
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        void AddDocuments()
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

        async Task GetCategory()
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

        async void GetSubCategoryByCategoryId(string categoryId)
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

        async Task GetCountries()
        {
            try
            {
                mCountries = await DependencyService.Get<IProfileRepository>().GetCountry();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/GetCountries: " + ex.Message);
            }
        }

        async Task GetProfile()
        {
            try
            {
                ProfileAPI profileAPI = new ProfileAPI();
                var mResponse = await profileAPI.GetMyProfileData();
                if (mResponse != null && mResponse.Succeeded)
                {
                    var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                    if (jObject != null)
                    {
                        mSellerDetail = jObject.ToObject<SellerDetails>();
                        if (mSellerDetail != null)
                        {
                            BindProfileDetails();
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
                Common.DisplayErrorMessage("AccountView/GetProfile: " + ex.Message);
            }
        }

        void BindProfileDetails()
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
                Settings.UserId = mSellerDetail.SellerId;
                txtFullName.Text = mSellerDetail.FullName;
                txtEmail.Text = mSellerDetail.Email;
                txtEmail.TextColor = Color.Gray;
                txtEmail.IsReadOnly = true;
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
                        txtLandmark.Text = mSellerDetail.Landmark;
                        pkNationality.Text = mSellerDetail.Nationality;
                    }
                }
                else
                {
                    txtBuildingNumber.Text = mSellerDetail.Building;
                    txtStreet.Text = mSellerDetail.Street;
                    txtCity.Text = mSellerDetail.City;
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

                txtDescription.Text = (string)mSellerDetail.CompanyProfile.Description;
                txtExperience.Text = (string)mSellerDetail.CompanyProfile.Experience;
                txtSupplyArea.Text = (string)mSellerDetail.CompanyProfile.AreaOfSupply;
                #endregion

                #region Bank Information
                txtGstNumber.Text = (string)mSellerDetail.BankInformation.Gstin;
                txtPan.Text = (string)mSellerDetail.BankInformation.Pan;
                txtBankAccount.Text = (string)mSellerDetail.BankInformation.BankAccountNumber;
                txtBankName.Text = (string)mSellerDetail.BankInformation.Branch;
                txtIfsc.Text = (string)mSellerDetail.BankInformation.Ifsc;
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

        Model.Request.SellerDetails FillProfileDetails()
        {
            #region Profile
            if (!Common.EmptyFiels(relativePath))
            {
                string baseURL = (string)App.Current.Resources["BaseURL"];
                mSellerDetail.ProfilePhoto = relativePath.Replace(baseURL, "");
                //mSellerDetail.ProfilePhoto = relativePath;
            }
            #endregion

            #region User Details
            mSellerDetail.UserId = Settings.UserId;
            mSellerDetail.SellerId = Settings.UserId;
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

            return mSellerDetail;
        }

        void FieldsTrim()
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

        async void UpdateProfile()
        {
            try
            {
                if (Validations())
                {
                    FieldsTrim();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mBuyerDetails = FillProfileDetails();

                    var mResponse = await profileAPI.SaveProfile(mBuyerDetails);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var updateId = mResponse.Data;
                        if (updateId != null)
                        {
                            Common.DisplaySuccessMessage(mResponse.Message);
                            //GetProfile();
                        }
                    }
                    else
                    {
                        if (mResponse == null)
                            return;

                        Common.DisplayErrorMessage(mResponse.Message);
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

        void AddBillingAddress()
        {
            if (!Common.EmptyFiels(pkNationality.Text))
            {
                if (!Common.EmptyFiels(pkNationality.Text))
                {
                    if (mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower()).Count() == 0)
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Nationality);
                        return;
                    }
                }
            }

            if (!Common.EmptyFiels(txtBuildingNumber.Text) || !Common.EmptyFiels(txtStreet.Text)
                || !Common.EmptyFiels(txtCity.Text) || !Common.EmptyFiels(pkNationality.Text)
                || !Common.EmptyFiels(txtLandmark.Text))
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

                mBillingAddress.BuildingNumber = txtBuildingNumber.Text;
                mBillingAddress.Street = txtStreet.Text;
                mBillingAddress.City = txtCity.Text;
                mBillingAddress.Landmark = txtLandmark.Text;
                mBillingAddress.Nationality = pkNationality.Text;

                if (isAdd)
                    mBillingAddresses.Add(mBillingAddress);

                ClearBillingAddress();
            }

            if (mBillingAddresses.Count == 0)
                lstBilling.IsVisible = false;
            else
            {
                lstBilling.IsVisible = true;
                lstBilling.ItemsSource = mBillingAddresses.ToList();
                lstBilling.HeightRequest = mBillingAddresses.Count * 120;
            }
        }

        void ClearBillingAddress()
        {
            txtBuildingNumber.Text = string.Empty;
            txtStreet.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtLandmark.Text = string.Empty;
            pkNationality.Text = string.Empty;
            mBillingAddress = null;
        }

        void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtEmail.Text))
                {
                    BoxEmail.BackgroundColor = (Color)App.Current.Resources["LightRed"];
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

                if (selectedSubCategory == null && selectedSubCategory.Count == 0 && pkSubCategory.SelectedIndex == -1)
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

        void RequiredBillingFields()
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

            if (Common.EmptyFiels(pkNationality.Text))
            {
                BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }

            if (Common.EmptyFiels(txtLandmark.Text))
            {
                BoxLandmark.BackgroundColor = (Color)App.Current.Resources["LightRed"];
            }



        }

        void UnfocussedFields(Entry entry = null, ExtAutoSuggestBox autoSuggestBox = null, Editor editor = null, Picker picker = null)
        {
            try
            {
                if (entry != null)
                {
                    if (entry.ClassId == "FullName")
                    {
                        BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Email")
                    {
                        BoxEmail.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "PhoneNumber")
                    {
                        BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "BuildingNumber")
                    {
                        BoxBuildingNumber.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Street")
                    {
                        BoxStreet.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "City")
                    {
                        BoxCity.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                    }
                    else if (entry.ClassId == "Landmark")
                    {
                        BoxLandmark.BackgroundColor = (Color)App.Current.Resources["LightGray"];
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
                }

                if (autoSuggestBox != null)
                {
                    if (autoSuggestBox.ClassId == "Nationality")
                    {
                        BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightGray"];
                        //if (!Common.EmptyFiels(pkNationality.Text))
                        //{
                        //    if (mCountries.Where(x => x.Name == pkNationality.Text).Count() == 0)
                        //    {
                        //        Common.DisplayErrorMessage(Constraints.InValid_Nationality);
                        //    }
                        //}
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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/UnfocussedFields: " + ex.Message);
            }
        }
        #endregion

        #region Events
        private void ImgMenu_Clicked(object sender, EventArgs e)
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
            Common.BindAnimation(imageButton: ImgBack);
            App.Current.MainPage = new MasterData.MasterDataPage();
        }

        private async void ImgCamera_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(image: ImgCamera);
                ImageConvertion.SelectedImagePath = imgUser;
                ImageConvertion.SetNullSource((int)FileUploadCategory.ProfilePicture);
                await ImageConvertion.SelectImage();

                if (ImageConvertion.SelectedImageByte != null)
                {
                    relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfilePicture);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/ImgCamera_Tapped: " + ex.Message);
            }
        }

        private void ImgEdit_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnUpdate);
            UpdateProfile();
        }

        private void GrdBilling_Tapped(object sender, EventArgs e)
        {
            if (GrdBilling.IsVisible == false)
            {
                GrdBilling.IsVisible = true;
                ImgDropDownBilling.Rotation = 180;
            }
            else
            {
                GrdBilling.IsVisible = false;
                ImgDropDownBilling.Rotation = 0;
            }
        }

        private void GrdCompanyProfile_Tapped(object sender, EventArgs e)
        {
            if (GrdCompanyProfile.IsVisible == false)
            {
                GrdCompanyProfile.IsVisible = true;
                ImgDownDownCompanyProfile.Rotation = 180;
            }
            else
            {
                GrdCompanyProfile.IsVisible = false;
                ImgDownDownCompanyProfile.Rotation = 0;
            }
        }

        private void GrdBankInfo_Tapped(object sender, EventArgs e)
        {
            if (GrdBankInfo.IsVisible == false)
            {
                GrdBankInfo.IsVisible = true;
                ImgDropDownBankInfo.Rotation = 180;
            }
            else
            {
                GrdBankInfo.IsVisible = false;
                ImgDropDownBankInfo.Rotation = 0;
            }
        }

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
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
                mCountriesData = new ObservableCollection<string>(mCountries.Where(x => x.Name.ToLower().Contains(pkNationality.Text.ToLower())).Select(x => x.Name));
            }
            else
            {
                mCountriesData = new ObservableCollection<string>(mCountries.Select(x => x.Name));
            }
        }

        private void AutoSuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
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

        private void AutoSuggestBox_SuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }

        private async void ImgUplodeDocument_Clicked(object sender, EventArgs e)
        {
            await FileSelection.FilePickup();
            relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);

            if (!Common.EmptyFiels(relativePath))
            {
                if (documentList == null)
                    documentList = new List<string>();

                documentList.Add(relativePath);
                AddDocuments();
            }
        }

        private void pkCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkCategory.SelectedIndex > -1 && pkCategory.SelectedItem != null)
                {
                    var catId = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).Select(x => x.CategoryId).FirstOrDefault();
                    if (catId != null)
                    {
                        GetSubCategoryByCategoryId(catId);
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
                }

                wlSubCategory.ItemsSource = selectedSubCategory.ToList();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnDeleteSubCategory: " + ex.Message);
            }
        }

        private async void Logout_Tapped(object sender, EventArgs e)
        {
            try
            {
                var isClose = await App.Current.MainPage.DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
                if (isClose)
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
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

                    App.Current.MainPage = new NavigationPage(new SplashScreen.WelcomePage(true));
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/DoLogout: " + ex.Message);
            }
        }

        private async void BtnDeleteBillding_Clicked(object sender, EventArgs e)
        {
            try
            {
                var isDelete = await App.Current.MainPage.DisplayAlert(Constraints.Alert, Constraints.AreYouSureWantDelete, Constraints.Yes, Constraints.No);
                if (isDelete)
                {
                    var btn = (Button)sender;
                    var mBillingAddress = (BillingAddress)btn.BindingContext;
                    if (mBillingAddress != null && mBillingAddresses != null && mBillingAddresses.Count > 0)
                    {
                        mBillingAddresses.Remove(mBillingAddress);
                    }
                }

                if (mBillingAddresses.Count == 0)
                    lstBilling.IsVisible = false;
                else
                {
                    lstBilling.IsVisible = true;
                    lstBilling.ItemsSource = mBillingAddresses.ToList();
                    lstBilling.HeightRequest = mBillingAddresses.Count * 120;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnDeleteBillding: " + ex.Message);
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
                    txtLandmark.Text = mBillingAddress.Landmark;
                    pkNationality.Text = mBillingAddress.Nationality;
                }

                if (mBillingAddresses.Count == 0)
                    lstBilling.IsVisible = false;
                else
                {
                    lstBilling.IsVisible = true;
                    lstBilling.ItemsSource = mBillingAddresses.ToList();
                    lstBilling.HeightRequest = mBillingAddresses.Count * 120;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/BtnEdit: " + ex.Message);
            }
        }

        private void txtOtherSubCategory_Completed(object sender, EventArgs e)
        {
            if (!Common.EmptyFiels(txtOtherSubCategory.Text))
            {
                if (selectedSubCategory.Where(x => x == txtOtherSubCategory.Text).Count() == 0)
                {
                    selectedSubCategory.Add(pkSubCategory.SelectedItem.ToString());
                }

                wlSubCategory.ItemsSource = selectedSubCategory.ToList();
            }
        }

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
    }
}