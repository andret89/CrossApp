using CrossApp.Models;
using Newtonsoft.Json;
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
            RequestPermisisionAsync();
        }

        public MainPage(string jsonStr)
        {
            InitializeComponent();
            RequestPermisisionAsync(jsonStr);
        }

        private async Task RequestPermisisionAsync(string jsonStr=null)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            if (status != PermissionStatus.Granted)
                status = await Utils.CheckPermissions(Permission.Storage);
            if (status == PermissionStatus.Granted)
            {
                if (jsonStr != null)
                {
                    var objDes = GetJson(jsonStr);
                    if(objDes != null)
                    {
                        var interventi = new InterventiModel();
                        Parser(objDes, interventi);
                        BindingContext = interventi;
                    }
                }
            }
        }

        //private async Task<string> PCLStorageSampleAsync()
        //{
        //    //IFolder rootFolder =  IFolder
        //    string path = @"/storage/emulated/0/Android/data/de.testo.ias2015app/files/reports/";
        //    var fileName = "2018-07-06_22-04-23.tjf";
        //    string s = await FileManager.MyRead(path, fileName);
        //    return s;
        //}


        /*
        string path = @"/storage/emulated/0/Android/data/de.testo.ias2015app/files/reports/";
        var fileName = "2018-07-06_22-04-23.tjf";
        var task = FileManager.MyRead(path, fileName);
        */

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

        private JsonClassModel GetJson(string jsonStr)
        {
            if (jsonStr != null )
                return JsonConvert.DeserializeObject<JsonClassModel>(jsonStr);
            else
                return null;
        }

        private void Parser(JsonClassModel objRoot, InterventiModel interventi)
        {

            var TF = getChannelElem(objRoot, "TF").values.First().description;
            var TA = getChannelElem(objRoot, "TA").values.First().description;
            var O2 = getChannelElem(objRoot, "O₂").values.First().description;
            var CO = getChannelElem(objRoot, "CO").values.First().description;
            var CO2 = getChannelElem(objRoot, "CO₂").values.First().description;
            var RC = getChannelElem(objRoot, "Rend").values.First().description;

            interventi.INT_SENS_TEMP_FUMI = Convert.ToDouble(TF);
            interventi.INT_SENS_TEMP_ARIA = Convert.ToDouble(TA);
            interventi.INT_SENS_O2 = Convert.ToDouble(O2);
            interventi.INT_SENS_CO2 = Convert.ToDouble(CO2);
            interventi.INT_SENS_CO_CORRETTO = Convert.ToDouble(CO);
            interventi.INT_SENS_REND_COMB = Convert.ToDouble(RC);
            //interventi.INT_SENS_REND_MIN = Convert.ToDouble(TF);
            //interventi.INT_MOD_TERM = Convert.ToDouble(TF);
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

