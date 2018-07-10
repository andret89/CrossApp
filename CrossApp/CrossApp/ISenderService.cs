using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CrossApp
{
    public interface ISenderService
    {
        string SendRequest();
        string OpenFile(FileStream file);
        void GetFileChoice();
        string GetTextFromClipboard();
    }
}
