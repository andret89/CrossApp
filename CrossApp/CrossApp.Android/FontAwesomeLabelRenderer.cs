using Android.Content;
using Android.Graphics;
using CrossApp.Droid;
using CrossApp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FontAwesomeLabel), typeof(FontAwesomeLabelRenderer))]

namespace CrossApp.Droid
{
    public class FontAwesomeLabelRenderer : LabelRenderer
    {
        public FontAwesomeLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets,
                     FontAwesomeLabel.FontAwesomeName + ".otf");
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Text")
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets,
                      FontAwesomeLabel.FontAwesomeName + ".otf");
        }
    }
}