using System;
using System.Collections.Generic;
using WinttOS.System.Processing;

namespace WinttOS.System.Services
{
    public class WinttServiceProvider : Process, IServiceProvider
    {
        #region Interface implemetation

        #region Fields / Variables

        private List<Service> _services = new();

        public WinttServiceProvider() : base("servprvd", ProcessType.KernelComponent)
        { }

        public List<Service> Services => _services;

        #endregion

        #region Methods

        public void AddService(Service service)
        {
            if (!_services.Contains(service))
            {
                _services.Add(service);

                WinttOS.ProcessManager.RegisterProcess(service);
            }
        }

        public void DoServiceProviderTick()
        {
            _services.ForEach(service =>
            {
                if(service.IsRunning)
                    service.onServiceTick();
            });
        }

        public override void Update()
        {
            DoServiceProviderTick();
        }
        public override void Start()
        {
            base.Start();

            foreach(var service in _services)
            {
                WinttOS.ProcessManager.StartProcess(service.Name);
                service.onServiceStart();
            }
        }

        public override void Stop()
        {
            base.Stop();

            foreach (var service in _services)
            {
                if (service.IsRunning && service.Running)
                {
                    service.onServiceFinish();
                    WinttOS.ProcessManager.StopProcess(service.Name);
                }
            }
        }

        public void FinishService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.Name == serviceName)
                {
                    if (service.IsRunning && service.Running)
                    {
                        service.onServiceFinish();
                        WinttOS.ProcessManager.StopProcess(service.Name);
                    }
                    return;
                }
            });
        }

        public (ServiceStatus, string) GetServiceStatus(string serviceName)
        {
            foreach(var service in _services)
            {
                if (service.Name == serviceName)
                    return (service.Status, service.ErrorMessage);
            }
            return (ServiceStatus.no_data, null);
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
                if (service.Name == serviceName && !service.IsRunning && !service.Running)
                {
                    service.onServiceStart();
                    WinttOS.ProcessManager.StopProcess(service.Name);
                }
            });
        }

        #endregion

        #endregion

        #region Methods

        public void StartAllServices()
        {
            _services.ForEach(service =>
            {
                StartService(service.Name);
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
