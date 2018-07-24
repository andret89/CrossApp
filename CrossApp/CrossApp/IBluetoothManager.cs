using System;
using System.Collections.Generic;
using System.Text;

namespace CrossApp
{
    interface IBluetoothManager
    {
        void EnableBluetooth();

        void DisableBluetooth();

        void RequestDiscoverable();
       
        void Paring();
    }
}
