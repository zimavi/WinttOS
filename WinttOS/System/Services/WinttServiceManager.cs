using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.System;
using WinttOS.System.Processing;

namespace WinttOS.System.Services
{
    public class WinttServiceManager : Process, IServiceManager
    {
        #region Interface implementation

        #region Fields / Variables

        private List<Service> services = new();

        public WinttServiceManager() : base("ServiceDaemon", ProcessType.KernelComponent)
        { }

        public List<Service> Services => services;

        #endregion

        #region Methods

        public void AddService(Service service)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.AddService()",
                "void(Service)", "WinttServiceManager.cs", 25));
            if (!services.Contains(service))
            {
                services.Add(service);

                service.IsProcessCritical = true;
                WinttOS.ProcessManager.TryRegisterProcess(service);
            }
            WinttCallStack.RegisterReturn();
        }

        public override void Start()
        {
            base.Start();

            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.Start()",
                "void()", "WinttServiceManager.cs", 53));

            foreach(var service in services)
            {
                WinttOS.ProcessManager.TryStartProcess(service.ProcessName);
            }
            WinttCallStack.RegisterReturn();
        }

        public override void Stop()
        {
            base.Stop();

            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.Stop()",
                "void()", "WinttServiceManager.cs", 68));

            foreach (var service in services)
            {
                if (service.IsServiceRunning && service.IsProcessRunning)
                {
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
                }
            }

            WinttCallStack.RegisterReturn();
        }

        public void FinishService(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.FinishService()",
                "void(string)", "WinttServiceManager.cs", 87));
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning && service.IsProcessRunning)
                    {
                        WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
                    }
                    WinttCallStack.RegisterReturn();
                    return;
                }
            });

            WinttCallStack.RegisterReturn();
        }

        public void RunServiceGC()
        {
            foreach(var service in services)
            {
                if(!WinttOS.ProcessManager.Processes.Contains(service))
                    services.Remove(service);
            }
        }

        public (ServiceStatus, string) GetServiceStatus(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.GetServiceStatus()",
                "(ServiceStatuc, string)(string)", "WinttServiceManager.cs", 108));
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
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.RestartService()",
                "void(string)", "WinttServiceManager.cs", 124));
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning)
                        WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
                    WinttOS.ProcessManager.TryStartProcess(service.ProcessName);
                    WinttCallStack.RegisterReturn();
                    return;
                }
            });
            WinttCallStack.RegisterReturn();
        }

        public void StartService(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.StartService()",
                "void(string)", "WinttServiceManager.cs", 142));
            services.ForEach(service =>
            {
                if (service.ProcessName == serviceName && !service.IsServiceRunning && !service.IsProcessRunning)
                {
                    service.OnServiceStart();
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
                }
            });
            WinttCallStack.RegisterReturn();
        }

        #endregion

        #endregion

        #region Methods

        public void StartAllServices()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.StartAllServices()",
                "void()", "WinttServiceManager.cs", 163));
            services.ForEach(service =>
            {
                StartService(service.ProcessName);
            });
            WinttCallStack.RegisterReturn();
        }

        public void StopAllServices()
        {
            WinttCallStack.RegisterCall(new("WinttOS.System.Services.WinttServiceManager.StopAllServices()",
                "void()", "WinttServiceManager.cs", 174));
            services.ForEach(service =>
            {
                if (service.IsServiceRunning)
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
            });
            WinttCallStack.RegisterReturn();
        }



        #endregion
    }
}
