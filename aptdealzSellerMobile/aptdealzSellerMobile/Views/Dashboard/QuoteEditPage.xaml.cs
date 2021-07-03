using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteEditPage : ContentPage
    {
        #region Constructor
        public QuoteEditPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

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

        private async void EditSubmit_Tapped(object sender, EventArgs e)
        {

            try
            {
                await frmEditSubmit.ScaleTo(0.9, 100, Easing.Linear);
                await frmEditSubmit.ScaleTo(1.0, 100, Easing.Linear);
            }
            catch (Exception ex)
            {


            }
        }

        private async void BackQoute_Tapped(object sender, EventArgs e)
        {

            try
            {
                await frmBackQuote.ScaleTo(0.9, 100, Easing.Linear);
                await frmBackQuote.ScaleTo(1.0, 100, Easing.Linear);
            }
            catch (Exception ex)
            {

            }
        }

        private async void frmQuoteDetailRevealContact_Tapped(object sender, EventArgs e)
        {
            try
            {
                await frmRevalContact.ScaleTo(0.9, 100, Easing.Linear);
                await frmRevalContact.ScaleTo(1.0, 100, Easing.Linear);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}