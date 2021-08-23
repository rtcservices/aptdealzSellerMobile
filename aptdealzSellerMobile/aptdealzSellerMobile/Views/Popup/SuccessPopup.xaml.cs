using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessPopup : PopupPage
    {
        #region Objects        
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public SuccessPopup(string ReqMessage)
        {
            InitializeComponent();
            lblMessage.Text = ReqMessage;
        }
        #endregion

        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }

        #region Events
        private void FrmHome_Tapped(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            isRefresh?.Invoke(true, EventArgs.Empty);
        }
        #endregion
    }
}