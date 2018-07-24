
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Android.Content;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using System;
using Android.Bluetooth;

namespace CrossApp.Droid
{
    //[IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/pdf")]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/json")]

    [Activity(Label = "CrossApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const string Tag = "MainActivity";

        public static BluetoothSocket BthSocket = null;

        const int RequestResolveError = 1000;

        BluetoothAdapter mBluetoothAdapter;

        protected override void OnCreate(Bundle bundle)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            CrossCurrentActivity.Current.Init(this, bundle);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
            HandlerIntentToJson();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                string dataString = null;
                try
                {
                    var uFile = data.Data;
                    Stream stream = ContentResolver.OpenInputStream(uFile);
                    using (var streamReader = new StreamReader(stream))
                    {
                        dataString = streamReader.ReadToEnd();
                    }
                    var type = ContentResolver.GetType(uFile);
                    ((App)Xamarin.Forms.Application.Current).SendFileData(dataString, type);
                }
                catch (Exception readEx)
                {
                    System.Diagnostics.Debug.Write(readEx);
                }

            }

        }

        public void HandlerIntentToJson()
        {
            Intent intent = Intent;
            string type = intent.Type;
            string jsonString = null;
            if (Intent.Action == Intent.ActionSend)
            {
                if (type.StartsWith("application/"))
                {
                    var key = "android.intent.extra.STREAM";
                    var filePathUri = Intent.GetParcelableExtra(key) as Android.Net.Uri;
                    Stream stream = ContentResolver.OpenInputStream(filePathUri);
                    System.Diagnostics.Debug.Write(filePathUri);

                    using (var streamReader = new StreamReader(stream))
                    {
                        jsonString = streamReader.ReadToEnd();
                    }
                    ((App)Xamarin.Forms.Application.Current).SendFileData(jsonString, type);
                }
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
        // Create a BroadcastReceiver for ACTION_FOUND
//        private BroadcastReceiver mBroadcastReceiver1 = new BroadcastReceiver()
//        {
//            public void onReceive(Context context, Intent intent)
//            {
//                String action = Intent.Action;
//                // When discovery finds a device
//                if (action.Equals(mBluetoothAdapter.ACTION_STATE_CHANGED))
//                {
//                    int state = Intent.GetIntExtra(BluetoothAdapter.ExtraState, mBluetoothAdapter.err);

//                    switch (state)
//                    {
//                        case BluetoothAdapter.STATE_OFF:
//                            Log.d(TAG, "onReceive: STATE OFF");
//                            break;
//                        case BluetoothAdapter.STATE_TURNING_OFF:
//                            Log.d(TAG, "mBroadcastReceiver1: STATE TURNING OFF");
//                            break;
//                        case BluetoothAdapter.STATE_ON:
//                            Log.d(TAG, "mBroadcastReceiver1: STATE ON");
//                            break;
//                        case BluetoothAdapter.STATE_TURNING_ON:
//                            Log.d(TAG, "mBroadcastReceiver1: STATE TURNING ON");
//                            break;
//                    }
//                }
//            };
//        }


//        protected override void onDestroy()
//        {
//            Log.d(TAG, "onDestroy: called.");
//            super.onDestroy();
//            unregisterReceiver(mBroadcastReceiver1);
//        }

//        @Override
//    protected void onCreate(Bundle savedInstanceState)
//        {
//            super.onCreate(savedInstanceState);
//            setContentView(R.layout.activity_main);
//            Button btnONOFF = (Button)findViewById(R.id.btnONOFF);

//            mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter();


//            btnONOFF.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view)
//            {
//                Log.d(TAG, "onClick: enabling/disabling bluetooth.");
//                enableDisableBT();
//            }
//        });

//    }

//    public void enableDisableBT()
//    {
//        if (mBluetoothAdapter == null)
//        {
//            Log.d(TAG, "enableDisableBT: Does not have BT capabilities.");
//        }
//        if (!mBluetoothAdapter.isEnabled())
//        {
//            Log.d(TAG, "enableDisableBT: enabling BT.");
//            Intent enableBTIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
//            startActivity(enableBTIntent);

//            IntentFilter BTIntent = new IntentFilter(BluetoothAdapter.ACTION_STATE_CHANGED);
//            registerReceiver(mBroadcastReceiver1, BTIntent);
//        }
//        if (mBluetoothAdapter.isEnabled())
//        {
//            Log.d(TAG, "enableDisableBT: disabling BT.");
//            mBluetoothAdapter.disable();

//            IntentFilter BTIntent = new IntentFilter(BluetoothAdapter.ACTION_STATE_CHANGED);
//            registerReceiver(mBroadcastReceiver1, BTIntent);
//        }

//    }

//}
