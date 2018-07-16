using Android.Content.PM;
using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Permissions;
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
            {"testot330i","de.testo.ias2015app"},{"testot330","testo.android.reader"},{"testosmartprobes","de.testo.smartprobesapp"}
        };

        List<string> ListAction = new List<string>(
            new string[] { "testot330i", "testot330", "testosmartprobes", "File", "Clipboard" });

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
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
            if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                status = await Utils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Storage);
            if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
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
            string action = await DisplayActionSheet("Importa da", "Cancel", "",
                ListAction.ElementAt(0), ListAction.ElementAt(1), ListAction.ElementAt(2), ListAction.ElementAt(3), ListAction.ElementAt(4));
            switch (action)
            {
                case "File":
                    DependencyService.Get<IAppHandler>().GetFileChoice();
                    break;
                case "Clipboard":
                    var jsonStr = DependencyService.Get<IAppHandler>().GetTextFromClipboard();
                    await SetJsonToViewAsync(jsonStr);
                    break;
                default:
                    EventOpenApp(action);
                    break;

            }
        }

        private void EventSaveData(object sender, EventArgs e)
        {
            //string path = @"/storage/emulated/0/Testo/Prova.pdf";
            string fileName = @"/Testo/prova.pdf";
            DependencyService.Get<IAppHandler>().OpenPDF(fileName);
        }

        private void EventOpenApp(string typeApp)
        {
            if (!DependencyService.Get<IAppHandler>().IsAppInstalled(DictDeviceApp.GetValueOrDefault(typeApp)))
            {
                DisplayAlert("Installare l'app", typeApp, "ok");
                return;
            }


            var appDevice = "";
            var dataRequest = false;
            appDevice = typeApp;

            var application_id = "com.companyname.CrossApp";
            var parameter = "targetapplication=default";
            string url;
            if (dataRequest)
                url = $"{appDevice}+{application_id}" +
                    $"://data?userinfo=parameter&json=base64_encoded_data";
            else
                url = $"{appDevice}://start?userinfo={parameter}," +
                    $"language=it_IT,tutorial=false&bundleid={application_id}";

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Xamarin.Forms.Device.OpenUri(new Uri(url));
            });
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
                        var TF = GetChannelElem(objRoot, "T_Flue").values.First().description;
                        var TA = GetChannelElem(objRoot, "T_Air").values.First().description;
                        var O2 = GetChannelElem(objRoot, "O2").values.First().description;
                        var CO = GetChannelElem(objRoot, "CO_Dil").values.First().description;
                        string RC = "";
                        if (GetChannelElem(objRoot, "Efficiency") != null)
                            RC = GetChannelElem(objRoot, "Efficiency").values.First().description;
                        if (GetChannelElem(objRoot, "Effg") != null)
                            RC = GetChannelElem(objRoot, "Effg").values.First().description;
                        string CO2 = "";
                        if (objRoot.properties.First().name.Equals("Fuel"))
                            CO2 = objRoot.properties.First().values.First().description.Replace("%", "");

                        interventi.INT_SENS_TEMP_FUMI = Double.Parse(TF);
                        interventi.INT_SENS_TEMP_ARIA = Double.Parse(TA);
                        interventi.INT_SENS_O2 = Double.Parse(O2);
                        interventi.INT_SENS_CO2 = Double.Parse(CO2);
                        interventi.INT_SENS_CO_CORRETTO = Double.Parse(CO);
                        interventi.INT_SENS_REND_COMB = Double.Parse(RC);
                        //interventi.INT_SENS_REND_MIN =Double.Parse(TF);
                        //interventi.INT_MOD_TERM =Double.Parse(TF);
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

