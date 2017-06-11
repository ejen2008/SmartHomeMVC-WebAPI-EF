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
                deviceDataView.DeviceList = GetAllDevices();
                EventStateDevice();

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

            deviceDataView.DeviceActive = indexActivDevice;
            deviceDataView.Message = null;
            TempData["deviceData"] = deviceDataView;
            return RedirectToAction("Index");
        }

        public ActionResult DeleteDevice(string parameter)// поменять на работу с id устройством............ (int? parameter)
        {
            List <DeviceDb> devicesDbList = deviceDbContext.Devices.ToList();
            DeviceDb activDeviceDb = devicesDbList.Find(dev => dev.Name == parameter);

            if (activDeviceDb != null)
            {
                devicesDbList.Remove(activDeviceDb);
                deviceDbContext.Devices.Remove(activDeviceDb);
                deviceDbContext.SaveChanges();

                MapperDevices mapper = new MapperDevices();
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
     }
}