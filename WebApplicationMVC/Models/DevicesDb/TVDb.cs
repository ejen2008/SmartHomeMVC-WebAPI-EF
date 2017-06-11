﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models.DevicesDb
{
    [Table("TV")]
    public class TVDb:DeviceDb
    {
        public byte Volume { get; set; }

        public int Channel { get; set; }
    }
}