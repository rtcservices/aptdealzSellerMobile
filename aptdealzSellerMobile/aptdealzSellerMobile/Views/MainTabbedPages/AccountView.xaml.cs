using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountView : ContentView
    {
        #region Object
        public event EventHandler isRefresh; 
        #endregion

        #region Constructor
        public AccountView()
        {
            InitializeComponent();
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
            isRefresh?.Invoke(true, EventArgs.Empty);
        }

        private void ImgCamera_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgEdit_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnUpdate_Clicked(object sender, EventArgs e)
        {

        }

        private void GrdBilling_Tapped(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {

            }
        }

        private void GrdCompanyProfile_Tapped(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
            }
        }

        private void GrdBankInfo_Tapped(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
            }
        }

        private void BtnUplodeDocs_Clicked(object sender, EventArgs e)
        {

        }

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        } 
        #endregion
    }
}