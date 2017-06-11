using SmartHome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplicationMVC.Models.DevicesDb;

namespace WebApplicationMVC.Models.Mapper
{
    public class MapperDevices
    {
        private Factory factory = new Factory();

        public DeviceDb GetDeviceDb(IDevicable device)
        {
            DeviceDb deviceDb = null;
            if (device is IVolumenable && device is ISwitchable)
            {
                deviceDb = new TVDb { Channel = ((ISwitchable)device).Current, Volume = ((IVolumenable)device).Volume };
            }
            else if (device is IVolumenable && device is ISwitchable && device is IBassable)
            {
                deviceDb = new SoundDeviceDb { Channel = ((ISwitchable)device).Current, Volume = ((IVolumenable)device).Volume, Bass = ((IBassable)device).BassLevel };
            }
            else if (device is ITemperaturable && device is ISpeedAirable)
            {
                deviceDb = new ConditionerDb { Temperature = ((ITemperaturable)device).Temperature, LevelSpeedAir = ((ISpeedAirable)device).LevelSpeed };
            }
            else if (device is ITemperaturable)
            {
                deviceDb = new HeaterDb { Temperature = ((ITemperaturable)device).Temperature };
            }
            else if (device is ISpeedAirable)
            {
                deviceDb = new BlowerDb { LevelSpeedAir = ((ISpeedAirable)device).LevelSpeed };
            }
            else
            {
            }
            deviceDb.Name = device.Name;
            deviceDb.State = device.State;
            return deviceDb;
        }

        public IDevicable GetDeviceModel(DeviceDb deviceDb)
        {
            IDevicable device = null;

            if (deviceDb is TVDb)
            {
                device = factory.CreatorTV(deviceDb.Name);
                ((ISwitchable)device).Current = ((TVDb)deviceDb).Channel;
                ((IVolumenable)device).Volume = ((TVDb)deviceDb).Volume;
            }
            else if (deviceDb is SoundDeviceDb)
            {
                device = factory.CreatorSound(deviceDb.Name);
                ((ISwitchable)device).Current = ((SoundDeviceDb)deviceDb).Channel;
                ((IVolumenable)device).Volume = ((SoundDeviceDb)deviceDb).Volume;
            }
            else if (deviceDb is ConditionerDb)
            {
                device = factory.CreatorConditioner(deviceDb.Name);
                ((ITemperaturable)device).Temperature= ((ConditionerDb)deviceDb).Temperature;

                if (((ConditionerDb)deviceDb).LevelSpeedAir == Speed.Low)
                {
                    ((ISpeedAirable)device).SpeedAirLow();

                }
                else if (((ConditionerDb)deviceDb).LevelSpeedAir == Speed.Medium)
                {
                    ((ISpeedAirable)device).SpeedAirMedium();
                }
                else
                {
                    ((ISpeedAirable)device).SpeedAirHight();
                }
            }
            else if (deviceDb is HeaterDb)
            {
                device = factory.CreatorHeater(deviceDb.Name);
                ((ITemperaturable)device).Temperature = ((HeaterDb)deviceDb).Temperature;
            }
            else if (deviceDb is BlowerDb)
            {
                device = factory.CreatorBlower(deviceDb.Name);
                if (((BlowerDb)deviceDb).LevelSpeedAir == Speed.Low)
                {
                    ((ISpeedAirable)device).SpeedAirLow();
                }
                else if (((BlowerDb)deviceDb).LevelSpeedAir == Speed.Medium)
                {
                    ((ISpeedAirable)device).SpeedAirMedium();
                }
                else
                {
                    ((ISpeedAirable)device).SpeedAirHight();
                }
            }
            else
            {
            }
            device.State = deviceDb.State;
            
            return device;
        }

        public List<DeviceDb> GetAllDeviceDb(IEnumerable<IDevicable> devices)
        {
            List<DeviceDb> devicesDb = new List<DeviceDb>();
            foreach (IDevicable device in devices)
            {
                devicesDb.Add(GetDeviceDb(device));
            }

            return devicesDb;
        }

        public List<IDevicable> GetAllDeviceModel(IEnumerable<DeviceDb> devicesDb)
        {
            List<IDevicable> devicesModel = new List<IDevicable>();
            foreach (DeviceDb deviceDb in devicesDb)
            {
                devicesModel.Add(GetDeviceModel(deviceDb));
            }
            return devicesModel;
        }


    }
}