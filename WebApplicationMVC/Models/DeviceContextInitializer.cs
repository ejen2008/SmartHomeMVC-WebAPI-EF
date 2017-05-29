using SmartHome;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplicationMVC.Models
{/*DropCreateDatabaseAlways CreateDatabaseIfNotExists*/
    public class DeviceContextInitializer : DropCreateDatabaseAlways<DeviceContext>
    {
        protected override void Seed(DeviceContext context)
        {
            context.Devices.Add(new TVDb { Id = 1, Name = "Samsung",  Volume = 15, State = false, Chanel = 2});
            context.Devices.Add(new SoundDeviceDb { Id = 2, Name = "Sony", Volume = 23, State = false, Channel = 3, Bass = 35 });
            context.Devices.Add(new ConditionerDb { Id = 3, Name = "Panasonic", Temperature = 25, State = false, LevelSpeedAir = Speed.Low });
            context.Devices.Add(new HeaterDb { Id = 4, Name = "HotHeater", Temperature = 26, State = false });
            context.Devices.Add(new BlowerDb { Id = 5, Name = "Dayson", State = false, LevelSpeedAir = Speed.Hight });
            context.SaveChanges();
        }
    }
}