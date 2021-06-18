using aptdealzSellerMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusPopup : PopupPage
    {
        #region Objects
        public event EventHandler isRefresh;
        private string isStatus;

        #endregion

        #region Constructor
        public StatusPopup(string isStatus = null)
        {
            InitializeComponent();
            this.isStatus = isStatus;
            BindStatus();
        }
        #endregion

        #region Methods
        private void BindStatus()
        {
            try
            {
                if (isStatus == "isRequest")
                {
                    lblOpen.Text = "Cleared";
                    lblRejected.Text = "Pending";
                }
                else if (isStatus == "isGrievanceRequest")
                {
                    lblOpen.Text = "Open";
                    lblRejected.Text = "Closed";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void BindSource(string viewSource)
        {
            if (!Common.EmptyFiels(viewSource))
            {
                if (viewSource == "All")
                {
                    ClearSource();
                    BtnAll.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == "Open")
                {
                    ClearSource();
                    BtnOpen.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == "Rejected")
                {
                    ClearSource();
                    BtnRejected.Source = Constraints.Redio_Selected;
                }
                else
                {
                    ClearSource();
                    BtnAll.Source = Constraints.Redio_Selected;
                }
            }
        }

        private void ClearSource()
        {
            BtnAll.Source = Constraints.Redio_UnSelected;
            BtnOpen.Source = Constraints.Redio_UnSelected;
            BtnRejected.Source = Constraints.Redio_UnSelected;
        }
        #endregion

        #region Events
        private void BtnAll_Clicked(object sender, EventArgs e)
        {
            BindSource("All");
            isRefresh?.Invoke("All", null);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnOpen_Clicked(object sender, EventArgs e)
        {
            BindSource("Open");
            isRefresh?.Invoke("Open", null);
            PopupNavigation.Instance.PopAsync();
        }

        private void BtnRejected_Clicked(object sender, EventArgs e)
        {
            BindSource("Rejected");
            isRefresh?.Invoke("Rejected", null);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}