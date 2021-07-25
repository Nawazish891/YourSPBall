using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace YourSPBall.Interface
{
    public interface IFileService
    {
        string SavePicture(string name, Stream data);
    }
}
