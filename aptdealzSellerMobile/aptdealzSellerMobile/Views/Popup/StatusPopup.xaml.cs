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
        private string PageName;
        #endregion

        #region Constructor
        public StatusPopup(int? StatusBy, string StatusPageName = null)
        {
            InitializeComponent();
            PageName = StatusPageName;
            BindSource(StatusBy);
        }
        #endregion

        #region Methods
        protected override bool OnBackgroundClicked()
        {
            base.OnBackgroundClicked();
            return false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindLabel();
        }

        private void BindLabel()
        {
            if (PageName == "Grievances")
            {
                lblFirstType.Text = GrievancesStatus.Pending.ToString();
                lblSecondType.Text = GrievancesStatus.Open.ToString();
                lblThirdType.Text = GrievancesStatus.Closed.ToString();
                lblFourType.Text = GrievancesStatus.All.ToString();
            }
            else
            {
                lblFirstType.Text = QuoteStatus.Submitted.ToString();
                lblSecondType.Text = QuoteStatus.Accepted.ToString();
                lblThirdType.Text = QuoteStatus.Rejected.ToString();
                lblFourType.Text = QuoteStatus.All.ToString();
            }
        }

        private void BindSource(int? viewSource)
        {
            try
            {
                if (viewSource == null)
                    return;

                if (viewSource == (int)QuoteStatus.Submitted || viewSource == (int)GrievancesStatus.Pending)
                {
                    ClearSource();
                    imgFirstType.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == (int)QuoteStatus.Accepted || viewSource == (int)GrievancesStatus.Open)
                {
                    ClearSource();
                    imgSecondType.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == (int)QuoteStatus.Rejected || viewSource == (int)GrievancesStatus.Closed)
                {
                    ClearSource();
                    imgThirdType.Source = Constraints.Redio_Selected;
                }
                else if (viewSource == (int)QuoteStatus.All || viewSource == (int)GrievancesStatus.All)
                {
                    ClearSource();
                    imgFourType.Source = Constraints.Redio_Selected;
                }
                else
                {
                    ClearSource();
                    imgFirstType.Source = Constraints.Redio_Selected;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("StatusPopup/BindSource: " + ex.Message);
            }
        }

        private void ClearSource()
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
            try
            {
                if (PageName == "Grievances")
                {
                    BindSource((int)GrievancesStatus.Pending);
                    isRefresh?.Invoke(GrievancesStatus.Pending.ToString(), null);
                }
                else
                {
                    BindSource((int)QuoteStatus.Submitted);
                    isRefresh?.Invoke(QuoteStatus.Submitted.ToString(), null);
                }
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("StatusPopup/StkFirstType: " + ex.Message);
            }
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {

            try
            {
                if (PageName == "Grievances")
                {
                    BindSource((int)GrievancesStatus.Open);
                    isRefresh?.Invoke(GrievancesStatus.Open.ToString(), null);
                }
                else
                {
                    BindSource((int)QuoteStatus.Accepted);
                    isRefresh?.Invoke(QuoteStatus.Accepted.ToString(), null);
                }
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("StatusPopup/StkSecondType: " + ex.Message);
            }
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (PageName == "Grievances")
                {
                    BindSource((int)GrievancesStatus.Closed);
                    isRefresh?.Invoke(GrievancesStatus.Closed.ToString(), null);
                }
                else
                {
                    BindSource((int)QuoteStatus.Rejected);
                    isRefresh?.Invoke(QuoteStatus.Rejected.ToString(), null);
                }
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("StatusPopup/StkThirdType: " + ex.Message);
            }
        }

        private void StkFourType_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (PageName == "Grievances")
                {
                    BindSource((int)GrievancesStatus.All);
                    isRefresh?.Invoke(GrievancesStatus.All.ToString(), null);
                }
                else
                {
                    BindSource((int)QuoteStatus.All);
                    isRefresh?.Invoke(QuoteStatus.All.ToString(), null);
                }
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("StatusPopup/StkFourType: " + ex.Message);
            }
        }
        #endregion
    }
}