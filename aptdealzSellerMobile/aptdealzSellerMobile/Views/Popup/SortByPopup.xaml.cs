using aptdealzSellerMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortByPopup : PopupPage
    {
        #region Objects
        public event EventHandler isRefresh;
        #endregion

        #region Constructor
        public SortByPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void BindSource(string viewSource)
        {
            if (!Common.EmptyFiels(viewSource))
            {
                if (viewSource == "ID")
                {
                    ClearSource();
                    BtnID.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == "Status")
                {
                    ClearSource();
                    BtnStatus.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == "Amount")
                {
                    ClearSource();
                    BtnAmount.Source = Constraints.Redio_Selected;
                }
                else
                {
                    ClearSource();
                    BtnID.Source = Constraints.Redio_Selected;
                }
            }
        }

        private void ClearSource()
        {
            BtnID.Source = Constraints.Redio_UnSelected;
            BtnStatus.Source = Constraints.Redio_UnSelected;
            BtnAmount.Source = Constraints.Redio_UnSelected;
        }

        #endregion

        #region Events
        private void BtnID_Clicked(object sender, EventArgs e)
        {
            BindSource("ID");
            isRefresh?.Invoke("ID", null);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnStatus_Clicked(object sender, EventArgs e)
        {
            BindSource("Status");
            isRefresh?.Invoke("Status", null);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnAmount_Clicked(object sender, EventArgs e)
        {
            BindSource("Amount");
            isRefresh?.Invoke("Amount", null);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}