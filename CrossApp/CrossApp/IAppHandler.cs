using System;

namespace CrossApp
{
    public interface IAppHandler
    {
        string ReadFile(string file);
        void GetFileChoice();
        string GetTextFromClipboard();
        void OpenPDF(string file);
        void DownloadFile(string fileName_, Byte[] document_);
        bool IsAppInstalled(string appName);
    }
}
