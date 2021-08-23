using aptdealzSellerMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentPopup : PopupPage
    {
        #region Objects
        // create objects here
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public PaymentPopup(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;
        }
        #endregion

        #region Events        
        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }

        private void FrmPay_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupNavigation.Instance.PopAsync();
                isRefresh?.Invoke(true, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PaymentPopup/FrmPay_Tapped: " + ex.Message);
            }
        }

        private void FrmBack_Tapped(object sender, EventArgs e)
        {
            try
            {
                PopupNavigation.Instance.PopAsync();
                isRefresh?.Invoke(false, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("PaymentPopup/FrmBack_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}