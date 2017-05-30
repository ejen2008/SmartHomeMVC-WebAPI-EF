﻿using SmartHome;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationMVC.Models;


namespace WebApplicationMVC.Controllers
{
    public class MainController : Controller
    {
        private DeviceContext deviceDb = new DeviceContext();
        //DeviceDataView deviceDataView = new DeviceDataView(new Views.ViewData.DeviceIconLink());
        DeviceDataView deviceDataView;
        Factory factory;
        List<IDevicable> devicesList;

        //
        // GET: /Main/
        [HttpGet]
        public ActionResult Index()
        {
            Console.WriteLine(deviceDb.Devices.Count());
            IEnumerable<DeviceDb> dev = deviceDb.Devices;
            string linkFile = LinkFileData();
            bool fileExist = System.IO.File.Exists(linkFile);
            if (fileExist == false)
            {
                deviceDataView = new DeviceDataView(new Views.ViewData.DeviceIconLink());
                factory = new Factory();
                devicesList = new List<IDevicable>();
                devicesList.Add(factory.CreatorTV("Samsung"));
                devicesList.Add(factory.CreatorSound("Sony"));
                devicesList.Add(factory.CreatorHeater("HotHeater"));
                devicesList.Add(factory.CreatorConditioner("Panasonic"));
                devicesList.Add(factory.CreatorBlower("Dayson"));
                deviceDataView.DeviceList = devicesList;
                //Session["Device"] = deviceDataView;
                //.......................Test.............................
                deviceDataView.DeviceActive = deviceDataView.DeviceList[0];
                //.......................Test.............................
                EventStateDevice();
                WriteData();
            }
            else if (TempData["deviceData"] != null)
            {
                deviceDataView = (DeviceDataView)TempData["deviceData"];
                WriteData();
            }
            else
            {
                deviceDataView = ReadData();
            }

            return View(deviceDataView);
        }
        
        [HttpPost]
        public ActionResult Index(string volume, string current, string temperature, string bass, string buttonSubmit)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            if (deviceDataView.DeviceActive.State == true)
            {
                devicesList = deviceDataView.DeviceList;
                IDevicable device = devicesList.Find(devices => devices == deviceDataView.DeviceActive);
                switch (buttonSubmit)
                {
                    case "volume":
                        {
                            byte valueParam;
                            byte.TryParse(volume, out valueParam);
                            ((IVolumenable)device).Volume = valueParam;
                            break;
                        }
                    case "current":
                        {
                            int valueParam;
                            int.TryParse(current, out valueParam);
                            ((ISwitchable)device).Current = valueParam;
                            break;
                        }
                    case "temperature":
                        {
                            byte valueParam;
                            byte.TryParse(temperature, out valueParam);
                            ((ITemperaturable)device).Temperature = valueParam;
                            break;
                        }
                    case "bass":
                        {
                            byte valueParam;
                            byte.TryParse(bass, out valueParam);
                            ((IBassable)device).BassLevel = valueParam;
                            break;
                        }
                }
                deviceDataView.Message = null;
            }
            else
            {
                deviceDataView.Message = deviceDataView.DeviceActive.Name + " выкл.";
            }
            TempData["deviceData"] = deviceDataView;
            return RedirectToAction("Index");
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
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            return View(deviceDataView);
        }
        [HttpPost]
        public ActionResult CreateDevice(string buttonSubmit, string nameDevice)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            devicesList = deviceDataView.DeviceList;
            factory = new Factory();
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
                deviceDataView.DeviceActive = devicesList.Last<IDevicable>();
                EventStateDevice();
                //WriteData();
                TempData["deviceData"] = deviceDataView;
                return RedirectToAction("Index");
            }
            else
            {
                deviceDataView.Message = "Устройство с таким именем уже имеется, введите другое имя.";
                return View(deviceDataView);
            }
        }

        public ActionResult ActiveDevice(string parameter)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            List<IDevicable> deviceList = deviceDataView.DeviceList;
            IDevicable activDevice = deviceList.Find(device => device.Name == parameter);
            deviceDataView.DeviceActive = activDevice;
            deviceDataView.Message = null;
            //WriteData();
            TempData["deviceData"] = deviceDataView;
            return RedirectToAction("Index");
        }

        public ActionResult DeleteDevice()
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            
            deviceDataView.DeviceList.Remove(deviceDataView.DeviceActive);
            if (deviceDataView.DeviceList.Count > 0)
            {
                deviceDataView.DeviceActive = deviceDataView.DeviceList[0];
            }
            else
            {
                deviceDataView.DeviceActive = null;
            }
            //deviceDataView.DeviceList = devicesList;
            //WriteData();
            TempData["deviceData"] = deviceDataView;
            return RedirectToAction("Index");
        }
        public ActionResult OnOffDevice()
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            IDevicable device = deviceDataView.DeviceActive;
            if (device.State == true)
            {
                device.Off();
            }
            else//false
            {
                device.On();
                deviceDataView.Message = null;

            }
            return RedirectToAction("Index");
        }


        public ActionResult Volume(string parameter)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            IDevicable device = deviceDataView.DeviceActive;
            if (device != null && device.State == true)
            {
                switch (parameter)
                {
                    case "Down":
                        {
                            ((IVolumenable)device).VolumeDown();
                            break;
                        }
                    case "Up":
                        {
                            ((IVolumenable)device).VolumeUp();
                            break;
                        }
                    case "Mute":
                        {
                            ((IVolumenable)device).Volume = 0;
                            break;
                        }
                }
                deviceDataView.Message = null;
            }
            else
            {
                deviceDataView.Message = device.Name + " выкл.";
            }
            return RedirectToAction("Index");
        }
        public ActionResult Chanel(string parameter)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            IDevicable device = deviceDataView.DeviceActive;
            if (device != null && device.State == true)
            {
                switch (parameter)
                {
                    case "Previos":
                        {
                            ((ISwitchable)device).Previous();
                            break;
                        }
                    case "Next":
                        {
                            ((ISwitchable)device).Next();
                            break;
                        }
                }
                deviceDataView.Message = null;
            }
            else
            {
                deviceDataView.Message = device.Name + " выкл.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Temperature(string parameter)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            IDevicable device = deviceDataView.DeviceActive;
            if (device != null && device.State == true)
            {
                switch (parameter)
                {
                    case "Down":
                        {
                            ((ITemperaturable)device).TemperatureDown();
                            break;
                        }
                    case "Up":
                        {
                            ((ITemperaturable)device).TemperatureUp();
                            break;
                        }
                }
                deviceDataView.Message = null;
            }
            else
            {
                deviceDataView.Message = device.Name + " выкл.";
            }
            return RedirectToAction("Index");
        }
        public ActionResult Bass(string parameter)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            IDevicable device = deviceDataView.DeviceActive;
            if (device != null && device.State == true)
            {
                switch (parameter)
                {
                    case "Down":
                        {
                            ((IBassable)device).BassDown();
                            break;
                        }
                    case "Up":
                        {
                            ((IBassable)device).BassUp();
                            break;
                        }
                }
                deviceDataView.Message = null;
            }
            else
            {
                deviceDataView.Message = device.Name + " выкл.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult SpeedAir(string parameter)
        {
            deviceDataView = ReadData();
            //deviceDataView = DeviceData();
            IDevicable device = deviceDataView.DeviceActive;
            if (device != null && device.State == true)
            {
                switch (parameter)
                {
                    case "Low":
                        {
                            ((ISpeedAirable)device).SpeedAirLow();
                            break;
                        }
                    case "Medium":
                        {
                            ((ISpeedAirable)device).SpeedAirMedium();
                            break;
                        }
                    default://Hight
                        {
                            ((ISpeedAirable)device).SpeedAirHight();
                            break;
                        }
                }
                deviceDataView.Message = null;
            }
            else
            {
                deviceDataView.Message = device.Name + " выкл.";
            }
            return RedirectToAction("Index");
        }
        //private DeviceDataView DeviceData()
        //{
        //    return (DeviceDataView)Session["Device"];
        //}

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
            return Server.MapPath("/devicesData.bin");
        }
    }
}