using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitQuoteView : ContentView
    {
        #region Constructor
        public SubmitQuoteView()
        {
            InitializeComponent();
        } 
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
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

        }

        private void ImgProductPck_Clicked(object sender, EventArgs e)
        {
            frmQuoteProduct.Focus();
        }

        private async void Submit_QuoteTapped(object sender, EventArgs e)
        {

            try
            {
                await SubmitQuote.ScaleTo(0.9, 100, Easing.Linear);
                await SubmitQuote.ScaleTo(1.0, 100, Easing.Linear);
            }
            catch (Exception ex)
            {


            }
        }
    } 
    #endregion
}