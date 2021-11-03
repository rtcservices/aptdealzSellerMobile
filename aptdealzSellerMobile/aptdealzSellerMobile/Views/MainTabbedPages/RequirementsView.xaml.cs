using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.Popup;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequirementsView : ContentView
    {
        #region [ Objects ]      
        private List<Requirement> mRequirements;
        private string filterBy = SortByField.Date.ToString();
        private string title = string.Empty;
        private bool isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion       

        #region [ Constructor ]
        public RequirementsView()
        {
            try
            {
                InitializeComponent();
                mRequirements = new List<Requirement>();
                pageNo = 1;
                GetActiveRequirements(filterBy, title, isAssending);

                MessagingCenter.Unsubscribe<string>(this, Constraints.Str_NotificationCount); MessagingCenter.Subscribe<string>(this, Constraints.Str_NotificationCount, (count) =>
                {
                    if (!Common.EmptyFiels(Common.NotificationCount))
                    {
                        lblNotificationCount.Text = count;
                        frmNotification.IsVisible = true;
                    }
                    else
                    {
                        frmNotification.IsVisible = false;
                        lblNotificationCount.Text = string.Empty;
                    }
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Methods ]
        private async void GetActiveRequirements(string FilterBy = "", string Title = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                RequirementAPI requirementAPI = new RequirementAPI();
                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }
                var mResponse = await requirementAPI.GetAllActiveRequirements(FilterBy, Title, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var requirements = result.ToObject<List<Requirement>>();
                    if (pageNo == 1)
                    {
                        mRequirements.Clear();
                    }

                    foreach (var mRequirement in requirements)
                    {
                        if (mRequirements.Where(x => x.RequirementId == mRequirement.RequirementId).Count() == 0)
                            mRequirements.Add(mRequirement);
                    }
                    BindList(mRequirements);
                }
                else
                {
                    lstRequirements.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    //if (mResponse.Message != null)
                    //{
                    //    lblNoRecord.Text = mResponse.Message;
                    //}
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindList(List<Requirement> mRequirementList)
        {
            try
            {
                if (mRequirementList != null && mRequirementList.Count > 0)
                {
                    lstRequirements.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstRequirements.ItemsSource = mRequirementList.ToList();
                }
                else
                {
                    lstRequirements.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/BindList: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        #region [ Header Navigation ]
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    await Navigation.PushAsync(new NotificationPage());
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RequirementsView/ImgNotification_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }
        #endregion

        #region [ Filtering ]
        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            var Tab = (Frame)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false;
                    var sortby = new FilterPopup(filterBy, "Active");
                    sortby.isRefresh += (s1, e1) =>
                    {
                        string result = s1.ToString();
                        if (!Common.EmptyFiels(result))
                        {
                            filterBy = result;
                            if (filterBy == SortByField.ID.ToString())
                            {
                                lblFilterBy.Text = filterBy;
                            }
                            else
                            {
                                lblFilterBy.Text = filterBy.ToCamelCase();
                            }
                            pageNo = 1;
                            mRequirements.Clear();
                            GetActiveRequirements(filterBy, title, isAssending);
                        }
                    };
                    await PopupNavigation.Instance.PushAsync(sortby);
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RequirementsView/CustomEntry_Unfocused: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (ImgSort.Source.ToString().Replace("File: ", "") == Constraints.Sort_ASC)
                {
                    ImgSort.Source = Constraints.Sort_DSC;
                    isAssending = false;
                }
                else
                {
                    ImgSort.Source = Constraints.Sort_ASC;
                    isAssending = true;
                }
                pageNo = 1;
                mRequirements.Clear();
                GetActiveRequirements(filterBy, title, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetActiveRequirements(filterBy, entrSearch.Text, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mRequirements.Clear();
                    GetActiveRequirements(filterBy, title, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            entrSearch.Text = string.Empty;
            BindList(mRequirements);
        }
        #endregion

        #region [ Listing ]
        private void BtnRequerments_Tapped(object sender, EventArgs e)
        {
            try
            {
                var imgExp = (ImageButton)sender;
                var viewCell = (ViewCell)imgExp.Parent.Parent.Parent;
                if (viewCell != null)
                {
                    viewCell.ForceUpdateSize();
                }

                var mRequirement = imgExp.BindingContext as Requirement;
                if (mRequirement != null && mRequirement.ArrowImage == Constraints.Arrow_Right)
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Down;
                    mRequirement.GridBg = (Color)App.Current.Resources["appColor8"];
                    mRequirement.MoreDetail = true;
                    mRequirement.HideDetail = false;
                    mRequirement.NameFont = 15;
                }
                else
                {
                    mRequirement.ArrowImage = Constraints.Arrow_Right;
                    mRequirement.GridBg = Color.Transparent;
                    mRequirement.MoreDetail = false;
                    mRequirement.HideDetail = true;
                    mRequirement.NameFont = 13;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/BtnRequerments_Tapped: " + ex.Message);
            }
        }

        private void lstRequirements_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mRequirements.Count < 10)
                    return;
                if (this.mRequirements.Count == 0)
                    return;

                var lastrequirement = this.mRequirements[this.mRequirements.Count - 1];
                var lastAppearing = (Requirement)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mRequirements.Count() >= totalAspectedRow)
                        {
                            GetActiveRequirements(filterBy, title, isAssending, false);
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private void lstRequirements_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstRequirements.IsRefreshing = true;
                pageNo = 1;
                mRequirements.Clear();
                GetActiveRequirements(filterBy, title, isAssending);
                lstRequirements.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("RequirementsView/lstRequirements_Refreshing: " + ex.Message);
            }
        }

        private void lstRequirements_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstRequirements.SelectedItem = null;
        }

        private async void GrdRequirements_Tapped(object sender, EventArgs e)
        {
            var GridExp = (Grid)sender;
            if (GridExp.IsEnabled)
            {
                try
                {
                    GridExp.IsEnabled = false;
                    var mRequirement = GridExp.BindingContext as Requirement;
                    await Navigation.PushAsync(new RequirementDetailPage(mRequirement.RequirementId));
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("RequirementsView/GrdRequirements_Tapped: " + ex.Message);
                }
                finally
                {
                    GridExp.IsEnabled = true;
                }
            }
        }
        #endregion
        #endregion
    }
}