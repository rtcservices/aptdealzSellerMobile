using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequirementDetailPage : ContentPage
    {
        #region Constructor
        public RequirementDetailPage()
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
            Navigation.PopAsync();
        }

        private async void frmReqDetailReject_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frmReqRejectQuote = (Frame)sender;
                if (frmReqRejectQuote != null)
                {
                    await frmReqRejectQuote.ScaleTo(0.9, 100, Easing.Linear);
                    await frmReqRejectQuote.ScaleTo(1.0, 100, Easing.Linear);
                }
            }
            catch (Exception ec)
            {

            }
        }

        private async void frmReqDetailProvideQoute_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frmProvideQuote = (Frame)sender;
                if (frmProvideQuote != null)
                {
                    await frmProvideQuote.ScaleTo(0.9, 100, Easing.Linear);
                    await frmProvideQuote.ScaleTo(1.0, 100, Easing.Linear);
                    await Navigation.PushAsync(new QuoteDetailPage());
                }
            }
            catch (Exception ec)
            {

            }
        }

        private async void frmReqDetailRevealContact_Tapped(object sender, EventArgs e)
        {
            try
            {
                var frmReveal = (Frame)sender;
                if (frmReveal != null)
                {
                    await frmReveal.ScaleTo(0.9, 100, Easing.Linear);
                    await frmReveal.ScaleTo(1.0, 100, Easing.Linear);
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
    }
}