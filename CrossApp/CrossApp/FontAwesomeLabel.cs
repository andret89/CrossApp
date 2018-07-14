using Xamarin.Forms;

namespace CrossApp
{

    public class FontAwesomeLabel : Label
    {
        public static readonly string FontAwesomeName = "FontAwesome";

        //Parameterless constructor for XAML
        public FontAwesomeLabel()
        {
            FontFamily = FontAwesomeName;
        }

        public FontAwesomeLabel(string fontAwesomeLabel = null)
        {
            FontFamily = FontAwesomeName;
            Text = fontAwesomeLabel;
        }

        public static class Icon
        {

            public static readonly string FA_Import = "\uf56f";
            public static readonly string FAImporta = "&#xf56f;";
            public static readonly string FAOpenApp = "&#xf139;";
            public static readonly string FASave = "&#xf019;";

        }
    }
}
