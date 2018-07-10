using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrossApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            RequestPermissionAsync();
        }


        private async Task<bool> RequestPermissionAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
                status = await Utils.CheckPermissions(Permission.Storage);
            if (status == PermissionStatus.Granted)
                return true;
            return false;
        }

        private Channel getChannelElem(JsonClassModel root, string key)
        {
            Channel ret = null;
            var channels = root.channels;
            foreach (Channel channel in channels)
            {
                if (channel.type.description.Equals(key))
                    ret = channel;
            }
            return ret;
        }

        public async Task SetJsonToViewAsync(string jsonStr)
        {
            bool status = await RequestPermissionAsync();
            if (status) {
                if (IsValidJson(jsonStr))
                {
                    var objRoot = JsonConvert.DeserializeObject<JsonClassModel>(jsonStr);

                    if (objRoot != null)
                    {
                        var interventi = new InterventiModel();
                        var TF = getChannelElem(objRoot, "TF").values.First().value;
                        var TA = getChannelElem(objRoot, "TA").values.First().value;
                        var O2 = getChannelElem(objRoot, "O₂").values.First().value;
                        var CO = getChannelElem(objRoot, "CO").values.First().value;
                        var CO2 = getChannelElem(objRoot, "CO₂").values.First().value;
                        var RC = getChannelElem(objRoot, "Rend").values.First().value;

                        interventi.INT_SENS_TEMP_FUMI = TF;
                        interventi.INT_SENS_TEMP_ARIA = TA;
                        interventi.INT_SENS_O2 = O2;
                        interventi.INT_SENS_CO2 = CO2;
                        interventi.INT_SENS_CO_CORRETTO = CO;
                        interventi.INT_SENS_REND_COMB = RC;
                        //interventi.INT_SENS_REND_MIN =TF;
                        //interventi.INT_MOD_TERM =TF;
                        BindingContext = interventi;
                    }
                }
            }
        }

        private bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (Exception) //some other exception
                { }
            }
            return false;
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ISenderService>().sendRequest();
        }
    }

}

