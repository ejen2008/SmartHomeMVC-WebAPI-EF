using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebApplicationMVC.Models
{
    public class DeviceContext:DbContext
    {
        static DeviceContext()
        {
            Database.SetInitializer<DeviceContext>(new DeviceContextInitializer());
        }

        public DbSet<DeviceDb> Devices {get; set;}
    }
}