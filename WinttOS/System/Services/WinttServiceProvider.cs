using System;
using System.Collections.Generic;
using WinttOS.System.Processing;

namespace WinttOS.System.Services
{
    public class WinttServiceProvider : Process, IServiceProvider
    {
        #region Interface implemetation

        #region Fields / Variables

        private List<Service> services = new();

        public WinttServiceProvider() : base("servprvd", ProcessType.KernelComponent)
        { }

        public List<Service> Services => services;

        #endregion

        #region Methods

        public void AddService(Service service)
        {
            if (!services.Contains(service))
            {
                services.Add(service);

                service.IsProcessCritical = true;
                WinttOS.ProcessManager.RegisterProcess(service);
            }
        }

        public void DoServiceProviderTick()
        {
            services.ForEach(service =>
            {
                if(service.IsServiceRunning)
                    service.onServiceTick();
            });
        }

        public override void Update() =>
            DoServiceProviderTick();
        public override void Start()
        {
            base.Start();

            foreach(var service in services)
            {
                WinttOS.ProcessManager.StartProcess(service.ProcessName);
                service.onServiceStart();
            }
        }

        public override void Stop()
        {
            base.Stop();

            foreach (var service in services)
            {
                if (service.IsServiceRunning && service.IsProcessRunning)
                {
                    service.onServiceFinish();
                    WinttOS.ProcessManager.StopProcess(service.ProcessName);
                }
            }
        }

        public void FinishService(string serviceName)
        {
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning && service.IsProcessRunning)
                    {
                        service.onServiceFinish();
                        WinttOS.ProcessManager.StopProcess(service.ProcessName);
                    }
                    return;
                }
            });
        }

        public (ServiceStatus, string) GetServiceStatus(string serviceName)
        {
            foreach(var service in services)
            {
                if (service.ProcessName == serviceName)
                    return (service.ServiceStatus, service.ServiceErrorMessage);
            }
            return (ServiceStatus.no_data, null);
        }

        public void RestartService(string serviceName)
        {
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning)
                        service.onServiceFinish();
                    service.onServiceStart();
                    return;
                }
            });
        }

        public void StartService(string serviceName)
        {
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName && !service.IsServiceRunning && !service.IsProcessRunning)
                {
                    service.onServiceStart();
                    WinttOS.ProcessManager.StopProcess(service.ProcessName);
                }
            });
        }

        #endregion

        #endregion

        #region Methods

        public void StartAllServices()
        {
            services.ForEach(service =>
            {
                StartService(service.ProcessName);
            });
        }

        public void StopAllServices()
        {
            services.ForEach(service =>
            {
                if(service.IsServiceRunning)
                    service.onServiceFinish();
            });
        }

        #endregion
    }
}
