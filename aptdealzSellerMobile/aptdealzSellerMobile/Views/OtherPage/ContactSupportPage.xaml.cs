using aptdealzSellerMobile.Model;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.OtherPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactSupportPage : ContentPage
    {
        #region Objects
        private List<MessageList> mMessageList = new List<MessageList>();
        #endregion

        #region Constructor
        public ContactSupportPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Method
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindMessages();
        }

        private void BindMessages()
        {
            mMessageList.Clear();
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(30, 30, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 30, 30, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(30, 30, 0, 0),
                MessagePosition = LayoutOptions.EndAndExpand,
                UserName = "Michal Beven",
                Time = "10:57 am",
                IsBuyer = true
            });
            mMessageList.Add(new MessageList()
            {
                Message = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.",
                MessageMargin = new Thickness(0, 30, 30, 0),
                MessagePosition = LayoutOptions.StartAndExpand,
                UserName = "Customer Support",
                Time = "10:57 am",
                IsContact = true
            });
            flvMain.FlowItemsSource = mMessageList.ToList();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void BtnSend_Clicked(object sender, EventArgs e)
        {

        } 
        #endregion
    }
}