using aptdealzSellerMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        #region Objects
        private List<Report> mReports = new List<Report>();
        #endregion

        #region Constructor
        public ReportPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Method
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindShippingData();
        }

        private void BindShippingData()
        {
            lstReports.ItemsSource = null;

            mReports = new List<Report>()
            {
                new Report
                {
                    Title="Total Sales",
                    Amount=12345
                },
                new Report
                {
                   Title="Uncleared Payments",
                    Amount=1234
                },
                new Report
                {
                    Title="Cleared Payments",
                    Amount=15234
                },
                new Report
                {
                    Title="Total Earninges",
                    Amount=18234
                },
                new Report
                {
                    Title="Grievances Raised",
                    Amount=5
                },
            };

            lstReports.ItemsSource = mReports.ToList();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ImgSearch_Tapped(object sender, EventArgs e)
        {

        }
        #endregion
    }
}