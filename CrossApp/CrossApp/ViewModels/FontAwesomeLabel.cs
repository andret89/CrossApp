using Xamarin.Forms;

namespace CrossApp.ViewModels
{

    public class FontAwesomeLabel : Label
    {
        public static readonly string FontAwesomeName = "FA_Solid";

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
    }
    public static class Icon
    {

        public static readonly string FA_Import = "\uf56f";
        public static readonly string FA_OpenApp = "\uf139";
        public static readonly string FA_Save = "\uf019";

    }
}
