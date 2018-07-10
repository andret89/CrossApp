using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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


        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private async Task ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            ToolbarItem toolBarItem = (ToolbarItem)sender;
            if (toolBarItem.Text.Equals("TestoApp"))
            {
                DependencyService.Get<ISenderService>().SendRequest();
            }
            else
            {
                var jsonStr = DependencyService.Get<ISenderService>().GetTextFromClipboard();
                if (!await SetJsonToViewAsync(jsonStr))
                    DependencyService.Get<ISenderService>().GetFileChoice();
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

        private async Task OpenFilePickerAsync()
        {
            try
            {

                FileData filedata = new FileData();
                var crossFileData = CrossFilePicker.Current;
                filedata = await crossFileData.PickFile();
                byte[] data = filedata.DataArray;
                string name = filedata.FileName;
                foreach (byte b in filedata.DataArray)
                {
                    string attachment = b.ToString();
                }

                // the dataarray of the file will be found in filedata.DataArray 
                // file name will be found in filedata.FileName;
                //etc etc.
                //var json = DependencyService.Get<ISenderService>().OpenFile(file);
                //RequestPermisisionAsync(json);


            }
            catch (Exception ex)
            {
                //ExceptionHandler.ShowException(ex.Message);
                Console.WriteLine(ex.Message);
            }
        }
    }

}

