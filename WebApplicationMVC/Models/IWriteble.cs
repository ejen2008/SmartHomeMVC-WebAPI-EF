using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    interface IWriteble
    {
        void Write(DeviceDataView data, string nameFile);
    }
}