﻿using aptdealzSellerMobile.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MasterData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDataPage : MasterDetailPage
    {
        public MasterDataPage()
        {
            InitializeComponent();
            BindNavigation();
        }

        void BindNavigation()
        {
            try
            {
                Common.MasterData = this;
                Common.MasterData.Master = new MenuPage();

                Common.MasterData.Detail = new NavigationPage(new MainTabbedPages.MainTabbedPage("Home"));
                MasterBehavior = MasterBehavior.Popover;
                Common.MasterData.IsGestureEnabled = true;
                Common.MasterData.IsPresented = false;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MasterDataPage/BindNavigation: " + ex.Message);
            }
        }
    }
}