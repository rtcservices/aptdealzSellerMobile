using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailView : ContentView
    {
        #region Costructor
        public OrderDetailView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private async void ScanQRCode_Tapped(object sender, EventArgs e)
        {

            try
            {
                await frmScanQRCode.ScaleTo(0.9, 100, Easing.Linear);
                await frmScanQRCode.ScaleTo(1.0, 100, Easing.Linear);
                //await Navigation.PushAsync(new Views.MainTabbedPages.MainTabbedPage(false));
            }
            catch (Exception ex)
            {


            }
        }
        #endregion
    }
}