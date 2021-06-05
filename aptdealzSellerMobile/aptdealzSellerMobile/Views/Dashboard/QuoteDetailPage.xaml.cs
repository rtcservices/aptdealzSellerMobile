using aptdealzSellerMobile.Views.MainTabbedPages;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuoteDetailPage : ContentPage
    {
        #region Objects
        public event EventHandler isRefreshSubmitQute;
        #endregion

        #region Constructor
        public QuoteDetailPage()
        {
            InitializeComponent();
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
            SubmitQuote.Focus();
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

        private async void Submit_QuoteTapped(object sender, EventArgs e)
        {
            try
            {
                await btnSubmitQuoteTapped.ScaleTo(0.9, 100, Easing.Linear);
                await btnSubmitQuoteTapped.ScaleTo(1.0, 100, Easing.Linear);
                await Navigation.PushAsync(new MainTabbedPage(true));
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion
    }
}