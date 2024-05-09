using System.Collections.Generic;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Processing;

namespace WinttOS.wSystem.Services
{
    public sealed class WinttServiceManager : Process, IServiceManager
    {
        #region Interface implementation

        #region Fields / Variables

        private List<Service> _services = new();

        public WinttServiceManager() : base("service.d", ProcessType.KernelComponent)
        { }

        public List<Service> Services => _services;

        #endregion

        #region Methods

        public void AddService(Service service)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.AddService()",
                "void(Service)", "WinttServiceManager.cs", 25));
            if (!_services.Contains(service))
            {
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, service.ServiceName);

                _services.Add(service);

                service.IsProcessCritical = true;
                WinttOS.ProcessManager.TryRegisterProcess(service);
            }
            WinttCallStack.RegisterReturn();
        }

        public override void Start()
        {
            base.Start();

            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.Start()",
                "void()", "WinttServiceManager.cs", 53));

            foreach(var service in _services)
            {
                WinttOS.ProcessManager.TryStartProcess(service.ProcessName);
            }
            WinttCallStack.RegisterReturn();
        }

        public override void Stop()
        {
            base.Stop();

            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.Stop()",
                "void()", "WinttServiceManager.cs", 68));

            foreach (var service in _services)
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.FinishService()",
                "void(string)", "WinttServiceManager.cs", 87));
            _services.ForEach(service =>
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
            foreach(var service in _services)
            {
                if(!WinttOS.ProcessManager.Processes.Contains(service))
                    _services.Remove(service);
            }
        }

        public (ServiceStatus, string) GetServiceStatus(string serviceName)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.GetServiceStatus()",
                "(ServiceStatuc, string)(string)", "WinttServiceManager.cs", 108));
            foreach (var service in _services)
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.RestartService()",
                "void(string)", "WinttServiceManager.cs", 124));
            _services.ForEach(service =>
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.StartService()",
                "void(string)", "WinttServiceManager.cs", 142));
            _services.ForEach(service =>
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
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.StartAllServices()",
                "void()", "WinttServiceManager.cs", 163));
            _services.ForEach(service =>
            {
                StartService(service.ProcessName);
            });
            WinttCallStack.RegisterReturn();
        }

        public void StopAllServices()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Sys.Services.WinttServiceManager.StopAllServices()",
                "void()", "WinttServiceManager.cs", 174));
            _services.ForEach(service =>
            {
                if (service.IsServiceRunning)
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
            });
            WinttCallStack.RegisterReturn();
        }



        #endregion
    }
}
