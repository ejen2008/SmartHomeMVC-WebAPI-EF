using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models.DevicesDb
{
    [Table("SoundDevice")]
    public class SoundDeviceDb: DeviceDb
    {
        public int Channel { get; set; }

        public byte Volume { get; set; }

        public byte Bass { get; set; }

    }
}