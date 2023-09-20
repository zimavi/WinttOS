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

        public bool IsRunning { get; private set; } = false;
        public ServiceStatus Status { get; private set; } = ServiceStatus.no_data;
        public string ErrorMessage { get; private set; } = null;
        public string ServiceName { get; private set; } = null;
        public virtual void onServiceStart()
        {
            IsRunning = true;
            Status = ServiceStatus.OK;
        }
        public virtual void onServiceFinish()
        {
            IsRunning = false;
            Status = ServiceStatus.OFF;
        }
        public void onServiceTick()
        {
            Status = ServiceStatus.OK;

            ServiceTick();

            Status = ServiceStatus.PENDING;
        }

        public abstract void ServiceTick();
    }
}
