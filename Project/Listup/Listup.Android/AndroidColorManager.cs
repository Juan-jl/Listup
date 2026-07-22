using Android.OS;
using Android.Views;
using Listup.Interfaces;
using Listup.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidColorManager))]
namespace Listup.Droid
{
    public class AndroidColorManager : IAndroidColorManager
    {
        public void SetStatusBarColor(string hexColor, bool blackIcons)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            var window = activity.Window;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                window.SetStatusBarColor(Android.Graphics.Color.ParseColor(hexColor));
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                var decorView = window.DecorView;
                var flags = (SystemUiFlags)decorView.SystemUiVisibility;

                if (blackIcons)
                    flags |= SystemUiFlags.LightStatusBar;
                else
                    flags &= ~SystemUiFlags.LightStatusBar;

                decorView.SystemUiVisibility = (StatusBarVisibility)flags;
            }
        }

        public void SetNavigationBarColor(string hexNavigationBarColor, bool blackIcons)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            var window = activity.Window;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.SetNavigationBarColor(Android.Graphics.Color.ParseColor(hexNavigationBarColor));
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var decorView = window.DecorView;
                var flags = (SystemUiFlags)decorView.SystemUiVisibility;

                if (blackIcons)
                    flags |= SystemUiFlags.LightNavigationBar;
                else
                    flags &= ~SystemUiFlags.LightNavigationBar;

                decorView.SystemUiVisibility = (StatusBarVisibility)flags;
            }
        }
    }
}