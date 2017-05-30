using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{
    [Table("Heater")]
    public class HeaterDb: DeviceDb
    {
        public byte Temperature { get; set; }
    }
}