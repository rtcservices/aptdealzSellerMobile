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
        #region [ Objects ]
        public event EventHandler isRefresh;
        private string PageName;
        #endregion

        #region [ Constructor ]
        public StatusPopup(int? StatusBy, string StatusPageName = null)
        {
            InitializeComponent();
            PageName = StatusPageName;
            BindSource(StatusBy);
        }
        #endregion

        #region [ Methods ]
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
                lblFourType.Text = GrievancesStatus.ReOpened.ToString();
                lblfiveType.Text = GrievancesStatus.All.ToString();
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

                ClearSource();
                if (PageName != "Grievances")
                {
                    if (viewSource == (int)QuoteStatus.Submitted)
                    {
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)QuoteStatus.Accepted)
                    {
                        imgSecondType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)QuoteStatus.Rejected)
                    {
                        imgThirdType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)QuoteStatus.All)
                    {
                        imgFourType.Source = Constraints.Redio_Selected;
                    }
                   
                    else
                    {
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
                }
                else
                {
                    if (viewSource == (int)GrievancesStatus.Pending)
                    {
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)GrievancesStatus.Open)
                    {
                        imgSecondType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)GrievancesStatus.Closed)
                    {
                        imgThirdType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)GrievancesStatus.ReOpened)
                    {
                        imgFourType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == (int)GrievancesStatus.All)
                    {
                        imgfiveType.Source = Constraints.Redio_Selected;
                    }
                    else
                    {
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
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
            imgfiveType.Source = Constraints.Redio_UnSelected;
        }
        #endregion

        #region [ Events ]
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
                    BindSource((int)GrievancesStatus.ReOpened);
                    isRefresh?.Invoke(GrievancesStatus.ReOpened.ToString(), null);
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

        private void StkFiveType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource((int)GrievancesStatus.All);
                isRefresh?.Invoke(GrievancesStatus.All.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("StatusPopup/StkFiveType_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}