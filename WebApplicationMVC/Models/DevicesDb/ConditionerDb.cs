using SmartHome;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models.DevicesDb
{
    [Table("Conditioner")]
    public class ConditionerDb:DeviceDb
    {
        public byte Temperature { get; set; }
        public Speed LevelSpeedAir { get; set; }
    }
}