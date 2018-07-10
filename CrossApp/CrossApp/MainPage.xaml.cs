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

        private Channel GetChannelElem(JsonClassModel root, string key)
        {
            Channel ret = null;
            var channels = root.channels;
            foreach (Channel channel in channels)
            {
                if (channel.type.xmlid.Equals(key))
                    ret = channel;
            }
            return ret;
        }

        public async Task SetJsonToViewAsync(string jsonStr)
        {
            bool status = await RequestPermissionAsync();
            if (status)
            {
                if (IsValidJson(jsonStr))
                {
                    var objRoot = JsonConvert.DeserializeObject<JsonClassModel>(jsonStr);

                    if (objRoot != null)
                    {
                        var interventi = new InterventiModel();
                        var TF = GetChannelElem(objRoot, "T_Flue").values.First().value;
                        var TA = GetChannelElem(objRoot, "T_Air").values.First().value;
                        var O2 = GetChannelElem(objRoot, "O2").values.First().value;
                        var CO = GetChannelElem(objRoot, "CO_Dil").values.First().value;
                        var CO2 = GetChannelElem(objRoot, "CO2").values.First().value;
                        var RC = GetChannelElem(objRoot, "Effg").values.First().value;

                        interventi.INT_SENS_TEMP_FUMI = DoubleRound(TF);
                        interventi.INT_SENS_TEMP_ARIA = DoubleRound(TA);
                        interventi.INT_SENS_O2 = DoubleRound(O2);
                        interventi.INT_SENS_CO2 = DoubleRound(CO2);
                        interventi.INT_SENS_CO_CORRETTO = DoubleRound(CO);
                        interventi.INT_SENS_REND_COMB = DoubleRound(RC);
                        //interventi.INT_SENS_REND_MIN =DoubleRound(TF);
                        //interventi.INT_MOD_TERM =DoubleRound(TF);
                        BindingContext = interventi;
                    }
                }
            }
        }

        private double DoubleRound(double value)
        {
            return Math.Round(value, 3);
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

