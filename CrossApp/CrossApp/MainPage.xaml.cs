using Android.Content;
using Android.Content.PM;
using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace CrossApp
{
    public partial class MainPage : ContentPage
    {
        Dictionary<string, string> DictDeviceApp = new Dictionary<string, string>
        {
            {"testot330i","de.testo.ias2015app"},{"testot330","testo.android.reader"}
            //{ "testosmartprobes","de.testo.smartprobesapp"}
        };

        List<string> ListAction = new List<string>(
            new string[] { "testot330i", "testot330", "File", "Clipboard" });

        private IBluetoothLE ble;
        private IAdapter adapter;
        private ObservableCollection<IDevice> deviceList;
        private IDevice device;

        public MainPage()
        {
            InitializeComponent();
            InitBluetooth();
            RequestPermissionAsync();
        }

        private void InitBluetooth()
        {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
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

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async void EventImportJson(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Importa da", "Cancel", "",
                ListAction.ElementAt(0), ListAction.ElementAt(1), ListAction.ElementAt(2),
                ListAction.ElementAt(3));
            switch (action)
            {
                case "File":

                    FileData fileData = new FileData();
                    fileData = await CrossFilePicker.Current.PickFile();
                    if (fileData != null)
                    {
                        string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                        if (IsValidXML(contents))
                            await SetXmlToViewAsync(contents);
                        else
                        {
                            if (IsValidJSON(contents))
                                await SetJsonToViewAsync(contents);
                        }
                    }
                    else
                        await DisplayAlert("Errore", "File non valido", "ok");


                    //DependencyService.Get<IAppHandler>().GetFileChoice();
                    break;
                case "Clipboard":
                    var jsonStr = DependencyService.Get<IAppHandler>().GetTextFromClipboard();
                    await SetJsonToViewAsync(jsonStr);
                    break;
                default:
                    EventOpenAppAsync(action);
                    break;

            }
        }

        private async Task EventSaveDataAsync(object sender, EventArgs e)
        {
            //string path = @"/storage/emulated/0/Testo/Prova.pdf";
            //string fileName = @"/Testo/prova.pdf";
            //DependencyService.Get<IAppHandler>().OpenPDF(fileName);
            //var application_id = "com.companyname.CrossApp";
            //var parameter = "targetapplication=default";
            //var appDevice = "testot330i";
            //var url = $"{appDevice}+{application_id}" +
            //    $"://data?userinfo=parameter&json=base64_encoded_data";
            //DependencyService.Get<IAppHandler>().OpenURL(url);
            if (ble.State == BluetoothState.Off)
                
            {
                deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);
                };
            }
            if(!ble.Adapter.IsScanning)
                await adapter.StartScanningForDevicesAsync();

        }

        private async Task EventOpenAppAsync(string typeApp)
        {
            var appName = DictDeviceApp.GetValueOrDefault(typeApp);
            if (!DependencyService.Get<IAppHandler>().IsAppInstalled(appName))
            {
                if (await DisplayAlert("Errore", $"Installare l'applicazione {typeApp}", "ok", "cancel"))
                    DependencyService.Get<IAppHandler>().InstallApplication(appName);
                return;
            }

            var application_id = "com.companyname.CrossApp";
            var parameter = "targetapplication=default";
            string url = $"{typeApp}://start?userinfo={parameter}," +
                    $"language=it_IT,tutorial=false&bundleid={application_id}";

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Xamarin.Forms.Device.OpenUri(new Uri(url));
            });
        }

        private string GetXmlValue(string key, XmlDocument doc)
        {
            XmlNodeList nl = doc.GetElementsByTagName(key);
            if (nl.Count == 0)
                return null;
            string value = nl[0].InnerXml;
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public async Task<bool> SetXmlToViewAsync(string xmlStr)
        {
            bool status = await RequestPermissionAsync();
            if (status)
            {
                if (IsValidXML(xmlStr))
                {
                    XmlDocument objRoot = new XmlDocument();
                    objRoot.LoadXml(xmlStr);

                    if (objRoot != null)
                    {
                        var interventi = new InterventiModel();
                        string ret;
                        if ((ret = GetXmlValue("T_Flue", objRoot)) != null)
                            interventi.INT_SENS_TEMP_FUMI = Convert.ToDouble(ret.Replace(".",","));
                        if ((ret = GetXmlValue("T_Air", objRoot)) != null)
                            interventi.INT_SENS_TEMP_ARIA = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetXmlValue("O2", objRoot)) != null)
                            interventi.INT_SENS_O2 = Convert.ToDouble(ret.Replace(".", ","));
                        //if ((ret = GetXmlValue("Efficiency", objRoot)) != null)
                        //    interventi.INT_SENS_REND_COMB = Convert.ToDouble(ret.Replace(".", ","));
                        //if ((ret = GetXmlValue("Effg", objRoot)) != null)
                        //    interventi.INT_SENS_REND_COMB = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetXmlValue("CO_Dil", objRoot)) != null)
                            interventi.INT_SENS_CO_CORRETTO = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetXmlValue("CO2", objRoot)) != null)
                            interventi.INT_SENS_CO2 = Convert.ToDouble(ret.Replace(".", ",").Replace("%", ""));
                        if ((ret = GetXmlValue("CO2Max", objRoot)) != null)
                            interventi.INT_SENS_CO2 = Convert.ToDouble(ret.Replace(".", ",").Replace("%",""));

                        BindingContext = interventi;
                        return true;
                    }
                }
                else
                    await DisplayAlert("Errore", "File non valido", "ok");

            }
            return status;
        }

        static bool IsValidXML(string xmlContent)
        {
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                doc.LoadXml(xmlContent);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private string GetJSONValue(string key, JsonTreeModel root)
        {
            string ret = null;
            var channels = root.channels;
            foreach (Channel channel in channels)
            {
                if (channel.type.xmlid.Equals(key,StringComparison.InvariantCultureIgnoreCase |
                    StringComparison.CurrentCultureIgnoreCase))
                    ret = channel.values.First().description;
            }
            if (ret == null)
            {
                var props = root.properties;
                foreach (Value2 value in props.First().values)
                {
                    if (value.name.Equals(key, StringComparison.InvariantCultureIgnoreCase | 
                        StringComparison.CurrentCultureIgnoreCase))
                        ret = value.description;
                }
            }
            return ret;
        }

        public async Task<bool> SetJsonToViewAsync(string jsonStr)
        {
            bool status = await RequestPermissionAsync();
            if (status)
            {
                if (IsValidJSON(jsonStr))
                {
                    var objRoot = JsonConvert.DeserializeObject<JsonTreeModel>(jsonStr);

                    if (objRoot != null)
                    {
                        var interventi = new InterventiModel();
                        string ret;
                        if ((ret = GetJSONValue("T_Flue", objRoot)) != null)
                            interventi.INT_SENS_TEMP_FUMI = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetJSONValue("T_Air", objRoot)) != null)
                            interventi.INT_SENS_TEMP_ARIA = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetJSONValue("O2", objRoot)) != null)
                            interventi.INT_SENS_O2 = Convert.ToDouble(ret.Replace(".", ","));
                        //if ((ret = GetJSONValue("Efficiency", objRoot)) != null)
                        //    interventi.INT_SENS_REND_COMB = Convert.ToDouble(ret.Replace(".", ","));
                        //if ((ret = GetJSONValue("Effg", objRoot)) != null)
                        //    interventi.INT_SENS_REND_COMB = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetJSONValue("CO_Dil", objRoot)) != null)
                            interventi.INT_SENS_CO_CORRETTO = Convert.ToDouble(ret.Replace(".", ","));
                        if ((ret = GetJSONValue("CO2", objRoot)) != null)
                            interventi.INT_SENS_CO2 = Convert.ToDouble(ret.Replace(".", ",").Replace("%", ""));
                        if ((ret = GetJSONValue("CO2Max", objRoot)) != null)
                            interventi.INT_SENS_CO2 = Convert.ToDouble(ret.Replace(".", ",").Replace("%", ""));

                        BindingContext = interventi;
                        return true;
                    }
                }
                else
                    await DisplayAlert("Errore", "File non valido", "ok");
            }
            return false;
        }

        private bool IsValidJSON(string strInput)
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

