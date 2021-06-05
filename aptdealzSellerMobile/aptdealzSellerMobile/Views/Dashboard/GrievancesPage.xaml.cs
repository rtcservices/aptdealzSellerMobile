using aptdealzSellerMobile.Model;
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

        #endregion

        #region Events
        private async void FrmStatus_Tapped(object sender, EventArgs e)
        {
            try
            {
                StatusPopup sortByPopup = new StatusPopup("isGrievanceRequest");
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
                //Common.DisplayErrorMessage("AppointmentsPage/imgAdd_Clicked: " + ex.Message);
            }
        }

        private async void FrmSortBy_Tapped(object sender, EventArgs e)
        {
            try
            {
                SortByPopup sortByPopup = new SortByPopup();
                sortByPopup.isRefresh += (s1, e1) =>
                {
                    string result = s1.ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        //Bind list as per result
                    }
                };
                await PopupNavigation.Instance.PushAsync(sortByPopup);
            }
            catch (Exception ex)
            {
                //Common.DisplayErrorMessage("AppointmentsPage/imgAdd_Clicked: " + ex.Message);
            }
        }

        private async void ImgSearch_Tapped(object sender, EventArgs e)
        {
            try
            {
                SearchPopup searchPopup = new SearchPopup();
                searchPopup.isRefresh += (s1, e1) =>
                {
                    lstGrievances.ItemsSource = mGrievances.ToList();

                };
                await PopupNavigation.Instance.PushAsync(searchPopup);
            }
            catch (Exception ex)
            {

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
        #endregion
    }
}