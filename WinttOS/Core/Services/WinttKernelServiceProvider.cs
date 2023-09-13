﻿using Cosmos.System.Coroutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Services
{
    public class WinttKernelServiceProvider : IServiceProvider
    {
        #region Interface implemetation

        #region Fields / Variables

        private List<IService> _services = new();

        public List<IService> Services => _services;

        #endregion

        #region Methods

        public void AddService(IService service)
        {
            if(!_services.Contains(service))
                _services.Add(service);
        }

        public void DoServiceProviderTick()
        {
            _services.ForEach(service =>
            {
                if(service.IsRunning)
                    service.onServiceTick();
            });
        }

        public void FinishService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.Name == serviceName)
                {
                    if (service.IsRunning)
                        service.onServiceFinish();
                    return;
                }
            });
        }

        public (ServiceStatus?, string) GetServiceStatus(string serviceName)
        {
            foreach(var service in _services)
            {
                if (service.Name == serviceName)
                    return (service.Status, service.ErrorMessage);
            }
            return (null, null);
        }

        public void RestartService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.Name == serviceName)
                {
                    if (service.IsRunning)
                        service.onServiceFinish();
                    service.onServiceStart();
                    return;
                }
            });
        }

        public void StartService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.Name == serviceName && !service.IsRunning)
                    service.onServiceStart();
            });
        }

        #endregion

        #endregion

        #region Methods

        public void StartAllServices()
        {
            _services.ForEach(service =>
            {
                //StartService(service.Name);
                if(!service.IsRunning)
                    service.onServiceStart();
            });
        }

        public void StopAllServices()
        {
            _services.ForEach(service =>
            {
                if(service.IsRunning)
                    service.onServiceFinish();
            });
        }

        #endregion
    }

    public class ServiceErrorException : Exception { }
}
