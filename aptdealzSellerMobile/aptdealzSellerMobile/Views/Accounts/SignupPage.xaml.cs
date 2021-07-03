using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage, INotifyPropertyChanged
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
        private bool isChecked = false;
        private List<Country> mCountries;
        private List<Category> mCategories;
        private List<SubCategory> mSubCategories;
        private List<string> selectedSubCategory;
        private string relativePath = string.Empty;
        private List<string> documentList;
        private bool isFirstLoad = true;
        private string ErrorMessage = string.Empty;
        #endregion

        #region Constructor
        public SignupPage()
        {
            InitializeComponent();
            BindObjects();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isChecked)
                imgCheck.Source = Constraints.CheckBox_Checked;
            else
                imgCheck.Source = Constraints.CheckBox_UnChecked;
        }

        async void BindObjects()
        {
            mCountries = new List<Country>();
            mCategories = new List<Category>();
            mSubCategories = new List<SubCategory>();
            selectedSubCategory = new List<string>();
            documentList = new List<string>();

            UserDialogs.Instance.ShowLoading(Constraints.Loading);
            await BindCategoriesList();
            await GetCountries();
            UserDialogs.Instance.HideLoading();
        }

        public bool Validations()
        {
            bool isValid = false;
            if (Common.EmptyFiels(txtFullName.Text) || Common.EmptyFiels(txtPassword.Text)
                || Common.EmptyFiels(txtEmail.Text) || Common.EmptyFiels(txtPhoneNumber.Text)
                || Common.EmptyFiels(txtBuildingNumber.Text) || Common.EmptyFiels(txtStreet.Text)
                || Common.EmptyFiels(txtCity.Text) || Common.EmptyFiels(txtLandmark.Text)
                || Common.EmptyFiels(pkNationality.Text) || Common.EmptyFiels(txtDescription.Text)
                || pkCategory.SelectedIndex == -1 || selectedSubCategory == null
                || Common.EmptyFiels(txtExperience.Text) || Common.EmptyFiels(txtSupplyArea.Text)
                || Common.EmptyFiels(txtGstNumber.Text) || Common.EmptyFiels(txtPan.Text)
                || Common.EmptyFiels(txtBankAccount.Text) || Common.EmptyFiels(txtBankName.Text)
                || Common.EmptyFiels(txtIfsc.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_All);

                RequiredFields();
                isValid = false;
            }

            if (Common.EmptyFiels(txtFullName.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_FullName);
            }
            else if (Common.EmptyFiels(txtPassword.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Password);
            }
            else if (!Common.IsValidPassword(txtPassword.Text))
            {
                DisplayAlert(Constraints.Alert, String.Format("The {0} must be at least {1} characters long and should have atleast one capital leter, special character ({2}) and digit.", "Password", 8, "#$^+=!*()@%&"), Constraints.Ok);
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
            else if (Common.EmptyFiels(txtBuildingNumber.Text))
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
            else if (Common.EmptyFiels(txtLandmark.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Landmark);
            }
            else if (Common.EmptyFiels(pkNationality.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Nationality);
            }
            else if (mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower()).Count() == 0)
            {
                Common.DisplayErrorMessage(Constraints.InValid_Nationality);
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
            else if (!isChecked)
            {
                Common.DisplayErrorMessage(Constraints.Agree_T_C);
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

        void BindDocumentList()
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
                Common.DisplayErrorMessage("SignupPage/BindDocumentList: " + ex.Message);
            }
        }

        async Task BindCategoriesList()
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
                Common.DisplayErrorMessage("SignupPage/GetCountries: " + ex.Message);
            }
        }

        Model.Request.Register FillRegister()
        {
            Register mRegister = new Register();
            try
            {
                mRegister.FullName = txtFullName.Text;
                mRegister.Email = txtEmail.Text;
                mRegister.PhoneNumber = txtPhoneNumber.Text;
                mRegister.AlternativePhoneNumber = txtAltPhoneNumber.Text;
                mRegister.Password = txtPassword.Text;
                mRegister.About = txtAbout.Text;
                mRegister.State = txtState.Text;
                mRegister.PinCode = txtPincode.Text;

                if (documentList.Count != 0)
                {
                    mRegister.Documents = documentList;
                }
                if (!Common.EmptyFiels(txtBuildingNumber.Text))
                {
                    mRegister.Building = txtBuildingNumber.Text;
                }
                if (!Common.EmptyFiels(txtStreet.Text))
                {
                    mRegister.Street = txtStreet.Text;
                }
                if (!Common.EmptyFiels(txtCity.Text))
                {
                    mRegister.City = txtCity.Text;
                }
                if (!Common.EmptyFiels(txtLandmark.Text))
                {
                    mRegister.Landmark = txtLandmark.Text;
                }
                if (!Common.EmptyFiels(pkNationality.Text))
                {
                    mRegister.CountryId = (int)(mCountries.Where(x => x.Name.ToLower() == pkNationality.Text.ToLower().ToString()).FirstOrDefault()?.CountryId);
                }
                if (!Common.EmptyFiels(App.latitude.ToString()))
                {
                    mRegister.Latitude = App.latitude;
                }
                if (!Common.EmptyFiels(App.longitude.ToString()))
                {
                    mRegister.Longitude = App.longitude;
                }
                if (!Common.EmptyFiels(txtDescription.Text))
                {
                    mRegister.Description = txtDescription.Text;
                }

                if (!Common.EmptyFiels(txtOtherCategory.Text))
                {
                    mRegister.Category = txtOtherCategory.Text;
                }
                else if (pkCategory.SelectedIndex > -1)
                {
                    mRegister.Category = mCategories.Where(x => x.Name == pkCategory.SelectedItem.ToString()).FirstOrDefault()?.Name;
                }

                if (selectedSubCategory != null)
                {
                    mRegister.SubCategories = selectedSubCategory;
                }

                if (!Common.EmptyFiels(txtExperience.Text))
                {
                    mRegister.Experience = txtExperience.Text;
                }
                if (!Common.EmptyFiels(txtSupplyArea.Text))
                {
                    mRegister.AreaOfSupply = txtSupplyArea.Text;
                }
                if (!Common.EmptyFiels(txtGstNumber.Text))
                {
                    mRegister.Gstin = txtGstNumber.Text;
                }
                if (!Common.EmptyFiels(txtPan.Text))
                {
                    mRegister.Pan = txtPan.Text;
                }
                if (!Common.EmptyFiels(txtBankAccount.Text))
                {
                    mRegister.BankAccountNumber = txtBankAccount.Text;
                }
                if (!Common.EmptyFiels(txtBankName.Text))
                {
                    mRegister.Branch = txtBankName.Text;
                }
                if (!Common.EmptyFiels(txtIfsc.Text))
                {
                    mRegister.Ifsc = txtIfsc.Text;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return mRegister;
        }

        async Task<Response> ValidateEmailAndPhoneExist()
        {
            Response mResponse = new Response();
            try
            {
                RegisterAPI registerAPI = new RegisterAPI();
                UniquePhoneNumber mUniquePhoneNumber = new UniquePhoneNumber();
                mUniquePhoneNumber.PhoneNumber = txtPhoneNumber.Text;

                UniqueEmail mUniqueEmail = new UniqueEmail();
                mUniqueEmail.Email = txtEmail.Text;

                mResponse = await registerAPI.IsUniqueEmail(mUniqueEmail);
                if (mResponse != null && mResponse.Succeeded)
                {
                    var result = (bool)mResponse.Data;
                    if (result)
                    {
                        mResponse = await registerAPI.IsUniquePhoneNumber(mUniquePhoneNumber);
                    }
                    else
                    {
                        return mResponse;
                    }
                }
                else
                {
                    return mResponse;
                }
            }
            catch (Exception)
            {
                return null;
            }
            return mResponse;
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
                if (!Common.EmptyFiels(txtAbout.Text))
                {
                    txtAbout.Text = txtAbout.Text.Trim();
                }
                if (!Common.EmptyFiels(txtState.Text))
                {
                    txtState.Text = txtState.Text.Trim();
                }
                if (!Common.EmptyFiels(txtPincode.Text))
                {
                    txtPincode.Text = txtPincode.Text.Trim();
                }
                #endregion

                #region Billing Address
                txtBuildingNumber.Text = txtBuildingNumber.Text.Trim();
                txtStreet.Text = txtStreet.Text.Trim();
                txtCity.Text = txtCity.Text.Trim();
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

        async void RegisterUser()
        {
            try
            {
                if (Validations())
                {
                    if (!await PinCodeValidation())
                        return;

                    FieldsTrim();
                    RegisterAPI registerAPI = new RegisterAPI();
                    var mRegister = FillRegister();

                    if (mRegister != null)
                    {
                        UserDialogs.Instance.ShowLoading(Constraints.Loading);
                        var mResponse = await ValidateEmailAndPhoneExist();

                        if (mResponse != null && mResponse.Succeeded)
                        {
                            mResponse = await registerAPI.Register(mRegister);
                            if (mResponse != null && mResponse.Succeeded)
                            {
                                // var UserCreated = mResponse.Message;
                                // Settings.UserId = (string)mResponse.Data;
                                // AuthenticationAPI authenticationAPI = new AuthenticationAPI();

                                //mResponse = await authenticationAPI.ActivateUser(Settings.UserId);
                                //if (mResponse != null && mResponse.Succeeded)
                                //{
                                //    Common.DisplaySuccessMessage(UserCreated);
                                //    App.Current.MainPage = new NavigationPage(new Views.Accounts.LoginPage());
                                //}
                                //else
                                //{
                                //    if (mResponse != null)
                                //        Common.DisplayErrorMessage(mResponse.Message);
                                //    else
                                //        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                                //}
                                Common.DisplaySuccessMessage(mResponse.Message);
                                ClearPropeties();
                            }
                            else
                            {
                                if (mResponse != null && mResponse.Message != null)
                                    Common.DisplayErrorMessage(mResponse.Message);
                                else
                                    Common.DisplayErrorMessage(Constraints.Something_Wrong);
                            }
                        }
                        else
                        {
                            Common.DisplayErrorMessage(mResponse.Message);
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
                Common.DisplayErrorMessage("SignupPage/RegisterUser: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        public void ClearPropeties()
        {
            try
            {
                isChecked = false;
                imgCheck.Source = Constraints.CheckBox_UnChecked;

                txtFullName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtPhoneNumber.Text = string.Empty;
                txtAltPhoneNumber.Text = string.Empty;
                txtBuildingNumber.Text = string.Empty;
                txtStreet.Text = string.Empty;
                txtCity.Text = string.Empty;
                txtLandmark.Text = string.Empty;
                txtDescription.Text = string.Empty;
                pkCategory.SelectedIndex = -1;
                pkSubCategory.SelectedIndex = -1;
                pkNationality.Text = string.Empty;
                txtOtherCategory.Text = string.Empty;
                txtOtherSubCategory.Text = string.Empty;
                txtExperience.Text = string.Empty;
                txtSupplyArea.Text = string.Empty;
                txtGstNumber.Text = string.Empty;
                txtPan.Text = string.Empty;
                txtBankAccount.Text = string.Empty;
                txtBankName.Text = string.Empty;
                txtIfsc.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/ClearPropeties: " + ex.Message);
            }
        }

        void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtFullName.Text))
                {
                    BoxFullName.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPassword.Text))
                {
                    BoxPassword.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtEmail.Text))
                {
                    BoxEmail.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(txtPhoneNumber.Text))
                {
                    BoxPhoneNumber.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

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

                if (Common.EmptyFiels(txtLandmark.Text))
                {
                    BoxLandmark.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                }

                if (Common.EmptyFiels(pkNationality.Text))
                {
                    BoxNationality.BackgroundColor = (Color)App.Current.Resources["LightRed"];
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
                Common.DisplayErrorMessage("SignupPage/EnableRequiredFields: " + ex.Message);
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
                    else if (entry.ClassId == "Password")
                    {
                        BoxPassword.BackgroundColor = (Color)App.Current.Resources["LightGray"];
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

        async Task<bool> PinCodeValidation()
        {
            bool isValid = false;
            try
            {
                if (!Common.EmptyFiels(txtPincode.Text))
                {
                    txtPincode.Text = txtPincode.Text.Trim();
                    if (Common.IsValidPincode(txtPincode.Text))
                    {
                        isValid = true;
                        //isValid = await DependencyService.Get<IProfileRepository>().ValidPincode(Convert.ToInt32(txtPincode.Text));
                    }
                    else
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Pincode);
                    }
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("AccountView/PinCodeValidation: " + ex.Message);
            }
            return isValid;
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void CompnyProfile_Tapped(object sender, EventArgs e)
        {
            if (imgCompanyProfileDown.Source.ToString().Replace("File: ", "") == Constraints.Arrow_Down)
            {
                imgCompanyProfileDown.Source = Constraints.Arrow_Up;
                grdShippingAddress.IsVisible = true;
            }
            else
            {
                imgCompanyProfileDown.Source = Constraints.Arrow_Down;
                grdShippingAddress.IsVisible = false;
            }
        }

        private void GstInformation_Tapped(object sender, EventArgs e)
        {
            if (imgGstDown.Source.ToString().Replace("File: ", "") == Constraints.Arrow_Down)
            {
                imgGstDown.Source = Constraints.Arrow_Up;
                grdBankInfo.IsVisible = true;
            }
            else
            {
                imgGstDown.Source = Constraints.Arrow_Down;
                grdBankInfo.IsVisible = false;
            }
        }

        private void StkLogin_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private void StkAcceptTerm_Tapped(object sender, EventArgs e)
        {
            if (imgCheck.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
            {
                isChecked = false;
                imgCheck.Source = Constraints.CheckBox_UnChecked;
            }
            else
            {
                isChecked = true;
                imgCheck.Source = Constraints.CheckBox_Checked;
            }
        }

        private void BtnCategory_Clicked(object sender, EventArgs e)
        {
            pkCategory.Focus();
        }

        private void BtnSubCategory_Clicked(object sender, EventArgs e)
        {
            pkSubCategory.Focus();
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
                Common.DisplayErrorMessage("SignupPage/Category_SelectedIndex: " + ex.Message);
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
                Common.DisplayErrorMessage("SignupPage/pkSubCategory_SelectedIndexChanged: " + ex.Message);
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
                Common.DisplayErrorMessage("SignupPage/BtnDeleteSubCategory_Clicked: " + ex.Message);
            }
        }

        private async void ImgUplodeDocument_Clicked(object sender, EventArgs e)
        {
            await FileSelection.FilePickup();
            relativePath = await DependencyService.Get<IFileUploadRepository>().UploadFile((int)FileUploadCategory.ProfileDocuments);
            if (!Common.EmptyFiels(relativePath))
            {
                documentList.Add(relativePath);
                BindDocumentList();
            }
        }

        int i = 0;
        private void AutoSuggestBox_TextChanged(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxTextChangedEventArgs e)
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

        private void AutoSuggestBox_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
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

        private void AutoSuggestBox_SuggestionChosen(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            pkNationality.Text = e.SelectedItem.ToString();
        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnSubmit);
            RegisterUser();
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

        private void ImgPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = Convert.ToString(imgEye.Source).Replace("File: ", string.Empty);
                if (selectedImage == Constraints.Img_Hide)
                {
                    txtPassword.IsPassword = false;
                    imgEye.Source = Constraints.Img_Show;
                }
                else
                {
                    txtPassword.IsPassword = true;
                    imgEye.Source = Constraints.Img_Hide;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("SignupPage/ImgPassword_Tapped: " + ex.Message);
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

        private async void txtPincode_Unfocused(object sender, FocusEventArgs e)
        {
            await PinCodeValidation();
        }
        #endregion
    }
}