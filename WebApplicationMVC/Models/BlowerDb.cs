using SmartHome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    [Table("Blower")]
    public class BlowerDb: DeviceDb
    {
        public Speed LevelSpeedAir { get; set; }
    }
}