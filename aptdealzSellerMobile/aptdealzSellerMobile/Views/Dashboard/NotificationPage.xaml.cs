using aptdealzSellerMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Dashboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPage : ContentPage
    {
        #region Objects
        private List<Notification> mNotifications = new List<Notification>();
        #endregion

        #region Cunstrucot
        public NotificationPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindNotificationData();
        }

        private void BindNotificationData()
        {
            lstNotification.ItemsSource = null;
            mNotifications = new List<Notification>()
            {
                new Notification{ NotificationTitle="New requirement posted REQ#123", NotificationDesc=""},
                new Notification{ NotificationTitle="Quote QUO#121 has been rejected", NotificationDesc=""},
                new Notification{ NotificationTitle="New response received for your grienvance GR#01", NotificationDesc=""},
                new Notification{ NotificationTitle="Received new order: INC#121", NotificationDesc=""},
                new Notification{ NotificationTitle="Your order for INV#121 has been shiped yet.", NotificationDesc=""},

            };
            lstNotification.ItemsSource = mNotifications.ToList();
        }
        #endregion

        #region Events
        private void ImgClose_Tapped(object sender, EventArgs e)
        {

        }

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
        #endregion
    }
}