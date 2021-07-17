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
        #endregion

        #region Constructor
        public StatusPopup(int? StatusBy)
        {
            InitializeComponent();
            BindSource(StatusBy);
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindLabel();
        }

        void BindLabel()
        {
            lblFirstType.Text = "Submitted";
            lblSecondType.Text = "Accepted";
            lblThirdType.Text = "Rejected";
            lblFourType.Text = "All";
        }

        void BindSource(int? viewSource)
        {
            if (viewSource == null)
                return;

            if (viewSource == (int)QuoteStatus.Submitted)
            {
                ClearSource();
                imgFirstType.Source = Constraints.Redio_Selected;
            }
            else if (viewSource == (int)QuoteStatus.Accepted)
            {
                ClearSource();
                imgSecondType.Source = Constraints.Redio_Selected;
            }
            else if (viewSource == (int)QuoteStatus.Rejected)
            {
                ClearSource();
                imgThirdType.Source = Constraints.Redio_Selected;
            }
            else if (viewSource == (int)QuoteStatus.All)
            {
                ClearSource();
                imgFourType.Source = Constraints.Redio_Selected;
            }
            else
            {
                ClearSource();
                imgFourType.Source = Constraints.Redio_Selected;
            }
        }

        void ClearSource()
        {
            imgFirstType.Source = Constraints.Redio_UnSelected;
            imgSecondType.Source = Constraints.Redio_UnSelected;
            imgThirdType.Source = Constraints.Redio_UnSelected;
            imgFourType.Source = Constraints.Redio_UnSelected;
        }
        #endregion

        #region Events
        private void StkFirstType_Tapped(object sender, EventArgs e)
        {
            BindSource((int)QuoteStatus.Submitted);
            isRefresh?.Invoke((int)QuoteStatus.Submitted, null);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {

            BindSource((int)QuoteStatus.Accepted);
            isRefresh?.Invoke((int)QuoteStatus.Accepted, null);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            BindSource((int)QuoteStatus.Rejected);
            isRefresh?.Invoke((int)QuoteStatus.Rejected, null);
            PopupNavigation.Instance.PopAsync();
        }

        private void StkFourType_Tapped(object sender, EventArgs e)
        {
            BindSource((int)QuoteStatus.All);
            isRefresh?.Invoke((int)QuoteStatus.All, null);
            PopupNavigation.Instance.PopAsync();
        }
        #endregion
    }
}