﻿using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class MasterPage : MasterDetailPage
    {
        static bool IsMasterOpen;
        public MasterPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
            }

            IsMasterOpen = IsPresented;

            IsPresentedChanged += MasterPage_IsPresentedChanged;

            if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            {
                var languageculture = Application.Current.Properties["AppLocale"].ToString();
                if (languageculture == "ar-AE")
                {
                    this.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    this.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            else
            {
                this.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        private void MasterPage_IsPresentedChanged(object sender, System.EventArgs e)
        {
            IsMasterOpen = IsPresented;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<string>(this, "MenuIconClick", (sender) =>
            {
                IsPresented = true;
            });

            MessagingCenter.Subscribe<string>(this, "MenuCloseIconClick", (sender) =>
            {
                IsPresented = false;
            });
            //IsPresented = false;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            MessagingCenter.Unsubscribe<string>(this, "MenuIconClick");
        }

        public static bool IsMasterPageVisible()
        {
            return IsMasterOpen;
        }
    }
}