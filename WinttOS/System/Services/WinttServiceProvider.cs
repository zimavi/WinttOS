using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
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
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.AddService()",
                "void(Service)", "WinttServiceProvider.cs", 25));
            if (!services.Contains(service))
            {
                services.Add(service);

                service.IsProcessCritical = true;
                WinttOS.ProcessManager.RegisterProcess(service);
            }
            WinttCallStack.RegisterReturn();
        }

        public void DoServiceProviderTick()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.DoServiceProviderTick()",
                "void()", "WinttServiceProvider.cs", 39));
            services.ForEach(service =>
            {
                if(service.IsServiceRunning)
                    service.onServiceTick();
            });
            WinttCallStack.RegisterReturn();
        }

        public override void Update() =>
            DoServiceProviderTick();
        public override void Start()
        {
            base.Start();

            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.Start()",
                "void()", "WinttServiceProvider.cs", 53));

            foreach(var service in services)
            {
                WinttOS.ProcessManager.StartProcess(service.ProcessName);
                service.onServiceStart();
            }
            WinttCallStack.RegisterReturn();
        }

        public override void Stop()
        {
            base.Stop();

            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.Stop()",
                "void()", "WinttServiceProvider.cs", 68));

            foreach (var service in services)
            {
                if (service.IsServiceRunning && service.IsProcessRunning)
                {
                    service.onServiceFinish();
                    WinttOS.ProcessManager.StopProcess(service.ProcessName);
                }
            }

            WinttCallStack.RegisterReturn();
        }

        public void FinishService(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.FinishService()",
                "void(string)", "WinttServiceProvider.cs", 87));
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning && service.IsProcessRunning)
                    {
                        service.onServiceFinish();
                        WinttOS.ProcessManager.StopProcess(service.ProcessName);
                    }
                    WinttCallStack.RegisterReturn();
                    return;
                }
            });

            WinttCallStack.RegisterReturn();
        }

        public (ServiceStatus, string) GetServiceStatus(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.GetServiceStatus()",
                "(ServiceStatuc, string)(string)", "WinttServiceProvider.cs", 108));
            foreach (var service in services)
            {
                if (service.ProcessName == serviceName)
                {
                    WinttCallStack.RegisterReturn();
                    return (service.ServiceStatus, service.ServiceErrorMessage);
                }
            }
            WinttCallStack.RegisterReturn();
            return (ServiceStatus.no_data, string.Empty);
        }

        public void RestartService(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.RestartService()",
                "void(string)", "WinttServiceProvider.cs", 124));
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning)
                        service.onServiceFinish();
                    service.onServiceStart();
                    WinttCallStack.RegisterReturn();
                    return;
                }
            });
            WinttCallStack.RegisterReturn();
        }

        public void StartService(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.StartService()",
                "void(string)", "WinttServiceProvider.cs", 142));
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName && !service.IsServiceRunning && !service.IsProcessRunning)
                {
                    service.onServiceStart();
                    WinttOS.ProcessManager.StopProcess(service.ProcessName);
                }
            });
            WinttCallStack.RegisterReturn();
        }

        #endregion

        #endregion

        #region Methods

        public void StartAllServices()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.StartAllServices()",
                "void()", "WinttServiceProvider.cs", 163));
            services.ForEach(service =>
            {
                StartService(service.ProcessName);
            });
            WinttCallStack.RegisterReturn();
        }

        public void StopAllServices()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceProvider.StopAllServices()",
                "void()", "WinttServiceProvider.cs", 174));
            services.ForEach(service =>
            {
                if(service.IsServiceRunning)
                    service.onServiceFinish();
            });
            WinttCallStack.RegisterReturn();
        }

        #endregion
    }
}
