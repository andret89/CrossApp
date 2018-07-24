using Android.Content;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace CrossApp
{
    class CustomBLE
    {
        private Application curr;
        private Context _context;
        private IBluetoothLE ble;
        private IAdapter adapter;
        private ObservableCollection<IDevice> deviceList;
        private IDevice device;

        public CustomBLE()
        {
            //_context = context;
            curr = Application.Current;
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();
        }
        public BluetoothState GetStatus() { return ble.State; }

        private async void Scanning()
        {
            try
            {
                deviceList.Clear();
                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);
                };

                //We have to test if the device is scanning 
                if (!ble.Adapter.IsScanning)
                {
                    await adapter.StartScanningForDevicesAsync();

                }
            }
            catch (Exception ex)
            {
                //_context.DisplayAlert("Notice", ex.Message.ToString(), "Error !");
                await curr.MainPage.DisplayAlert("Notice", ex.Message.ToString(), "Error !");
               Debug.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Connect to a specific device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Connect()
        {
            try
            {
                if (device != null)
                {
                    await adapter.ConnectToDeviceAsync(device);

                }
                else
                {
                    await curr.MainPage.DisplayAlert("Notice", "No Device selected !", "OK");
                }
            }
            catch (DeviceConnectionException ex)
            {
                //Could not connect to the device
                await curr.MainPage.DisplayAlert("Notice", ex.Message.ToString(), "OK");
            }
        }

        private async void KnowConnect()
        {

            try
            {
                await adapter.ConnectToKnownDeviceAsync(new Guid("guid"));

            }
            catch (DeviceConnectionException ex)
            {
                //Could not connect to the device
                await curr.MainPage.DisplayAlert("Notice", ex.Message.ToString(), "OK");
            }
        }

        IList<IService> Services;
        IService Service;

        /// <summary>
        /// Get list of services
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetServices()
        {
            Services = await device.GetServicesAsync();
            // Service = await device.GetServiceAsync(Guid.Parse("guid")); 
            //or we call the Guid of selected Device

            Service = await device.GetServiceAsync(device.Id);
        }

        IList<ICharacteristic> Characteristics;
        ICharacteristic Characteristic;
        /// <summary>
        /// Get Characteristics
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Getcharacters()
        {
            var characteristics = await Service.GetCharacteristicsAsync();
            Guid idGuid = Guid.Parse("guid");
            Characteristic = await Service.GetCharacteristicAsync(idGuid);
            //  Characteristic.CanRead
        }

        IDescriptor descriptor;
        IList<IDescriptor> descriptors;

        private async void Descriptors()
        {
            descriptors = await Characteristic.GetDescriptorsAsync();
            descriptor = await Characteristic.GetDescriptorAsync(Guid.Parse("guid"));

        }

        private async void DescRW()
        {
            var bytes = await descriptor.ReadAsync();
            await descriptor.WriteAsync(bytes);
        }

        private async void GetRW()
        {
            var bytes = await Characteristic.ReadAsync();
            await Characteristic.WriteAsync(bytes);
        }

        private async void Update()
        {
            Characteristic.ValueUpdated += (o, args) =>
            {
                var bytes = args.Characteristic.Value;
            };
            await Characteristic.StartUpdatesAsync();
        }

        /// <summary>
        /// Select Items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void lv_ItemSelected()
        //{
        //    if (lv.SelectedItem == null)
        //    {
        //        return;
        //    }
        //    device = lv.SelectedItem as IDevice;
        //}
    }
}
