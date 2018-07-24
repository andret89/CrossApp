using System.Collections;
using Android.Bluetooth;
using Android.Content;
using Android.Widget;
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
            ArrayList list = new ArrayList();
            foreach (BluetoothDevice bt in blue.BondedDevices)
            {
                list.Add(bt.Name);
            }
        }
             
    }
}