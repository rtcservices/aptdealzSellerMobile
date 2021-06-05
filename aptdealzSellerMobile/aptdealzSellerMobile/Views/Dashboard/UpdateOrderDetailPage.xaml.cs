using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateOrderDetailPage : ContentPage
    {
        #region Constructor
        public UpdateOrderDetailPage()
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

        private void ImgOrderStatusPck_Clicked(object sender, EventArgs e)
        {
            pckOrderStatus.Focus();
        }

        private void StkActivity_Tapped(object sender, EventArgs e)
        {
            if (lblAccepted.Text == "Accepted")
            {
                lblAccepted.Text = "Delivered";
                StkAccept.IsVisible = false;
                BtnUpdate.IsVisible = false;
                //deliver grid true
                StkDelivered.IsVisible = true;
            }
            else
            {
                lblAccepted.Text = "Accepted";
                StkAccept.IsVisible = true;
                BtnUpdate.IsVisible = true;
                //deliver grid false
                StkDelivered.IsVisible = false;
            }
        }

        private async void frmUpdateTapped_Tapped(object sender, EventArgs e)
        {
            try
            {
                await frmUpdateTapped.ScaleTo(0.9, 100, Easing.Linear);
                await frmUpdateTapped.ScaleTo(1.0, 100, Easing.Linear);
                await Navigation.PushAsync(new Views.MainTabbedPages.MainTabbedPage(false));
            }
            catch (Exception ex)
            {


            }
        } 
        #endregion
    }
}