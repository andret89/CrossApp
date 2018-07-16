using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrossApp
{
    public partial class MainPage : ContentPage
    {
        Dictionary<string, string> DictDeviceApp = new Dictionary<string, string>
        {
            {"330i", "testot330i"},{"330","testot330"},{"samrtprobes","testosmartprobes"}
        };


        public MainPage()
        {
            InitializeComponent();
            InitPicker();
            RequestPermissionAsync();
            btnOpenApp.IsEnabled = false;

        }

        public void InitPicker()
        {

            foreach (string elm in DictDeviceApp.Keys)
            {
                DevicePicker.Items.Add(elm);
            }
            DevicePicker.SelectedIndexChanged += (sender, args) =>
            {
                if (DevicePicker.SelectedIndex != -1)
                {
                    var key = DevicePicker.Items[DevicePicker.SelectedIndex];
                    if (DictDeviceApp.TryGetValue(key, out string val))
                    {
                        try
                        {
                            App.Current.Properties.Remove("TYPE_DEVICE");
                        }
                        catch { }
                        App.Current.Properties.Add("TYPE_DEVICE", val);
                        btnOpenApp.IsEnabled = true;
                    }
                }
                else
                    btnOpenApp.IsEnabled = false;
            };
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

        private Channel GetChannelElem(JsonTreeModel root, string key)
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

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void EventImportJson(object sender, EventArgs e)
        {
            var jsonStr = DependencyService.Get<IAppHandler>().GetTextFromClipboard();
            if (!await SetJsonToViewAsync(jsonStr))
                DependencyService.Get<IAppHandler>().GetFileChoice();

        }

        private void EventSaveData(object sender, EventArgs e)
        {
            string path = @"/storage/emulated/0/Download/Prova.pdf";
            DependencyService.Get<IAppHandler>().OpenPDF(path);
            //Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            //{
            //    Xamarin.Forms.Device.OpenUri(new Uri(path));
            //});
        }

        private void EventOpenApp(object sender, EventArgs e)
        {
            var appDevice = "";
            var dataRequest = false;
            if (App.Current.Properties.TryGetValue("TYPE_DEVICE", out object valProp))
            {
                appDevice = (string)valProp;

                var application_id = "com.companyname.CrossApp";
                var parameter = "targetapplication=default";
                string url;
                if (dataRequest)
                    url = $"{appDevice}+{application_id}://data?userinfo=parameter&json=base64_encoded_data";
                else
                    url = $"{appDevice}://start?userinfo={parameter}," +
                        $"language=it_IT,tutorial=false&bundleid={application_id}";

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Forms.Device.OpenUri(new Uri(url));
                });
            }
        }

        public async Task<bool> SetJsonToViewAsync(string jsonStr)
        {
            bool status = await RequestPermissionAsync();
            if (status)
            {
                if (IsValidJson(jsonStr))
                {
                    var objRoot = JsonConvert.DeserializeObject<JsonTreeModel>(jsonStr);

                    if (objRoot != null)
                    {
                        var interventi = new InterventiModel();
                        var TF = GetChannelElem(objRoot, "T_Flue").values.First().value;
                        var TA = GetChannelElem(objRoot, "T_Air").values.First().value;
                        var O2 = GetChannelElem(objRoot, "O2").values.First().value;
                        var CO = GetChannelElem(objRoot, "CO_Dil").values.First().value;
                        double RC = 0;
                        if (GetChannelElem(objRoot, "Efficiency") != null)
                            RC = GetChannelElem(objRoot, "Efficiency").values.First().value;
                        double CO2 = 0;
                        if (objRoot.properties.First().name.Equals("Fuel"))
                            CO2 = objRoot.properties.First().values.First().value;

                        interventi.INT_SENS_TEMP_FUMI = DoubleRound(TF);
                        interventi.INT_SENS_TEMP_ARIA = DoubleRound(TA);
                        interventi.INT_SENS_O2 = DoubleRound(O2);
                        interventi.INT_SENS_CO2 = DoubleRound(CO2);
                        interventi.INT_SENS_CO_CORRETTO = DoubleRound(CO);
                        interventi.INT_SENS_REND_COMB = DoubleRound(RC);
                        //interventi.INT_SENS_REND_MIN =DoubleRound(TF);
                        //interventi.INT_MOD_TERM =DoubleRound(TF);
                        BindingContext = interventi;
                        return true;
                    }
                }
            }
            return false;
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

        private double DoubleRound(double value)
        {
            return Math.Round(value, 3);
        }
    }

}

