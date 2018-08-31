using Xamarin.Forms;

namespace CrossApp.Views
{
    internal class RilevazioniPage : Page
    {
        private string dataString;
        private string type;

        public RilevazioniPage(string dataString, string type)
        {
            this.dataString = dataString;
            this.type = type;
        }
    }
}