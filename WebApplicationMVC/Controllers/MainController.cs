using SmartHome;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Models;
using AutoMapper;
using WebApplicationMVC.Models.DevicesDb;
using WebApplicationMVC.Models.Mapper;

namespace WebApplicationMVC.Controllers
{
    public class MainController : Controller
    {
        private DeviceContext deviceDbContext = new DeviceContext();
        private DeviceDataView deviceDataView = new DeviceDataView(new Views.ViewData.DeviceIconLink());
        //private DeviceDataView deviceDataView;
        private List<IDevicable> devicesList;


        [HttpGet]
        public ActionResult Index()
        {

            if (TempData["deviceData"] != null)
            {
                deviceDataView = (DeviceDataView)TempData["deviceData"];
            }
            else
            {
                //deviceDataView.DeviceActive = 0;
                //DeviceDb activeDevice = deviceDbContext.Devices.ElementAt(activeDeviceNum).;
                //DeviceDb activeDevices = deviceDbContext.Devices.OfType<TVDb>().ElementAt(activeDeviceNum);

                //DeviceDb activeDeviceDb = deviceDbContext.Devices.Find(0);//выбрать полность с таблицы
                //devicesDb.Select(dev => dev.Id = 0) = activeDeviceDb;
                //devicesDb.ElementAt(0)
                //deviceDbContext.Devices.ElementAt<DeviceDb>(0);
                //deviceDbContext.Devices.Include(dev => dev);

                deviceDataView.DeviceList = GetAllDevices();
                EventStateDevice();
                //deviceDataView.DeviceActive = deviceDataView.DeviceList[0];
                deviceDataView.DeviceActive = 0;
            }

            return View(deviceDataView);
        }

        private void EventStateDevice()
        {
            devicesList = deviceDataView.DeviceList;

            List<IDevicable> heaters = devicesList.FindAll(dev => dev is ITemperaturable);// находим все устройства с интерфейсом ITemperaturable
            heaters.RemoveAll(dev => dev is ISpeedAirable);// удаляем все устройства с интерфейсом ISpeedAirable, остаются устройства с интерфейсом ITemperaturable
            List<IDevicable> conditioners = devicesList.FindAll(dev => dev is ITemperaturable && dev is ISpeedAirable);// находим все устройства с интерфейсом ITemperaturable и ISpeedAirable т.е. кондиционеры

            foreach (IDevicable heater in heaters)
            {
                foreach (IDevicable conditioner in conditioners)
                {
                    conditioner.stateDevice += heater.Off;
                    heater.stateDevice += conditioner.Off;
                }
            }
        }

        [HttpGet]
        public ActionResult CreateDevice()
        {
            deviceDataView.DeviceList = GetAllDevices();
            return View(deviceDataView);
        }

        [HttpPost]
        public ActionResult CreateDevice(string buttonSubmit, string nameDevice)
        {
            deviceDataView.DeviceList = GetAllDevices();
            devicesList = deviceDataView.DeviceList;
            Factory factory = new Factory();

            bool nameDouble = devicesList.Exists(device => device.Name == nameDevice);

            if (string.IsNullOrEmpty(nameDevice) == false && nameDouble == false)
            {
                switch (buttonSubmit)
                {
                    case "TV":
                        {
                            devicesList.Add(factory.CreatorTV(nameDevice));
                            break;
                        }
                    case "SD":
                        {
                            devicesList.Add(factory.CreatorSound(nameDevice));
                            break;
                        }
                    case "condit":
                        {
                            devicesList.Add(factory.CreatorConditioner(nameDevice));
                            break;
                        }
                    case "heater":
                        {
                            devicesList.Add(factory.CreatorHeater(nameDevice));
                            break;
                        }
                    default://blower
                        {
                            devicesList.Add(factory.CreatorBlower(nameDevice));
                            break;
                        }
                }
                deviceDataView.Message = null;
                //deviceDataView.DeviceActive = devicesList.Last<IDevicable>();
                deviceDataView.DeviceActive = devicesList.Count - 1;
                EventStateDevice();

                MapperDevices mapper = new MapperDevices();
                DeviceDb deviceDb = mapper.GetDeviceDb(devicesList.Last<IDevicable>());
                deviceDbContext.Devices.Add(deviceDb);
                deviceDbContext.SaveChanges();

                TempData["deviceData"] = deviceDataView;
                return RedirectToAction("Index");
            }
            else
            {
                deviceDataView.Message = "Устройство с таким именем уже имеется, введите другое имя.";
                return View(deviceDataView);
            }
        }


        public ActionResult ActiveDevice(string parameter) // поменять на работу с id устройством
        {
            deviceDataView.DeviceList = GetAllDevices();
            devicesList = deviceDataView.DeviceList;
            IDevicable activDevice = devicesList.Find(device => device.Name == parameter);
            int indexActivDevice = devicesList.IndexOf(activDevice);
            //IDevicable activDevice = devicesList.Find(device => device.Name.ToLower() == parameter.ToLower());

            deviceDataView.DeviceActive = indexActivDevice;
            deviceDataView.Message = null;
            TempData["deviceData"] = deviceDataView;
            return RedirectToAction("Index");
        }

        public ActionResult DeleteDevice(string parameter)// поменять на работу с id устройством............ (int? parameter)
        {
            //DeviceDb device;
            //if (parameter != null && )
            //{
            //device = deviceDbContext.Devices.Find(parametr);
            //}
            //if (device != null)
            //{
            //    deviceDbContext.Devices.Remove(parameter);
            //}

            

            //IDevicable activDevice = devicesList.Find(device => device.Name == parameter);
            //string Name = parameter;
            List <DeviceDb> devicesDbList = deviceDbContext.Devices.ToList();
            DeviceDb activDeviceDb = devicesDbList.Find(dev => dev.Name == parameter);
            //DeviceDb deviceDb = deviceDbContext.Devices.Find(Name);
            

            if (activDeviceDb != null)
            {
                devicesDbList.Remove(activDeviceDb);
                deviceDbContext.Devices.Remove(activDeviceDb);
                deviceDbContext.SaveChanges();

                MapperDevices mapper = new MapperDevices();
                //DeviceDb deviceDb = mapper.GetDeviceDb(activDevice);


                deviceDataView.DeviceList = mapper.GetAllDeviceModel(devicesDbList);
            }
            //
            if (deviceDataView.DeviceList.Count > 0)
            {
                deviceDataView.DeviceActive = 0;
            }
            else
            {
                deviceDataView.DeviceActive = null;
            }

            TempData["deviceData"] = deviceDataView;
            return RedirectToAction("Index");
        }

        private List<IDevicable> GetAllDevices()
        {
            IEnumerable<DeviceDb> devicesDbList = deviceDbContext.Devices.ToList();
            MapperDevices mapper = new MapperDevices();
            List<IDevicable> devices = mapper.GetAllDeviceModel(devicesDbList);
            return devices;
        }
        //public ActionResult OnOffDevice()
        //{
        //    deviceDataView = ReadData();
        //    //deviceDataView = DeviceData();
        //    IDevicable device = deviceDataView.DeviceActive;
        //    if (device.State == true)
        //    {
        //        device.Off();
        //    }
        //    else//false
        //    {
        //        device.On();
        //        deviceDataView.Message = null;

        //    }
        //    return RedirectToAction("Index");
        //}


        //public ActionResult Volume(string parameter)
        //{
        //    deviceDataView = ReadData();
        //    //deviceDataView = DeviceData();
        //    IDevicable device = deviceDataView.DeviceActive;
        //    if (device != null && device.State == true)
        //    {
        //        switch (parameter)
        //        {
        //            case "Down":
        //                {
        //                    ((IVolumenable)device).VolumeDown();
        //                    break;
        //                }
        //            case "Up":
        //                {
        //                    ((IVolumenable)device).VolumeUp();
        //                    break;
        //                }
        //            case "Mute":
        //                {
        //                    ((IVolumenable)device).Volume = 0;
        //                    break;
        //                }
        //        }
        //        deviceDataView.Message = null;
        //    }
        //    else
        //    {
        //        deviceDataView.Message = device.Name + " выкл.";
        //    }
        //    return RedirectToAction("Index");
        //}
        //public ActionResult Chanel(string parameter)
        //{
        //    deviceDataView = ReadData();
        //    //deviceDataView = DeviceData();
        //    IDevicable device = deviceDataView.DeviceActive;
        //    if (device != null && device.State == true)
        //    {
        //        switch (parameter)
        //        {
        //            case "Previos":
        //                {
        //                    ((ISwitchable)device).Previous();
        //                    break;
        //                }
        //            case "Next":
        //                {
        //                    ((ISwitchable)device).Next();
        //                    break;
        //                }
        //        }
        //        deviceDataView.Message = null;
        //    }
        //    else
        //    {
        //        deviceDataView.Message = device.Name + " выкл.";
        //    }
        //    return RedirectToAction("Index");
        //}

        //public ActionResult Temperature(string parameter)
        //{
        //    deviceDataView = ReadData();
        //    //deviceDataView = DeviceData();
        //    IDevicable device = deviceDataView.DeviceActive;
        //    if (device != null && device.State == true)
        //    {
        //        switch (parameter)
        //        {
        //            case "Down":
        //                {
        //                    ((ITemperaturable)device).TemperatureDown();
        //                    break;
        //                }
        //            case "Up":
        //                {
        //                    ((ITemperaturable)device).TemperatureUp();
        //                    break;
        //                }
        //        }
        //        deviceDataView.Message = null;
        //    }
        //    else
        //    {
        //        deviceDataView.Message = device.Name + " выкл.";
        //    }
        //    return RedirectToAction("Index");
        //}
        //public ActionResult Bass(string parameter)
        //{
        //    deviceDataView = ReadData();
        //    //deviceDataView = DeviceData();
        //    IDevicable device = deviceDataView.DeviceActive;
        //    if (device != null && device.State == true)
        //    {
        //        switch (parameter)
        //        {
        //            case "Down":
        //                {
        //                    ((IBassable)device).BassDown();
        //                    break;
        //                }
        //            case "Up":
        //                {
        //                    ((IBassable)device).BassUp();
        //                    break;
        //                }
        //        }
        //        deviceDataView.Message = null;
        //    }
        //    else
        //    {
        //        deviceDataView.Message = device.Name + " выкл.";
        //    }
        //    return RedirectToAction("Index");
        //}

        //public ActionResult SpeedAir(string parameter)
        //{
        //    deviceDataView = ReadData();
        //    //deviceDataView = DeviceData();
        //    IDevicable device = deviceDataView.DeviceActive;
        //    if (device != null && device.State == true)
        //    {
        //        switch (parameter)
        //        {
        //            case "Low":
        //                {
        //                    ((ISpeedAirable)device).SpeedAirLow();
        //                    break;
        //                }
        //            case "Medium":
        //                {
        //                    ((ISpeedAirable)device).SpeedAirMedium();
        //                    break;
        //                }
        //            default://Hight
        //                {
        //                    ((ISpeedAirable)device).SpeedAirHight();
        //                    break;
        //                }
        //        }
        //        deviceDataView.Message = null;
        //    }
        //    else
        //    {
        //        deviceDataView.Message = device.Name + " выкл.";
        //    }
        //    return RedirectToAction("Index");
        //}
        ////private DeviceDataView DeviceData()
        ////{
        ////    return (DeviceDataView)Session["Device"];
        ////}

        //private void WriteData()
        //{
        //    if (deviceDataView != null)
        //    {
        //        IWriteble write = new WriteBin();
        //        string linkFileData = LinkFileData();
        //        write.Write(deviceDataView, linkFileData);
        //    }
        //}
        //private DeviceDataView ReadData()
        //{
        //    IReadable read = new ReadBin();
        //    string linkFileData = LinkFileData();
        //    DeviceDataView data = read.ReadDevicesData(linkFileData);
        //    return data;
        //}
        //private string LinkFileData()
        //{
        //    return Server.MapPath("/devicesData.bin");
        //}
    }
}