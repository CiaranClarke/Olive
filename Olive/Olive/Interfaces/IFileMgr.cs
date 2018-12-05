using System;
using System.Collections.Generic;
using System.Text;

namespace Olive.Interfaces
{
    public interface IFileMgr
    {
        string GetDocumentPath();
        string GetImagesPath();
        string GetBase64ImageString(string filename);
    }
}
