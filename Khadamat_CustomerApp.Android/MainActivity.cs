﻿using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Firebase;
using HockeyApp.Android;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using Xamarin;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Droid
{
    [Activity(Label = "Khadamat_CustomerApp", Icon = "@drawable/logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        string HOCKEYAPP_APPID = "febab6eb36ed4e3bb55ba976df3d2df4";
        protected override void OnResume()
        {
            base.OnResume();
            CrashManager.Register(this, HOCKEYAPP_APPID);
        }
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Xamarin.Essentials.Platform.Init(this, bundle);
            Forms.SetFlags("CollectionView_Experimental");
            Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            XF.Material.Droid.Material.Init(this, bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            ImageCircleRenderer.Init();
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);
            FirebaseApp.InitializeApp(this);
            GetGalleryPermissions();
            LoadApplication(new App(new AndroidInitializer()));

            CheckForGoogleServices();
            FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public bool CheckForGoogleServices()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(Android.App.Application.Context, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Long);
                }
                else
                {
                    Toast.MakeText(Android.App.Application.Context, " This device does not support Google Play Services ", ToastLength.Long);
                }
                return false;
            }
            return true;
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        const string permissionRS = Manifest.Permission.ReadExternalStorage;
        const string permissionWS = Manifest.Permission.WriteExternalStorage;
        const string permissionCM = Manifest.Permission.Camera;
        const string permissionCL = Manifest.Permission.AccessCoarseLocation;
        const string permissionFL = Manifest.Permission.AccessFineLocation;
        const int RequestLocationId = 0;
        readonly string[] permissions =
        {
           Manifest.Permission.ReadExternalStorage,
           Manifest.Permission.WriteExternalStorage,
           Manifest.Permission.Camera,
           Manifest.Permission.AccessCoarseLocation,
           Manifest.Permission.AccessFineLocation
        };
        public void GetGalleryPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23 && this.CheckSelfPermission(permissionRS) != (int)Permission.Granted)
            {
                this.RequestPermissions(permissions, RequestLocationId);
            }
            if ((int)Build.VERSION.SdkInt >= 23 && this.CheckSelfPermission(permissionWS) != (int)Permission.Granted)
            {
                this.RequestPermissions(permissions, RequestLocationId);
            }
            if ((int)Build.VERSION.SdkInt >= 23 && this.CheckSelfPermission(permissionCM) != (int)Permission.Granted)
            {
                this.RequestPermissions(permissions, RequestLocationId);
            }
            if ((int)Build.VERSION.SdkInt >= 23 && (this.CheckSelfPermission(permissionCL) != (int)Permission.Granted) || this.CheckSelfPermission(permissionFL) != (int)Permission.Granted)
            {
                this.RequestPermissions(permissions, RequestLocationId);
            }
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
