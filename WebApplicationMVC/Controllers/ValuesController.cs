using SmartHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class ValuesController : ApiController
    {
        DeviceDataView deviceDataView = new DeviceDataView(new Views.ViewData.DeviceIconLink());
        public string Put(string id, [FromBody]string textBox)
        {
            deviceDataView = ReadData();
            List<IDevicable> deviceList = deviceDataView.DeviceList;
            IDevicable device = deviceList.Find(dev => dev == deviceDataView.DeviceActive);
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
                            byte.TryParse(textBox, out data);
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
                            int.TryParse(textBox, out data);
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
                            byte.TryParse(textBox, out data);
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
                            byte.TryParse(textBox, out data);
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
                            break;
                        }
                    default:
                        {
                            result = "deviceErrorStateFalse";
                            break;
                        }
                }
            }
            WriteData();
            return result;
        }
        private void WriteData()
        {
            if (deviceDataView != null)
            {
                IWriteble write = new WriteBin();
                string linkFileData = LinkFileData();
                write.Write(deviceDataView, linkFileData);
            }
        }
        private DeviceDataView ReadData()
        {
            IReadable read = new ReadBin();
            string linkFileData = LinkFileData();
            DeviceDataView data = read.ReadDevicesData(linkFileData);
            return data;
        }
        private string LinkFileData()
        {
            return System.Web.Hosting.HostingEnvironment.MapPath("/devicesData.bin");
        }

    }
}
