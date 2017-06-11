using SmartHome;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplicationMVC.Models.DevicesDb;

namespace WebApplicationMVC.Models
{/*DropCreateDatabaseAlways CreateDatabaseIfNotExists*/
    public class DeviceContextInitializer : CreateDatabaseIfNotExists<DeviceContext>
    {
        protected override void Seed(DeviceContext context)
        {
            context.Devices.Add(new TVDb { Name = "Samsung",  Volume = 15, State = false, Channel = 2});
            context.Devices.Add(new SoundDeviceDb { Name = "Sony", Volume = 23, State = false, Channel = 3, Bass = 35 });
            context.Devices.Add(new ConditionerDb { Name = "Panasonic", Temperature = 25, State = false, LevelSpeedAir = Speed.Low });
            context.Devices.Add(new HeaterDb { Name = "HotHeater", Temperature = 26, State = false });
            context.Devices.Add(new BlowerDb { Name = "Dayson", State = false, LevelSpeedAir = Speed.Hight });
            context.SaveChanges();
        }
    }
}