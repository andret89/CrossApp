using CrossApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CrossApp.Models;
using Newtonsoft.Json;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System.IO;
using System.Reflection;
using System.Xml;
using Plugin.ShareFile;
using System.Diagnostics;
using Plugin.Toasts;
using Plugin.LocalNotifications;

namespace CrossApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InterventiPage : ContentPage
    {
        Dictionary<string, string> DictDeviceAppDroid = new Dictionary<string, string>
        {
            {"testot330i","de.testo.ias2015app"},{"testot330","testo.android.reader"}
        };
        Dictionary<string, string> DictDeviceAppIOS = new Dictionary<string, string>
        {
            {"testot330","id992414788"},{"testot330i","id1007290554"}
        };

        List<string> ListAction = new List<string>(
            new string[] { "testot330i", "testot330", "File", "Clipboard" });

        InterventiModel interventi = new InterventiModel();

        public InterventiPage()
        {
            InitializeComponent();
        }

        public InterventiPage(string data, string type)
        {
            InitializeComponent();
            if (type.Equals("text/xml"))
                SetXmlToViewAsync(data);
            else
                if (type.Equals("application/json"))
                SetJsonToViewAsync(data);
            BindingContext = interventi;
        }

        private async void EventImportJson(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Importa da", "Cancel", null,
                ListAction.ElementAt(0), ListAction.ElementAt(1), ListAction.ElementAt(2));
            switch (action)
            {
                case "File":
                    FileData fileData = null;
                    var dataString = "";

#if __IOS__
                /*  
                    var fileName = "Testo_data.xml";
                    var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string filePath = Path.Combine(documentsPath, fileName);
                    dataString = File.ReadAllText(filePath);
                
                */ 
                    var assembly = IntrospectionExtensions.GetTypeInfo(typeof(InterventiPage)).Assembly;
                    Stream stream = assembly.GetManifestResourceStream("CrossApp.iOS.Resources.Testo_data.xml");

                    using (var reader = new StreamReader(stream))
                    {
                        dataString = reader.ReadToEnd();
                    }
            
#elif __ANDROID__
                   
                    try
                    {
                        fileData = await CrossFilePicker.Current.PickFile();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
#else
                    var str = DependencyService.Get<IAppHandler>().FileChoice();
                    fileData = new FileData();
#endif

                    if (!String.IsNullOrEmpty(dataString) || fileData != null)
                    {
                        string contents;
                        if (fileData != null)
                            contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                        else
                            contents = dataString;
                        if (Utils.IsValidXML(contents))
                            await SetXmlToViewAsync(contents);
                        else
                        {
                            if (Utils.IsValidJSON(contents))
                                await SetJsonToViewAsync(contents);
                        }
                        BindingContext = interventi;
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

        private async void EventSaveData(object sender, EventArgs e)
        {
            Plugin.XSnack.CrossXSnack.Current.ShowMessage("Salvataggio su db", 3);
            //CrossLocalNotifications.Current.Show("Info","Salvataggio su db");
            //var notificator = DependencyService.Get<IToastNotificator>();
            new NotImplementedException();

        }

        private async Task EventOpenAppAsync(string AppName)
        {
            string appIdStrore = null;

#if __iOS__
            appIdStrore = DictDeviceAppIOS.GetValueOrDefault(AppName);

#endif
            appIdStrore = DictDeviceAppDroid.GetValueOrDefault(AppName);
#if __ANDROID__
            if (!DependencyService.Get<IAppHandler>().IsAppInstalled(appIdStrore, null))
                if (await DisplayAlert("Errore", $"Installare l'applicazione {AppName}", "ok", "cancel"))
                {
                    DependencyService.Get<IAppHandler>().InstallApplication(appIdStrore, AppName);
                    return;
                }
#endif
            string application_id = App.PackageName; ; //"com.companyname.CrossApp";
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
            bool status = await Utils.RequestPermissionAsync();
            if (status)
            {
                if (Utils.IsValidXML(xmlStr))
                {
                    XmlDocument objRoot = new XmlDocument();
                    objRoot.LoadXml(xmlStr);

                    if (objRoot != null)
                    {
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

                        return true;
                    }
                }
                else
                    await DisplayAlert("Errore", "File non valido", "ok");

            }
            return status;
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
            if (ret != null && ret.Equals("-"))
                ret = null;

            return ret;
        }

        public async Task<bool> SetJsonToViewAsync(string jsonStr)
        {
            bool status = await Utils.RequestPermissionAsync();
            if (status)
            {
                if (Utils.IsValidJSON(jsonStr))
                {
                    var objRoot = JsonConvert.DeserializeObject<JsonTreeModel>(jsonStr);

                    if (objRoot != null)
                    {

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

                        return true;
                    }
                }
                else
                    await DisplayAlert("Errore", "File non valido", "ok");
            }
            return false;
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            ((App)Application.Current).OnLogoutAsync();
        }

        private void OnShareButtonClicked(object sender, EventArgs e)
        {
            ((App)Application.Current).OnShare();
        }
    }
}