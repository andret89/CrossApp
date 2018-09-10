using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListPage : ContentPage
	{
		public ListPage ()
		{
			InitializeComponent ();
		}
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            ((App)Application.Current).OnLogoutAsync();
        }

        private async Task OnTapIntervento(object sender, EventArgs e)
        {
            await DisplayAlert("clicked","ok","ncancel");
        }
    }
}