using Android.Content;
using Android.Content.PM;
using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
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
            {"testot330i","de.testo.ias2015app"},{"testot330","testo.android.reader"},{"testosmartprobes","de.testo.smartprobesapp"}
        };

        List<string> ListAction = new List<string>(
            new string[] { "testot330i", "testot330", "testosmartprobes", "File", "Clipboard" });

        public MainPage()
        {
            InitializeComponent();
            RequestPermissionAsync();

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

        private Channel GetChannelElemJSON(JsonTreeModel root, string key)
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
                ListAction.ElementAt(0), ListAction.ElementAt(1), ListAction.ElementAt(2),
                ListAction.ElementAt(3), ListAction.ElementAt(4));
            switch (action)
            {
                case "File":
                    /*
                    FileData fileData = new FileData();
                    fileData = await CrossFilePicker.Current.PickFile();
                    if (fileData != null)
                    {
                        string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                        await SetXmlToViewAsync(contents);
                    }
                    else
                        await DisplayAlert("Errore", "File non valido", "ok");
                        */

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
                DisplayAlert("Errore", $"Installare l'applicazione {typeApp}", "ok");
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
                    //XmlSerializer serializer = new XmlSerializer(typeof(SingleResult));
                    //StringReader stringReader = new StringReader(xmlStr);
                    //var objRoot = (SingleResult)serializer.Deserialize(stringReader);
                    XmlDocument objRoot = new XmlDocument();
                    objRoot.LoadXml(xmlStr);


                    if (objRoot != null)
                    {
                        //Dictionary<string, object> dictXml = new Dictionary<string, object>();
                        var interventi = new InterventiModel();
                        string ret;
                        if ((ret = GetXmlValue("T_Flue", objRoot)) != null)
                            interventi.INT_SENS_TEMP_FUMI = Convert.ToDouble(ret.Replace(".",","));
                        if ((ret = GetXmlValue("T_Air", objRoot)) != null)
                            interventi.INT_SENS_TEMP_ARIA = Convert.ToDouble(ret);
                        if ((ret = GetXmlValue("O2", objRoot)) != null)
                            interventi.INT_SENS_O2 = Convert.ToDouble(ret);
                        if ((ret = GetXmlValue("Efficiency", objRoot)) != null)
                            interventi.INT_SENS_REND_COMB = Convert.ToDouble(ret);
                        if ((ret = GetXmlValue("Effg", objRoot)) != null)
                            interventi.INT_SENS_REND_COMB = Convert.ToDouble(ret);
                        if ((ret = GetXmlValue("CO_Dil", objRoot)) != null)
                            interventi.INT_SENS_CO_CORRETTO = Convert.ToDouble(ret);
                        if ((ret = GetXmlValue("CO2Max", objRoot)) != null)
                            interventi.INT_SENS_CO2 = Convert.ToDouble(ret);

                        //string CO2 = "";

                        //if (objRoot.properties.First().name.Equals("Fuel"))
                        //    CO2 = objRoot.properties.First().values.First().description.Replace("%", "");

                        //interventi.INT_SENS_REND_MIN =Double.Parse(TF);
                        //interventi.INT_MOD_TERM =Double.Parse(TF);
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
                        var TF = GetChannelElemJSON(objRoot, "T_Flue").values.First().description;
                        var TA = GetChannelElemJSON(objRoot, "T_Air").values.First().description;
                        var O2 = GetChannelElemJSON(objRoot, "O2").values.First().description;
                        var CO = GetChannelElemJSON(objRoot, "CO_Dil").values.First().description;
                        string RC = "";
                        if (GetChannelElemJSON(objRoot, "Efficiency") != null)
                            RC = GetChannelElemJSON(objRoot, "Efficiency").values.First().description;
                        if (GetChannelElemJSON(objRoot, "Effg") != null)
                            RC = GetChannelElemJSON(objRoot, "Effg").values.First().description;
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
                else
                    await DisplayAlert("Errore", "File non valido", "ok");
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

