using aptdealzSellerMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowImagePopup : PopupPage
    {
        #region [ Objects ]      
        public event EventHandler isRefresh;
        string ImageUrl = string.Empty;
        #endregion

        public ShowImagePopup(string imageUrl)
        {
            InitializeComponent();
            ImageUrl = imageUrl;

        }
        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ImgProduct.Source = ImageUrl;
        }

        private void ImgClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                isRefresh?.Invoke(true, EventArgs.Empty);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ShowImagePopup/ImgClose_Clicked: " + ex.Message);
            }
        }
    }
}