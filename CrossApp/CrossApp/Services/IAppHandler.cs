using System;
using System.Threading.Tasks;

namespace CrossApp.Services
{
    public interface IAppHandler
    {
        string FileChoice();
        string GetTextFromClipboard();
        void OpenPDF(string file);
        void DownloadFile(string fileName_, Byte[] document_);
        bool IsAppInstalled(string packageName, string appName);
        void InstallApplication(string packageName, string appName);
        void OpenURL(string url);

    }
}
