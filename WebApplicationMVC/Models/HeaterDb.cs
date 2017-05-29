using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    public class HeaterDb: DeviceDb
    {
        public byte Temperature { get; set; }
    }
}