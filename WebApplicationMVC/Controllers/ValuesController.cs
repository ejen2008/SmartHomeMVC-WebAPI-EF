using SmartHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplicationMVC.Models;
using WebApplicationMVC.Models.DevicesDb;
using WebApplicationMVC.Models.Mapper;

namespace WebApplicationMVC.Controllers
{
    public class ValuesController : ApiController
    {
        private DeviceContext deviceDbContext = new DeviceContext();
        MapperDevices mapper = new MapperDevices();

        public string Put(string id, [FromBody]string [] parameters)//textBox
        {

            string nameDevice = parameters[0];
            string textBoxValue = null;
            if (parameters.Length == 2)
            {
                textBoxValue = parameters[1];
            }

            List < DeviceDb > devicesDbList = deviceDbContext.Devices.ToList();
            DeviceDb deviceDb = devicesDbList.Find(dev => dev.Name == nameDevice);
            IDevicable device = mapper.GetDeviceModel(deviceDb);

            string result;
            
            if (device.State == true)
            {
                switch (id)
                {
                    case "onOff":
                        {
                            device.Off();
                            result = device.State.ToString();
                            break;
                        }
                    case "volumeDown":
                        {
                            ((IVolumenable)device).VolumeDown();
                            result = ((IVolumenable)device).Volume.ToString();
                            break;
                        }
                    case "volumeUp":
                        {
                            ((IVolumenable)device).VolumeUp();
                            result = ((IVolumenable)device).Volume.ToString();
                            break;
                        }
                    case "volumeMute":
                        {
                            ((IVolumenable)device).Volume = 0;
                            result = "0";
                            break;
                        }
                    case "volume":
                        {
                            byte data;
                            byte.TryParse(textBoxValue, out data);
                            ((IVolumenable)device).Volume = data;
                            result = ((IVolumenable)device).Volume.ToString();
                            break;
                        }
                    case "chanelPrevios":
                        {
                            ((ISwitchable)device).Previous();
                            result = ((ISwitchable)device).Current.ToString();
                            break;
                        }
                    case "chanelNext":
                        {
                            ((ISwitchable)device).Next();
                            result = ((ISwitchable)device).Current.ToString();
                            break;
                        }
                    case "current":
                        {
                            int data;
                            int.TryParse(textBoxValue, out data);
                            ((ISwitchable)device).Current = data;
                            result = ((ISwitchable)device).Current.ToString();
                            break;
                        }
                    case "tempDown":
                        {
                            ((ITemperaturable)device).TemperatureDown();
                            result = ((ITemperaturable)device).Temperature.ToString();
                            break;
                        }
                    case "tempUp":
                        {
                            ((ITemperaturable)device).TemperatureUp();
                            result = ((ITemperaturable)device).Temperature.ToString();
                            break;
                        }
                    case "temperature":
                        {
                            byte data;
                            byte.TryParse(textBoxValue, out data);
                            ((ITemperaturable)device).Temperature = data;
                            result = ((ITemperaturable)device).Temperature.ToString();
                            break;
                        }
                    case "bassDown":
                        {
                            ((IBassable)device).BassDown();
                            result = ((IBassable)device).BassLevel.ToString();
                            break;
                        }
                    case "bassUp":
                        {
                            ((IBassable)device).BassUp();
                            result = ((IBassable)device).BassLevel.ToString();
                            break;
                        }
                    case "bass":
                        {
                            byte data;
                            byte.TryParse(textBoxValue, out data);
                            ((IBassable)device).BassLevel = data;
                            result = ((IBassable)device).BassLevel.ToString();
                            break;
                        }
                    case "speedAirLow":
                        {
                            ((ISpeedAirable)device).SpeedAirLow();
                            result = id;
                            break;
                        }
                    case "speedAirMedium":
                        {
                            ((ISpeedAirable)device).SpeedAirMedium();
                            result = id;
                            break;
                        }
                    case "speedAirHight":
                        {
                            ((ISpeedAirable)device).SpeedAirHight();
                            result = id;
                            break;
                        }
                    default:
                        {
                            result = "неизвестная команда";
                            break;
                        }
                }
            }
            else//false
            {
                switch (id)
                {
                    case "onOff":
                        {
                            device.On();
                            result = device.State.ToString();
                            deviceDb.State = device.State; // я нашел такой выход
                            break;
                        }
                    default:
                        {
                            result = "deviceErrorStateFalse";
                            break;
                        }
                }
            }
            //int idDevice = deviceDb.Id;
            //deviceDb = mapper.GetDeviceDb(device);
            //deviceDb.Id = idDevice;
            
            deviceDbContext.Entry(deviceDb).State = System.Data.Entity.EntityState.Modified;
            deviceDbContext.SaveChanges();

            return result;
        }

        private List<IDevicable> GetAllDevices()
        {
            List<DeviceDb> devicesDbList;
            devicesDbList = deviceDbContext.Devices.ToList();
            
            List<IDevicable> devices = mapper.GetAllDeviceModel(devicesDbList);
            return devices;
        }
    }
}
