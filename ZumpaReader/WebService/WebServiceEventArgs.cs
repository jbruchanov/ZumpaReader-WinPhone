using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZumpaReader.Model;

namespace ZumpaReader.WebService
{    

    public class WSDownloadEventArgs : EventArgs
    {
        public ZumpaItemsResult Result {get;set;}
    }

    public class WSErrorEventArgs : EventArgs
    {
        public Exception Error {get;set;}
    }
}
