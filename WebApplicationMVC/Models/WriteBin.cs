using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Xml.Serialization;

namespace WebApplicationMVC.Models
{
    public class WriteBin : IWriteble
    {
        public void Write(DeviceDataView data, string nameFile)
        {
            BinaryFormatter binFormatter = new BinaryFormatter();

            using (FileStream deviceData = new FileStream(nameFile, FileMode.Create))
            {
                binFormatter.Serialize(deviceData, data);
            }
        }

    }
}   