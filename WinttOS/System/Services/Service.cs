using WinttOS.System.Processing;

namespace WinttOS.System.Services
{
    public abstract class Service : Process
    {
        public Service(string ProcessName) : this(ProcessName, ProcessName) { }

        public Service(string ProcessName, string ServiceName) : base(ProcessName, ProcessType.Service)
        {
            this.ServiceName = ServiceName;
        }

        public bool IsServiceRunning { get; private set; } = false;
        public ServiceStatus ServiceStatus { get; private set; } = ServiceStatus.no_data;
        public string ServiceErrorMessage { get; private set; } = null;
        public string ServiceName { get; private set; } = null;
        public virtual void onServiceStart()
        {
            IsServiceRunning = true;
            ServiceStatus = ServiceStatus.OK;
        }
        public virtual void onServiceFinish()
        {
            IsServiceRunning = false;
            ServiceStatus = ServiceStatus.OFF;
        }
        public void onServiceTick()
        {
            ServiceStatus = ServiceStatus.OK;

            ServiceTick();

            ServiceStatus = ServiceStatus.PENDING;
        }

        public abstract void ServiceTick();
    }
}
