using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Xml.Serialization;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Models
{
    public class ReadBin : IReadable
    {
        //public List<IEmployeeble> Read(string nameFile)
        //{
        //    List<IEmployeeble> employee = null;
        //    BinaryFormatter binFormater = new BinaryFormatter();
        //    using (FileStream employeeData = new FileStream(nameFile, FileMode.OpenOrCreate))
        //    {
        //        if (employeeData.Length != 0)
        //        {
        //            employee = (List<IEmployeeble>)binFormater.Deserialize(employeeData);
        //        }
        //    }
        //    return employee;
        //}

        public DeviceDataView ReadDevicesData(string nameFile)
        {
            DeviceDataView data = null; 
            BinaryFormatter binFormatter = new BinaryFormatter();
            using (FileStream devicesData = new FileStream(nameFile, FileMode.OpenOrCreate))
                if (devicesData.Length != 0)
                {
                    data = (DeviceDataView)binFormatter.Deserialize(devicesData);
                }
                else { }

            return data;
        }

    }
}
