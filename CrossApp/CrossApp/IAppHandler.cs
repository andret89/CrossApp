using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CrossApp
{
    public interface IAppHandler
    {
        string SendRequest();
        string OpenFile(string file);
        void GetFileChoice();
        string GetTextFromClipboard();
        Task<bool> LaunchApp(string uri);
        void OpenPDF(string file);
        void DownloadFile(string fileName_, Byte[] document_);
    }
}
