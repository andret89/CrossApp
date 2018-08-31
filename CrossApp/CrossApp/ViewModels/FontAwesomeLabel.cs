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
        public static readonly string FA_Logout = "\uf2f5";
        public static readonly string FAAllegato = "\uf543";
        public static readonly string FAArrowDown = "\uf063";
        public static readonly string FAArrowUp = "\uf062";
        public static readonly string FABriefcase = "\uf0b1";
        public static readonly string FACalendar = "\uf073";
        public static readonly string FACalculator = "\uf1ec";
        public static readonly string FACar = "\uf1b9";
        public static readonly string FACartArrowDown = "\uf218";
        public static readonly string FACartel = "\uf277";
        public static readonly string FACC = "\uf20a";
        public static readonly string FACheckboxChecked = "\uf14a";
        public static readonly string FACheckboxUnchecked = "\uf0c8";
        public static readonly string FAClock = "\uf017";
        public static readonly string FAComment = "\uf075";
        public static readonly string FACreditCard = "\uf283";
        public static readonly string FADownloadFile = "\uf019";
        public static readonly string FADriverLicense = "\uf2c2";
        public static readonly string FAEmail = "\uf0e0";
        public static readonly string FAEuro = "\uf153";
        public static readonly string FAFumi = "\uf382";
        public static readonly string FAGroup = "\uf0c0";
        public static readonly string FAHandshake = "\uf2b5";
        public static readonly string FAHome = "\uf015";
        public static readonly string FAHourglass = "\uf251";
        public static readonly string FAImport = "\uf56f";
        public static readonly string FALineChart = "\uf201";
        public static readonly string FAMap = "\uf279";
        public static readonly string FAMapMarker = "\uf041";
        public static readonly string FAMapPin = "\uf276";
        public static readonly string FANote = "\uf044";
        public static readonly string FAP = "\uf288";
        public static readonly string FAPercent = "\uf295";
        public static readonly string FAPhone = "\uf095";
        public static readonly string FAPlane = "\uf072";
        public static readonly string FASave = "\uf0c7";
        public static readonly string FAShare = "\uf1e0";
        public static readonly string FASliders = "\uf1de";
        public static readonly string FATag = "\uf02b";
        public static readonly string FAToolbox = "\uf552";
        public static readonly string FAUser = "\uf007";

    }
}
