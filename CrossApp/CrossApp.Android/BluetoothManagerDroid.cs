using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Content;
using Android.Widget;
using Java.IO;
using Java.Util;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(CrossApp.Droid.BluetoothManagerDroid))]

namespace CrossApp.Droid
{

    class BluetoothManagerDroid : IBluetoothManager
    {
        Context context = Android.App.Application.Context;
        BluetoothAdapter blue = BluetoothAdapter.DefaultAdapter;
       
        ArrayList list_Of_Devices;           // list view for paired devices
        EditText bluetoothName;            // user bluetooth name edit text

        public void EnableBluetooth()
        {

            if (!blue.Enable())
            {
                Intent o = new Intent(BluetoothAdapter.ActionRequestEnable);
                var activity = CrossCurrentActivity.Current.Activity;
                activity.StartActivityForResult(o, 0);
                Toast.MakeText(context, "Enabled", ToastLength.Short).Show();
            }

        }

        public void DisableBluetooth()
        {
            blue.Disable();
            Toast.MakeText(context, "Bluetooth Disabled", ToastLength.Short).Show();

        }

        public void RequestDiscoverable()
        {
            Intent visible = new Intent(BluetoothAdapter.ActionRequestDiscoverable);
            var activity = CrossCurrentActivity.Current.Activity;
            activity.StartActivityForResult(visible, 0);
        }
               
        public void Paring()
        {
            Java.Util.ArrayList list = new ArrayList();
            foreach (BluetoothDevice bt in blue.BondedDevices)
            {
                list.Add(bt.Name);
            }
        }

		private CancellationTokenSource _ct { get; set; }

        const int RequestResolveError = 1000;


        #region IBth implementation

        /// <summary>
        /// Start the "reading" loop 
        /// </summary>
        /// <param name="name">Name of the paired bluetooth device (also a part of the name)</param>
        public void Start(string name, int sleepTime = 200, bool readAsCharArray = false)
        {

            Task.Run(async () => loop(name, sleepTime, readAsCharArray));
        }



        private async Task loop(string name, int sleepTime, bool readAsCharArray)
        {
            BluetoothDevice device = null;
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            BluetoothSocket BthSocket = null;

            //Thread.Sleep(1000);
            _ct = new CancellationTokenSource();
            while (_ct.IsCancellationRequested == false)
            {

                try
                {
                    Thread.Sleep(sleepTime);

                    adapter = BluetoothAdapter.DefaultAdapter;

                    if (adapter == null)
                        System.Diagnostics.Debug.WriteLine("No Bluetooth adapter found.");
                    else
                        System.Diagnostics.Debug.WriteLine("Adapter found!!");

                    if (!adapter.IsEnabled)
                        System.Diagnostics.Debug.WriteLine("Bluetooth adapter is not enabled.");
                    else
                        System.Diagnostics.Debug.WriteLine("Adapter enabled!");

                    System.Diagnostics.Debug.WriteLine("Try to connect to " + name);

                    foreach (var bd in adapter.BondedDevices)
                    {
                        System.Diagnostics.Debug.WriteLine("Paired devices found: " + bd.Name.ToUpper());
                        if (bd.Name.ToUpper().IndexOf(name.ToUpper()) >= 0)
                        {

                            System.Diagnostics.Debug.WriteLine("Found " + bd.Name + ". Try to connect with it!");
                            device = bd;
                            break;
                        }
                    }

                    if (device == null)
                        System.Diagnostics.Debug.WriteLine("Named device not found.");
                    else
                    {
                        UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                        if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                            BthSocket = device.CreateInsecureRfcommSocketToServiceRecord(uuid);
                        else
                            BthSocket = device.CreateRfcommSocketToServiceRecord(uuid);

                        if (BthSocket != null)
                        {


                            //Task.Run ((Func<Task>)loop); /*) => {
                            await BthSocket.ConnectAsync();


                            if (BthSocket.IsConnected)
                            {
                                System.Diagnostics.Debug.WriteLine("Connected!");
                                var mReader = new InputStreamReader(BthSocket.InputStream);
                                var buffer = new BufferedReader(mReader);
                                //buffer.re
                                while (_ct.IsCancellationRequested == false)
                                {
                                    if (buffer.Ready())
                                    {
                                        //										string barcode =  buffer
                                        //string barcode = buffer.

                                        //string barcode = await buffer.ReadLineAsync();
                                        char[] chr = new char[100];
                                        //await buffer.ReadAsync(chr);
                                        string barcode = "";
                                        if (readAsCharArray)
                                        {

                                            await buffer.ReadAsync(chr);
                                            foreach (char c in chr)
                                            {

                                                if (c == '\0')
                                                    break;
                                                barcode += c;
                                            }

                                        }
                                        else
                                            barcode = await buffer.ReadLineAsync();

                                        if (barcode.Length > 0)
                                        {
                                            System.Diagnostics.Debug.WriteLine("Letto: " + barcode);
                                            Xamarin.Forms.MessagingCenter.Send<App, string>((App)Xamarin.Forms.Application.Current, "Barcode", barcode);
                                        }
                                        else
                                            System.Diagnostics.Debug.WriteLine("No data");

                                    }
                                    else
                                        System.Diagnostics.Debug.WriteLine("No data to read");

                                    // A little stop to the uneverending thread...
                                    System.Threading.Thread.Sleep(sleepTime);
                                    if (!BthSocket.IsConnected)
                                    {
                                        System.Diagnostics.Debug.WriteLine("BthSocket.IsConnected = false, Throw exception");
                                        throw new Exception();
                                    }
                                }

                                System.Diagnostics.Debug.WriteLine("Exit the inner loop");

                            }
                        }
                        else
                            System.Diagnostics.Debug.WriteLine("BthSocket = null");

                    }


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
                }

                finally
                {
                    if (BthSocket != null)
                        BthSocket.Close();
                    device = null;
                    adapter = null;
                }
            }

            System.Diagnostics.Debug.WriteLine("Exit the external loop");
        }

        /// <summary>
        /// Cancel the Reading loop
        /// </summary>
        /// <returns><c>true</c> if this instance cancel ; otherwise, <c>false</c>.</returns>
        public void Cancel()
        {
            if (_ct != null)
            {
                System.Diagnostics.Debug.WriteLine("Send a cancel to task!");
                _ct.Cancel();
            }
        }

        public ObservableCollection<string> PairedDevices()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }


        #endregion
    }
}