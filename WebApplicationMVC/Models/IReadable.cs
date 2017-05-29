using SmartHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    interface IReadable
    {
        DeviceDataView ReadDevicesData(string nameFile);
    }
}