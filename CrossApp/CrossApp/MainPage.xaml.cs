using Android.Content;
using CrossApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
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

        private Channel getChannelElem(JsonTreeModel root, string key)
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

        private JsonTreeModel GetJson(string jsonStr)
        {
            var jsonStrTrim = jsonStr; //.Trim();
            if (jsonStrTrim != null )
                return JsonConvert.DeserializeObject<JsonTreeModel>(jsonStrTrim);
            else
                return null;
        }

        private void Parser(JsonTreeModel objRoot, InterventiModel interventi)
        {

            var TF = getChannelElem(objRoot, "T_Flue").values.First().value;
            var TA = getChannelElem(objRoot, "T_Air").values.First().value;
            var O2 = getChannelElem(objRoot, "O2").values.First().value;
            var CO = getChannelElem(objRoot, "CO_Dil").values.First().value;
            var RC = getChannelElem(objRoot, "Efficiency").values.First().value;
            var CO2 = objRoot.properties.First().values.First().value;

            interventi.INT_SENS_TEMP_FUMI =TF;
            interventi.INT_SENS_TEMP_ARIA = TA;
            interventi.INT_SENS_O2 = O2;
            interventi.INT_SENS_CO2 = CO2;
            interventi.INT_SENS_CO_CORRETTO = CO;
            interventi.INT_SENS_REND_COMB = RC;
            //interventi.INT_SENS_REND_MIN = TF;
            //interventi.INT_MOD_TERM = TF;
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            ToolbarItem toolBarItem = (ToolbarItem)sender;
            if (toolBarItem.Text.Equals("TestoApp"))
            {
               DependencyService.Get<ISenderService>().SendRequest();
            }
            else
            {
                var jsonStr = DependencyService.Get<ISenderService>().GetTextFromClipboard();
                if (IsValidJson(jsonStr))
                {
                    var objDes = GetJson(jsonStr);
                    if (objDes != null)
                    {
                        var interventi = new InterventiModel();
                        Parser(objDes, interventi);
                        BindingContext = interventi;
                    }
                }
                //    RequestPermisisionAsync(jsonStr);

                //OpenFilePickerAsync();
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

