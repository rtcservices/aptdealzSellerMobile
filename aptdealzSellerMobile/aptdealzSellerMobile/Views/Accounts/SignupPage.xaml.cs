using aptdealzSellerMobile.Utility;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        #region Constructor
        public SignupPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ResetPassword_Tapped(object sender, EventArgs e)
        {

        }

        private void CompnyProfile_Tapped(object sender, EventArgs e)
        {
            if (imgCompanyProfileDown.Source.ToString().Replace("File: ", "") == Constraints.Down_Arrow)
            {
                imgCompanyProfileDown.Source = Constraints.Up_Arrow;
                grdShippingAddress.IsVisible = true;
            }
            else
            {
                imgCompanyProfileDown.Source = Constraints.Down_Arrow;
                grdShippingAddress.IsVisible = false;
            }
        }

        private void GstInformation_Tapped(object sender, EventArgs e)
        {
            if (imgGstDown.Source.ToString().Replace("File: ", "") == Constraints.Down_Arrow)
            {
                imgGstDown.Source = Constraints.Up_Arrow;
                grdBankInfo.IsVisible = true;
            }
            else
            {
                imgGstDown.Source = Constraints.Down_Arrow;
                grdBankInfo.IsVisible = false;
            }
        }

        private async void Submit_Tapped(object sender, EventArgs e)
        {
            try
            {
                await frmSubmitTapped.ScaleTo(0.9, 100, Easing.Linear);
                await frmSubmitTapped.ScaleTo(1.0, 100, Easing.Linear);
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
            }
        }

        private void StkSignup_Tapped(object sender, EventArgs e)
        {

        }

        private async void Login_Tapped(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new LoginPage());
        }

        private void ImgCoutryPck_Clicked(object sender, EventArgs e)
        {
            try
            {
                pckCategory.Focus();

            }

            catch (Exception ex)
            {

            }
        }

        private void StkAcceptTerm_Tapped(object sender, EventArgs e)
        {

            try
            {
                if (imgCheck.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Check)
                {
                    imgCheck.Source = Constraints.CheckBox_UnCheck;
                }
                else
                {
                    imgCheck.Source = Constraints.CheckBox_Check;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}