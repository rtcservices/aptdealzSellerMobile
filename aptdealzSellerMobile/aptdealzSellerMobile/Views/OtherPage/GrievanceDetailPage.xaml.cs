using aptdealzSellerMobile.Utility;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GrievanceDetailPage : ContentPage
    {
        #region Constructor
        public GrievanceDetailPage()
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

        private async void FrmSubmit_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(frame: frmGrievance);
                await Navigation.PushAsync(new Views.MainTabbedPages.MainTabbedPage("Home")); ;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailPage/FrmSubmit_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}