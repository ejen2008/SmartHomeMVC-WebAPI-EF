using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartHome;

namespace WebApplicationMVC.Models.DevicesDb
{
    public class DeviceDb
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

    }
}