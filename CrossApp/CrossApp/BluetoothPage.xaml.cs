using Plugin.BluetoothLE;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluetoothPage : ContentPage
    {
        public BluetoothPage()
        {
            this.BindingContext = new MyPageViewModel();

            Picker pickerBluetoothDevices = new Picker() { Title = "Select a bth device" };
            pickerBluetoothDevices.SetBinding(Picker.ItemsSourceProperty, "ListOfDevices");
            pickerBluetoothDevices.SetBinding(Picker.SelectedItemProperty, "SelectedBthDevice");
            pickerBluetoothDevices.SetBinding(VisualElement.IsEnabledProperty, "IsPickerEnabled");

            Entry entrySleepTime = new Entry() { Keyboard = Keyboard.Numeric, Placeholder = "Sleep time" };
            entrySleepTime.SetBinding(Entry.TextProperty, "SleepTime");

            Button buttonConnect = new Button() { Text = "Connect", IsEnabled=true };
            buttonConnect.SetBinding(Button.CommandProperty, "ConnectCommand");
            buttonConnect.SetBinding(VisualElement.IsEnabledProperty, "IsConnectEnabled");

            Button buttonDisconnect = new Button() { Text = "Disconnect", IsEnabled = true };
            buttonDisconnect.SetBinding(Button.CommandProperty, "DisconnectCommand");
            buttonDisconnect.SetBinding(VisualElement.IsEnabledProperty, "IsDisconnectEnabled");

            StackLayout slButtons = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { buttonDisconnect, buttonConnect } };

            ListView lv = new ListView();
            lv.SetBinding(ListView.ItemsSourceProperty, "ListOfBarcodes");
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
            lv.ItemTemplate.SetBinding(TextCell.TextProperty, ".");

            int topPadding = 0;
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                topPadding = 20;

            StackLayout sl = new StackLayout { Children = { pickerBluetoothDevices, entrySleepTime, slButtons, lv }, Padding = new Thickness(0, topPadding, 0, 0) };
            Content = sl;
        }


        private void SearchDevice(object sender, System.EventArgs e) { }
        private void DevicesList_OnItemSelected(object sender, System.EventArgs e) { }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
    //        device = DevicesList.SelectedItem as IDevice;

    //        var result = await DisplayAlert("AVISO", "Deseja se conectar a esse dispositivo?", "Conectar", "Cancelar");

    //        if (!result)
    //            return;

    //        //Stop Scanner
    //        await adapter.StopScanningForDevicesAsync();

    //        try
    //        {
    //            await adapter.ConnectToDeviceAsync(device);

    //            await DisplayAlert("Conectado", "Status:" + device.State, "OK");

    //        }
    //        catch (DeviceConnectionException ex)
    //        {
    //            await DisplayAlert("Erro", ex.Message, "OK");
    //        }

    //    }
    //}
}