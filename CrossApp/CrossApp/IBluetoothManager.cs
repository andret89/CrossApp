using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CrossApp
{
    interface IBluetoothManager
    {
        void EnableBluetooth();

        void DisableBluetooth();

        void RequestDiscoverable();
       
        void Paring();
        void Start(string name, int sleepTime, bool readAsCharArray);
        void Cancel();
        ObservableCollection<string> PairedDevices();
    }
}
