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

        private void FrmSubmit_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: frmGrievance);
                //App.Current.MainPage = new MasterData.MasterDataPage();
                Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievanceDetailPage/FrmSubmit_Tapped: " + ex.Message);
            }
        }
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
    }
}