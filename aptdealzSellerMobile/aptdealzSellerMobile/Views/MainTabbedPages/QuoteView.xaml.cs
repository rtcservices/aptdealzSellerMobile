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
    public partial class QuoteView : ContentView
    {
        #region [ Objects ]
        private List<Quote> mQuote;
        private string filterBy = SortByField.Date.ToString();
        private string title = string.Empty;
        private int? statusBy = null;
        private bool? isAssending = false;
        private readonly int pageSize = 10;
        private int pageNo;
        #endregion

        #region [ Constructor ]
        public QuoteView()
        {
            try
            {
                InitializeComponent();
                mQuote = new List<Quote>();
                pageNo = 1;
                GetSubmittedQuotes(statusBy, title, filterBy, isAssending);

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
                Common.DisplayErrorMessage("QuoteView/Ctor: " + ex.Message);
            }
        }
        #endregion

        #region [ Method ]       
        private async void GetSubmittedQuotes(int? StatusBy = null, string Title = "", string FilterBy = "", bool? SortBy = null, bool isLoader = true)
        {
            try
            {
                QuoteAPI quoteAPI = new QuoteAPI();

                if (isLoader)
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                }

                var mResponse = await quoteAPI.GetSubmittedQuotesByMe(StatusBy, Title, FilterBy, SortBy, pageNo, pageSize);
                if (mResponse != null && mResponse.Succeeded)
                {
                    JArray result = (JArray)mResponse.Data;
                    var mQuotes = result.ToObject<List<Quote>>();
                    if (pageNo == 1)
                    {
                        mQuote.Clear();
                    }

                    foreach (var quote in mQuotes)
                    {
                        if (mQuote.Where(x => x.QuoteId == quote.QuoteId).Count() == 0)
                            mQuote.Add(quote);
                    }
                    BindList(mQuote);
                }
                else
                {
                    lstQuotesDetails.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                    //if (mResponse.Message != null)
                    //{
                    //    lblNoRecord.Text = mResponse.Message;
                    //}
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/GetRequirements: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private void BindList(List<Quote> mQuotes)
        {
            try
            {
                if (mQuotes != null && mQuotes.Count > 0)
                {
                    lstQuotesDetails.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstQuotesDetails.ItemsSource = mQuotes.ToList();
                }
                else
                {
                    lstQuotesDetails.ItemsSource = null;
                    lstQuotesDetails.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/BindList: " + ex.Message);
            }
        }
        #endregion

        #region [ Events ]
        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(image: ImgMenu);
                await Navigation.PushAsync(new OtherPage.SettingsPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/ImgMenu_Tapped: " + ex.Message);
            }
        }

        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }

        private async void ImgNotification_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NotificationPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/ImgNotification_Tapped: " + ex.Message);
            }
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("FAQHelp"));
        }

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Utility.Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
        }

        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pageNo = 1;
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    GetSubmittedQuotes(statusBy, entrSearch.Text, filterBy, isAssending, false);
                }
                else
                {
                    pageNo = 1;
                    mQuote.Clear();
                    GetSubmittedQuotes(statusBy, title, filterBy, isAssending);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                entrSearch.Text = string.Empty;
                BindList(mQuote);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/BtnClose_Clicked: " + ex.Message);
            }
        }

        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var ImgASC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_ASC : Constraints.Sort_ASC_Dark;
                var ImgDSC = (Application.Current.UserAppTheme == OSAppTheme.Light) ? Constraints.Sort_DSC : Constraints.Sort_DSC_Dark;

                if (ImgSort.Source.ToString().Replace("File: ", "") == ImgASC)
                {
                    ImgSort.Source = ImgDSC;
                    isAssending = false;
                }
                else
                {
                    ImgSort.Source = ImgASC;
                    isAssending = true;
                }
                pageNo = 1;
                mQuote.Clear();
                GetSubmittedQuotes(statusBy, title, filterBy, isAssending);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var sortby = new FilterPopup(filterBy, "Quote");
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
                        mQuote.Clear();
                        GetSubmittedQuotes(statusBy, title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortby);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/FrmFilterBy_Tapped: " + ex.Message);
            }
        }

        private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                var statusPopup = new StatusPopup(statusBy);
                statusPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        lblStatus.Text = result;
                        statusBy = Common.GetQuoteStatus(result);
                        pageNo = 1;
                        mQuote.Clear();
                        GetSubmittedQuotes(statusBy, title, filterBy, isAssending);
                    }
                };
                await PopupNavigation.Instance.PushAsync(statusPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/FrmStatusBy_Tapped: " + ex.Message);
            }
        }

        private void BtnQuotes_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectGrid = (ImageButton)sender;
                var setHight = (ViewCell)selectGrid.Parent.Parent.Parent;
                if (setHight != null)
                {
                    setHight.ForceUpdateSize();
                }

                var mQuote = (Quote)selectGrid.BindingContext;
                if (mQuote != null && mQuote.ArrowImage == Constraints.Arrow_Right)
                {
                    mQuote.ArrowImage = Constraints.Arrow_Down;
                    mQuote.GridBg = (Application.Current.UserAppTheme == OSAppTheme.Light) ? (Color)App.Current.Resources["appColor8"] : Color.Transparent;
                    mQuote.MoreDetail = true;
                    mQuote.OldDetail = false;
                }
                else
                {
                    mQuote.ArrowImage = Constraints.Arrow_Right;
                    mQuote.GridBg = Color.Transparent;
                    mQuote.MoreDetail = false;
                    mQuote.OldDetail = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/BtnQuotes_Tapped: " + ex.Message);
            }
        }

        private void lstQuotesDetails_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lstQuotesDetails.IsRefreshing = true;
                pageNo = 1;
                mQuote.Clear();
                GetSubmittedQuotes(statusBy, title, filterBy, isAssending);
                lstQuotesDetails.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/lstRequirements_Refreshing: " + ex.Message);
            }
        }

        private void lstQuotesDetails_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (this.mQuote.Count < 10)
                    return;
                if (this.mQuote.Count == 0)
                    return;

                var lastrequirement = this.mQuote[this.mQuote.Count - 1];
                var lastAppearing = (Quote)e.Item;
                if (lastAppearing != null)
                {
                    if (lastrequirement == lastAppearing)
                    {
                        var totalAspectedRow = pageSize * pageNo;
                        pageNo += 1;

                        if (this.mQuote.Count() >= totalAspectedRow)
                        {
                            GetSubmittedQuotes(statusBy, title, filterBy, isAssending, false);
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
                Common.DisplayErrorMessage("QuoteView/ItemAppearing: " + ex.Message);
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void GrdList_Tapped(object sender, EventArgs e)
        {
            var Tab = (Grid)sender;
            try
            {
                var mQuote = Tab.BindingContext as Quote;
                await Navigation.PushAsync(new Dashboard.QuoteDetailsPage(mQuote.QuoteId));
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("QuoteView/GrdList_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}