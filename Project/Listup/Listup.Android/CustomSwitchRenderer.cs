using Android.Content.Res;
using Android.Graphics;
using Listup.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Switch), typeof(CustomSwitchRenderer))]
namespace Listup.Droid
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        public CustomSwitchRenderer(Android.Content.Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var typedValue = new Android.Util.TypedValue();
                Context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorAccent, typedValue, true);
                var accentColor = new Android.Graphics.Color(typedValue.Data);

                Control.ThumbDrawable.SetColorFilter(accentColor, PorterDuff.Mode.SrcIn);
                Control.TrackDrawable.SetColorFilter(accentColor, PorterDuff.Mode.SrcIn);
            }
        }
    }
}