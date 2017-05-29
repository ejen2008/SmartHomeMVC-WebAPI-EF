using SmartHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    public class BlowerDb: DeviceDb
    {
        public Speed LevelSpeedAir { get; set; }
    }
}