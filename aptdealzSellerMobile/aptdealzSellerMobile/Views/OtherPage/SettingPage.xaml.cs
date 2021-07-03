using aptdealzSellerMobile.Utility;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        #region Constructor
        public SettingPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void ImgSwitch_Tapped(object sender, EventArgs e)
        {
            if (imgSwitch.Source.ToString().Replace("File: ", "") == Constraints.Switch_Off)
            {
                imgSwitch.Source = Constraints.Switch_On;
            }
            else
            {
                imgSwitch.Source = Constraints.Switch_Off;
            }
        }

        private void ImgAlertTone_Tapped(object sender, EventArgs e)
        {
            pkAlertTone.Focus();
        }

        private void ImgLanguage_Tapped(object sender, EventArgs e)
        {
            pkLanguage.Focus();
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

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}