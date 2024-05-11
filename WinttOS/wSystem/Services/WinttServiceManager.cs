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
            if (!_services.Contains(service))
            {
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.NONE, service.ServiceName);

                _services.Add(service);

                service.IsProcessCritical = true;
                WinttOS.ProcessManager.TryRegisterProcess(service);
            }
        }

        public override void Start()
        {
            base.Start();

            foreach(var service in _services)
            {
                WinttOS.ProcessManager.TryStartProcess(service.ProcessName);
            }
        }

        public override void Stop()
        {
            base.Stop();

            foreach (var service in _services)
            {
                if (service.IsServiceRunning && service.IsProcessRunning)
                {
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
                }
            }
        }

        public void FinishService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning && service.IsProcessRunning)
                    {
                        Logger.DoOSLog("[Info] Stopping service " + service.ServiceName + " (PID " + service.ProcessID + ")");
                        WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
                    }
                    return;
                }
            });
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
            foreach (var service in _services)
            {
                if (service.ProcessName == serviceName)
                {
                    return (service.ServiceStatus, service.ServiceErrorMessage);
                }
            }
            return (ServiceStatus.no_data, string.Empty);
        }

        public void RestartService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.ProcessName == serviceName)
                {
                    if (service.IsServiceRunning)
                        FinishService(service.ProcessName);
                    StartService(service.ProcessName);
                    return;
                }
            });
        }

        public void StartService(string serviceName)
        {
            _services.ForEach(service =>
            {
                if (service.ProcessName == serviceName && !service.IsServiceRunning && !service.IsProcessRunning)
                {
                    Logger.DoOSLog("[Info] Starting service " + service.ServiceName + " (PID " + service.ProcessID + ")");
                    service.OnServiceStart();
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
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
                StartService(service.ProcessName);
            });
        }

        public void StopAllServices()
        {
            _services.ForEach(service =>
            {
                if (service.IsServiceRunning)
                    WinttOS.ProcessManager.TryStopProcess(service.ProcessName);
            });
        }



        #endregion
    }
}
