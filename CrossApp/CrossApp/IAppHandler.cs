﻿using System;

namespace CrossApp
{
    public interface IAppHandler
    {
        void GetFileChoice();
        string GetTextFromClipboard();
        void OpenPDF(string file);
        void DownloadFile(string fileName_, Byte[] document_);
        bool IsAppInstalled(string appName);
        void InstallApplication(string appPackageName);
        void OpenURL(string url);
        void EnableBluetooth();

    }
}
