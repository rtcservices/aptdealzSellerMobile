using aptdealzSellerMobile.Utility;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;


namespace aptdealzSellerMobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopup : PopupPage
    {
        #region Objects       
        public event EventHandler isRefresh;
        private string PageName;
        #endregion

        #region Constructor        
        public FilterPopup(string SortBy, string SortPageName)
        {
            InitializeComponent();
            PageName = SortPageName;
            BindSource(SortBy);
        }
        #endregion

        #region Methos
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
            try
            {
                if (PageName == "Active")
                {
                    StkFourType.IsVisible = true;
                    lblFirstType.Text = "ID";
                    lblSecondType.Text = "Date";
                    lblThirdType.Text = "Quotes";
                    lblFourType.Text = "TotalPriceEstimation";
                }
                else if (PageName == "Quote")
                {
                    StkFourType.IsVisible = true;
                    lblFirstType.Text = "ID";
                    lblSecondType.Text = "Date";
                    lblThirdType.Text = "Amount";
                    lblFourType.Text = "Validity";
                }
                else if (PageName == "Order")
                {
                    StkFourType.IsVisible = false;
                    lblFirstType.Text = "ID";
                    lblSecondType.Text = "Date";
                    lblThirdType.Text = "Amount";
                }
                else
                {
                    StkFourType.IsVisible = true;
                    lblFirstType.Text = "ID";
                    lblSecondType.Text = "Date";
                    lblThirdType.Text = "Quotes";
                    lblFourType.Text = "TotalPriceEstimation";
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/BindLabel: " + ex.Message);
            }
        }

        private void BindSource(string viewSource)
        {
            try
            {
                if (!Common.EmptyFiels(viewSource))
                {
                    if (viewSource == SortByField.ID.ToString())
                    {
                        ClearSource();
                        imgFirstType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == SortByField.Date.ToString())
                    {
                        ClearSource();
                        imgSecondType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == SortByField.Quotes.ToString() || viewSource == SortByField.Amount.ToString())
                    {
                        ClearSource();
                        imgThirdType.Source = Constraints.Redio_Selected;
                    }
                    else if (viewSource == SortByField.Validity.ToString() || viewSource == SortByField.TotalPriceEstimation.ToString())
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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/BindSource: " + ex.Message);
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
                BindSource(SortByField.ID.ToString());
                isRefresh?.Invoke(SortByField.ID.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkFirstType: " + ex.Message);
            }
        }

        private void StkSecondType_Tapped(object sender, EventArgs e)
        {
            try
            {
                BindSource(SortByField.Date.ToString());
                isRefresh?.Invoke(SortByField.Date.ToString(), null);
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkSecondType: " + ex.Message);
            }
        }

        private void StkThirdType_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (PageName != "Active")
                {
                    BindSource(SortByField.Amount.ToString());
                    isRefresh?.Invoke(SortByField.Amount.ToString(), null);
                }
                else
                {
                    BindSource(SortByField.Quotes.ToString());
                    isRefresh?.Invoke(SortByField.Quotes.ToString(), null);

                }
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkThirdType: " + ex.Message);
            }
        }

        private void StkFourType_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (PageName != "Active")
                {
                    BindSource(SortByField.Validity.ToString());
                    isRefresh?.Invoke(SortByField.Validity.ToString(), null);
                }
                else
                {
                    BindSource(SortByField.TotalPriceEstimation.ToString());
                    isRefresh?.Invoke(SortByField.TotalPriceEstimation.ToString(), null);

                }
                PopupNavigation.Instance.PopAsync();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("FilterPopup/StkFourType: " + ex.Message);
            }
        }
        #endregion
    }
}