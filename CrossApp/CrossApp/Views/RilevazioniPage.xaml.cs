﻿
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CrossApp.Models;
using CrossApp.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace CrossApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RilevazioniPage : ContentPage
    {
        Dictionary<string, string> DictDeviceAppDroid = new Dictionary<string, string>
        {
            {"testot330i","de.testo.ias2015app"},{"testot330","testo.android.reader"}
            //{ "testosmartprobes","de.testo.smartprobesapp"}
        };
        Dictionary<string, string> DictDeviceAppIOS = new Dictionary<string, string>
        {
            {"testot330","id992414788"},{"testot330i","id1007290554"}

        };
        
        List<string> ListAction = new List<string>(
            new string[] { "testot330i", "testot330", "File", "Clipboard" });

        public RilevazioniPage()
        {
            InitializeComponent();
            RequestPermissionAsync();

        }

        public RilevazioniPage(string data, string type)
        {


            if (type.Equals("text/xml"))
                SetXmlToViewAsync(data);
            else
                 if (type.Equals("application/json"))
                SetJsonToViewAsync(data);

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
            await Navigation.PopAsync(true);
        }

        private async void EventImportJson(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Importa da", "Cancel", null,
                ListAction.ElementAt(0), ListAction.ElementAt(1), ListAction.ElementAt(2),
                ListAction.ElementAt(3));
            switch (action)
            {
                case "File":

                    FileData fileData = null;
                    var dataString = "";
                    switch (Xamarin.Forms.Device.RuntimePlatform)
                    {
                        case Xamarin.Forms.Device.Android:
                            try
                            {
                                fileData = await CrossFilePicker.Current.PickFile();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case Xamarin.Forms.Device.iOS:
                            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(RilevazioniPage)).Assembly;
                            Stream stream = assembly.GetManifestResourceStream("CrossApp.iOS.Resources.Testo_data.xml");

                            using (var reader = new StreamReader(stream))
                            {
                                dataString = reader.ReadToEnd();
                            }
                            break;
                        default:
                            var str = DependencyService.Get<IAppHandler>().FileChoice();
                            fileData = new FileData();
                            break;
                    }
                    if (!String.IsNullOrEmpty(dataString) || fileData != null)
                    {
                        string contents;
                        if (fileData != null)
                            contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                        else
                            contents = dataString;
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
                    break;
                case "Clipboard":
                    var jsonStr = DependencyService.Get<IAppHandler>().GetTextFromClipboard();
                    await SetJsonToViewAsync(jsonStr);
                    break;
                case "Cancel":
                    break;
                default:
                    try
                    {
                        await EventOpenAppAsync(action);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await DisplayAlert("Errore", $"verifica se è presente l'applicazione {action}", "ok");
                    }

                    break;

            }
        }

        private void EventSaveData(object sender, EventArgs e)
        {
            string path = @"/storage/emulated/0/Testo/Prova.pdf";
            string fileName = @"/Testo/prova.pdf";
            DependencyService.Get<IAppHandler>().OpenPDF(fileName);
            //var application_id = "com.companyname.CrossApp";
            //var parameter = "targetapplication=default";
            //var appDevice = "testot330i";
            //var url = $"{appDevice}+{application_id}" +
            //    $"://data?userinfo=parameter&json=base64_encoded_data";
            //DependencyService.Get<IAppHandler>().OpenURL(url);

        }

        private async Task EventOpenAppAsync(string AppName)
        {
            string appIdStrore = null;
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {

                case Xamarin.Forms.Device.Android:
                    appIdStrore = DictDeviceAppDroid.GetValueOrDefault(AppName);
                    if (!DependencyService.Get<IAppHandler>().IsAppInstalled(appIdStrore, null))
                    {
                        if (await DisplayAlert("Errore", $"Installare l'applicazione {AppName}", "ok", "cancel"))
                            DependencyService.Get<IAppHandler>().InstallApplication(appIdStrore, AppName);
                        return;
                    }
                    break;
                case Xamarin.Forms.Device.iOS:
                    appIdStrore = DictDeviceAppDroid.GetValueOrDefault(AppName);
                    break;
                default:
                    break;
            }

            var application_id = App.PackageName; ; //"com.companyname.CrossApp";
            var parameter = "targetapplication=default";

            string url = $"{AppName}://start?userinfo={parameter}," +
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
                            interventi.INT_SENS_TEMP_FUMI = Convert.ToDouble(ret.Replace(".", ","));
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
                            interventi.INT_SENS_CO2 = Convert.ToDouble(ret.Replace(".", ",").Replace("%", ""));

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
                if (channel.type.xmlid.Equals(key, StringComparison.InvariantCultureIgnoreCase |
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    ret = channel.values.First().description;
                    break;
                }
            }
            if (ret == null)
            {
                var props = root.properties;
                foreach (Value2 value in props.First().values)
                {
                    if (value.name.Equals(key, StringComparison.InvariantCultureIgnoreCase |
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        ret = value.description;
                        break;
                    }
                }
            }
            if (ret.Equals("-"))
                ret = null;

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
