using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.OtherPage;
using aptdealzSellerMobile.Views.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GrievancesPage : ContentPage
    {
        #region Objects
        private List<Grievances> mGrievances = new List<Grievances>();
        #endregion

        #region Constructor
        public GrievancesPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindGrievancesData();
        }

        private void BindGrievancesData()
        {
            mGrievances = new List<Grievances>()
            {
                 new Grievances
                  {
                    GrievanceId="GR#123",
                    GrievanceStatus="Open",
                    Description="Need 5 Canon A210 All In One Printers",
                  },
                  new Grievances
                  {
                    GrievanceId="GR#123",
                    GrievanceStatus="Closed",
                    Description="Need 5 Canon A210 All In One Printers",
                  },
                   new Grievances
                  {
                    GrievanceId="GR#123",
                    GrievanceStatus="Closed",
                    Description="Need 5 Canon A210 All In One Printers",
                  },
                     new Grievances
                  {
                    GrievanceId="GR#123",
                    GrievanceStatus="Closed",
                    Description="Need 5 Canon A210 All In One Printers",
                  },
                       new Grievances
                  {
                    GrievanceId="GR#123",
                    GrievanceStatus="Closed",
                    Description="Need 5 Canon A210 All In One Printers",
                  },
                         new Grievances
                  {
                    GrievanceId="GR#123",
                    GrievanceStatus="Closed",
                    Description="Need 5 Canon A210 All In One Printers",
                  },
            };
            lstGrievances.ItemsSource = mGrievances.ToList();
        }

        void BindList()
        {
            try
            {
                if (mGrievances != null && mGrievances.Count > 0)
                {
                    lstGrievances.IsVisible = true;
                    FrmSortBy.IsVisible = true;
                    //FrmStatusBy.IsVisible = true;
                    FrmSearchBy.IsVisible = true;
                    FrmFilterBy.IsVisible = true;
                    lblNoRecord.IsVisible = false;
                    lstGrievances.ItemsSource = mGrievances.ToList();
                }
                else
                {
                    lstGrievances.IsVisible = false;
                    //FrmStatusBy.IsVisible = false;
                    FrmSearchBy.IsVisible = false;
                    FrmSortBy.IsVisible = false;
                    FrmFilterBy.IsVisible = false;
                    lblNoRecord.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Grievance/BindList: " + ex.Message);
            }
        }

        #endregion

        #region Events
        //private async void FrmStatusBy_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        StatusPopup statusPopup = new StatusPopup();
        //        statusPopup.isRefresh += (s1, e1) =>
        //        {
        //            string result = s1.ToString();
        //            if (!Common.EmptyFiels(result))
        //            {
        //                Bind list as per result
        //            }
        //        };
        //        await PopupNavigation.Instance.PushAsync(statusPopup);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        private void FrmSortBy_Tapped(object sender, EventArgs e)
        {
           
        }

        private async void FrmFilterBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup("", "");
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!Common.EmptyFiels(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("GrievancesPage/FrmSortBy_Tapped: " + ex.Message);
            }
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void lstGrievance_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            lstGrievances.SelectedItem = null;
            Navigation.PushAsync(new GrievanceDetailPage());
        }
        
        private void entrSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!Common.EmptyFiels(entrSearch.Text))
                {
                    var grievanceSearch = mGrievances.Where(x =>
                                                        x.GrievanceId.ToLower().Contains(entrSearch.Text.ToLower())).ToList();
                    if (grievanceSearch != null && grievanceSearch.Count > 0)
                    {
                        lstGrievances.IsVisible = true;
                        FrmSortBy.IsVisible = true;
                        //FrmStatusBy.IsVisible = true;
                        FrmFilterBy.IsVisible = true;
                        lblNoRecord.IsVisible = false;
                        lstGrievances.ItemsSource = grievanceSearch.ToList();
                    }
                    else
                    {
                        lstGrievances.IsVisible = false;
                       // FrmStatusBy.IsVisible = false;
                        FrmSortBy.IsVisible = false;
                        FrmFilterBy.IsVisible = false;
                        lblNoRecord.IsVisible = true;
                    }
                }
                else
                {
                    BindList();
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Grievance/entrSearch_TextChanged: " + ex.Message);
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            try
            {
                entrSearch.Text = string.Empty;
                BindList();
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

       
    }
}